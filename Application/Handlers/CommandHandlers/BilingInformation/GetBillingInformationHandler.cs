using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Billing;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers.BilingInformation
{
    public class GetBillingInformationHandler : IRequestHandler<GetBilingInformationCommand, CommandResponse<BillingInformation>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;

        public GetBillingInformationHandler(IBillingInformationRepository billingInformationRepository)
        {
            _billingInformationRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<BillingInformation>> Handle(GetBilingInformationCommand request, CancellationToken cancellationToken)
        {

            var result = (await _billingInformationRepository.GetAsync(p=>p.CompanyId== request.CompanyId)).FirstOrDefault();

            return new Success<BillingInformation>() { Data = result };
        }

    }
}

