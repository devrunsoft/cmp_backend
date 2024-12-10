using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers
{

    public class GetAllServiceAppointmentEmergencyByOprAddressAndServiceTypeIdHandler : IRequestHandler<GetEmergencyServiceAppointmentByOprAddressAndServiceTypeIdCommand, CommandResponse<List<ServiceAppointmentEmergency>>>
    {
        private readonly IServiceAppointmentEmergencyRepository _serviceAppointmentRepository;

        public GetAllServiceAppointmentEmergencyByOprAddressAndServiceTypeIdHandler(IServiceAppointmentEmergencyRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<List<ServiceAppointmentEmergency>>> Handle(GetEmergencyServiceAppointmentByOprAddressAndServiceTypeIdCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetList(
                (p) => p.OperationalAddressId == request.OperationalAddressId &&
                p.CompanyId == request.CompanyId &&
                p.Invoice.Status == "paid" &&
                p.Status != (int)ServiceStatus.canceled &&
                p.ServiceCrmId == request.ServiceTypeId
                )
                ).ToList();

            return new Success<List<ServiceAppointmentEmergency>>() { Data = result };

        }

    }
}

