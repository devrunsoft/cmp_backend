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

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddLocationHandler : IRequestHandler<AddLocationCompanyCommand, CommandResponse<object>>
    {
        private readonly ILocationCompanyRepository _locationRepository;

        public AddLocationHandler(ILocationCompanyRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<CommandResponse<object>> Handle(AddLocationCompanyCommand request, CancellationToken cancellationToken)
        {

            var entity = new LocationCompany()
            {
                Capacity= request.Capacity,
                Comment= request.Comment,
                CompanyId= request.CompanyId,
                Lat= request.Lat,
                Long= request.Long,
                Name=request.Name,
                PrimaryFirstName= request.PrimaryFirstName,
                PrimaryLastName= request.PrimaryLastName,
                PrimaryPhonNumber= request.PrimaryPhonNumber,
                Type= (int) request.Type,
                OperationalAddressId = request.OperationalAddressId

            };

            var result= await _locationRepository.AddAsync(entity);

            return new CommandResponse<object>() { Success=true,Data= result, Message="Location added successfully!" };
        }

    }
}
