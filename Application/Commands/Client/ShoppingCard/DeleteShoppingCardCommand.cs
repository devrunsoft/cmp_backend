using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class DeleteShoppingCardCommand : IRequest<CommandResponse<object>>
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
    }
}

