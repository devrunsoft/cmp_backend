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

    public class GetInvoiceByProductIdHandler : IRequestHandler<GetInvoiceByProductIdCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public GetInvoiceByProductIdHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(GetInvoiceByProductIdCommand request, CancellationToken cancellationToken)
        {
            //TODO
            var entity = (await _invoiceRepository.GetAsync(p =>p.CompanyId == request.CompanyId)).FirstOrDefault();

            return new Success<Invoice>() { Data = entity };

        }

    }
}

