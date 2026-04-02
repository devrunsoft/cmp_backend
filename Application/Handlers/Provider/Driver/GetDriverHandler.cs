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
    public class GetDriverHandler : IRequestHandler<GetDriverCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        public GetDriverHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(GetDriverCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.Id , 
            query => query.Include(x => x.Person))).FirstOrDefault();
            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(result) };
        }
    }
}

