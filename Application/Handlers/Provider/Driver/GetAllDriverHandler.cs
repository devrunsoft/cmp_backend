using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application
{
    public class GetAllDriverHandler : IRequestHandler<GetAllDriverCommand, CommandResponse<List<DriverResponse>>>
    {
        private readonly IDriverRepository _repository;
        public GetAllDriverHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<DriverResponse>>> Handle(GetAllDriverCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p=>p.ProviderId== request.ProviderId,
                query => query.Include(x => x.Person))).ToList();

            return new Success<List<DriverResponse>>() { Data = result.Select(x=> DriverMapper.Mapper.Map<DriverResponse>(x)).ToList() };
        }
    }
}

