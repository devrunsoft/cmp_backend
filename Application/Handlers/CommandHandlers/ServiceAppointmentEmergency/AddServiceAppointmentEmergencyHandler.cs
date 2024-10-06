using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application.Handlers
{

    public class AddServiceAppointmentEmergencyHandler : IRequestHandler<AddServiceAppointmentEmergencyCommand, CommandResponse<ServiceAppointmentEmergency>>
    {
        private readonly IServiceAppointmentEmergencyRepository _serviceAppointmentEmergencyRepository;

        public AddServiceAppointmentEmergencyHandler(IServiceAppointmentEmergencyRepository serviceAppointmentEmergencyRepository)
        {
            _serviceAppointmentEmergencyRepository = serviceAppointmentEmergencyRepository;
        }

        public async Task<CommandResponse<ServiceAppointmentEmergency>> Handle(AddServiceAppointmentEmergencyCommand request, CancellationToken cancellationToken)
        {
            var entity = new ServiceAppointmentEmergency()
            {
                CompanyId = request.CompanyId,
                FrequencyType=request.FrequencyType,
                //LocationCompanyId=request.LocationCompanyId,
                ServiceTypeId=(int)request.ServiceTypeId,
                OperationalAddressId=request.OperationalAddressId
            };

            var result = await _serviceAppointmentEmergencyRepository.AddAsync(entity);

            return new Success<ServiceAppointmentEmergency>() { Data = result, Message = "Service Enrolled Successfully!" };

        }

    }
}

