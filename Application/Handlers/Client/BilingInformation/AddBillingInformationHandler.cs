using Barbara.Core.Entities;
using Barbara.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Entities;
using ScoutDirect.Application.Responses;

using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Repositories;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddBillingInformationHandler : IRequestHandler<AddBilingInformationCommand, CommandResponse<BillingInformation>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;
        private readonly ICompanyRepository _companyRepository;

        public AddBillingInformationHandler(IBillingInformationRepository billingInformationRepository, ICompanyRepository _companyRepository)
        {
            _billingInformationRepository = billingInformationRepository;
            this._companyRepository = _companyRepository;
        }

        public async Task<CommandResponse<BillingInformation>> Handle(AddBilingInformationCommand requests, CancellationToken cancellationToken)
        {

            //var lastResult = await _billingInformationRepository.GetAsync(p => p.CompanyId == requests.CompanyId);

            var company = (await _companyRepository.GetAsync(p => p.Id == requests.CompanyId)).FirstOrDefault();
            company.CorporateAddress = requests.CorporateAddress;
            await _companyRepository.UpdateAsync(company);

            foreach (var request in requests.BilingInformationInputs)
            {
                if (request.Address == null)
                    continue;

            if (request.Id == null)
            {
                var entity = new BillingInformation()
                {
                    Address = request.Address,
                    CardholderName = request.CardholderName,
                    ZIPCode = request.ZIPCode,
                    State = request.State,
                    IsPaypal = request.IsPaypal,
                    Expiry = request.Expiry,
                    CVC = request.CVC,
                    CardNumber = request.CardNumber,
                    City = request.City,
                    CompanyId = requests.CompanyId

                };

                var result = await _billingInformationRepository.AddAsync(entity);

            }
            else
            {
                var entity = (await _billingInformationRepository.GetAsync(p => p.Id == request.Id)).FirstOrDefault();
                entity.Address = request.Address;
                entity.CardholderName = request.CardholderName;
                entity.ZIPCode = request.ZIPCode;
                entity.State = request.State;
                entity.IsPaypal = request.IsPaypal;
                entity.Expiry = request.Expiry;
                entity.CVC = request.CVC;
                entity.CardNumber = request.CardNumber;
                entity.City = request.City;
                entity.CompanyId = requests.CompanyId;
               await _billingInformationRepository.UpdateAsync(entity);
            }

            }
          return new Success<BillingInformation>();

        }

    }
}
