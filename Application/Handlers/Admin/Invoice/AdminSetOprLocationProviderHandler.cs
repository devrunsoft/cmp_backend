using System;
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminSetOprLocationProviderHandler : IRequestHandler<AdminSetOprLocationProviderCommand, CommandResponse<Invoice>>
    {
        private readonly IOperationalAddressRepository _locationCompanyRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IinvoiceRepository _invoiceRepository;

        public AdminSetOprLocationProviderHandler(IOperationalAddressRepository locationCompanyRepository, IProviderReposiotry providerReposiotry,
            IinvoiceRepository invoiceRepository, IBaseServiceAppointmentRepository baseServiceAppointmentRepository)
        {
            _locationCompanyRepository = locationCompanyRepository;
            _providerReposiotry = providerReposiotry;
            _invoiceRepository = invoiceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminSetOprLocationProviderCommand request, CancellationToken cancellationToken)
        {
            var invoice = (await _invoiceRepository.GetAsync(p => p.InvoiceId == request.InvoiceId, query =>
                query.Include(p => p.BaseServiceAppointment))).FirstOrDefault();

            if (!(invoice.Status == InvoiceStatus.Needs_Assignment))
            {
                return new NoAcess<Invoice>() { Data = invoice };
            }

            foreach (var serviceAppointment in invoice.BaseServiceAppointment)
            {
                if (serviceAppointment.Id == request.ServiceId)
                {
                    serviceAppointment.ProviderId = request.ProviderId;
                    await _baseServiceAppointmentRepository.UpdateAsync(serviceAppointment);
                }
            }

            if (invoice.BaseServiceAppointment.Any(p => p.ProviderId != null))
            {
                var distinctProviders = invoice.BaseServiceAppointment.Select(p => p.ProviderId).Distinct().Count();
                if (distinctProviders == 1)
                {
                    invoice.Status = InvoiceStatus.Processing_Provider;
                    invoice.ProviderId = request.ProviderId;
                }
                else
                {
                 //invoice.Status = (int)InvoiceStatus.ProcessingSeprateProvider;
                }

                await _invoiceRepository.UpdateAsync(invoice);

            }

            if (!invoice.BaseServiceAppointment.Any(p => p.ProviderId == null))
            {
                invoice.Status = InvoiceStatus.Processing_Provider;
                await _invoiceRepository.UpdateAsync(invoice);
            }


            //if (invoice.BaseServiceAppointment.Any(p => p.ProviderId != null) && invoice.BaseServiceAppointment.Any(p => p.ProviderId == null))
            //{
            //    invoice.Status = (int)InvoiceStatus.ProcessingPartialProvider;
            //    await _invoiceRepository.UpdateAsync(invoice);
            //}

            return new Success<Invoice>() { Data = invoice };
        }
    }
}

