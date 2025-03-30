//using Bazaro.Application.Commands;
//using Bazaro.Application.Mapper;
//using Bazaro.Application.Responses.Base;
//using Bazaro.Core.Entities;
//using Bazaro.Core.Repositories;
//using MediatR;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Bazaro.Application.Handlers.CommandHandlers
//{
//    public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, CommandResponse>
//    {
//        private readonly IAddressRepository _addressRepository;

//        public UpdateAddressHandler(IAddressRepository addressRepository)
//        {
//            _addressRepository = addressRepository;
//        }

//        public async Task<CommandResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
//        {
//            var address = await _addressRepository.GetByIdAsync(request.Id);

//            address.UpdatedAt = System.DateTime.Now;
//            address.CustomerId = request.CustomerId;
//            address.Name  = request.Name;
//            address.BuildingNumber  = request.BuildingNumber;
//            address.Street  = request.Street;
//            address.Unit  = request.Unit;
//            address.Long  = request.Long;
//            address.Lat  = request.Lat;

//            await _addressRepository.UpdateAsync(address);
//            return new Success() { Id = request.Id };
//        }
//    }
//}
