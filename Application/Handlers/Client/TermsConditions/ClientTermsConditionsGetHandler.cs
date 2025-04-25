using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Enums;
using System.Linq;

namespace CMPNatural.Application
{
    public class ClientTermsConditionsGetHandler : IRequestHandler<ClientTermsConditionsGetCommand, CommandResponse<string>>
    {
        private readonly ITermsConditionsRepository _repository;
        public ClientTermsConditionsGetHandler(ITermsConditionsRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<string>> Handle(ClientTermsConditionsGetCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(x=>x.Type == TermsConditionsEnum.Client && x.Enable)).LastOrDefault();

            if (entity != null)
            {
                return new Success<string>() { Data =  entity.Content };
            }
            return new Success<string>() { Data = null };
        }
    }
}