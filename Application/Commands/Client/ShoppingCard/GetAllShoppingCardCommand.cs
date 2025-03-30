using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class GetAllShoppingCardCommand : IRequest<CommandResponse<List<ShoppingCard>>>
    {
        public long CompanyId { get; set; }
    }
}

