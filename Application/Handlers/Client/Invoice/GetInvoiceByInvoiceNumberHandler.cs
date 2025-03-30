using System;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;

namespace CMPNatural.Application.Handlers
{

    public class GetInvoiceByInvoiceNumberHandler : IRequestHandler<GetInvoiceByInvoiceNumberCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public GetInvoiceByInvoiceNumberHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(GetInvoiceByInvoiceNumberCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.InvoiceCrmId == request.invoiceNumber && p.CompanyId == request.CompanyId)).FirstOrDefault();

            return new Success<Invoice>() { Data = entity, Message = "Successfull!" };

        }

    }
}

