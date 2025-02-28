using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Invoice;
using ScoutDirect.Core.Entities.Base;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.Admin.Invoice;

namespace CMPNatural.Application.Handlers
{

    public class AdminSentInvoiceHandler : IRequestHandler<AdminSentInvoiceCommand, CommandResponse<object>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public AdminSentInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<object>> Handle(AdminSentInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId)).FirstOrDefault();

            entity.Status = (int)request.Status;

            await _invoiceRepository.UpdateAsync(entity);

            return new Success<object>() { Data = entity, Message = "Successfull!" };

        }

    }
}

