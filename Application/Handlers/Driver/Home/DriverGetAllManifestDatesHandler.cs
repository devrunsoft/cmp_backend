using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Commands.Driver.Home;
using CMPNatural.Application.Responses.Driver;
using System.Collections.Generic;

namespace CMPNatural.Application
{

    public class DriverGetAllManifestDatesHandler : IRequestHandler<DriverGetAllManifestDatesCommand, CommandResponse<List<ManifestDatesResponse>>>
    {
        private readonly IDriverManifestRepository _repository;

        public DriverGetAllManifestDatesHandler(IDriverManifestRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<ManifestDatesResponse>>> Handle(DriverGetAllManifestDatesCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsync(
                p => p.DriverId == request.DriverId &&
                p.Manifest.ServiceDateTime!=null &&
                p.Manifest.ServiceDateTime.Value.Month == request.Date.Month &&
                p.Manifest.ServiceDateTime.Value.Year == request.Date.Year,
                query => query.Include(x => x.Manifest)
            );

            var grouped = result
                .GroupBy(x => x.Manifest.ServiceDateTime.Value.Date) // group by day
                .Select(g => new ManifestDatesResponse
                {
                    Date = g.Key,
                    DriverManifests = g.ToList(),
                    Count = g.Count()
                })
                .ToList();

            return new Success<List<ManifestDatesResponse>>() {
                Data = grouped
            };

        }

    }
}

