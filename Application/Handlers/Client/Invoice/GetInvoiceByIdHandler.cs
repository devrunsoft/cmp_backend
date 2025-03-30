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

    public class GetInvoiceByIdHandler : IRequestHandler<GetInvoiceByIdCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public GetInvoiceByIdHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(GetInvoiceByIdCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p =>p.Id == request.Id && p.CompanyId ==request.CompanyId)).FirstOrDefault();

            return new Success<Invoice>() { Data = entity, Message = "Successfull!" };

        }

    }
}

