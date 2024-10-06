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
    public class AddBillingInformationHandler : IRequestHandler<AddBilingInformationCommand, CommandResponse<BillingInformation>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;

        public AddBillingInformationHandler(IBillingInformationRepository billingInformationRepository)
        {
            _billingInformationRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<BillingInformation>> Handle(AddBilingInformationCommand request, CancellationToken cancellationToken)
        {

            var lastResult = await _billingInformationRepository.GetAsync(p => p.CompanyId == request.CompanyId);
            if (lastResult == null|| lastResult.Count==0)
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
                    CompanyId = request.CompanyId

                };

                var result = await _billingInformationRepository.AddAsync(entity);


                return new Success<BillingInformation>() { Data = result, Message = "Billing Information added successfully!" };
            }
            else
            {
                return new NoAcess<BillingInformation>() { Message = "Biling Information Already Registred!" };
            }
        }

    }
}
