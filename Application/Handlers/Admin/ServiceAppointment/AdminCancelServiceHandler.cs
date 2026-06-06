using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Handlers.Admin.ServiceAppointment
{

    //TODO
    public class AdminCancelServiceHandler : IRequestHandler<AdminCancelServiceCommand, CommandResponse<BaseServiceAppointment>>
    {
        private readonly IBaseServiceAppointmentRepository _invoiceRepository;

        public AdminCancelServiceHandler(IBaseServiceAppointmentRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<BaseServiceAppointment>> Handle(AdminCancelServiceCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetAsync( x => x.Id == request.ServiceAppointmentId,
                query => query.Include(i => i.ServiceAppointmentLocations)));


            return new Success<BaseServiceAppointment>() { };
        }
    }
}

