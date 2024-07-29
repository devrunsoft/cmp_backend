using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Enums;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class RegisterCustomerHandler : IRequestHandler<RegisterCustomerCommand, CommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPersonRepository _personRepository;

        public RegisterCustomerHandler(ICustomerRepository customerRepository, IPersonRepository personRepository)
        {
            _customerRepository = customerRepository;
            _personRepository = personRepository;
        }

        public async Task<CommandResponse> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync((long)request.PersonId);

            if (customer == null)
            {
                customer = new Customer()
                {
                    Id = (long)request.PersonId,
                    IntroducerMobile = request.IntroducerMobile,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                };

                await _customerRepository.AddAsync(customer);
            }

            var person = await _personRepository.GetByIdAsync((long)request.PersonId);

            if (person != null)
            {
                person.Name = request.Name;
                await _personRepository.UpdateAsync(person);
            }


            return new Success() { Id = customer.Id };
        }
    }
}
