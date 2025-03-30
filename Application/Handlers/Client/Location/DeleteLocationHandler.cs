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
    public class DeleteLocationHandler : IRequestHandler<DeleteLocationCommand, CommandResponse<object>>
    {
        private readonly ILocationCompanyRepository _locationRepository;

        public DeleteLocationHandler(ILocationCompanyRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<CommandResponse<object>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {

            var entity = await _locationRepository.GetByIdAsync(request.Id);

            if (entity.CompanyId != request.CompanyId)
            {
                return new NoAcess() { Message = "No Access to delete!" };
            }

            await _locationRepository.DeleteAsync(entity);

            return new Success<object>() { Data = entity, Message = "Location deleted successfully!" };
        }

    }
}
