using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllTarrifCommissionQuery : PagedQueryRequest, IRequest<List<TarrifCommissionResponse>>
    {
    }
}
