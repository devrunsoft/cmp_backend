using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.ServiceAppointment;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.ServiceAppointmentEmergency;

namespace CMPNatural.Application.Handlers
{

    public class DeleteServiceAppointmentEmergencyHandler : IRequestHandler<DeleteServiceAppointmentEmergencyCommand, CommandResponse<object>>
    {
        private readonly IServiceAppointmentEmergencyRepository _serviceAppointmentRepository;

        public DeleteServiceAppointmentEmergencyHandler(IServiceAppointmentEmergencyRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<object>> Handle(DeleteServiceAppointmentEmergencyCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetAsync(p=>p.Id== request.Id && p.CompanyId==request.CompanyId)).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<object>() { Message = "No Access To Delete This Service" };
            }

            result.Status = (int)ServiceStatus.canceled;
            await _serviceAppointmentRepository.UpdateAsync(result);
            return new Success<object>() { Data = result, Message = "Service Deleted Successfully!" };

        }

    }
}

