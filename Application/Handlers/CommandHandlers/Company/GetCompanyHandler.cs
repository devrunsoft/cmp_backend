using Barbara.Core.Entities;
using Barbara.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Entities;
using ScoutDirect.Application.Responses;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Company;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class GetCompanyHandler : IRequestHandler<GetCompanyCommand, CompanyResponse>
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyResponse> Handle(GetCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            return CompanyMapper.Mapper.Map<CompanyResponse>(company);
        }

    }
}
