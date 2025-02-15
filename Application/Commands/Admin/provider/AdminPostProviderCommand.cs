using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands.Admin.provider
{
    public class AdminPostProviderCommand : ProviderInput , IRequest<CommandResponse<Provider>>
    {

    }
}

