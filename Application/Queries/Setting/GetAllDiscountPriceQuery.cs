using Bazaro.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllDiscountPriceQuery : IRequest<List<DiscountPriceResponse>>
    {
    }
}
