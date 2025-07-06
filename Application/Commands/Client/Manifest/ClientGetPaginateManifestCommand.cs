using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class ClientGetPaginateManifestCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<Manifest>>>
    {
		public ClientGetPaginateManifestCommand()
		{
		}
		public long CompanyId { get; set; }
        public long OperationalAddressId { get; set; }
        public ManifestStatus? Status { get; set; }
	}
}