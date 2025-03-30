//using Bazaro.Application.Commands;
//using Bazaro.Application.Responses;
//using Bazaro.Application.Responses.Base;
//using Bazaro.Application.Validator;
//using Bazaro.Core.Entities;
//using Bazaro.Core.Repositories;
//using MediatR;
//using ScoutDirect.Application.Responses;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Bazaro.Application.Handlers.CommandHandlers
//{
//    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, CommandResponse>
//    {
//        private readonly IAddressRepository _addressRepository;

//        public DeleteAddressHandler(IAddressRepository addressRepository)
//        {
//            _addressRepository = addressRepository;
//        }

//        public async Task<CommandResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
//        {
//            var address = await _addressRepository.GetByIdAsync(request.Id);
//            address.UpdatedAt = System.DateTime.Now;
//            address.IsActive = false; 
//            await _addressRepository.UpdateAsync(address);
//            return new Success() { Id = request.Id };
//        }
//    }
//}
