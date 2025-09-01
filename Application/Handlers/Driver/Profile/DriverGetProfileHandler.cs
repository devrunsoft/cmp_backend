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

namespace CMPNatural.Application
{
    public class DriverGetProfileHandler : IRequestHandler<DriverGetProfileCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        public DriverGetProfileHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(DriverGetProfileCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.DriverId , 
            query => query.Include(x => x.Person))).FirstOrDefault();
            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(result) };
        }
    }
}

