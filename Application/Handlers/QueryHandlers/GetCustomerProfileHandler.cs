using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetCustomerProfileHandler : IRequestHandler<GetCustomerProfileQuery, CustomerProfileResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerProfileHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerProfileResponse> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            var model = await _customerRepository.GetCustomerProfileByIdAsync(request.Id);

            if (model == null)
                return new CustomerProfileResponse();

            var Result = CustomerProfileMapper.Mapper.Map<CustomerProfileResponse>(model);
            Result.Blocked = model.Blockeds.Any(p => p.Id == request.Id && p.ShopId == request.ShopId);
            return Result;
        }
    }
}