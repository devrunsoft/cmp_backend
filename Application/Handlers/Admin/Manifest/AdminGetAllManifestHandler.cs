using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminGetAllManifestHandler : IRequestHandler<AdminGetAllManifestCommand, CommandResponse<PagesQueryResponse<Manifest>>>
    {
        private readonly IManifestRepository _repository;

        public AdminGetAllManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Manifest>>> Handle(AdminGetAllManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p=> request.Status == null || p.Status == request.Status, query => query
            .Include(x => x.Invoice)
            .ThenInclude(x => x.Company)));
            return new Success<PagesQueryResponse<Manifest>>() { Data = result };
        }
    }
}

