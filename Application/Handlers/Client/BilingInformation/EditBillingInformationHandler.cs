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
using System.ComponentModel.Design;
using System.Net;
using System.Reflection.Emit;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class EditBillingInformationHandler : IRequestHandler<EditBilingInformationCommand, CommandResponse<object>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;

        public EditBillingInformationHandler(IBillingInformationRepository billingInformationRepository)
        {
            _billingInformationRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<object>> Handle(EditBilingInformationCommand request, CancellationToken cancellationToken)
        {

            var entity = await _billingInformationRepository.GetByIdAsync(request.Id);

            if (entity.CompanyId != request.CompanyId)
            {
                return new NoAcess() { Message = "No Access to Edit!" };
            }

            entity.Address = request.Address;
            entity.CardholderName = request.CardholderName;
            entity.ZIPCode = request.ZIPCode;
            entity.State = request.State;
            entity.IsPaypal = request.IsPaypal;
            entity.Expiry = request.Expiry;
            entity.CVC = request.CVC;
            entity.CardNumber = request.CardNumber;
            entity.City = request.City;
            entity.CompanyId = request.CompanyId;

            await _billingInformationRepository.UpdateAsync(entity);


            return new Success<object>() { Success = true, Data = entity, Message = "Billing Information Updated successfully!" };
        }

    }
}
