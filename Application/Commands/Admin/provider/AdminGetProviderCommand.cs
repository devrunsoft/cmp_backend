using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{ 
    public class AdminGetProviderCommand : IRequest<CommandResponse<Provider>>
    {
        public long Id { get; set; }
    }
}

