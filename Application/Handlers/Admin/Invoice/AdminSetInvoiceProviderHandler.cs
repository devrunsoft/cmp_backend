using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin.Invoice;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminSetInvoiceProviderHandler : IRequestHandler<AdminSetInvoiceProviderCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;

        public AdminSetInvoiceProviderHandler(IinvoiceRepository invoiceRepository, IProviderReposiotry providerReposiotry,
            IBaseServiceAppointmentRepository baseServiceAppointmentRepository)
        {
            _invoiceRepository = invoiceRepository;
            _providerReposiotry = providerReposiotry;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminSetInvoiceProviderCommand request, CancellationToken cancellationToken)
        {
            //int MaxDistance = 2000; //meter
            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId, query=>
            query.Include(p=>p.BaseServiceAppointment))).FirstOrDefault();

            if (invoice.Status != InvoiceStatus.Needs_Assignment && invoice.Status != InvoiceStatus.Scaduled)
            {
                return new NoAcess<Invoice>() { Data = invoice };
            }

            foreach (var serviceAppointment in invoice.BaseServiceAppointment)
            {
                serviceAppointment.ProviderId = request.ProviderId;

                if (invoice.Status != InvoiceStatus.Scaduled)
                {
                    serviceAppointment.Status = ServiceStatus.Proccessing;
                }
                else
                {
                    serviceAppointment.Status = ServiceStatus.Scaduled;
                }

                await _baseServiceAppointmentRepository.UpdateAsync(serviceAppointment);
            }
            invoice.ProviderId = request.ProviderId;

            if (invoice.Status != InvoiceStatus.Scaduled)
            {
                invoice.Status = InvoiceStatus.Processing_Provider;
            }

            await _invoiceRepository.UpdateAsync(invoice);

            return new Success<Invoice>() { Data = invoice };
        }
    }
}

