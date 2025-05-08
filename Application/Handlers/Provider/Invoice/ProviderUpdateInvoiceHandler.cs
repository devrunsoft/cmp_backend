using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Application.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ScoutDirect.Core.Entities.Base;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Application
{
    public class ProviderUpdateInvoiceHandler : IRequestHandler<ProviderUpdateInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IManifestRepository _repository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;

        public ProviderUpdateInvoiceHandler(IinvoiceRepository invoiceRepository,
             IBaseServiceAppointmentRepository baseServiceAppointmentRepository,
             IManifestRepository _repository, IAppInformationRepository _apprepository, IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository)
        {
            _invoiceRepository = invoiceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
            this._repository = _repository;
            this._apprepository = _apprepository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(ProviderUpdateInvoiceCommand requests, CancellationToken cancellationToken)
        {
            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == requests.InvoiceId && p.ProviderId == requests.ProviderId, query => query.Include(x => x.Company))).FirstOrDefault();
            var entity = (await _repository.GetAsync(x => x.InvoiceId == invoice.Id)).FirstOrDefault();

            if (invoice.Status != InvoiceStatus.Processing_Provider)
            {
                return new NoAcess<Invoice>
                {
                    Message = "You cannot edit this invoice because it is no longer in the 'Processing by Provider' status."
                };
            }

            if (entity.Status != ManifestStatus.Processing)
            {
                return new NoAcess<Invoice>
                {
                    Message = "You cannot edit this invoice because the related manifest is not in 'Processing' status."
                };
            }

            var CompanyId = invoice.CompanyId;
            var services = (await _baseServiceAppointmentRepository.GetAsync(p => p.InvoiceId == invoice.Id)).ToList();

            InvoiceStatus invoiceStatus = InvoiceStatus.Submited_Provider;

            foreach (var request in requests.ServiceAppointment)
            {
                if (services.Any(x => x.Id == request.Id))
                {
                    var srv = services.FirstOrDefault(x => x.Id == request.Id);
                    if (srv.Qty != request.qty)
                    {
                        srv.Status = ServiceStatus.Updated_Provider;
                        invoiceStatus = InvoiceStatus.Updated_Provider;
                        srv.Qty = request.qty;
                    }
                    else
                    {
                        srv.Status = ServiceStatus.Submited_Provider;
                    }

                    await _baseServiceAppointmentRepository.UpdateAsync(srv);
                }
            }

            invoice.Status = invoiceStatus;
            invoice.CalculateAmountByamount();
            await _invoiceRepository.UpdateAsync(invoice);

            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            await CreateManifestContent.CreateContent(invoice, information, entity, _serviceAppointmentLocationRepository);
            entity.Status = ManifestStatus.Send_To_Admin;
            entity.ManifestNumber = entity.Number;
            await _repository.UpdateAsync(entity);

            return new Success<Invoice>() { Data = invoice, Message = "Successfull!" };

        }
    }
}
