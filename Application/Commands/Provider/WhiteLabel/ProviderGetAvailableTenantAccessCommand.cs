using System.Collections.Generic;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider.WhiteLabel
{
    public class ProviderGetAvailableTenantAccessCommand : IRequest<CommandResponse<List<NameAndValue<long>>>>
    {
    }
}
