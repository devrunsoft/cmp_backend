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
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Models;

namespace CMPNatural.Application
{
    public class AdminProviderUpdateInvoiceHandler : IRequestHandler<AdminProviderUpdateInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ICompanyContractRepository _companyContractRepository;
        private readonly IAppInformationRepository _appRepository;
        private readonly IManifestRepository _repository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        private readonly AppSetting _appSetting;
        private readonly ILocationCompanyRepository locationCompanyRepository;

        public AdminProviderUpdateInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository,
             IBaseServiceAppointmentRepository baseServiceAppointmentRepository, IContractRepository _contractRepository,
             ICompanyContractRepository _companyContractRepository, IAppInformationRepository _appRepository,IManifestRepository _repository,
             IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository, AppSetting appSetting , ILocationCompanyRepository locationCompanyRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productPriceRepository = productPriceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
            this._contractRepository = _contractRepository;
            this._companyContractRepository = _companyContractRepository;
            this._appRepository = _appRepository;
            this._repository = _repository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
            this._appSetting = appSetting;
            this.locationCompanyRepository = locationCompanyRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminProviderUpdateInvoiceCommand requests, CancellationToken cancellationToken)
        {

            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == requests.InvoiceId, query => query.Include(x => x.Company)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ProductPrice)
                .ThenInclude(p => p.Product)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ServiceAppointmentLocations)
                .ThenInclude(p => p.LocationCompany)
            )).FirstOrDefault();

            if (invoice.Status == InvoiceStatus.Send_Payment)
            {
                return new NoAcess<Invoice>() { Message = "No Access To Edit This Invoice" };
            }

            if (invoice.Status != InvoiceStatus.Submited_Provider && invoice.Status != InvoiceStatus.Updated_Provider && invoice.Status != InvoiceStatus.Processing_Provider && invoice.Status != InvoiceStatus.Send_To_Admin)
            {
                return new NoAcess<Invoice>
                {
                    Message = "You cannot edit this invoice because it has not been submitted by the provider."
                };
            }

            var CompanyId = invoice.CompanyId;
            var services = (await _baseServiceAppointmentRepository.GetAsync(p => p.InvoiceId == invoice.Id , query => query.Include(x=>x.ServiceAppointmentLocations))).ToList();
            var entity = (await _repository.GetAsync(x => x.InvoiceId == invoice.Id)).FirstOrDefault();
            var locations = (await locationCompanyRepository.GetAsync(x => x.CompanyId == invoice.CompanyId,
            query => query.Include(x => x.CapacityEntity)
            )).ToList();

            List<BaseServiceAppointment> lstCustom = new List<BaseServiceAppointment>();
            List<ServiceAppointmentEmergency> lstCustomEmrgency = new List<ServiceAppointmentEmergency>();



            foreach (var request in requests.ServiceAppointment)
            {

                if (services.Any(x => x.Id == request.Id))
                {
                    var resultPrice = (await _productPriceRepository.GetAsync(x=>x.Id == request.ProductPriceId ,
                        query=> query.Include(x=>x.Product))).FirstOrDefault();

                    if ((resultPrice.Product.ServiceType == (int)ServiceType.Cooking_Oil_Collection || resultPrice.Product.ServiceType == (int)ServiceType.Grease_Trap_Management)
                        && request.ServiceAppointmentLocations.Any(x=>x.OilQuality == null))
                    {
                        return new NoAcess<Invoice>
                        {
                            Message = "You cannot edit this invoice because one or more services are missing oil quality information from the provider."
                        };
                    }


                    var srv = services.FirstOrDefault(x => x.Id == request.Id);

                    foreach (var item in srv.ServiceAppointmentLocations)
                    {
                        var loc = request.ServiceAppointmentLocations.FirstOrDefault(x=>x.LocationCompanyId == item.LocationCompanyId);
                        item.FactQty = loc.FactQty;
                        item.OilQuality = loc.OilQuality;
                    }

                    srv = request.ToEntity(srv, CompanyId , invoice.Id);
                    srv.ProductPrice = resultPrice;
                    srv.Status = ServiceStatus.Complete;
                    srv.FactQty = request.FactQty ?? request.qty;
                    srv.OilQuality = request.OilQuality;
                    lstCustom.Add(srv);
                }
                else
                {
                    var resultPrice = (await _productPriceRepository.GetAsync(x => x.Id == request.ProductPriceId, query => query.Include(x => x.Product))).FirstOrDefault();

                    if ((resultPrice.Product.ServiceType == (int)ServiceType.Cooking_Oil_Collection || resultPrice.Product.ServiceType == (int)ServiceType.Grease_Trap_Management) && request.OilQuality == null)
                    {
                        return new NoAcess<Invoice>
                        {
                            Message = "You cannot edit this invoice because one or more services are missing oil quality information from the provider."
                        };
                    }

                        var command = new ServiceAppointment()
                        {
                            CompanyId = CompanyId,
                            FrequencyType = request.FrequencyType,
                            ServiceTypeId = resultPrice.Product.ServiceType,
                            ServicePriceCrmId = "",
                            ServiceCrmId = "",
                            StartDate = request.StartDate ?? DateTime.Now,
                            OperationalAddressId = request.OperationalAddressId,
                            Status = ServiceStatus.Complete,
                            Subsidy = request.Subsidy,
                            IsEmegency = request.ServiceKind == ServiceKind.Emergency,
                            Qty = request.qty,
                            FactQty = request.FactQty,
                            Amount = request.Amount,
                            ProductId = request.ProductId,
                            ProductPrice = resultPrice,
                            ProductPriceId = request.ProductPriceId,
                            ServiceAppointmentLocations = request.ServiceAppointmentLocations
                           .Select(x =>
                           {
                               var locationCompany = locations.FirstOrDefault(l => l.Id == x.LocationCompanyId);
                               return new ServiceAppointmentLocation
                               {
                                   LocationCompanyId = x.LocationCompanyId,
                                   Qty = locationCompany?.CapacityEntity?.Qty ?? 1,
                                   FactQty = x.FactQty,
                                   OilQuality = x.OilQuality
                               };
                           }).ToList(),

                            DayOfWeek = string.Join(",", request.DayOfWeek.Select(x => x.GetDescription())),
                            FromHour = request.FromHour,
                            ToHour = request.ToHour,
                            OilQuality = request.OilQuality
                        };
                        lstCustom.Add(command);


                }
            }


            invoice.BaseServiceAppointment = lstCustom;
            invoice.Address = requests.Address;
            invoice.Status = InvoiceStatus.Send_Payment;
            invoice.Amount = requests.Amount;
            invoice.Comment = requests.Comment;
            invoice.CalculateAmountByamount();

            await _invoiceRepository.UpdateAsync(invoice);
            //await _baseServiceAppointmentRepository.DeleteRangeAsync(services);

            var information = (await _appRepository.GetAllAsync()).LastOrDefault();
            //await CreateManifestContent.CreateContent(invoice, information, entity, _serviceAppointmentLocationRepository,_appSetting);
            entity.Content = "";
            entity.Status = ManifestStatus.Completed;
            entity.ManifestNumber = entity.Number;
            await _repository.UpdateAsync(entity);

            return new Success<Invoice>() { Data = invoice };

        }

    }
}

