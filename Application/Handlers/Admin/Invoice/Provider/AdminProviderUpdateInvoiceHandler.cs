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

        public AdminProviderUpdateInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository,
             IBaseServiceAppointmentRepository baseServiceAppointmentRepository, IContractRepository _contractRepository,
             ICompanyContractRepository _companyContractRepository, IAppInformationRepository _appRepository,IManifestRepository _repository,
             IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productPriceRepository = productPriceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
            this._contractRepository = _contractRepository;
            this._companyContractRepository = _companyContractRepository;
            this._appRepository = _appRepository;
            this._repository = _repository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminProviderUpdateInvoiceCommand requests, CancellationToken cancellationToken)
        {

            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == requests.InvoiceId, query => query.Include(x => x.Company))).FirstOrDefault();
            if (invoice.Status != InvoiceStatus.Processing_Provider)
            {
                return new NoAcess<Invoice>() { Message = "No Access To Edit Paid Invoice" };
            }

            var CompanyId = invoice.CompanyId;
            var services = (await _baseServiceAppointmentRepository.GetAsync(p => p.InvoiceId == invoice.Id)).ToList();
            var entity = (await _repository.GetAsync(x => x.InvoiceId == invoice.Id)).FirstOrDefault();

            List<BaseServiceAppointment> lstCustom = new List<BaseServiceAppointment>();
            List<ServiceAppointmentEmergency> lstCustomEmrgency = new List<ServiceAppointmentEmergency>();




            foreach (var request in requests.ServiceAppointment)
            {
                if (services.Any(x => x.Id == request.Id))
                {
                    var resultPrice = await _productPriceRepository.GetByIdAsync(request.ProductPriceId);
                    var srv = services.FirstOrDefault(x => x.Id == request.Id);
                    srv = request.ToEntity(srv, CompanyId , invoice.Id);
                    srv.ProductPrice = resultPrice;
                    srv.Status = ServiceStatus.Complete;
                    lstCustom.Add(srv);
                }
                else
                {
                    var resultPrice = await _productPriceRepository.GetByIdAsync(request.ProductPriceId);
                    if (request.ServiceKind == ServiceKind.Custom)
                    {
                        var command = new ServiceAppointment()
                        {
                            CompanyId = CompanyId,
                            FrequencyType = request.FrequencyType,
                            ServiceTypeId = (int)request.ServiceTypeId,
                            ServicePriceCrmId = "",
                            ServiceCrmId = "",
                            StartDate = request.StartDate ?? DateTime.Now,
                            OperationalAddressId = request.OperationalAddressId,
                            Status = ServiceStatus.Complete,
                            Subsidy = request.Subsidy,
                            IsEmegency = false,
                            Qty = request.qty,
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
                            ServiceTypeId = (int)request.ServiceTypeId,
                            ServicePriceCrmId = "",
                            ServiceCrmId = "",
                            Amount = request.Amount,
                            Subsidy = request.Subsidy,
                            ProductId = request.ProductId,
                            ProductPrice = resultPrice,
                            ProductPriceId = request.ProductPriceId,
                            OperationalAddressId = request.OperationalAddressId,
                            Status = ServiceStatus.Complete,
                            IsEmegency = true,
                            Qty = request.qty,
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
            }


            invoice.BaseServiceAppointment = lstCustom;
            invoice.Address = requests.Address;
            invoice.Status = InvoiceStatus.Send_Payment;
            invoice.Amount = requests.Amount;
            invoice.CalculateAmountByamount();

            await _invoiceRepository.UpdateAsync(invoice);
            //await _baseServiceAppointmentRepository.DeleteRangeAsync(services);

            var information = (await _appRepository.GetAllAsync()).LastOrDefault();
            await CreateManifestContent.CreateContent(invoice, information, entity, _serviceAppointmentLocationRepository);
            entity.Status = ManifestStatus.Completed;
            await _repository.UpdateAsync(entity);

            return new Success<Invoice>() { Data = invoice, Message = "Successfull!" };

        }

    }
}

