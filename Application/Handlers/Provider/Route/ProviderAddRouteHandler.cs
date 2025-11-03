using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Linq;
using System.Collections.Generic;
using CMPNatural.Application.Commands.Provider.Biling;
using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CMPNatural.Application
{
    public class ProviderAddRouteHandler : IRequestHandler<ProviderAddRouteCommand, CommandResponse<Route>>
    {
        private readonly IRouteRepository _repository;
        private readonly IManifestRepository _manifestRepository;

        public ProviderAddRouteHandler(IRouteRepository repository, IManifestRepository manifestRepository)
        {
            _repository = repository;
            _manifestRepository = manifestRepository;
        }

        public async Task<CommandResponse<Route>> Handle(ProviderAddRouteCommand request, CancellationToken cancellationToken)
        {
            // Assumptions about shapes:
            // request.payload : IEnumerable<RouteItemDto>
            // RouteItemDto.SalIds : IEnumerable<SalRefDto> where SalRefDto has { int SalId; int ManifestId; }

            // 1) Gather all referenced manifestIds & per-manifest SAL ids from payload
            var payload = request.payload;
            var payloadByManifest = payload
                .SelectMany(r => r.manifestIds);

            var allManifestIds = payloadByManifest.ToList();

            // 2) Load all manifests (and their SALs) in one shot
            var manifests = await _manifestRepository.GetAsync(
                m => allManifestIds.Contains(m.Id) && m.ProviderId == request.ProviderId,
                query => query
                    .Include(m => m.ServiceAppointmentLocation)
                    .ThenInclude(inv => inv.ServiceAppointment)
            );
            var manifestMap = manifests.ToDictionary(m => m.Id);

            // ---- Validation bucket ----

            // 3) Check manifests exist & status
            foreach (var manifestId in allManifestIds)
            {
                if (!manifestMap.TryGetValue(manifestId, out var manifest))
                {
                    return new NoAcess<Route>()
                    {
                        Message = $"Manifest with Id {manifestId} was not found."
                    };
                }

                if (manifest.Status != ManifestStatus.Assigned)
                {
                    var number = string.IsNullOrWhiteSpace(manifest.ManifestNumber) ? "(no number)" : manifest.ManifestNumber;

                    return new NoAcess<Route>()
                    {
                        Message = $"Manifest {number} (Id {manifest.Id}) cannot be assigned — current status is '{manifest.Status}'. " +
                        $"Required status: '{ManifestStatus.Assigned}'."
                    };
                }
            }

            // 4) Check completeness (all SALs present for each manifest)
            //foreach (var kvp in payloadByManifest)
            //{
            //    var manifestId = kvp;

            //    if (!manifestMap.TryGetValue(manifestId, out var manifest))
            //        continue; // existence already reported above

            //    // Collect ALL SAL ids on this manifest (across all base services)
            //    var allSalIdsInManifest = (manifest.Invoice?.BaseServiceAppointment)
            //        .SelectMany(bs => bs.ServiceAppointmentLocations)
            //        .Where(sal => sal?.Id != null)
            //        .Select(sal => sal!.Id)
            //        .ToHashSet();

            //    // If you need to filter which SALs are "assignable", do it here:
            //    // e.g. .Where(sal => sal.Status == ServiceStatus.Pending)

            //    // Missing SALs = those in manifest but not in payload
            //    var missing = allSalIdsInManifest.Except(providedSalIds).ToList();
            //    if (missing.Count > 0)
            //    {
            //        var number = string.IsNullOrWhiteSpace(manifest.ManifestNumber) ? "(no number)" : manifest.ManifestNumber;
            //        return new NoAcess<Route>()
            //        {
            //            Message = $"Manifest {number} (Id {manifest.Id}) is incomplete. " +
            //            $"Missing {missing.Count} service location(s): [{string.Join(", ", missing)}]. " +
            //            $"All service locations in a manifest must be included."
            //        };
            //    }
            //}

            // 5) Check for duplicate SALs across routes (each SAL may only be assigned once)
            //var allPayloadSals = payload.SelectMany(r => r.manifestIds).ToList();
            //var duplicateSalIds = allPayloadSals
            //    .Where(g => g.Count() > 1)
            //    .Select(g => g.Key)
            //    .ToList();

            //if (duplicateSalIds.Count > 0)
            //{
            //    return new NoAcess<Route>()
            //    {
            //        Message = $"Duplicate service locations detected across routes: [{string.Join(", ", duplicateSalIds)}]. Each location can be assigned only once.",
            //    };
            //}

            // If any validation failed, return aggregated errors
            //if (errors.Count > 0)
            //{
            //    return new NoAcess<Route>()
            //    {
            //        Message = "Route assignment validation failed.",
            //    };
            //}

            foreach (var manifest in manifests)
            {
                manifest.Status = ManifestStatus.Assigned_To_Driver;
                await _manifestRepository.UpdateAsync(manifest);
            }

            // 7) Persist routes
            foreach (var item in payload)
            {
                var routeEntity = new Route
                {
                    CreateAt = DateTime.Now,
                    Date = item.Date,
                    DriverId = item.DriverId,
                    Name = item.Name,
                    ProviderId = request.ProviderId,
                    Items = (item.manifestIds)
                        .Select(x => new RouteServiceAppointmentLocation
                        {
                            ServiceAppointmentLocationId = manifests.FirstOrDefault(p => p.Id == x).ServiceAppointmentLocationId,
                            ManifestId = x,
                            ManifestNumber = manifests.FirstOrDefault(p=>p.Id == x).ManifestNumber,
                        })
                        .ToList()
                };

                await _repository.AddAsync(routeEntity);
            }

            return new Success<Route>() { Message = "Routes assigned successfully." };
        }

        // DTO placeholders to illustrate shapes (remove if you have real ones)
        public class RouteItemDto
        {
            public string RouteId { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public DateTime? Date { get; set; }
            public int? DriverId { get; set; }
            public IEnumerable<SalRefDto> SalIds { get; set; } = Enumerable.Empty<SalRefDto>();
        }

        public class SalRefDto
        {
            public int SalId { get; set; }
            public int ManifestId { get; set; }
        }
    }
}