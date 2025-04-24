using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers
{
    public class EditServiceAppointmentHandler : IRequestHandler<EditerviceAppointmentCommand, CommandResponse<BaseServiceAppointment>>
    {
        private readonly IServiceAppointmentRepository _serviceAppointmentRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        public EditServiceAppointmentHandler(IServiceAppointmentRepository billingInformationRepository, IProductPriceRepository _productPriceRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
            this._productPriceRepository = _productPriceRepository;
        }

        public async Task<CommandResponse<BaseServiceAppointment>> Handle(EditerviceAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Fetch the existing entity
            var entity = (await _serviceAppointmentRepository.GetAsync(x=> x.Id == request.ServiceId && x.CompanyId == request.CompanyId)).FirstOrDefault();

            if (entity == null)
            {
                return new NoAcess<BaseServiceAppointment>
                {
                    Message = $"Service appointment was not found."
                };
            }

            if (entity.Status != ServiceStatus.Draft)
            {
                return new NoAcess<BaseServiceAppointment>() { Message = $"Cannot edit the service for this Request'. Only Requests in 'Draft' status can be modified." };
            }

            var resultPrice = await _productPriceRepository.GetByIdAsync(request.ProductPriceId);

            entity.FrequencyType = request.FrequencyType;
            entity.ServiceTypeId = (int)request.ServiceTypeId;
            entity.ServicePriceCrmId = "";
            entity.ServiceCrmId = "";
            entity.StartDate = request.StartDate ?? entity.StartDate;
            entity.OperationalAddressId = request.OperationalAddressId;
            entity.Subsidy = request.Subsidy;
            entity.Qty = request.qty;
            entity.Amount = request.Amount;
            entity.ProductId = request.ProductId;
            entity.ProductPrice = resultPrice;
            entity.ProductPriceId = request.ProductPriceId;
            entity.ServiceAppointmentLocations = request.LocationCompanyIds?
                .Select(id => new ServiceAppointmentLocation { LocationCompanyId = id })
                .ToList();
            entity.DayOfWeek = request.DayOfWeek != null
                ? string.Join(",", request.DayOfWeek.Select(x => x.GetDescription()))
                : entity.DayOfWeek;
            entity.FromHour = request.FromHour;
            entity.ToHour = request.ToHour;

            // Optional: update locations
            if (request.LocationCompanyIds != null && request.LocationCompanyIds.Any())
            {
                entity.ServiceAppointmentLocations = request.LocationCompanyIds
                    .Select(id => new ServiceAppointmentLocation { LocationCompanyId = id })
                    .ToList();
            }

             await _serviceAppointmentRepository.UpdateAsync(entity);

            return new Success<BaseServiceAppointment>
            {
                Data = entity,
                Message = "Service updated successfully!"
            };
        }
    }
}
