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
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class EditLocationHandler : IRequestHandler<EditLocationCompanyCommand, CommandResponse<object>>
    {
        private readonly ILocationCompanyRepository _locationRepository;
        private readonly ICapacityRepository _capacityRepository;

        public EditLocationHandler(ILocationCompanyRepository locationRepository, ICapacityRepository capacityRepository)
        {
            _locationRepository = locationRepository;
            _capacityRepository = capacityRepository;
        }

        public async Task<CommandResponse<object>> Handle(EditLocationCompanyCommand request, CancellationToken cancellationToken)
        {

            var cap = await _capacityRepository.GetByIdAsync(request.CapacityId);
            var entity = await _locationRepository.GetByIdAsync(request.Id);

            if (entity.CompanyId != request.CompanyId)
            {
                return new NoAcess() { Message = "No Access to Edit!" };
            }

            if (cap==null)
            {
                return new NoAcess() { Message = "Please Select the Capacity!" };
            }

            entity.Capacity = cap.Qty;
            entity.Comment = request.Comment;
            entity.CompanyId = request.CompanyId;
            entity.Lat = request.Lat;
            entity.Long = request.Long;
            entity.Name = request.Name;
            entity.PrimaryFirstName = request.PrimaryFirstName;
            entity.PrimaryLastName = request.PrimaryLastName;
            entity.PrimaryPhonNumber = request.PrimaryPhonNumber;
            entity.Type = (int)request.Type;
            entity.CapacityId = request.CapacityId;
            entity.Address = request.Address??"";

            await _locationRepository.UpdateAsync(entity);


            return new Success<object>() { Data = entity, Message = "Location updated successfully!" };
        }

    }
}
