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
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Handlers
{
    public class AdminSentInvoiceHandler : IRequestHandler<AdminSentInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        public AdminSentInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminSentInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId)).FirstOrDefault();
            entity.Status = (int)request.Status;

            entity.CalculateAmount();

            await _invoiceRepository.UpdateAsync(entity);

            return new Success<Invoice>() { Data = entity, Message = "Successfull!" };

        }
    }
}

