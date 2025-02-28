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

    public class GetServiceAppointmentByOprAddressAndServiceTypeIdHandler : IRequestHandler<GetServiceAppointmentByOprAddressAndServiceTypeCommand, CommandResponse<List<ServiceAppointment>>>
    {
        private readonly IServiceAppointmentRepository _serviceAppointmentRepository;

        public GetServiceAppointmentByOprAddressAndServiceTypeIdHandler(IServiceAppointmentRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<List<ServiceAppointment>>> Handle(GetServiceAppointmentByOprAddressAndServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetList(
                (p) => p.OperationalAddressId == request.OperationalAddressId &&
                p.CompanyId == request.CompanyId &&
                p.Invoice.Status == (int)InvoiceStatus.paid &&
      
                p.ServiceCrmId == request.ServiceTypeId
                )
                ).ToList();

            return new Success<List<ServiceAppointment>>() { Data = result };

        }

    }
}

