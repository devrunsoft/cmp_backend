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

    public class GetServiceAppointmentHandler : IRequestHandler<GetServiceAppointmentCommand, CommandResponse<ServiceAppointment>>
    {
        private readonly IServiceAppointmentRepository _serviceAppointmentRepository;

        public GetServiceAppointmentHandler(IServiceAppointmentRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<ServiceAppointment>> Handle(GetServiceAppointmentCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetList(
                (p) => p.Id == request.Id &&
                p.CompanyId == request.CompanyId &&
                p.Invoice.Status == InvoiceStatus.Processing_Provider &&
                p.Status != ServiceStatus.Canceled
                )
                ).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<ServiceAppointment>();
            }

            return new Success<ServiceAppointment>() { Data = result };

        }

    }
}

