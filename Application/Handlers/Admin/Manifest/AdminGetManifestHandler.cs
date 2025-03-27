using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminGetManifestHandler : IRequestHandler<AdminGetManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _repository;

        public AdminGetManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<Manifest>> Handle(AdminGetManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.Id, query=> query
            .Include(x=>x.Invoice)
            .ThenInclude(x=>x.Company)
            )).FirstOrDefault();
            return new Success<Manifest>() { Data = result };
        }
    }
}

