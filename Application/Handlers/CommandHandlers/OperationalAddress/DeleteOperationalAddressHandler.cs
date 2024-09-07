//using Barbara.Core.Entities;
//using Barbara.Application.Responses;
//using MediatR;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using ScoutDirect.Core.Repositories;
//using ScoutDirect.Core.Entities;
//using ScoutDirect.Application.Responses;

//using CMPNatural.Application.Responses;
//using CMPNatural.Core.Entities;
//using CMPNatural.Application.Mapper;
//using CMPNatural.Application.Commands;
//using CMPNatural.Core.Repositories;
//using CMPNatural.Application.Commands.OperationalAddress;
//using System.ComponentModel.Design;
//using System.Diagnostics.Metrics;
//using System.Net;

//namespace CMPNatural.Application.Handlers.CommandHandlers
//{
//    public class DeleteOperationalAddressHandler : IRequestHandler<DeleteOperationalAddressCommand, CommandResponse<object>>
//    {
//        private readonly IOperationalAddressRepository _operationalAddressRepository;

//        public DeleteOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository)
//        {
//            _operationalAddressRepository = operationalAddressRepository;
//        }

//        public async Task<CommandResponse<object>> Handle(DeleteOperationalAddressCommand request, CancellationToken cancellationToken)
//        {

//            var entity = await _operationalAddressRepository.GetByIdAsync(request.Id);

//            if (entity.CompanyId != request.CompanyId)
//            {
//                return new NoAcess() { Message = "No Access to delete!" };
//            }
//            await _operationalAddressRepository.DeleteAsync(entity);

//            return new CommandResponse<object>() { Success = true, Data = entity, Message = "OperationalAddres deleted successfully!" };
//        }

//    }
//}
