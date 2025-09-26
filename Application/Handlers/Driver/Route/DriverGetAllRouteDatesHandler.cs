using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Responses.Driver;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{

    public class DriverGetAllRouteDatesHandler : IRequestHandler<DriverGetAllRouteDatesCommand, CommandResponse<List<RouteDateResponse>>>
    {
        private readonly IRouteRepository _repository;

        public DriverGetAllRouteDatesHandler(IRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<RouteDateResponse>>> Handle(DriverGetAllRouteDatesCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsync(
                p => p.DriverId == request.DriverId &&
                p.Date.Month == request.Date.Month &&
                p.Date.Year == request.Date.Year,
                query => query
                .Include(x => x.Items)
                .ThenInclude(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.ProductPrice)

                .Include(x => x.Items)
                .ThenInclude(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.LocationCompany)

            );

            var grouped = result
                .Select(g => new RouteDateResponse
                {
                    Id = g.Id,
                    Date = g.Date,
                    Name = g.Name,
                })
                .ToList();

            return new Success<List<RouteDateResponse>>()
            {
                Data = grouped
            };

        }

    }
}

