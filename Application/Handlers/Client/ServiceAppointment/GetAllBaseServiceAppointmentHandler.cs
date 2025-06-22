using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Base;
using System.Linq.Expressions;

namespace CMPNatural.Application
{
    public class GetAllBaseServiceAppointmentHandler : IRequestHandler<GetAllBaseServiceAppointmentCommand,
        CommandResponse<PagesQueryResponse<BaseServiceAppointment>>>
    {
        private readonly IBaseServiceAppointmentRepository _serviceAppointmentRepository;

        public GetAllBaseServiceAppointmentHandler(IBaseServiceAppointmentRepository billingInformationRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<BaseServiceAppointment>>> Handle(GetAllBaseServiceAppointmentCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<BaseServiceAppointment, bool>> predicate;

            if (request.Status == LogOfServiceEnum.All_past_services)
            {
                predicate = p =>
                    p.CompanyId == request.CompanyId && p.OperationalAddressId == request.OperationalAddressId &&
                    (p.Status == ServiceStatus.Complete || p.Status == ServiceStatus.Canceled);
            }
            else if (request.Status == LogOfServiceEnum.Next_30_days)
            {
                var now = DateTime.UtcNow;
                var future = now.AddDays(30);
                predicate = p =>
                    p.CompanyId == request.CompanyId && p.OperationalAddressId == request.OperationalAddressId &&
                    //p.Status == ServiceStatus.Scaduled &&
                    p.ScaduleDate >= now && p.ScaduleDate <= future;
            }
            else if (request.Status == LogOfServiceEnum.All_upcoming_services)
            {
                //var now = DateTime.UtcNow;
                //var future = now.AddDays(30);
                predicate = p =>
                    p.CompanyId == request.CompanyId && p.OperationalAddressId == request.OperationalAddressId &&
                    p.Status == ServiceStatus.Scaduled;
                    //&&
                    //p.ScaduleDate >= now && p.ScaduleDate <= future;
            }
            else
            {
                predicate = p => p.CompanyId == request.CompanyId && p.OperationalAddressId == request.OperationalAddressId;
            }

            var result = await _serviceAppointmentRepository.GetBasePagedAsync(
                request,
                predicate,
                query => query
                    .Include(i => i.Product)
                    .Include(i => i.ProductPrice)
            );


            return new Success<PagesQueryResponse<BaseServiceAppointment>>() { Data = result };

        }

    }
}

