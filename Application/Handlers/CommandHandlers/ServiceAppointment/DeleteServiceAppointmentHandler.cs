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

namespace CMPNatural.Application.Handlers
{

    public class DeleteServiceAppointmentHandler : IRequestHandler<DeleteServiceAppointmentCommand, CommandResponse<object>>
    {
        private readonly IServiceAppointmentRepository _serviceAppointmentRepository;

        public DeleteServiceAppointmentHandler(IServiceAppointmentRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<object>> Handle(DeleteServiceAppointmentCommand request, CancellationToken cancellationToken)
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

