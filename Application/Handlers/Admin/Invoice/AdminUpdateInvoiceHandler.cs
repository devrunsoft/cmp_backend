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
using ScoutDirect.Core.Entities.Base;

namespace CMPNatural.Application
{
    public class AdminUpdateInvoiceHandler : IRequestHandler<AdminUpdateInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ICompanyContractRepository _companyContractRepository;
        private readonly IAppInformationRepository _appRepository;

        public AdminUpdateInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository ,
             IBaseServiceAppointmentRepository baseServiceAppointmentRepository, IContractRepository _contractRepository,
             ICompanyContractRepository _companyContractRepository, IAppInformationRepository _appRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productPriceRepository = productPriceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
            this._contractRepository = _contractRepository;
            this._companyContractRepository = _companyContractRepository;
            this._appRepository = _appRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminUpdateInvoiceCommand requests, CancellationToken cancellationToken)
        {

            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == requests.InvoiceId, query => query.Include(x => x.Company))).FirstOrDefault();
            if (invoice.Status != InvoiceStatus.Draft)
            {
                return new NoAcess<Invoice>() { Message = "No Access To Edit Paid Invoice" };
            }
            if (invoice.ContractId != null)
            {
                return new NoAcess<Invoice>() { Message = "You cannot edit an invoice that is linked to a contract." };
            }
            var CompanyId = invoice.CompanyId;
            var services = (await _baseServiceAppointmentRepository.GetAsync(p => p.InvoiceId == invoice.Id)).ToList();
             
            List <BaseServiceAppointment> lstCustom = new List<BaseServiceAppointment>();
            List<ServiceAppointmentEmergency> lstCustomEmrgency = new List<ServiceAppointmentEmergency>();
            List<InvoiceProduct> invoiceProducts = new List<InvoiceProduct>();

            foreach (var request in requests.ServiceAppointment)
            {
                invoiceProducts.Add(new InvoiceProduct()
                {
                    ProductPriceId = request.ProductPriceId,
                });

                var resultPrice = (await _productPriceRepository.GetAsync(x => x.Id == request.ProductPriceId, query => query.Include(x => x.Product))).FirstOrDefault();

                if (request.ServiceKind == ServiceKind.Custom)
                {
                    var command = new ServiceAppointment()
                    {
                        CompanyId = CompanyId,
                        FrequencyType = request.FrequencyType,
                        ServiceTypeId = resultPrice.Product.ServiceType,
                        ServicePriceCrmId = "",
                        ServiceCrmId = "",
                        StartDate = request.StartDate ?? DateTime.Now,
                        OperationalAddressId = request.OperationalAddressId,
                        Status = ServiceStatus.Draft,
                        Subsidy = request.Subsidy,
                        IsEmegency = false,
                        Qty = request.qty,
                        FactQty = request.qty,
                        Amount = request.Amount,
                        ProductId = request.ProductId,
                        ProductPrice = resultPrice,
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
                        CompanyId = CompanyId,
                        FrequencyType = request.FrequencyType,
                        StartDate = DateTime.Now,
                        ServiceTypeId = resultPrice.Product.ServiceType,
                        ServicePriceCrmId = "",
                        ServiceCrmId = "",
                        Amount = request.Amount,
                        Subsidy = request.Subsidy,
                        ProductId = request.ProductId,
                        ProductPrice = resultPrice,
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
                    };
                    lstCustom.Add(command);
                }
            }   


            invoice.BaseServiceAppointment = lstCustom;
            invoice.InvoiceProduct = invoiceProducts;
            invoice.SendDate = DateTime.Now;
            invoice.Address = requests.Address;
            invoice.Status = InvoiceStatus.Pending_Signature;
            invoice.Amount = requests.Amount;
            invoice.CalculateAmountByamount();

            await _baseServiceAppointmentRepository.DeleteRangeAsync(services);

            if (requests.CreateContract)
            {
                var result = await new AdminCreateCompanyContractHandler(_companyContractRepository, _contractRepository, _invoiceRepository, _appRepository)
                    .Create(invoice, invoice.CompanyId);

                if (!result.IsSucces())
                {
                    return new NoAcess<Invoice>() { Data = invoice, Message = result.Message };
                }
                invoice.ContractId = result.Data.Id;
            }
            await _invoiceRepository.UpdateAsync(invoice);

            return new Success<Invoice>() { Data = invoice, Message = "Successfull!" };

        }

    }
}

