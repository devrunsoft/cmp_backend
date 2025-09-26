//using System;
//using CMPNatural.Core.Entities;
//using CMPNatural.Core.Enums;
//using CMPNatural.Core.Repositories;
//using MediatR;
//using ScoutDirect.Application.Responses;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using CMPNatural.Application.Commands.Admin.Invoice;
//using CMPNatural.Application.Handlers;
//using Microsoft.EntityFrameworkCore;
//using ScoutDirect.Core.Entities.Base;
//using CMPNatural.Core.Models;

//namespace CMPNatural.Application
//{
//    public class AdminUpdateRequestInvoiceHandler : IRequestHandler<AdminUpdateInvoiceCommand, CommandResponse<Invoice>>
//    {
//        private readonly IinvoiceRepository _invoiceRepository;
//        private readonly IProductPriceRepository _productPriceRepository;
//        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
//        private readonly IContractRepository _contractRepository;
//        private readonly ICompanyContractRepository _companyContractRepository;
//        private readonly IAppInformationRepository _appRepository;
//        private readonly IRequestTerminateRepository requestTerminate;
//        private readonly IMediator mediator;
//        private readonly AppSetting _appSetting;
//        private readonly ILocationCompanyRepository locationCompanyRepository;

//        public AdminUpdateRequestInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository,
//             IBaseServiceAppointmentRepository baseServiceAppointmentRepository, IContractRepository _contractRepository,
//             ICompanyContractRepository _companyContractRepository, IAppInformationRepository _appRepository, IRequestTerminateRepository requestTerminate, AppSetting appSetting,
//             IMediator mediator, ILocationCompanyRepository locationCompanyRepository)
//        {
//            _invoiceRepository = invoiceRepository;
//            _productPriceRepository = productPriceRepository;
//            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
//            this._contractRepository = _contractRepository;
//            this._companyContractRepository = _companyContractRepository;
//            this._appRepository = _appRepository;
//            this._appSetting = appSetting;
//            this.requestTerminate = requestTerminate;
//            this.mediator = mediator;
//            this.locationCompanyRepository = locationCompanyRepository;
//        }

//        public async Task<CommandResponse<Invoice>> Handle(AdminUpdateInvoiceCommand requests, CancellationToken cancellationToken)
//        {

//            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == requests.InvoiceId, query => query.Include(x => x.Company))).FirstOrDefault();
//            bool canEdit = false;
//            if (invoice.Status != InvoiceStatus.Draft)
//            {
//                var terminateRequest = (await requestTerminate.GetAsync(p => p.InvoiceNumber == invoice.InvoiceId)).FirstOrDefault();
//                if (terminateRequest != null && terminateRequest.RequestTerminateStatus == null)
//                {
//                    canEdit = true;
//                }
//                else
//                {
//                    return new NoAcess<Invoice>() { Message = "No Access To edit this Invoice" };
//                }
//            }
//            if (invoice.ContractId != null && !canEdit)
//            {
//                return new NoAcess<Invoice>() { Message = "You cannot edit an invoice that is linked to a contract." };
//            }
//            var CompanyId = invoice.CompanyId;
//            var services = (await _baseServiceAppointmentRepository.GetAsync(p => p.InvoiceId == invoice.Id)).ToList();

//            List<BaseServiceAppointment> lstCustom = new List<BaseServiceAppointment>();
//            List<ServiceAppointmentEmergency> lstCustomEmrgency = new List<ServiceAppointmentEmergency>();
//            List<InvoiceProduct> invoiceProducts = new List<InvoiceProduct>();

//            var locations = (await locationCompanyRepository.GetAsync(x => x.CompanyId == invoice.CompanyId,
//            query => query.Include(x => x.CapacityEntity)
//            )).ToList();

//            foreach (var request in requests.ServiceAppointment)
//            {
//                invoiceProducts.Add(new InvoiceProduct()
//                {
//                    ProductPriceId = request.ProductPriceId,
//                });

//                var resultPrice = (await _productPriceRepository.GetAsync(x => x.Id == request.ProductPriceId, query => query.Include(x => x.Product))).FirstOrDefault();

