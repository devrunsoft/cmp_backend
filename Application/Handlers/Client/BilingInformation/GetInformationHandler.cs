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
using CMPNatural.Application.Responses.Information;
using ScoutDirect.Core.Repositories;

namespace CMPNatural.Application.Handlers.CommandHandlers.BilingInformation
{
    public class GetInformationHandler : IRequestHandler<GetInformationCommand, CommandResponse<InfromationResponse>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;
        private readonly ICompanyRepository _companyRepository;

        public GetInformationHandler(IBillingInformationRepository billingInformationRepository, ICompanyRepository _companyRepository)
        {
            _billingInformationRepository = billingInformationRepository;
            this._companyRepository = _companyRepository;
        }

        public async Task<CommandResponse<InfromationResponse>> Handle(GetInformationCommand request, CancellationToken cancellationToken)
        {

            var result = (await _billingInformationRepository.GetAsync(p => p.CompanyId == request.CompanyId)).ToList();

            var company = (await _companyRepository.GetAsync(p => p.Id == request.CompanyId)).FirstOrDefault();

            return new Success<InfromationResponse>() { Data = new InfromationResponse()
            {
                billingInformation = result,
                CorporateAddress = company.CorporateAddress ?? ""

            } };
        }

    }
}

