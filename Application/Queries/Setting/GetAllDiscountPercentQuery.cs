using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllDiscountPercentQuery : IRequest<List<DiscountPercentResponse>>
    {
    }
}
