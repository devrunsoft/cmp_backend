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
using CMPNatural.Application.Services;
using System.IO;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;

        public UpdateCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task< CommandResponse<object>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company != null)
            {

                company.CompanyName = request.CompanyName;
                company.Position = request.Position;
                company.PrimaryFirstName = request.PrimaryFirstName;
                company.PrimaryLastName = request.PrimaryLastName;
                company.PrimaryPhonNumber = request.PrimaryPhonNumber;
                company.SecondaryFirstName = request.SecondaryFirstName;
                company.SecondaryLastName = request.SecondaryLastName;
                company.SecondaryPhoneNumber = request.SecondaryPhoneNumber;

                await _companyRepository.UpdateAsync(company);
                return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
            }

            return new NoAcess();
        }

    }
}
