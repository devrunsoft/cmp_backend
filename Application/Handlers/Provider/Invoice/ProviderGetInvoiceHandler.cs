using System;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Application.Mapper;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers
{

    public class ProviderGetInvoiceHandler : IRequestHandler<ProviderGetInvoiceCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public ProviderGetInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(ProviderGetInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId && p.ProviderId == request.ProviderId,
                 query => query.Include(i => i.Company)
                .ThenInclude(x => x.BillingInformation)
                .Include(i => i.Provider)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ProductPrice)
                .ThenInclude(p => p.Product)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ServiceAppointmentLocations)
                .ThenInclude(p => p.LocationCompany)

            )).FirstOrDefault();
            return new Success<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(entity), Message = "Successfull!" };

        }

    }
}

