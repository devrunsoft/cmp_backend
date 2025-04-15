using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CMPNatural.Core.Enums;
using System.Linq;

namespace CMPNatural.Application
{
    public class ProviderGetAllActiveManifestHandler : IRequestHandler<ProviderGetAllActiveManifestCommand, CommandResponse<List<Manifest>>>
    {
        private readonly IManifestRepository _repository;

        public ProviderGetAllActiveManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<List<Manifest>>> Handle(ProviderGetAllActiveManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ProviderId == request.ProviderId && p.Status != ManifestStatus.Completed
            && p.Status != ManifestStatus.Canceled

            , query => query
            .Include(x => x.Invoice)
            .ThenInclude(x => x.Company))).ToList();
            return new Success<List<Manifest>>() { Data = result };
        }
    }
}

