using System;
using CMPNatural.Application.Model;
using CMPNatural.Application.Responses.Information;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Billing
{
    public class GetInformationCommand : IRequest<CommandResponse<InfromationResponse>>
    {
        public long CompanyId { get; set; }
    }
}

