using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class DriverGetAllManifestCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<Manifest>>>
    {
        public long DriverId { get; set; }

        public ManifestStatus? Status { get; set; }

        public DateTime? startDate { get; set; }

        public DateTime? endDate { get; set; }
    }
}

