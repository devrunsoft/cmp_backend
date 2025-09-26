using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using CMPNatural.Core.Enums;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Stripe.Forwarding;

namespace CMPNatural.Application.Handlers
{

    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IInvoiceSourceRepository _invoiceSourceRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly ILocationCompanyRepository locationCompanyRepository;

        public CreateInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository, IInvoiceSourceRepository invoiceSourceRepository, ILocationCompanyRepository locationCompanyRepository)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceSourceRepository = invoiceSourceRepository;
            _productPriceRepository = productPriceRepository;
            this.locationCompanyRepository = locationCompanyRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(CreateInvoiceCommand requests, CancellationToken cancellationToken)
        {
            List<BaseServiceAppointment> lstCustom = new List<BaseServiceAppointment>();
            List<ServiceAppointmentEmergency> lstCustomEmrgency = new List<ServiceAppointmentEmergency>();

            List<InvoiceProduct> invoiceProducts = new List<InvoiceProduct>();

            foreach (var request in requests.Services)
            {
                invoiceProducts.Add(new InvoiceProduct()
                {
                    ProductPriceId = request.ProductPriceId,
                });

               var resultPrice = (await _productPriceRepository.GetAsync(x=>x.Id == request.ProductPriceId , query => query.Include(x=>x.Product))).FirstOrDefault();

                var locations = (await locationCompanyRepository.GetAsync(x => x.CompanyId == requests.CompanyId,
                 query => query.Include(x => x.CapacityEntity)
                 )).ToList();

                 var command = new ServiceAppointment()
                    {
                        CompanyId = requests.CompanyId,
                        FrequencyType = request.FrequencyType,
                        ServiceTypeId = resultPrice.Product.ServiceType,
                        ServicePriceCrmId = "",
                        ServiceCrmId = "",
                        StartDate = request.StartDate??DateTime.Now,
                        ScaduleDate = request.StartDate ?? DateTime.Now,
                        OperationalAddressId = request.OperationalAddressId,
                        Status = ServiceStatus.Draft,
                        IsEmegency = request.ServiceKind == Core.Enums.ServiceKind.Emergency,
                        Qty = request.qty,
                        FactQty = request.qty,
                        Amount = resultPrice.Amount,
                        ProductPrice = resultPrice,
                        ProductId = request.ProductId,
                        ProductPriceId = request.ProductPriceId,
                        ServiceAppointmentLocations = request.LocationCompanyIds
                           .Select(id =>
                           {
                               var locationCompany = locations.FirstOrDefault(l => l.Id == id);
                               return new ServiceAppointmentLocation
                               {
                                   LocationCompanyId = id,
                                   Qty = locationCompany?.CapacityEntity?.Qty ?? 1
                               };
                           }).ToList(),
                     DayOfWeek = string.Join(",", request.DayOfWeek.Select(x => x.GetDescription())),
                     FromHour = request.FromHour,
                     ToHour = request.ToHour,
                 };
                    lstCustom.Add(command);
                

                
            }

            await _invoiceSourceRepository.AddAsync(new InvoiceSource(){
                CreatedAt = DateTime.Now,
                InvoiceId = requests.InvoiceId,
                OperationalAddressId = requests.OperationalAddressId,
                BillingInformationId = requests.BillingInformationId
            });

            var entity = new Invoice()
            {
                CompanyId = requests.CompanyId,
                Status = InvoiceStatus.Draft,
                InvoiceId = requests.InvoiceId,
                BaseServiceAppointment = lstCustom,
                Amount = requests.Amount,
                InvoiceProduct = invoiceProducts,
                Address = requests.Address,
                OperationalAddressId = requests.OperationalAddressId,
                CreatedAt = DateTime.Now,
                BillingInformationId = requests.BillingInformationId
            };
            entity.CalculateAmount();
            entity.RequestNumber = "";
            entity.InvoiceNumber = "";
            var result = await _invoiceRepository.AddAsync(entity);
            entity.RequestNumber = entity.ReqNumber;
            entity.InvoiceNumber = entity.Number;
            await _invoiceRepository.UpdateAsync(entity);

            return new Success<Invoice>() { Data = result };

        }

    }
}

