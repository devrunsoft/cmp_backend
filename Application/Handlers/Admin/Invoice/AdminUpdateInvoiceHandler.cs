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

namespace CMPNatural.Application
{
    public class AdminUpdateInvoiceHandler : IRequestHandler<AdminUpdateInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;

        public AdminUpdateInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository ,
             IBaseServiceAppointmentRepository baseServiceAppointmentRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productPriceRepository = productPriceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminUpdateInvoiceCommand requests, CancellationToken cancellationToken)
        {

            var invoice = (await _invoiceRepository.GetAsync(p => p.InvoiceId == requests.InvoiceId)).FirstOrDefault();

            if (invoice.Status == (int)InvoiceStatus.paid)
            {
                return new NoAcess<Invoice>() { Message = "No Access To Edit Paid Invoice" };
            }

            var CompanyId = invoice.CompanyId;

            invoice.SendDate = DateTime.Now;
            invoice.Address = requests.Address;
            invoice.Status = requests.ForceToPay ? (int)InvoiceStatus.SentForPay : (int)InvoiceStatus.Processing;
            invoice.Amount = requests.Amount;


            var services = (await _baseServiceAppointmentRepository.GetAsync(p => p.InvoiceId == invoice.Id)).ToList();
            await _baseServiceAppointmentRepository.DeleteRangeAsync(services);
             
            List <BaseServiceAppointment> lstCustom = new List<BaseServiceAppointment>();
            List<ServiceAppointmentEmergency> lstCustomEmrgency = new List<ServiceAppointmentEmergency>();
            List<InvoiceProduct> invoiceProducts = new List<InvoiceProduct>();

            foreach (var request in requests.ServiceAppointment)
            {
                invoiceProducts.Add(new InvoiceProduct()
                {
                    ProductPriceId = request.ProductPriceId,
                });

                var resultPrice = await _productPriceRepository.GetByIdAsync(request.ProductPriceId);

                if (request.ServiceKind == Core.Enums.ServiceKind.Custom)
                {
                    var command = new ServiceAppointment()
                    {
                        CompanyId = CompanyId,
                        FrequencyType = request.FrequencyType,
                        //LocationCompanyId=request.LocationCompanyId,
                        ServiceTypeId = (int)request.ServiceTypeId,
                        ServicePriceCrmId = "",
                        ServiceCrmId = "",
                        DueDate = request.StartDate ?? DateTime.Now,
                        OperationalAddressId = request.OperationalAddressId,
                        Status = (int)ServiceStatus.draft,
                        IsEmegency = false,
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
                        CompanyId = CompanyId,
                        FrequencyType = request.FrequencyType,
                        DueDate = DateTime.Now,
                        //LocationCompanyId=request.LocationCompanyId,
                        ServiceTypeId = (int)request.ServiceTypeId,
                        ServicePriceCrmId = "",
                        ServiceCrmId = "",
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

            invoice.BaseServiceAppointment = lstCustom;
            invoice.InvoiceProduct = invoiceProducts;

            await _invoiceRepository.UpdateAsync(invoice);
            return new Success<Invoice>() { Data = invoice, Message = "Successfull!" };

        }

    }
}

