//using Bazaro.Application.Commands;
//using Bazaro.Application.Mapper;
//using Bazaro.Application.Responses;
//using Bazaro.Application.Responses.Base;
//using Bazaro.Core.Entities;
//using Bazaro.Core.Repositories;
//using MediatR;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Bazaro.Application.Handlers.CommandHandlers
//{
//    public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, CommandResponse>
//    {
//        private readonly IAddressRepository _addressRepository;

//        public CreateAddressHandler(IAddressRepository addressRepository)
//        {
//            _addressRepository = addressRepository;
//        }

//        public async Task<CommandResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
//        {
//            var address = InputAddressModelMapper.Mapper.Map<Address>(request);

//            address.CustomerId = request.CustomerId;
//            address.IsActive = true;
//            address.CreatedAt = System.DateTime.Now;

//            var resultModel = await _addressRepository.AddAsync(address);
//            return new Success() { Id = resultModel.Id, Data = AddressMapper.Mapper.Map<AddressResponse>(resultModel) };
//        }
//    }
//}
