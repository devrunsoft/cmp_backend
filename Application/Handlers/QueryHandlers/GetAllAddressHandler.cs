using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllAddressHandler : IRequestHandler<GetAllAddressQuery, AddressResponse>
    {
        private readonly IAddressRepository _AddressRepo;

        public GetAllAddressHandler(IAddressRepository AddressRepository)
        {
            _AddressRepo = AddressRepository;
        }

        public async Task<AddressResponse> Handle(GetAllAddressQuery request, CancellationToken cancellationToken)
        {
            Address Result;

            if (request.ShopId != null)
                Result = await _AddressRepo.GetAddressByShopIdAsync((int)request.ShopId);
            else
                Result = await _AddressRepo.GetAddressByIdAsync((int)request.Id);

            if (Result == null)
                return null;    

            return AddressMapper.Mapper.Map<AddressResponse>(Result);
        }
    }
}