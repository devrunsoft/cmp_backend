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

namespace CMPNatural.Application.Handlers
{

    public class SentInvoiceHandler : IRequestHandler<SentInvoiceCommand, CommandResponse<object>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public SentInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<object>> Handle(SentInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p=>p.CompanyId == request.CompanyId && p.Id == request.InvoiceId)).FirstOrDefault();

            entity.Status = (int)request.Status;

            await _invoiceRepository.UpdateAsync(entity);

            return new Success<object>() { Data = entity, Message = "Successfull!" };

        }

    }
}

