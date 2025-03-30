using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Repositories;
using System.Linq;

namespace CMPNatural.Application.Handlers.Admin.Auth
{
    public class AdminTwoFactorHandler : IRequestHandler<AdminTwoFactorCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _adminRepository;
        public AdminTwoFactorHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<CommandResponse<AdminEntity>> Handle(AdminTwoFactorCommand request, CancellationToken cancellationToken)
        {
            var admin = (await _adminRepository.GetAsync(p => p.Id == request.AdminId)).FirstOrDefault();
            if (admin == null)
            {
                return new NoAcess<AdminEntity>() { Message = "Login failed. Please check your username and password and try again." };
            }
            if (admin.IsActive==false)
            {
                return new NoAcess<AdminEntity>() { Message = "Your account is inactive. Please contact support for assistance." };
            }

            admin.Code = request.Code;
            admin.CodeTime = DateTime.Now.AddMinutes(30);
            await _adminRepository.UpdateAsync(admin);

            return new Success<AdminEntity>() { Data = admin };

       
        }

    }
}

