using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
 
namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SetPersonGenderHandler : IRequestHandler<SetPersonGenderCommand, CommandResponse>
    {
        private readonly IPersonRepository _personRepository;

        public SetPersonGenderHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<CommandResponse> Handle(SetPersonGenderCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.PersonId);

            person.Gender = request.Gender;
            await _personRepository.UpdateAsync(person);

            return new Success();
        }
    }

}
