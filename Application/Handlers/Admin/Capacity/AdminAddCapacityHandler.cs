using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminAddCapacityHandler : IRequestHandler<AdminAddCapacityCommand, CommandResponse<Capacity>>
    {
        private readonly ICapacityRepository _repository;
        public AdminAddCapacityHandler(ICapacityRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }
        public async Task<CommandResponse<Capacity>> Handle(AdminAddCapacityCommand request, CancellationToken cancellationToken)
        {
            var entity = new Capacity
            {
                Enable = request.Enable,
                Name = request.Name,
                Qty = request.Qty,
                ServiceType = (int) request.ServiceType
            };

            var result = await _repository.AddAsync(entity);
            return new Success<Capacity>() { Data = result };
        }
    }
}

