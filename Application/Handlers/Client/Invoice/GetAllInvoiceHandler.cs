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

namespace CMPNatural.Application.Handlers
{

    public class GetAllInvoiceHandler : IRequestHandler<GetAllInvoiceCommand, CommandResponse<PagesQueryResponse<InvoiceResponse>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public GetAllInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<InvoiceResponse>>> Handle(GetAllInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetBasePagedAsync(request , p => p.CompanyId == request.CompanyId && p.OperationalAddressId == request.OperationalAddressId
            && (p.Status == InvoiceStatus.Send_Payment
            || p.Status == InvoiceStatus.Complete), query => query
            .Include(p => p.BaseServiceAppointment)
            .Include(x => x.Provider)
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

