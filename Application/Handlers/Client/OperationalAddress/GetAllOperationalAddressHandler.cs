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
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography;

namespace CMPNatural.Application.Handlers
{
    public class GetAllOperationalAddressHandler : IRequestHandler<GetAllOperationalAddressCommand, CommandResponse<List<OperationalAddress>>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;

        public GetAllOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository)
        {
            _operationalAddressRepository = operationalAddressRepository;
        }

        public async Task<CommandResponse<List<OperationalAddress>>> Handle(GetAllOperationalAddressCommand request, CancellationToken cancellationToken)
        {
            List<OperationalAddress> result = (await _operationalAddressRepository.GetWithChild(p => p.CompanyId == request.CompanyId
            && (request.OperationalAddressId == null || request.OperationalAddressId == 0 || p.Id == request.OperationalAddressId))).ToList();
            return new Success<List<OperationalAddress>>() { Data = result };
        }

    }
}
