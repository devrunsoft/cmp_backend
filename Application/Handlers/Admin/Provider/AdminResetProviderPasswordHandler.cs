using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminResetProviderPasswordHandler : IRequestHandler<AdminResetProviderPasswordCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IPersonRepository _personRepository;

        public AdminResetProviderPasswordHandler(IProviderReposiotry providerReposiotry, IPersonRepository _personRepository)
        {
            _providerReposiotry = providerReposiotry;
            this._personRepository = _personRepository;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminResetProviderPasswordCommand request, CancellationToken cancellationToken)
        {

            var result = (await _providerReposiotry.GetAsync(x => x.Id == request.ProviderId)).FirstOrDefault();

            result.Password = request.Password;

            await _providerReposiotry.UpdateAsync(result);

            return new Success<Provider>() { Data = result };

        }
    }
}

