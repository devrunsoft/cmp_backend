using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Enums;
using System.Linq;
using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers
{

    public class AddServiceAppointmentHandler : IRequestHandler<AddServiceAppointmentCommand, CommandResponse<BaseServiceAppointment>>
    {
        private readonly IBaseServiceAppointmentRepository _serviceAppointmentRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly ILocationCompanyRepository locationCompanyRepository;
        private readonly IinvoiceRepository _iinvoiceRepository;

        public AddServiceAppointmentHandler(IBaseServiceAppointmentRepository billingInformationRepository , IProductPriceRepository productPriceRepository,
            IinvoiceRepository _iinvoiceRepository, ILocationCompanyRepository locationCompanyRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
            _productPriceRepository = productPriceRepository;
            this._iinvoiceRepository = _iinvoiceRepository;
            this.locationCompanyRepository = locationCompanyRepository;
        }

        public async Task<CommandResponse<BaseServiceAppointment>> Handle(AddServiceAppointmentCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _iinvoiceRepository.GetByIdAsync(request.InvoiceId);

            if(invoice.Status != InvoiceStatus.Draft)
            {
              return new NoAcess<BaseServiceAppointment>() { Message = $"Cannot add a service for this Request'. Only Requests in 'Draft' status can be modified." };
            }

            BaseServiceAppointment entity;
            var resultPrice = (await _productPriceRepository.GetAsync(x => x.Id == request.ProductPriceId, query => query.Include(x => x.Product))).FirstOrDefault();

            var dof = request.DayOfWeek.Count == 0 ? request.DayOfWeek : Enum.GetValues(typeof(DayOfWeekEnum)).Cast<DayOfWeekEnum>().ToList();

            var locations = (await locationCompanyRepository.GetAsync(x=>x.CompanyId == request.CompanyId ,
                query => query.Include(x=>x.Capacity)
                )).ToList();

            if (request.ServiceKind == ServiceKind.Custom)
            {
                 entity = new ServiceAppointment()
                {
                    CompanyId = request.CompanyId,
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
                   .Select(id =>
                   {
                       var locationCompany = locations.FirstOrDefault(l => l.Id == id);
                       return new ServiceAppointmentLocation
                       {
                           LocationCompanyId = id,
                           Qty = locationCompany?.CapacityEntity?.Qty ?? 1
                       };
                   }).ToList(),

                    DayOfWeek = string.Join(",", dof.Select(x => x.GetDescription())),
                    FromHour = request.FromHour,
                    ToHour = request.ToHour,
                };
            }
            else
            {
                entity = new ServiceAppointmentEmergency()
                {
                    CompanyId = request.CompanyId,
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
                   .Select(id =>
                   {
                       var locationCompany = locations.FirstOrDefault(l => l.Id == id);
                       return new ServiceAppointmentLocation
                       {
                           LocationCompanyId = id,
                           Qty = locationCompany?.CapacityEntity?.Qty ?? 1
                       };
                   }).ToList(),

                    DayOfWeek = string.Join(",", dof.Select(x => x.GetDescription())),
                    FromHour = request.FromHour,
                    ToHour = request.ToHour,
                };
            }

            var result = await _serviceAppointmentRepository.AddAsync(entity);

            return new Success<BaseServiceAppointment>() { Data = result, Message = "Service Enrolled Successfully!" };

        }

    }
}

