using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllTarrifQuery : /*PagedQueryRequest ,*/ IRequest<List<TariffResponse>>
    {
        public int ShopId { get; set; }
    }
}
