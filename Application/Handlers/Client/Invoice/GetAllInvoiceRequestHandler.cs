using System;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Base;
using Stripe;

namespace CMPNatural.Application.Handlers
{

    public class GetAllInvoiceRequestHandler : IRequestHandler<GetAllInvoiceRequestCommand, CommandResponse<PagesQueryResponse<InvoiceResponse>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public GetAllInvoiceRequestHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<InvoiceResponse>>> Handle(GetAllInvoiceRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetBasePagedAsync(request, p => p.CompanyId == request.CompanyId && p.OperationalAddressId == request.OperationalAddressId &&
            (p.Status == InvoiceStatus.Draft 
            || p.Status == InvoiceStatus.Pending_Signature || p.Status == InvoiceStatus.Needs_Admin_Signature
            || p.Status == InvoiceStatus.Needs_Admin_Signature || p.Status == InvoiceStatus.Processing_Provider
            || p.Status == InvoiceStatus.Canceled
            )
            , query => query
            .Include(p => p.BaseServiceAppointment)
            ));

            var model = new PagesQueryResponse<InvoiceResponse>(
              entity.elements.Select(p => InvoiceMapper.Mapper.Map<InvoiceResponse>(p)).ToList(),
              entity.pageNumber,
              entity.totalPages,
              entity.totalElements);

            return new Success<PagesQueryResponse<InvoiceResponse>>() { Data = model, Message = "Successfull!" };

        }

    }
}

