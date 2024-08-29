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

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, CompanyResponse>
    {
        private readonly ICompanyRepository _companyRepository;

        public UpdateCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyResponse> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company != null)
            {

                company.AccountNumber = request.AccountNumber;
                //company.BusinessEmail = request.BusinessEmail;
                company.CompanyName = request.CompanyName;
                company.Position = request.Position;
                company.PrimaryFirstName = request.PrimaryFirstName;
                company.PrimaryLastName = request.PrimaryLastName;
                company.PrimaryPhonNumber = request.PrimaryPhonNumber;
                company.ReferredBy = request.ReferredBy;
                company.SecondaryFirstName = request.SecondaryFirstName;
                company.SecondaryLastName = request.SecondaryLastName;
                company.SecondaryPhoneNumber = request.SecondaryPhoneNumber;
                //company.Registered = false;
                company.Type = (int)request.Type;
                company.Password = request.Password;

            

                await _companyRepository.UpdateAsync(company);
                return CompanyMapper.Mapper.Map<CompanyResponse>(company);
            }

            return new CompanyResponse();
        }

    }
}
