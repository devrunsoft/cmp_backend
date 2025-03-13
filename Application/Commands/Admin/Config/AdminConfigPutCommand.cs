using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminConfigPutCommand : IRequest<CommandResponse<Config>>
    {
        public AdminConfigPutCommand()
        {
        }
        public string TermAndCondition { get; set; }
    }
}

