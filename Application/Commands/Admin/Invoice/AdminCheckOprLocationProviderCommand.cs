using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application.Commands.Admin.Invoice
{
    public class AdminCheckOprLocationProviderCommand : IRequest<CommandResponse<List<Provider>>>
    {
        public AdminCheckOprLocationProviderCommand()
        {
        }
        public long OperationalAddressId { get; set; }
        public long ProductId { get; set; }
    }
}

