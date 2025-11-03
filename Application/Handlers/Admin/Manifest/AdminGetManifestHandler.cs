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
using CMPNatural.Application.Responses;
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application
{
    public class AdminGetManifestHandler : IRequestHandler<AdminGetManifestCommand, CommandResponse<ManifestResponse>>
    {
        private readonly IManifestRepository _repository;

        public AdminGetManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<ManifestResponse>> Handle(AdminGetManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.Id, query=> query

            .Include(x => x.Request)
            .ThenInclude(x=>x.OperationalAddress)

            .Include(x => x.Request)
            .ThenInclude(x => x.BillingInformation)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.LocationCompany)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.Product)


            .Include(x => x.Request)
            .ThenInclude(x=>x.Company)
            .Include(x => x.Provider)
            )).FirstOrDefault();

            return new Success<ManifestResponse>() { Data = ManifestMapper.Mapper.Map<ManifestResponse>(result) };
        }
    }
}

