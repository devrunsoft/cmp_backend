using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class DeleteShoppingCardCommand : IRequest<CommandResponse<ShoppingCard>>
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
    }
}

