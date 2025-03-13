using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands.Service
{

    public class GetProductPricePaginateCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<ProductPrice>>>
    {
        public GetProductPricePaginateCommand()
        {
        }

        public long ProductId { get; set; }
        public bool? Enable { get; set; }
    }
}

