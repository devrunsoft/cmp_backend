using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{

    public class AdminCheckInvoiceProviderHandler : IRequestHandler<AdminCheckInvoiceProviderCommand, CommandResponse<List<Provider>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminCheckInvoiceProviderHandler(IinvoiceRepository invoiceRepository, IProviderReposiotry providerReposiotry)
        {
            _invoiceRepository = invoiceRepository;
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<List<Provider>>> Handle(AdminCheckInvoiceProviderCommand request, CancellationToken cancellationToken)
        {
            //int MaxDistance = 2000; //meter
            var invoices = (await _invoiceRepository.GetAsync(p=>p.InvoiceId== request.InvoiceId,
                query => query.Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ServiceAppointmentLocations)
                .ThenInclude(p => p.LocationCompany))).FirstOrDefault();

            var locations = invoices.BaseServiceAppointment.SelectMany(p => p.ServiceAppointmentLocations).ToList();

            var providers = (await _providerReposiotry.GetAllSearchProviderAllInvoiceAsync(locations)).ToList();

            return new Success<List<Provider>>() { Data = providers };
        }
    }
}
