using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class EditBilingInformationCommand : BilingInformationInput, IRequest<CommandResponse<object>>
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
    }
}