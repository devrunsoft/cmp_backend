using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Command;
using System.Linq;
using CMPNatural.Application.Commands.Client;
using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminRequestTerminatePostHandler : IRequestHandler<AdminRequestTerminatePostCommand, CommandResponse<RequestTerminate>>
    {
        private readonly IRequestTerminateRepository terminateRepository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IManifestRepository _manifestRepository;

        public AdminRequestTerminatePostHandler(IinvoiceRepository invoiceRepository, IManifestRepository _manifestRepository, IRequestTerminateRepository terminateRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._manifestRepository = _manifestRepository;
            this.terminateRepository = terminateRepository;
        }

        public async Task<CommandResponse<RequestTerminate>> Handle(AdminRequestTerminatePostCommand request, CancellationToken cancellationToken)
        {
            var terminaterequest = (await terminateRepository.GetAsync(x=>x.Id == request.Id)).FirstOrDefault();

            var invoices = (await _invoiceRepository.GetAsync(p => p.InvoiceId == terminaterequest.InvoiceNumber && p.CompanyId == terminaterequest.CompanyId &&
            (p.Status == InvoiceStatus.Draft || p.Status == InvoiceStatus.Scaduled),
                query => query
                .Include(i => i.BaseServiceAppointment)
                )).ToList();

            foreach (var item in invoices)
            {

                foreach (var service in item.BaseServiceAppointment)
                {
                    service.Status = ServiceStatus.Canceled;
                }
                item.Status = InvoiceStatus.Canceled;
                //////
                var manifest = (await _manifestRepository.GetAsync(x => x.InvoiceId == item.Id)).FirstOrDefault();
                manifest.Status = ManifestStatus.Canceled;
                await _manifestRepository.UpdateAsync(manifest);
                await _invoiceRepository.UpdateAsync(item);
            }

            terminaterequest.RequestTerminateStatus = RequestTerminateProcessEnum.Terminated;
            await terminateRepository.UpdateAsync(terminaterequest);

            return new Success<RequestTerminate>() { Data = terminaterequest };
        }
    }
}

