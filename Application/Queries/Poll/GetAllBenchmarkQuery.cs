using Bazaro.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllBenchmarkQuery : IRequest<List<BenchmarkResponse>>
    {
        public string FormKey { get; set; }
    }
}
