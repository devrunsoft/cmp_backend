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
    public class AdminSetInvoiceProviderHandler : IRequestHandler<AdminSetInvoiceProviderCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _invoiceRepository;
        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;

        public AdminSetInvoiceProviderHandler(IManifestRepository invoiceRepository, IProviderReposiotry providerReposiotry,
            IBaseServiceAppointmentRepository baseServiceAppointmentRepository)
        {
            _invoiceRepository = invoiceRepository;
            _providerReposiotry = providerReposiotry;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
        }

        public async Task<CommandResponse<Manifest>> Handle(AdminSetInvoiceProviderCommand request, CancellationToken cancellationToken)
        {
            //int MaxDistance = 2000; //meter
            var manifest = (await _invoiceRepository.GetAsync(p => p.Id == request.ManifestId, query=>
            query.Include(p=>p.ServiceAppointmentLocation).ThenInclude(x=>x.ServiceAppointment))).FirstOrDefault();

            //if (manifest.Status != ManifestStatus.Draft && manifest.Status != ManifestStatus.Scaduled)
            //{
            //    return new NoAcess<Manifest>() { Data = manifest };
            //}

            var serviceAppointment = manifest.ServiceAppointmentLocation.ServiceAppointment;
                serviceAppointment.ProviderId = request.ProviderId;

                if (manifest.Status != ManifestStatus.Scaduled)
                {
                    serviceAppointment.Status = ServiceStatus.Proccessing;
                }
                else
                {
                    serviceAppointment.Status = ServiceStatus.Scaduled;
                }

                await _baseServiceAppointmentRepository.UpdateAsync(serviceAppointment);

            //manifest.ProviderId = request.ProviderId;

            //if (manifest.Status != InvoiceStatus.Scaduled)
            //{
            //    manifest.Status = InvoiceStatus.Processing_Provider;
            //}

            await _invoiceRepository.UpdateAsync(manifest);

            return new Success<Manifest>() { Data = manifest };
        }
    }
}

