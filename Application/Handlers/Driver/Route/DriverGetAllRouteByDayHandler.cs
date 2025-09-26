using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Responses.Driver;
using System.Collections.Generic;
using CMPNatural.Application.Commands.Driver.Route;

namespace CMPNatural.Application
{

    public class DriverGetAllRouteByDayHandler : IRequestHandler<DriverGetAllRouteByDayCommand, CommandResponse<List<RouteDateResponse>>>
    {
        private readonly IRouteRepository _repository;

        public DriverGetAllRouteByDayHandler(IRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<RouteDateResponse>>> Handle(DriverGetAllRouteByDayCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsync(
                p => p.DriverId == request.DriverId &&
                p.Date.Month == request.Date.Month &&
                p.Date.Year == request.Date.Year &&
                p.Date.Day == request.Date.Day
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

