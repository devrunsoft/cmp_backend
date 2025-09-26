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
            var result = (await _repository.GetBasePagedAsync(request,
                x => x.CompanyId == request.CompanyId &&
                (request.OperationalAddressId == 0 || x.OperationalAddressId == request.OperationalAddressId),
            query => query
            .Include(x => x.Invoice)
            .ThenInclude(x => x.Company)
             .Include(x => x.Invoice)
            .ThenInclude(x => x.Provider)
            ));
            return new Success<PagesQueryResponse<Manifest>>() { Data = result };
        }
    }
}

