using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Collections.Generic;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{

    public class DriverUploadBeforServiceAppointmentLocationFileHandler : IRequestHandler<DriverUploadBeforServiceAppointmentLocationFileCommand, CommandResponse<List<ServiceAppointmentLocationFile>>>
    {
        private readonly IServiceAppointmentLocationFileRespository _repository;
        private readonly IRouteServiceAppointmentLocationRepository routeServiceAppointmentLocationRepository;

        public DriverUploadBeforServiceAppointmentLocationFileHandler(IServiceAppointmentLocationFileRespository repository,IRouteServiceAppointmentLocationRepository routeServiceAppointmentLocationRepository)
        {
            _repository = repository;
            this.routeServiceAppointmentLocationRepository = routeServiceAppointmentLocationRepository;
        }

        public async Task<CommandResponse<List<ServiceAppointmentLocationFile>>> Handle(DriverUploadBeforServiceAppointmentLocationFileCommand request, CancellationToken cancellationToken)
        {

            var routeresult = (await routeServiceAppointmentLocationRepository.GetAsync(x =>
            x.Route.DriverId == request.DriverId &&
            x.RouteId == request.RouteId &&
            x.ServiceAppointmentLocation.Status == ServiceStatus.Arrived,
            query=> query.Include(x=>x.Route)
            .Include(x=>x.ServiceAppointmentLocation)
            )).ToList();

            if(routeresult.Count() == 0)
            {
                return new NoAcess<List<ServiceAppointmentLocationFile>>()
                {
                    Message = "You can upload 'before work' photos only after you ARRIVE at this location " +
                        "and if the stop belongs to you."
                };
            }

            foreach (var item in routeresult)
            {

                List<ServiceAppointmentLocationFile> serviceAppointmentLocationFiles = new List<ServiceAppointmentLocationFile>
            {

                new ServiceAppointmentLocationFile()
                {
                    ProviderId = request.ProviderId,
                    DriverId = request.DriverId,
                    Link = request.firstPic,
                    RouteId = request.RouteId,
                    ServiceAppointmentLocationId = item.ServiceAppointmentLocationId,

                    Status = ServiceAppointmentLocationFileEnum.Before
                },
                new ServiceAppointmentLocationFile()
                {
                    ProviderId = request.ProviderId,
                    DriverId = request.DriverId,
                    Link = request.secondPic,
                    RouteId = request.RouteId,
                    ServiceAppointmentLocationId = item.ServiceAppointmentLocationId,
                    Status = ServiceAppointmentLocationFileEnum.Before
                },

                new ServiceAppointmentLocationFile()
                {
                    ProviderId = request.ProviderId,
                    DriverId = request.DriverId,
                    Link = request.thirdPic,
                    RouteId = request.RouteId,
                    ServiceAppointmentLocationId = item.ServiceAppointmentLocationId,
                    Status = ServiceAppointmentLocationFileEnum.Before
                },

                new ServiceAppointmentLocationFile()
                {
                    ProviderId = request.ProviderId,
                    DriverId = request.DriverId,
                    Link = request.forthPic,
                    RouteId = request.RouteId,
                    ServiceAppointmentLocationId = item.ServiceAppointmentLocationId,
                    Status = ServiceAppointmentLocationFileEnum.Before
                }
            };

                await _repository.AddRangeAsync(serviceAppointmentLocationFiles);

                item.ServiceAppointmentLocation.Status = ServiceStatus.Photo_Before_Work;

                await routeServiceAppointmentLocationRepository.UpdateAsync(item);

            }

            return new Success<List<ServiceAppointmentLocationFile>>()
            {
                Data = new List<ServiceAppointmentLocationFile>()
            };

        }

    }
}

