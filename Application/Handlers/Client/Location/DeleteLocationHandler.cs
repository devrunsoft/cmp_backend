using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Barbara.Application.Responses;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class DeleteLocationHandler : IRequestHandler<DeleteLocationCommand, CommandResponse<object>>
    {
        private readonly ILocationCompanyRepository _locationRepository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        private readonly IRouteServiceAppointmentLocationRepository _routeServiceAppointmentLocationRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly IManifestGreaseServiceDetailRepository _manifestGreaseServiceDetailRepository;

        public DeleteLocationHandler(
            ILocationCompanyRepository locationRepository,
            IServiceAppointmentLocationRepository serviceAppointmentLocationRepository,
            IRouteServiceAppointmentLocationRepository routeServiceAppointmentLocationRepository,
            IManifestRepository manifestRepository,
            IManifestGreaseServiceDetailRepository manifestGreaseServiceDetailRepository)
        {
            _locationRepository = locationRepository;
            _serviceAppointmentLocationRepository = serviceAppointmentLocationRepository;
            _routeServiceAppointmentLocationRepository = routeServiceAppointmentLocationRepository;
            _manifestRepository = manifestRepository;
            _manifestGreaseServiceDetailRepository = manifestGreaseServiceDetailRepository;
        }

        public async Task<CommandResponse<object>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _locationRepository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return new NoAcess() { Message = "Location not found!" };
            }

            if (entity.CompanyId != request.CompanyId)
            {
                return new NoAcess() { Message = "No Access to delete!" };
            }

            var serviceAppointmentLocations = (await _serviceAppointmentLocationRepository
                .GetAsync(x => x.LocationCompanyId == request.Id))
                .ToList();

            if (serviceAppointmentLocations.Any())
            {
                var serviceAppointmentLocationIds = serviceAppointmentLocations
                    .Select(x => x.Id)
                    .ToList();

                var routeLinks = await _routeServiceAppointmentLocationRepository
                    .GetAsync(x => serviceAppointmentLocationIds.Contains(x.ServiceAppointmentLocationId));
                if (routeLinks.Any())
                {
                    return new NoAcess() { Message = "This location is used in route items and cannot be deleted." };
                }

                var manifests = await _manifestRepository
                    .GetAsync(x => serviceAppointmentLocationIds.Contains(x.ServiceAppointmentLocationId));
                if (manifests.Any())
                {
                    return new NoAcess() { Message = "This location is used in manifests and cannot be deleted." };
                }

                var greaseDetails = await _manifestGreaseServiceDetailRepository
                    .GetAsync(x => serviceAppointmentLocationIds.Contains(x.ServiceAppointmentLocationId));
                if (greaseDetails.Any())
                {
                    return new NoAcess() { Message = "This location is used in service details and cannot be deleted." };
                }

                return new NoAcess() { Message = "This location is used in service appointments and cannot be deleted." };
            }

            await _locationRepository.DeleteAsync(entity);

            return new Success<object>() { Data = entity, Message = "Location deleted successfully!" };
        }
    }
}
