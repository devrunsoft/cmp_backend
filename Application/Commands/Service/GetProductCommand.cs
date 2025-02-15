using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application.Commands.Service
{
    public class GetProductCommand : IRequest<CommandResponse<Product>>
    {
        public long ProductId { get; set; }
    }
}

