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

    public class CreateProviderForgotPasswordLinkHandler : IRequestHandler<CreateProviderForgotPasswordLinkCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _companyRepository;

        public CreateProviderForgotPasswordLinkHandler(IProviderReposiotry companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<Provider>> Handle(CreateProviderForgotPasswordLinkCommand request, CancellationToken cancellationToken)
        {
            var company = (await _companyRepository.GetAsync(x=>x.Email== request.Email)).FirstOrDefault();

            if (company == null)
            {
                return new NoAcess<Provider>() { };
            }

            company.ActivationLink = Guid.NewGuid();
            await _companyRepository.UpdateAsync(company);

            return new Success<Provider>() { Data = (company) };
        }
    }
}

