using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminUpdateCapacityHandler : IRequestHandler<AdminUpdateCapacityCommand, CommandResponse<Capacity>>
    {
        private readonly ICapacityRepository _repository;
        public AdminUpdateCapacityHandler(ICapacityRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }
        public async Task<CommandResponse<Capacity>> Handle(AdminUpdateCapacityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            entity.Enable = request.Enable;
            entity.Name = request.Name;
            entity.Qty = request.Qty;
            entity.ServiceType = (int)request.ServiceType;
            entity.Order = request.Order;

            await _repository.UpdateAsync(entity);
            return new Success<Capacity>() { Data = entity };

        }
    }
}

