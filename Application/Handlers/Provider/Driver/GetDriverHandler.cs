using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;

namespace CMPNatural.Application
{
    public class GetDriverHandler : IRequestHandler<GetDriverCommand, CommandResponse<Driver>>
    {
        private readonly IDriverRepository _repository;
        public GetDriverHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Driver>> Handle(GetDriverCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ProviderId == request.ProviderId && p.Id == request.Id)).FirstOrDefault();
            return new Success<Driver>() { Data = result };
        }
    }
}

