using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminCancelManifestHandler : IRequestHandler<AdminCancelManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _repository;

        public AdminCancelManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<Manifest>> Handle(AdminCancelManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.Id)).FirstOrDefault();
            result.Status = ManifestStatus.Canceled;
            await _repository.UpdateAsync(result);

            return new Success<Manifest>() { Data = result };
        }
    }
}

