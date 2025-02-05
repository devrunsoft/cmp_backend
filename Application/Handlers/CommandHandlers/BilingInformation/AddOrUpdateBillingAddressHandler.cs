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
using CMPNatural.Application.Commands.Billing;
using ScoutDirect.Core.Entities.Base;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddOrUpdateBillingAddressHandler : IRequestHandler<AddOrUpdateBillingAddressCommand, CommandResponse<BillingInformation>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;

        public AddOrUpdateBillingAddressHandler(IBillingInformationRepository billingInformationRepository)
        {
            _billingInformationRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<BillingInformation>> Handle(AddOrUpdateBillingAddressCommand request, CancellationToken cancellationToken)
        {
            var lastResult = (await _billingInformationRepository.GetAsync(p => p.CompanyId == request.CompanyId)).FirstOrDefault();
            if (lastResult == null)
            {
                var entity = new BillingInformation()
                {
                    Address = request.Address,
                    CompanyId = request.CompanyId,
                    City = request.City,
                    ZIPCode = request.ZIPCode,
                    State = request.State,

                };
                var result = await _billingInformationRepository.AddAsync(entity);
                return new Success<BillingInformation>() { Data = result, Message = "Billing Information Address added successfully!" };
            }
            else
            {
                lastResult.Address = request.Address;
                lastResult.City = request.City;
                lastResult.ZIPCode = request.ZIPCode;
                lastResult.State = request.State;
                await _billingInformationRepository.UpdateAsync(lastResult);
                return new Success<BillingInformation>() { Data = lastResult, Message = "Billing Information Address updated successfully!" };
            }
        }

    }
}
