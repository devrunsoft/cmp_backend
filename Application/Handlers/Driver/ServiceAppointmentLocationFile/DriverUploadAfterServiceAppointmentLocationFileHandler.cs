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
using CMPNatural.Application.Responses.Driver;
using Stripe.Forwarding;

namespace CMPNatural.Application
{

    public class DriverUploadAfterServiceAppointmentLocationFileHandler : IRequestHandler<DriverUploadAfterServiceAppointmentLocationFileCommand, CommandResponse<ServiceCompletingResponse>>
    {
        private readonly IServiceAppointmentLocationFileRespository _repository;
        private readonly IRouteRepository _routeRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly IRouteServiceAppointmentLocationRepository routeServiceAppointmentLocationRepository;

        public DriverUploadAfterServiceAppointmentLocationFileHandler(IServiceAppointmentLocationFileRespository repository,IRouteServiceAppointmentLocationRepository routeServiceAppointmentLocationRepository,
             IRouteRepository _routeRepository, IManifestRepository _manifestRepository)
        {
            _repository = repository;
            this._routeRepository = _routeRepository;
            this.routeServiceAppointmentLocationRepository = routeServiceAppointmentLocationRepository;
            this._manifestRepository = _manifestRepository;
        }

        public async Task<CommandResponse<ServiceCompletingResponse>> Handle(DriverUploadAfterServiceAppointmentLocationFileCommand request, CancellationToken cancellationToken)
        {

            var routeresult = (await routeServiceAppointmentLocationRepository.GetAsync(x =>
            x.Route.DriverId == request.DriverId &&
            x.RouteId == request.RouteId &&
            x.ServiceAppointmentLocation.Status == ServiceStatus.Driver_Update_Service,
            query=> query.Include(x=>x.Route)
            .Include(x=>x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            )).ToList();

            if(routeresult.Count() == 0)
            {
                return new NoAcess<ServiceCompletingResponse>()
                {
                    Message = "You can upload 'before work' photos only after you complete manifest"
                };
            }

            foreach (var item in routeresult)
            {

                List<ServiceAppointmentLocationFile> serviceAppointmentLocationFiles = new List<ServiceAppointmentLocationFile>
            {

                new ServiceAppointmentLocationFile()
                {
                    ProviderId =request.ProviderId,
                    DriverId = request.DriverId,
                    Link = request.firstPic,
                    RouteId = request.RouteId,
                    ServiceAppointmentLocationId = item.ServiceAppointmentLocationId,
                    Status = ServiceAppointmentLocationFileEnum.After
                },
                new ServiceAppointmentLocationFile()
                {
                    ProviderId =request.ProviderId,
                    DriverId = request.DriverId,
                    Link = request.secondPic,
                    RouteId = request.RouteId,
                    ServiceAppointmentLocationId = item.ServiceAppointmentLocationId,
                    Status = ServiceAppointmentLocationFileEnum.After
                },
                new ServiceAppointmentLocationFile()
                {
                    ProviderId =request.ProviderId,
                    DriverId = request.DriverId,
                    Link = request.thirdPic,
                    RouteId = request.RouteId,
                    ServiceAppointmentLocationId = item.ServiceAppointmentLocationId,
                    Status = ServiceAppointmentLocationFileEnum.After
                },
                new ServiceAppointmentLocationFile()
                {
                    ProviderId =request.ProviderId,
                    DriverId = request.DriverId,
                    Link = request.forthPic,
                    RouteId = request.RouteId,
                    ServiceAppointmentLocationId = item.ServiceAppointmentLocationId,
                    Status = ServiceAppointmentLocationFileEnum.After
                }
            };

                await _repository.AddRangeAsync(serviceAppointmentLocationFiles);

                item.ServiceAppointmentLocation.Status = ServiceStatus.Done_Driver;
                item.ServiceAppointmentLocation.ServiceAppointment.Status = ServiceStatus.Done_Driver;

                await routeServiceAppointmentLocationRepository.UpdateAsync(item);

            }

            var hasAny = (await routeServiceAppointmentLocationRepository.GetAsync(x =>
            x.Route.DriverId == request.DriverId &&
            x.RouteId == request.RouteId &&
            x.ServiceAppointmentLocation.Status != ServiceStatus.Done_Driver,
            query => query.Include(x => x.Route)
            .Include(x => x.ServiceAppointmentLocation)
            )).Any();

            if (!hasAny)
            {
                var route = (await _routeRepository.GetAsync(x => x.Id == request.RouteId)).FirstOrDefault();
                route.Status = RouteStatus.Completed;
                await _routeRepository.UpdateAsync(route);
            }

            await handleManifest(routeresult);

            return new Success<ServiceCompletingResponse>()
            {
                Data = new ServiceCompletingResponse()
                {
                    IsDone = !hasAny,
                    Services = new List<ServiceAppointmentLocation>()
                }
            };

        }

        private async Task handleManifest(List<RouteServiceAppointmentLocation> data)
        {
            foreach (var item in data.GroupBy(x=>x.ManifestId))
            {

                var manifest = (await _manifestRepository.GetAsync(x => x.Id == item.Key,
                    query=> query
                    .Include(x=>x.ServiceAppointmentLocation)
                    .ThenInclude(x=>x.ServiceAppointment)
                )).FirstOrDefault();

                //if (manifest.ServiceAppointmentLocation.ServiceAppointment.Any(x=> x.Status != ServiceStatus.Done_Driver &&
                //x.Status != ServiceStatus.Complete))
                //{
                //    manifest.Status = ManifestStatus.Partially_Completed;
                //}
                //else
                //{
                //    manifest.Status = ManifestStatus.Send_To_Admin;
                //}

                manifest.Status = ManifestStatus.Send_To_Provider;

                await _manifestRepository.UpdateAsync(manifest);

            }
        }

    }
}

