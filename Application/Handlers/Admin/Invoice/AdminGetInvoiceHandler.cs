using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application.Handlers.Admin.Invoice
{
    public class AdminGetInvoiceHandler : IRequestHandler<AdminGetInvoiceCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public AdminGetInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(AdminGetInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetAsync(p=>p.InvoiceId == request.InvoiceId,
                query => query.Include(i => i.Company)
                .ThenInclude(x=>x.BillingInformation)
                .Include(i => i.Provider)
                .Include(i=>i.BaseServiceAppointment)
                .ThenInclude(i=>i.ProductPrice)
                .ThenInclude(p=>p.Product)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ServiceAppointmentLocations)
                .ThenInclude(p=>p.LocationCompany)
                )).FirstOrDefault();

            var model = InvoiceMapper.Mapper.Map<InvoiceResponse>(invoices);

            return new Success<InvoiceResponse>() { Data = model };
        }
    }
}

