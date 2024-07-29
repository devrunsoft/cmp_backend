using Barbara.Core.Entities;
using Barbara.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Entities;
using ScoutDirect.Application.Responses;

using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Repositories;
using CMPNatural.Application.Commands.OperationalAddress;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class GetOperationalAddressHandler : IRequestHandler<GetOperationalAddressCommand, CommandResponse<object>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;

        public GetOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository)
        {
            _operationalAddressRepository = operationalAddressRepository;
        }

        public async Task<CommandResponse<object>> Handle(GetOperationalAddressCommand request, CancellationToken cancellationToken)
        {
            
            OperationalAddress result = (await _operationalAddressRepository.GetAsync(p=>p.CompanyId==request.CompanyId)).FirstOrDefault();


            return new Success<object>() {  Data = result };
        }

    }
}
