using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;

using ScoutDirect.Core.Authentication;
using System.Linq;
using System.Collections.Generic;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class ProviderGetAllInvoiceHandler : IRequestHandler<ProviderGetAllInvoiceCommand, CommandResponse<PagesQueryResponse<InvoiceResponse>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public ProviderGetAllInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<InvoiceResponse>>> Handle(ProviderGetAllInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetBasePagedAsync(request, x=> x.ProviderId == request.ProviderId && (request.Status == null || x.Status == request.Status) ,
                query => query.Include(i => i.Company)));

            var model = new PagesQueryResponse<InvoiceResponse>(
                invoices.elements.Select(p => InvoiceMapper.Mapper.Map<InvoiceResponse>(p)).ToList(),
                invoices.pageNumber,
                invoices.totalPages,
                invoices.totalElements);

            return new Success<PagesQueryResponse<InvoiceResponse>>() { Data = model };
        }
    }
}

