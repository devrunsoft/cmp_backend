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
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class GetAllBaseServiceAppointmentHandler : IRequestHandler<GetAllBaseServiceAppointmentCommand,
        CommandResponse<List<BaseServiceAppointment>>>
    {
        private readonly IBaseServiceAppointmentRepository _serviceAppointmentRepository;

        public GetAllBaseServiceAppointmentHandler(IBaseServiceAppointmentRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<List<BaseServiceAppointment>>> Handle(GetAllBaseServiceAppointmentCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetAsync(
                (p) =>
                p.CompanyId == request.CompanyId &&
                p.Invoice.Status == InvoiceStatus.Processing_Provider,
                query => query.Include(i => i.Product).Include(i => i.ProductPrice)
                )
                ).ToList();

            return new Success<List<BaseServiceAppointment>>() { Data = result };

        }

    }
}

