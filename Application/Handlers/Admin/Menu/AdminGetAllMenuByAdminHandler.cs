using System;
using CMPNatural.Application.Commands.Admin.Menu;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminGetAllMenuByAdminHandler : IRequestHandler<AdminGetAllMenuByAdminCommand, CommandResponse<List<Menu>>>
    {
        private readonly IAdminMenuAccessRepository _repository;
        private readonly IAdminRepository _adminRepository;
        private readonly IMenuRepository _menuRepository;
        public AdminGetAllMenuByAdminHandler(IAdminMenuAccessRepository repository, IAdminRepository adminRepository, IMenuRepository menuRepository)
        {
            _repository = repository;
            _adminRepository = adminRepository;
            _menuRepository = menuRepository;
        }
        public async Task<CommandResponse<List<Menu>>> Handle(AdminGetAllMenuByAdminCommand request, CancellationToken cancellationToken)
        {
            var isSuperAdmin = (await _adminRepository.GetByIdAsync(request.AdminId)).Role == "SuperAdmin";
            List<Menu> entity;

            if (isSuperAdmin)
            {
                entity = (await _menuRepository.GetAllAsync()).ToList();
            }
            else
            {
                entity = (await _repository.GetAsync(p => p.AdminId == request.AdminId, query => query.Include(x => x.Menu))).Select(p => p.Menu).ToList();
            }

            return new Success<List<Menu>>() { Data = entity };
        }
    }
}

