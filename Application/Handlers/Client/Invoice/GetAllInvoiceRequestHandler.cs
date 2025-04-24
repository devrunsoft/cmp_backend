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

namespace CMPNatural.Application.Handlers
{

    public class GetAllInvoiceRequestHandler : IRequestHandler<GetAllInvoiceRequestCommand, CommandResponse<List<InvoiceResponse>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public GetAllInvoiceRequestHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<List<InvoiceResponse>>> Handle(GetAllInvoiceRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p =>p.CompanyId == request.CompanyId && (p.Status != InvoiceStatus.Send_Payment), query => query
            .Include(p=>p.BaseServiceAppointment)
            )).OrderByDescending(x => x.Id).ToList();

            return new Success<List<InvoiceResponse>>() { Data = entity.Select((p)=> InvoiceMapper.Mapper.Map<InvoiceResponse>(p)).ToList(), Message = "Successfull!" };

        }

    }
}

