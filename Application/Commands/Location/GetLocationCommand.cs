using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class GetLocationCompanyCommand : LocationCompanyInput, IRequest<CommandResponse<List<LocationCompany>>>
    {
        public GetLocationCompanyCommand()
        {
        }

        public long CompanyId { get; set; }

    }
}

