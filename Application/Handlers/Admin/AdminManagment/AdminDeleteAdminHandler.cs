using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin.AdminManagment;

namespace CMPNatural.Application
{
    public class AdminDeleteAdminHandler : IRequestHandler<AdminDeleteAdminCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _repository;
        public AdminDeleteAdminHandler(IAdminRepository repository)
        {
            _repository = repository;
        }
        public async Task<CommandResponse<AdminEntity>> Handle(AdminDeleteAdminCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);


            await _repository.DeleteAsync(entity);
            return new Success<AdminEntity>() { Data = entity };
        }
    }
}

