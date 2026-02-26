using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CMPNatural.Application
{
    public class ProviderGetProviderContractHandler : IRequestHandler<ProviderGetProviderContractCommand, CommandResponse<ProviderContract>>
    {
        private readonly IProviderContractRepository _repository;

        public ProviderGetProviderContractHandler(IProviderContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<ProviderContract>> Handle(ProviderGetProviderContractCommand request, CancellationToken cancellationToken)
        {
            var model = (await _repository.GetByIdAsync(request.Id));
            return new Success<ProviderContract>() { Data = model };
        }
    }
}

