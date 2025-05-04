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

    public class AdminGetAllInvoiceHandler : IRequestHandler<AdminGetAllInvoiceCommand, CommandResponse<PagesQueryResponse<InvoiceResponse>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public AdminGetAllInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<InvoiceResponse>>> Handle(AdminGetAllInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetBasePagedAsync(request, x=> (request.Status == null || x.Status == request.Status) ,
                query => query.Include(i => i.Company).Include(x=>x.Provider)));

            var model = new PagesQueryResponse<InvoiceResponse>(
                invoices.elements.Select(p => InvoiceMapper.Mapper.Map<InvoiceResponse>(p)).ToList(),
                invoices.pageNumber,
                invoices.totalPages,
                invoices.totalElements);

            return new Success<PagesQueryResponse<InvoiceResponse>>() { Data = model };
        }
    }
}

