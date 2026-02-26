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
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddLocationHandler : IRequestHandler<AddLocationCompanyCommand, CommandResponse<object>>
    {
        private readonly ILocationCompanyRepository _locationRepository;
        private readonly ICapacityRepository _capacityRepository;

        public AddLocationHandler(ILocationCompanyRepository locationRepository, ICapacityRepository capacityRepository)
        {
            _locationRepository = locationRepository;
            _capacityRepository = capacityRepository;
        }

        public async Task<CommandResponse<object>> Handle(AddLocationCompanyCommand request, CancellationToken cancellationToken)
        {
            var cap = await _capacityRepository.GetByIdAsync(request.CapacityId);
            if (request.CapacityId == null || request.CapacityId==0)
            {
                return new NoAcess<object>() { Success = false, Message = "Please select a capacity." };
            }

            if (request.Lat == 0 || request.Long == 0)
            {
                return new NoAcess<object>() { Success = false, Message = "Please search and select your location." };
            }

            var entity = new LocationCompany()
            {
                Capacity= cap.Qty,
                Comment = request.Comment,
                CompanyId= request.CompanyId,
                Lat= request.Lat,
                Long= request.Long,
                Name=request.Name,
                PrimaryFirstName= request.PrimaryFirstName,
                PrimaryLastName= request.PrimaryLastName,
                PrimaryPhonNumber= request.PrimaryPhonNumber,
                Type= (int) request.Type,
                OperationalAddressId = request.OperationalAddressId,
                CapacityId = request.CapacityId,
                Address = request.Address?? "",

            };

            var result= await _locationRepository.AddAsync(entity);

            return new CommandResponse<object>() { Success=true,Data= result, Message="Location added successfully!" };
        }

    }
}
