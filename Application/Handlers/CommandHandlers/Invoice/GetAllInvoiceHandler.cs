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

namespace CMPNatural.Application.Handlers
{

    public class GetAllInvoiceHandler : IRequestHandler<GetAllInvoiceCommand, CommandResponse<List<Invoice>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public GetAllInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<List<Invoice>>> Handle(GetAllInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p =>p.CompanyId == request.CompanyId, query => query.Include(i => i.InvoiceProduct)
          
            .Include(p=>p.BaseServiceAppointment)
            .ThenInclude(p=>p.ProductPrice)
            .ThenInclude(p=>p.Product)
            )).ToList();

            return new Success<List<Invoice>>() { Data = entity, Message = "Successfull!" };

        }

    }
}

