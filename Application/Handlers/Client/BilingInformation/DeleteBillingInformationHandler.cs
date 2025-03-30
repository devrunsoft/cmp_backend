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
using CMPNatural.Application.Commands.Billing;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class DeleteBillingInformationHandler : IRequestHandler<DeleteBilingInformationCommand, CommandResponse<object>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;

        public DeleteBillingInformationHandler(IBillingInformationRepository billingInformationRepository)
        {
            _billingInformationRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<object>> Handle(DeleteBilingInformationCommand request, CancellationToken cancellationToken)
        {

            var entity = await _billingInformationRepository.GetByIdAsync(request.Id);

            if (entity.CompanyId != request.CompanyId)
            {
                return new NoAcess() { Message = "No Access to Edit!" };
            }
            await _billingInformationRepository.DeleteAsync(entity);

            return new Success<object>() { Success = true, Message = "Billing Information Deleted successfully!" };
        }

    }
}
