using System;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class GetAllDriverHandler : IRequestHandler<GetAllDriverCommand, CommandResponse<List<Driver>>>
    {
        private readonly IDriverRepository _repository;
        public GetAllDriverHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<Driver>>> Handle(GetAllDriverCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p=>p.ProviderId== request.ProviderId)).ToList();
            return new Success<List<Driver>>() { Data = result };
        }
    }
}

