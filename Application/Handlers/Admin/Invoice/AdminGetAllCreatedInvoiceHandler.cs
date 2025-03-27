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
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminGetAllCreatedInvoiceHandler : IRequestHandler<AdminGetAllCreatedInvoiceCommand, CommandResponse<PagesQueryResponse<InvoiceResponse>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public AdminGetAllCreatedInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<InvoiceResponse>>> Handle(AdminGetAllCreatedInvoiceCommand request, CancellationToken cancellationToken)
        {
            if(request.Status == InvoiceStatus.Draft){
              return new NoAcess<PagesQueryResponse<InvoiceResponse>>() { };
            }

            var invoices = (await _invoiceRepository.GetBasePagedAsync(request, x=> (request.Status == null ?
            x.Status != InvoiceStatus.Draft && x.Status != InvoiceStatus.Needs_Admin_Signature &&
            x.Status != InvoiceStatus.Pending_Signature && x.Status != InvoiceStatus.Needs_Assignment
            : x.Status == request.Status),

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

