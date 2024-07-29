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

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddBillingInformationHandler : IRequestHandler<AddBilingInformationCommand, CommandResponse<object>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;

        public AddBillingInformationHandler(IBillingInformationRepository billingInformationRepository)
        {
            _billingInformationRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<object>> Handle(AddBilingInformationCommand request, CancellationToken cancellationToken)
        {

            var entity = new BillingInformation()
            {
            Address=request.Address,
            CardholderName=request.CardholderName,
            ZIPCode=request.ZIPCode,
            State=request.State,
            IsPaypal=request.IsPaypal,
            Expiry=request.Expiry,
            CVC=request.CVC,
            CardNumber=request.CardNumber,
            City=request.City,
            CompanyId=request.CompanyId

            };

            var result = await _billingInformationRepository.AddAsync(entity);


            return new Success<object>() { Data = result,  Message = "Billing Information added successfully!" };
        }

    }
}
