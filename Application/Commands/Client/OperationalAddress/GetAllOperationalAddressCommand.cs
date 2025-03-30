using System;
using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class GetAllOperationalAddressCommand : IRequest<CommandResponse<List<OperationalAddress>>>
    {
        public GetAllOperationalAddressCommand()
        {
        }

        public long CompanyId { get; set; }
        public long? OperationalAddressId { get; set; }
    }
}

