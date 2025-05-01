using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminDropDownContractActiveCommand : IRequest<CommandResponse<List<NameAndValue<Contract>>>>
    {
        public AdminDropDownContractActiveCommand()
        {
        }
    }
}

