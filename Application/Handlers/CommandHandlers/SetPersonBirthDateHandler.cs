using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
 
namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SetPersonBirthDateHandler : IRequestHandler<SetPersonBirthDateCommand, CommandResponse>
    {
        private readonly IPersonRepository _personRepository;

        public SetPersonBirthDateHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<CommandResponse> Handle(SetPersonBirthDateCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.PersonId);

            person.Age = request.BirthDate;
            await _personRepository.UpdateAsync(person);

            return new Success();
        }
    }

}
