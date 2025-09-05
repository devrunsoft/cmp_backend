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
    public class ClientGetManifestHandler : IRequestHandler<ClientGetManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _repository;

        public ClientGetManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<Manifest>> Handle(ClientGetManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.Id, query=> query
            .Include(x=>x.Invoice)
            .ThenInclude(x=>x.BaseServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.Invoice)
            .ThenInclude(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.Product)

            .Include(x => x.Invoice)
            .ThenInclude(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ServiceAppointmentLocations)
            .ThenInclude(x => x.LocationCompany)


            .Include(x => x.Invoice)
            .ThenInclude(x=>x.Company)

            .Include(x => x.Provider)
            )).FirstOrDefault();
            return new Success<Manifest>() { Data = result };
        }
    }
}

