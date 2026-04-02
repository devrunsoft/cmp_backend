using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using ScoutDirect.Core.Entities.Base;
using CMPNatural.Application.Commands.Provider.Driver;

namespace CMPNatural.Application
{
    public class CheckDriverByEmailHandler : IRequestHandler<CheckDriverByEmailCommand, CommandResponse<List<DriverResponse>>>
    {
        private readonly IDriverRepository _repository;
        public CheckDriverByEmailHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<DriverResponse>>> Handle(CheckDriverByEmailCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Email.Contains(request.Email),
            query => query.Include(x => x.Person))).ToList();
            return new Success<List<DriverResponse>>() { Data = result.Select(p=> DriverMapper.Mapper.Map<DriverResponse>(p) ).ToList()};
        }
    }
}

