//using System;
//using CMPNatural.Application.Commands.Admin.provider;
//using CMPNatural.Core.Entities;
//using CMPNatural.Core.Repositories;
//using MediatR;
//using ScoutDirect.Application.Responses;
//using System.Threading;
//using System.Threading.Tasks;

//namespace CMPNatural.Application.Handlers.Admin.provider
//{
//    public class AdminChangeStatusProviderServiceAssignmentHandler : IRequestHandler<AdminChangeStatusProviderServiceAssignmentCommand, CommandResponse<ProviderServiceAssignment>>
//    {
//        private readonly IProviderServiceAssignmentRepository _providerService;
//        public AdminChangeStatusProviderServiceAssignmentHandler(IProviderServiceAssignmentRepository providerService)
//        {
//            _providerService = providerService;
//        }

//        public async Task<CommandResponse<ProviderServiceAssignment>> Handle(AdminChangeStatusProviderServiceAssignmentCommand request, CancellationToken cancellationToken)
//        {
//            var entity = await _providerService.GetByIdAsync(request.Id);
//            entity.Status = (int)request.Status;
//            await _providerService.UpdateAsync(entity);

//            return new Success<ProviderServiceAssignment>() { Data = entity };

//        }
//    }
//}

