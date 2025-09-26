using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class ProviderAddRouteCommand : IRequest<CommandResponse<Route>>
    {
        public List<RouteAssignmentCommand> payload { get; set; }
        public long ProviderId { get; set; }
    }
    public class RouteAssignmentCommand
    {
        /// <summary>Client route id (e.g., "r-abc123").</summary>
        public string RouteId { get; set; } = string.Empty;

        /// <summary>Human name shown in UI.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Route date. If you're only storing a date (no time),
        /// you can keep this nullable or map to midnight.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>Selected driver id (nullable until chosen).</summary>
        public int DriverId { get; set; }

        /// <summary>All ServiceAppointmentLocation ids in this route.</summary>
        public List<DnDItem> SalIds { get; set; } = new();

        ///// <summary>Grouping of SAL ids by ManifestId.</summary>
        //public Dictionary<int, List<int>> ByManifest { get; set; } = new();

        ///// <summary>Grouping of SAL ids by BaseServiceId.</summary>
        //public Dictionary<int, List<int>> ByBaseService { get; set; } = new();
    }

    public class DnDItem
    {
        /// <summary>
        /// Stable client id (e.g., "sal-<id>" or generated)
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// ServiceAppointmentLocations.Id (server id)
        /// </summary>
        public long SalId { get; set; }

        /// <summary>
        /// ClientBaseServiceEntity.Id
        /// </summary>
        public int BaseServiceId { get; set; }

        /// <summary>
        /// ManifestEntity.Id
        /// </summary>
        public long ManifestId { get; set; }

        /// <summary>
        /// Base service name or type
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Base service / product code
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Location name
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Location address (optional)
        /// </summary>
        public string? Address { get; set; }
    }
}

