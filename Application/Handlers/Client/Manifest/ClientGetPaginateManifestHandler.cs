using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class ClientGetPaginateManifestHandler : IRequestHandler<ClientGetPaginateManifestCommand, CommandResponse<PagesQueryResponse<Manifest>>>
    {
        private readonly IManifestRepository _repository;

        public ClientGetPaginateManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Manifest>>> Handle(ClientGetPaginateManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p=>
            (p.Status == ManifestStatus.Assigned || p.Status == ManifestStatus.Completed
            || p.Status == ManifestStatus.Processing || p.Status == ManifestStatus.Send_To_Admin)
            &&
            p.CompanyId == request.CompanyId
            && (request.Status == null || p.Status == request.Status)
            && p.ProviderId !=null, query => query
            .Include(x => x.Invoice)
            .ThenInclude(x => x.Company)
             .Include(x => x.Invoice)
            .ThenInclude(x => x.Provider)
            ));
            return new Success<PagesQueryResponse<Manifest>>() { Data = result };
        }
    }
}

