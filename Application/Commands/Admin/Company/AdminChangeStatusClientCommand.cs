using System;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Base;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands.Admin.Company
{
    public class AdminChangeStatusClientCommand : IRequest<CommandResponse<CompanyResponse>>
    {
        public long ClientId { get; set; }
        public CompanyStatus Status { get; set; }
    }
}

