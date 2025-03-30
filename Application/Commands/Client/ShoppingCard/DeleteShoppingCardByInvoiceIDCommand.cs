using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.ShoppingCard
{

    public class DeleteAllShoppingCardCommand : IRequest<CommandResponse<object>>
    {
        public long CompanyId { get; set; }
    }
}