//                if (request.ServiceKind == ServiceKind.Custom)
//                {
//                    var command = new ServiceAppointment()
//                    {
//                        CompanyId = CompanyId,
//                        FrequencyType = request.FrequencyType,
//                        ServiceTypeId = resultPrice.Product.ServiceType,
//                        ServicePriceCrmId = "",
//                        ServiceCrmId = "",
//                        StartDate = request.StartDate ?? DateTime.Now,
//                        OperationalAddressId = request.OperationalAddressId,
//                        Status = ServiceStatus.Draft,
//                        Subsidy = request.Subsidy,
//                        IsEmegency = false,
//                        Qty = request.qty,
//                        FactQty = request.qty,
//                        Amount = request.Amount,
//                        ProductId = request.ProductId,
//                        ProductPrice = resultPrice,
//                        ProductPriceId = request.ProductPriceId,
//                        ServiceAppointmentLocations = request.LocationCompanyIds
//                           .Select(id =>
//                           {
//                               var locationCompany = locations.FirstOrDefault(l => l.Id == id);
//                               return new ServiceAppointmentLocation
//                               {
//                                   LocationCompanyId = id,
//                                   Qty = locationCompany?.CapacityEntity?.Qty ?? 1
//                               };
//                           }).ToList(),
//                        DayOfWeek = string.Join(",", request.DayOfWeek.Select(x => x.GetDescription())),
//                        FromHour = request.FromHour,
//                        ToHour = request.ToHour,
//                    };
//                    lstCustom.Add(command);
//                }
//                else
//                {
//                    var command = new ServiceAppointmentEmergency()
//                    {
//                        CompanyId = CompanyId,
//                        FrequencyType = request.FrequencyType,
//                        StartDate = DateTime.Now,
//                        ServiceTypeId = resultPrice.Product.ServiceType,
//                        ServicePriceCrmId = "",
//                        ServiceCrmId = "",
//                        Amount = request.Amount,
//                        Subsidy = request.Subsidy,
//                        ProductId = request.ProductId,
//                        ProductPrice = resultPrice,
//                        ProductPriceId = request.ProductPriceId,
//                        OperationalAddressId = request.OperationalAddressId,
//                        Status = ServiceStatus.Draft,
//                        IsEmegency = true,
//                        Qty = request.qty,
//                        FactQty = request.qty,
//                        ServiceAppointmentLocations = request.LocationCompanyIds
//                           .Select(id =>
//                           {
//                               var locationCompany = locations.FirstOrDefault(l => l.Id == id);
//                               return new ServiceAppointmentLocation
//                               {
//                                   LocationCompanyId = id,
//                                   Qty = locationCompany?.CapacityEntity?.Qty ?? 1
//                               };
//                           }).ToList(),
//                        DayOfWeek = string.Join(",", request.DayOfWeek.Select(x => x.GetDescription())),
//                        FromHour = request.FromHour,
//                        ToHour = request.ToHour,
//                    };
//                    lstCustom.Add(command);
//                }
//            }

//            if (!canEdit)
//            {
//                invoice.SendDate = DateTime.Now;
//                invoice.Status = InvoiceStatus.Pending_Signature;
//            }

//            invoice.BaseServiceAppointment = lstCustom;
//            invoice.InvoiceProduct = invoiceProducts;
//            invoice.Address = requests.Address;
//            invoice.Amount = requests.Amount;
//            invoice.CalculateAmountByamount();

//            await _baseServiceAppointmentRepository.DeleteRangeAsync(services);

//            if (requests.CreateContract && invoice.ContractId == null)
//            {
//                var result = await new AdminCreateCompanyContractHandler(_companyContractRepository, _contractRepository, _invoiceRepository, _appRepository, _appSetting)
//                    .Create(invoice, invoice.CompanyId);

//                if (!result.IsSucces())
//                {
//                    return new NoAcess<Invoice>() { Data = invoice, Message = result.Message };
//                }
//                invoice.ContractId = result.Data.Id;
//                invoice.InvoiceNumber = invoice.Number;
//            }
//            else if (canEdit)
//            {
//                var contract = (await _companyContractRepository.GetAsync(x => x.Id == invoice.ContractId.Value)).FirstOrDefault();
//                await mediator.Send(new AdminUpdateCompanyContractCommand() { CompanyContractId = invoice.ContractId.Value, ContractId = contract.ContractId });
//            }

//            await _invoiceRepository.UpdateAsync(invoice);

//            return new Success<Invoice>() { Data = invoice };

//        }

//    }
//}

