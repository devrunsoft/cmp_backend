using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;


namespace CMPNatural.Application
{
    public class AdminGetManifestCommand : IRequest<CommandResponse<ManifestResponse>>
    {
        public AdminGetManifestCommand()
        {
        }
        public long Id { get; set; }
    }
}

