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
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminGetCompletingManifestHandler : IRequestHandler<AdminGetCompletingManifestCommand, CommandResponse<List<Manifest>>>
    {
        private readonly IManifestRepository _repository;

        public AdminGetCompletingManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<List<Manifest>>> Handle(AdminGetCompletingManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.CompanyId == request.CompanyId && p.OperationalAddressId == request.OperationalAddressId &&
            p.Status == ManifestStatus.Send_To_Admin
            || p.Status == ManifestStatus.Send_To_Provider
            || p.Status == ManifestStatus.Assigned_To_Driver,
                query => query

            .Include(x => x.OperationalAddress)
            .Include(x => x.Company)
            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.LocationCompany)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.Product)

            .Include(x => x.Request)
            )).ToList();

            return new Success<List<Manifest>>() { Data = result };
        }
    }
}

