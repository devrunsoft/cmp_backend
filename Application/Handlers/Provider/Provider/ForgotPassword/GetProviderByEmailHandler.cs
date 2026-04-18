using System;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Core.Repositories;

namespace CMPNatural.Application.Handlers
{

    public class GetProviderByEmailHandler : IRequestHandler<GetProviderByEmailCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _companyRepository;

        public GetProviderByEmailHandler(IProviderReposiotry companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<Provider>> Handle(GetProviderByEmailCommand request, CancellationToken cancellationToken)
        {
            var company = (await _companyRepository.GetAsync(x => x.Email == request.Email)).FirstOrDefault();

            if (company == null)
            {
                return new NoAcess<Provider>() { };
            }
            return new Success<Provider>() { Data = (company) };
        }
    }
}

