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
using CMPNatural.Core.Models;

namespace CMPNatural.Application
{
    public class ProviderUpdateInvoiceHandler : IRequestHandler<ProviderUpdateInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IManifestRepository _repository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        private readonly AppSetting _appSetting;

        public ProviderUpdateInvoiceHandler(IinvoiceRepository invoiceRepository,
             IBaseServiceAppointmentRepository baseServiceAppointmentRepository,
             IManifestRepository _repository, IAppInformationRepository _apprepository, IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository, AppSetting appSetting)
        {
            _invoiceRepository = invoiceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
            this._repository = _repository;
            this._apprepository = _apprepository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
            this._appSetting = appSetting;
        }

        public async Task<CommandResponse<Invoice>> Handle(ProviderUpdateInvoiceCommand requests, CancellationToken cancellationToken)
        {
            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == requests.InvoiceId && p.ProviderId == requests.ProviderId, query => query.Include(x => x.Company)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ProductPrice)
                .ThenInclude(p => p.Product)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ServiceAppointmentLocations)
                .ThenInclude(p => p.LocationCompany)
                )).FirstOrDefault();

            var entity = (await _repository.GetAsync(x => x.InvoiceId == invoice.Id)).FirstOrDefault();

            if (invoice.Status == InvoiceStatus.Send_Payment)
            {
                return new NoAcess<Invoice>
                {
                    Message = "You cannot edit this invoice because it is no longer in the 'Processing by Provider' status."
                };
            }

            if (entity.Status != ManifestStatus.Processing && entity.Status != ManifestStatus.Assigned)
            {
                return new NoAcess<Invoice>
                {
                    Message = "You cannot edit this invoice because the related manifest is not in 'Processing' status."
                };
            }

            var CompanyId = invoice.CompanyId;
            var services = (await _baseServiceAppointmentRepository.GetAsync(p => p.InvoiceId == invoice.Id)).ToList();

            //InvoiceStatus invoiceStatus = InvoiceStatus.Submited_Provider;

            foreach (var request in requests.ServiceAppointment)
            {
                if (services.Any(x => x.Id == request.Id))
                {
                    var srv = services.FirstOrDefault(x => x.Id == request.Id);
                    if (srv.Qty != request.FactQty)
                    {
                        srv.Status = ServiceStatus.Updated_Provider;
                        //invoiceStatus = InvoiceStatus.Updated_Provider;
                    }
                    else
                    {
                        srv.Status = ServiceStatus.Submited_Provider;
                    }

                    srv.OilQuality = request.OilQuality;
                    srv.FactQty = request.FactQty;
                    await _baseServiceAppointmentRepository.UpdateAsync(srv);
                }
            }
            invoice.Comment = requests.Comment;
            invoice.Status = InvoiceStatus.Send_To_Admin;
            invoice.CalculateAmountByamount();

            await _invoiceRepository.UpdateAsync(invoice);
            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            await CreateManifestContent.CreateContent(invoice, information, entity, _serviceAppointmentLocationRepository, _appSetting);
            entity.Status = ManifestStatus.Send_To_Admin;
            entity.ManifestNumber = entity.Number;
            await _repository.UpdateAsync(entity);

            return new Success<Invoice>() { Data = invoice, Message = "Successfull!" };

        }
    }
}
