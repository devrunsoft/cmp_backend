using System;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMPNatural.Application
{

    public class DriverCompleteServiceAppointmentLocationHandler : IRequestHandler<DriverCompleteServiceAppointmentLocationCommand, CommandResponse<List<ServiceAppointmentLocation>>>
    {
        private readonly IServiceAppointmentLocationFileRespository _repository;
        private readonly IRouteServiceAppointmentLocationRepository routeServiceAppointmentLocationRepository;

        public DriverCompleteServiceAppointmentLocationHandler(IServiceAppointmentLocationFileRespository repository, IRouteServiceAppointmentLocationRepository routeServiceAppointmentLocationRepository)
        {
            _repository = repository;
            this.routeServiceAppointmentLocationRepository = routeServiceAppointmentLocationRepository;
        }

        public async Task<CommandResponse<List<ServiceAppointmentLocation>>> Handle(DriverCompleteServiceAppointmentLocationCommand request, CancellationToken cancellationToken)
        {

            var routeresult = (await routeServiceAppointmentLocationRepository.GetAsync(x =>
            x.Route.DriverId == request.DriverId &&
            x.RouteId == request.RouteId &&
            x.ServiceAppointmentLocation.Status == ServiceStatus.Photo_Before_Work,
            query => query.Include(x => x.Route)
            .Include(x => x.ServiceAppointmentLocation)
            )).ToList();

            if (routeresult.Count() == 0)
            {
                return new NoAcess<List<ServiceAppointmentLocation>>()
                {
                    Message = "You can upload 'before work' photos only after you ARRIVE at this location " +
                        "and if the stop belongs to you."
                };
            }

            foreach (var item in routeresult)
            {
                var i = request.Services.FirstOrDefault(x=>x.Id == item.ServiceAppointmentLocation.Id);
                item.ServiceAppointmentLocation.Status = ServiceStatus.Driver_Update_Service;
                item.ServiceAppointmentLocation.FactQty = i.FactQty;
                item.ServiceAppointmentLocation.OilQuality = i.OilQuality;
                item.ServiceAppointmentLocation.Comment = request.Comment;
                await routeServiceAppointmentLocationRepository.UpdateAsync(item);

            }

            return new Success<List<ServiceAppointmentLocation>>()
            {
                Data =  routeresult.Select(x=>x.ServiceAppointmentLocation).ToList()
            };

        }

    }
}

