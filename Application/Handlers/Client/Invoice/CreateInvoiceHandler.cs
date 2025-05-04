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

namespace CMPNatural.Application.Handlers
{

    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IInvoiceSourceRepository _invoiceSourceRepository;
        private readonly IProductPriceRepository _productPriceRepository;

        public CreateInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository, IInvoiceSourceRepository invoiceSourceRepository)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceSourceRepository = invoiceSourceRepository;
            _productPriceRepository = productPriceRepository;
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

                if (request.ServiceKind == Core.Enums.ServiceKind.Custom)
                {
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
                        IsEmegency =false,
                        Qty = request.qty,
                        FactQty = request.qty,
                        Amount = resultPrice.Amount,
                        ProductPrice = resultPrice,
                        ProductId = request.ProductId,
                        ProductPriceId = request.ProductPriceId,
                        ServiceAppointmentLocations = request.LocationCompanyIds
                        .Select(id => new ServiceAppointmentLocation { LocationCompanyId = id })
                        .ToList(),
                     DayOfWeek = string.Join(",", request.DayOfWeek.Select(x => x.GetDescription())),
                     FromHour = request.FromHour,
                     ToHour = request.ToHour,
                 };
                    lstCustom.Add(command);
                }
                else
                {
                    var command = new ServiceAppointmentEmergency()
                    {
                        CompanyId = requests.CompanyId,
                        FrequencyType = request.FrequencyType,
                        StartDate = DateTime.Now,
                        ServiceTypeId = resultPrice.Product.ServiceType,
                        ServicePriceCrmId = "",
                        ServiceCrmId ="",
                        Amount = resultPrice.Amount * request.qty,
                        ProductId = request.ProductId,
                        ProductPriceId = request.ProductPriceId,
                        OperationalAddressId = request.OperationalAddressId,
                        Status = ServiceStatus.Draft,
                        IsEmegency = true,
                        Qty = request.qty,
                        FactQty = request.qty,
                        ServiceAppointmentLocations = request.LocationCompanyIds
                        .Select(id => new ServiceAppointmentLocation { LocationCompanyId = id })
                        .ToList(),
                        DayOfWeek = string.Join(",", request.DayOfWeek.Select(x => x.GetDescription())),
                        FromHour = request.FromHour,
                        ToHour = request.ToHour,
                        ScaduleDate = DateTime.Now
                    };
                    lstCustom.Add(command);
                }
            }
            await _invoiceSourceRepository.AddAsync(new InvoiceSource(){
                CreatedAt = DateTime.Now,
                InvoiceId = requests.InvoiceId,
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
            };
            entity.CalculateAmount();
            var result = await _invoiceRepository.AddAsync(entity);
            return new Success<Invoice>() { Data = result, Message = "Successfull!" };

        }

    }
}

