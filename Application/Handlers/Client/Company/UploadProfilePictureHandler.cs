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
using CMPNatural.Application.Services;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class UploadProfilePictureHandler : IRequestHandler<UploadProfilePictureCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;

        public UploadProfilePictureHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<object>> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company != null)
            {
                string ProfilePicture = FileHandler.ClientfileHandler(request.Path, request.ProfilePicture, "ProfilePicture", company.Id, request.ProfilePicture.Name);
                company.ProfilePicture = ProfilePicture;

                await _companyRepository.UpdateAsync(company);
                return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
            }

            return new NoAcess();
        }

    }
}
