using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class DeleteOperationalAddressCommand : IRequest<CommandResponse<OperationalAddress>>
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
    }
}

