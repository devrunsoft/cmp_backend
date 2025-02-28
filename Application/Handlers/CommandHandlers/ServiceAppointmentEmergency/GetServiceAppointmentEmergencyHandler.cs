using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers
{

    public class GetServiceAppointmentEmergencyHandler : IRequestHandler<GetServiceAppointmentEmergencyCommand, CommandResponse<ServiceAppointmentEmergency>>
    {
        private readonly IServiceAppointmentEmergencyRepository _serviceAppointmentRepository;

        public GetServiceAppointmentEmergencyHandler(IServiceAppointmentEmergencyRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<ServiceAppointmentEmergency>> Handle(GetServiceAppointmentEmergencyCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetList(
                (p) => p.Id == request.Id &&
                p.CompanyId == request.CompanyId &&
                p.Invoice.Status == (int)InvoiceStatus.paid &&
                p.Status != (int)ServiceStatus.Canceled
                )
                ).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<ServiceAppointmentEmergency>();
            }

            return new Success<ServiceAppointmentEmergency>() { Data = result };

        }

    }
}

