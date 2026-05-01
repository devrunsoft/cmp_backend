using System;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Repositories;
using System.Linq;
using ScoutDirect.Core.Caching;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.Driver;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class DriverGoogleLoginHandler : IRequestHandler<DriverGoogleLoginCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        private readonly IConfiguration _configuration;

        public DriverGoogleLoginHandler(IDriverRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(DriverGoogleLoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Credential))
            {
                return new NoAcess<DriverResponse> { Message = "Google credential is required." };
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
                return new NoAcess<DriverResponse> { Message = "Invalid Google credential." };
            }


            var admin = (await _repository.GetAsync(p => p.Email == payload.Email, query => query.Include(x => x.Person))).FirstOrDefault();

            if (admin == null)
            {
                return new NoAcess<DriverResponse>() { Message = "Login failed. Please check your username and password and try again." };
            }
            if (admin.Status == DriverStatus.Blocked)
            {
                return new NoAcess<DriverResponse>() { Message = "Your account is inactive. Please contact support for assistance." };
            }

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(admin) };

        }

    }
}

