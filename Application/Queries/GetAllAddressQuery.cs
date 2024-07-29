using Bazaro.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllAddressQuery : IRequest<AddressResponse>
    {
        public int? Id { get; set; }
        public int? ShopId { get; set; }
    }
}
