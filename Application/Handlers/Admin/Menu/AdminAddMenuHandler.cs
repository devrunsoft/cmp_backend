using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Handlers.Admin.AdminManagment;
using CMPNatural.Application.Commands.Admin.Menu;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminAddMenuHandler : IRequestHandler<AdminAddMenuCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminMenuAccessRepository _repository;
        public AdminAddMenuHandler(IAdminMenuAccessRepository repository)
        {
            _repository = repository;
        }
        public async Task<CommandResponse<AdminEntity>> Handle(AdminAddMenuCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(p=>p.AdminId == request.AdminId)).ToList();
            await _repository.DeleteRangeAsync(entity);
            List<AdminMenuAccess> access = new List<AdminMenuAccess>();
            foreach (var item in request.MenuIds)
            {
                access.Add(new AdminMenuAccess() { AdminId = request.AdminId , MenuId = item});
            }

            await _repository.AddRangeAsync(access);
            return new Success<AdminEntity>() { };
        }
    }
}