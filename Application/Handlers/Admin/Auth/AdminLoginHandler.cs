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

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminLoginHandler : IRequestHandler<AdminLoginCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ICacheService _cache;

        public AdminLoginHandler(IAdminRepository adminRepository, Func<CacheTech, ICacheService> _cacheService)
        {
            _adminRepository = adminRepository;
            _cache = _cacheService(CacheTech.Memory);
        }

        public async Task<CommandResponse<AdminEntity>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            var admin = (await _adminRepository.GetAsync(p => p.Email == request.Email,query=>query.Include(x=>x.Person))).FirstOrDefault();

            if (admin == null)
            {
                return new NoAcess<AdminEntity>() { Message = "Login failed. Please check your username and password and try again." };
            }
            if (admin.IsActive==false)
            {
                return new NoAcess<AdminEntity>() { Message = "Your account is inactive. Please contact support for assistance." };
            }
            if (admin.Password != request.Password)
            {
                return new NoAcess<AdminEntity>() { Message = "Login failed. Please check your username and password and try again." };
            }

            var cacheKey = $"menu_access_{admin.Id}";
            _cache.Remove(cacheKey);

            return new Success<AdminEntity>() { Data = admin };

       
        }

    }
}

