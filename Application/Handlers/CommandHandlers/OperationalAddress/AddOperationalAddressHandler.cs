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

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddOperationalAddressHandler : IRequestHandler<AddOperationalAddressCommand, CommandResponse<object>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;

        public AddOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository)
        {
            _operationalAddressRepository = operationalAddressRepository;
        }

        public async Task<CommandResponse<object>> Handle(AddOperationalAddressCommand request, CancellationToken cancellationToken)
        {

            var entity = new Core.Entities.OperationalAddress()
            {
            Address=request.Address,
            BusinessId=request.BusinessId,
            CompanyId=request.CompanyId,
            County=request.County,
            CrossStreet=request.CrossStreet,
            FirstName=request.FirstName,
            LastName=request.LastName,
            LocationPhone=request.LocationPhone

            };

            var result = await _operationalAddressRepository.AddAsync(entity);


            return new CommandResponse<object>() { Success = true, Data = result, Message = "OperationalAddres Added Successfully!" };
        }

    }
}
