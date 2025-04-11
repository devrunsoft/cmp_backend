using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using CMPNatural.Application.Responses;
using ScoutDirect.Core.Repositories;

namespace CMPNatural.Application
{
    public class ProviderActivateEmailHandler : IRequestHandler<ProviderActivateEmailCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _repository;

        public ProviderActivateEmailHandler(IProviderReposiotry repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Provider>> Handle(ProviderActivateEmailCommand request, CancellationToken cancellationToken)
        {
            Provider model = (await _repository.GetAsync(p => p.ActivationLink == request.activationLink)).FirstOrDefault();

            if (model == null)
            {
                return new NoAcess<Provider>();
            }
            else if (model.Status != ProviderStatus.PendingEmail)
            {
                return new NoAcess<Provider>();
            }
            else
            {
                model.Status = ProviderStatus.Pending;
                await _repository.UpdateAsync(model);
            }
            return new Success<Provider>() { Data = model };
        }
    }
}

