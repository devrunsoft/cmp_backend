using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using Google.Apis.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Handlers.Admin.Auth
{
    public class AdminGoogleLoginHandler : IRequestHandler<AdminLoginGoogleCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;

        public AdminGoogleLoginHandler(
            IAdminRepository adminRepository,
            IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _configuration = configuration;
        }

        public async Task<CommandResponse<AdminEntity>> Handle(AdminLoginGoogleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Credential))
            {
                return new NoAcess<AdminEntity> { Message = "Google credential is required." };
            }

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.Credential,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[]
                        {
                            _configuration["GoogleAuth:ClientId"]
                        }
                    });
            }
            catch
            {
                return new NoAcess<AdminEntity> { Message = "Invalid Google credential." };
            }

            if (!payload.EmailVerified)
            {
                return new NoAcess<AdminEntity> { Message = "Google email is not verified." };
            }

            var admin = (await _adminRepository.GetAsync(
                p => p.Email == payload.Email,
                query => query.Include(x => x.Person)))
                .FirstOrDefault();

            if (admin == null)
            {
                return new NoAcess<AdminEntity> { Message = "No admin account exists for this Google account." };
            }

            if (admin.IsActive == false)
            {
                return new NoAcess<AdminEntity> { Message = "Your account is inactive. Please contact support for assistance." };
            }

            return new Success<AdminEntity> { Data = admin };
        }
    }
}
