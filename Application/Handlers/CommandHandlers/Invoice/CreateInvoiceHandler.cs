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

               var resultPrice =await _productPriceRepository.GetByIdAsync(request.ProductPriceId);

                if (request.ServiceKind == Core.Enums.ServiceKind.Custom)
                {
                 var command = new ServiceAppointment()
                    {
                        CompanyId = requests.CompanyId,
                        FrequencyType = request.FrequencyType,
                        //LocationCompanyId=request.LocationCompanyId,
                        ServiceTypeId = (int)request.ServiceTypeId,
                        ServicePriceCrmId = "",
                        ServiceCrmId = "",
                        DueDate = request.StartDate??DateTime.Now,
                        OperationalAddressId = request.OperationalAddressId,
                        Status = (int)ServiceStatus.draft,
                        IsEmegency=false,
                        Qty = request.qty,
                        Amount = resultPrice.Amount * request.qty,
                        ProductId = request.ProductId,
                        ProductPriceId = request.ProductPriceId,
                        ServiceAppointmentLocations = request.LocationCompanyIds
                        .Select(id => new ServiceAppointmentLocation { LocationCompanyId = id })
                        .ToList()
                 };
                    lstCustom.Add(command);
                }
                else
                {
                    var command = new ServiceAppointmentEmergency()
                    {
                        CompanyId = requests.CompanyId,
                        FrequencyType = request.FrequencyType,
                        DueDate = DateTime.Now,
                        //LocationCompanyId=request.LocationCompanyId,
                        ServiceTypeId = (int)request.ServiceTypeId,
                        ServicePriceCrmId = "",
                        ServiceCrmId ="",
                        Amount = resultPrice.Amount * request.qty,
                        ProductId = request.ProductId,
                        ProductPriceId = request.ProductPriceId,
                        //StartDate = request.StartDate,
                        OperationalAddressId = request.OperationalAddressId,
                        Status = (int)ServiceStatus.draft,
                        IsEmegency = true,
                        Qty = request.qty,
                        ServiceAppointmentLocations = request.LocationCompanyIds
                        .Select(id => new ServiceAppointmentLocation { LocationCompanyId = id })
                        .ToList()
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
                InvoiceCrmId= requests.InvoiceCrmId,
                Status = (int)InvoiceStatus.draft,
                InvoiceId = requests.InvoiceId,
                BaseServiceAppointment = lstCustom,
                Amount = requests.Amount,
                RegisterDate= DateTime.Now,
                InvoiceProduct = invoiceProducts,
                Address = requests.Address,
                OperationalAddressId = requests.OperationalAddressId
                //InvoiceNumber = request.InvoiceNumber
            };
            var result = await _invoiceRepository.AddAsync(entity);
            return new Success<Invoice>() { Data = result, Message = "Successfull!" };

        }

    }
}

