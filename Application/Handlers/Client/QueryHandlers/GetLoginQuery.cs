using Barbara.Application.Queries;
using Barbara.Application.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Application.Queries;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Application.Responses;
using ScoutDirect.Application.Responses.Base;
using CMPNatural.Application.Commands;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers.QueryHandlers
{
    public class GetLoginQuery : IRequestHandler<LoginCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _personRepository;

        public GetLoginQuery(ICompanyRepository personRepository)
        {
            _personRepository = personRepository;

        }

        public async Task<CommandResponse<object>> Handle(LoginCompanyCommand request, CancellationToken cancellationToken)
        {
            var person = (await _personRepository.GetAsync(x=>x.BusinessEmail == request.BusinessEmail && x.Password == request.Password)).FirstOrDefault();

            if (person == null)
            {
                return new CommandResponse<object>() { Success = false, Message="Please Register!" };
            }

            if (person.Status == CompanyStatus.Blocked)
            {
                return new CommandResponse<object>() { Success = false, Message = "Your account is currently blocked. Please contact support for assistance." };
            }

            if (person.Password == request.Password) {
                return new CommandResponse<object>() { Success = true, Message = "Login Successfull" , Data = person };
            }

            else
            {
                return new CommandResponse<object>() { Success = false, Message = "Username or Password aren't correct!" };
            }



        }
    }
}
