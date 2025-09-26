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
using CMPFile;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class UploadProfilePictureHandler : IRequestHandler<UploadProfilePictureCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IFileStorage fileStorage;

        public UploadProfilePictureHandler(ICompanyRepository companyRepository, IFileStorage fileStorage)
        {
            _companyRepository = companyRepository;
            this.fileStorage = fileStorage;
        }

        public async Task<CommandResponse<object>> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company != null)
            {
                string ProfilePicture = await fileStorage.AppfileHandler(request.ProfilePicture);
                company.ProfilePicture = ProfilePicture;

                await _companyRepository.UpdateAsync(company);
                return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
            }

            return new NoAcess();
        }

    }
}
