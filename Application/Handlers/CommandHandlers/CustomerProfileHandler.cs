using Bazaro.Application.Commands;
using Bazaro.Application.Responses;
using Bazaro.Application.Responses.Base;
using BazaroApp.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class CustomerProfileHandler : IRequestHandler<CustomerProfileCommand, CustomerProfileResponse>
    {
        private readonly ICustomerProfileRepository _customerProfileRepository;

        public CustomerProfileHandler(ICustomerProfileRepository customerProfileRepository)
        {
            _customerProfileRepository = customerProfileRepository;
        }

        public Task<CustomerProfileResponse> Handle(CustomerProfileCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CustomerProfileResponse() { });
        }

    }

}
