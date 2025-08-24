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
using CMPNatural.Core.Enums;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Handlers
{
    public class ClientCancelRequestHandler : IRequestHandler<ClientCancelRequestCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IManifestRepository _manifestRepository;

        public ClientCancelRequestHandler(IinvoiceRepository invoiceRepository, IManifestRepository _manifestRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._manifestRepository = _manifestRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(ClientCancelRequestCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetAsync(p => p.RequestNumber == request.RequestNumber && p.CompanyId == request.CompanyId &&
              (p.Status == InvoiceStatus.Draft),
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
                //var manifest = (await _manifestRepository.GetAsync(x => x.InvoiceId == item.Id)).FirstOrDefault();
                //manifest.Status = ManifestStatus.Canceled;
                //await _manifestRepository.UpdateAsync(manifest);
                await _invoiceRepository.UpdateAsync(item);
            }
            

            return new Success<Invoice>() { Data = invoices.FirstOrDefault() };
        }
    }
}

