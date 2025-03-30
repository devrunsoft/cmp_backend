using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Service
{
    public class GetProductPriceCommand : IRequest<CommandResponse<List<ProductPrice>>>
    {
        public long ProductId { get; set; }
    }
}

