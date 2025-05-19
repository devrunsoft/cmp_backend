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
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminLoginCodeHandler : IRequestHandler<AdminLoginCodeCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _adminRepository;

        public AdminLoginCodeHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<CommandResponse<AdminEntity>> Handle(AdminLoginCodeCommand request, CancellationToken cancellationToken)
        {
            var admin = (await _adminRepository.GetAsync(p => p.Email == request.Email, query => query.Include(x => x.Person))).FirstOrDefault();

            if (admin == null)
            {
                return new NoAcess<AdminEntity>() { Message = "Login failed. Please check your username and password and try again." };
            }
            if (admin.IsActive==false)
            {
                return new NoAcess<AdminEntity>() { Message = "Your account is inactive. Please contact support for assistance." };
            }
            if (admin.Code != request.Code)
            {
                return new NoAcess<AdminEntity>() { Message = "Login failed. Please check your code and try again." };
            }

            return new Success<AdminEntity>() { Data = admin };

       
        }

    }
}

