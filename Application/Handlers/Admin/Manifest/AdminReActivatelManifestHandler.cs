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
    public class AdminReActivatelManifestHandler : IRequestHandler<AdminReActivatelManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _repository;

        public AdminReActivatelManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<Manifest>> Handle(AdminReActivatelManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.Id , query => query.Include(x=>x.Invoice))).FirstOrDefault();

            if (result.ProviderId == null)
            {
                result.Status = result.Invoice.Status == InvoiceStatus.Scaduled ? ManifestStatus.Scaduled : ManifestStatus.Draft;
            }
            else
            {
                result.Status = ManifestStatus.Assigned;
            }

            await _repository.UpdateAsync(result);

            return new Success<Manifest>() { Data = result };
        }
    }
}

