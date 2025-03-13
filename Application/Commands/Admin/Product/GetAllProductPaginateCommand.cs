using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class GetAllProductPaginateCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<Product>>>
    {
		public GetAllProductPaginateCommand()
		{
		}

		public bool? Enable { get; set; }
		public ServiceType? ServiceType { get; set; }
        public ProductCollection? Type { get; set; }
    }
}

