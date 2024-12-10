using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers
{

    public class AddServiceAppointmentHandler : IRequestHandler<AddServiceAppointmentCommand, CommandResponse<ServiceAppointment>>
    {
        private readonly IServiceAppointmentRepository _serviceAppointmentRepository;

        public AddServiceAppointmentHandler(IServiceAppointmentRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<ServiceAppointment>> Handle(AddServiceAppointmentCommand request, CancellationToken cancellationToken)
        {
            var entity = new ServiceAppointment()
            {
                CompanyId = request.CompanyId,
                FrequencyType=request.FrequencyType,
                //LocationCompanyId=request.LocationCompanyId,
                ServiceTypeId=(int) request.ServiceTypeId,
                ServicePriceCrmId=request.ServicePriceId,
                ServiceCrmId=request.ServiceCrmId,
                StartDate=request.StartDate ?? DateTime.Now,
                OperationalAddressId =request.OperationalAddressId,
                Status = (int)ServiceStatus.draft,
                InvoiceId = request.InvoiceId
            };

            var result = await _serviceAppointmentRepository.AddAsync(entity);

            return new Success<ServiceAppointment>() { Data = result, Message = "Service Enrolled Successfully!" };

        }

    }
}

