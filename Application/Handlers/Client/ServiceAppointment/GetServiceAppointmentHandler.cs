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

    public class GetServiceAppointmentHandler : IRequestHandler<GetServiceAppointmentCommand, CommandResponse<BaseServiceAppointment>>
    {
        private readonly IBaseServiceAppointmentRepository _serviceAppointmentRepository;

        public GetServiceAppointmentHandler(IBaseServiceAppointmentRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<BaseServiceAppointment>> Handle(GetServiceAppointmentCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetAsync(
                (p) => p.Id == request.Id &&
                p.CompanyId == request.CompanyId
                //&&
                //(
                //p.Status == ServiceStatus.Proccessing ||
                //p.Status == ServiceStatus.Scaduled ||
                //p.Status == ServiceStatus.Draft)
                //&&
                //p.Status != ServiceStatus.Canceled
                )
                ).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<BaseServiceAppointment>();
            }

            return new Success<BaseServiceAppointment>() { Data = result };

        }

    }
}

