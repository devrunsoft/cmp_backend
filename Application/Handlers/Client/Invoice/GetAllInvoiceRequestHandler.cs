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

    public class GetAllInvoiceRequestHandler : IRequestHandler<GetAllInvoiceRequestCommand, CommandResponse<PagesQueryResponse<RequestResponse>>>
    {
        private readonly IRequestRepository _invoiceRepository;

        public GetAllInvoiceRequestHandler(IRequestRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<RequestResponse>>> Handle(GetAllInvoiceRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetBasePagedAsync(request, p => p.CompanyId == request.CompanyId &&
            (request.OperationalAddressId == 0 || p.OperationalAddressId == request.OperationalAddressId)
            , query => query
            .Include(p => p.BaseServiceAppointment)
            ));

            var model = new PagesQueryResponse<RequestResponse>(
              entity.elements.Select(p => RequestMapper.Mapper.Map<RequestResponse>(p)).ToList(),
              entity.pageNumber,
              entity.totalPages,
              entity.totalElements);

            return new Success<PagesQueryResponse<RequestResponse>>() { Data = model };

        }

    }
}

