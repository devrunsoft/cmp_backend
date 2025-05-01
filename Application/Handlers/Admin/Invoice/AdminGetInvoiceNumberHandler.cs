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

    public class AdminGetInvoiceNumberHandler : IRequestHandler<AdminGetInvoiceNumberCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public AdminGetInvoiceNumberHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminGetInvoiceNumberCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.invoiceNumber)).FirstOrDefault();

            return new Success<Invoice>() { Data = entity, Message = "Successfull!" };

        }

    }
}

