using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using CMPFile;
using CMPNatural.Application.Commands.Driver.Profile;

namespace CMPNatural.Application
{
    public class DriverUpdateProfileHandler : IRequestHandler<DriverUpdateProfileCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        private readonly IProviderDriverRepository _providerDriverRepository;
        private readonly IFileStorage fileStorage;

        public DriverUpdateProfileHandler(IDriverRepository repository, IProviderDriverRepository providerDriverRepository, IFileStorage fileStorage)
        {
            _repository = repository;
            _providerDriverRepository = providerDriverRepository;
            this.fileStorage = fileStorage;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(DriverUpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var existingDriver = (await _repository.GetAsync(x => x.Email == request.Email && x.Id != request.DriverId)).Any();
            if (existingDriver)
            {
                return new NoAcess<DriverResponse>() { Message = "A driver with this email already exists." };
            }

            var entity = (await _repository.GetAsync(
                x => x.Id == request.DriverId,
                query => query.Include(x => x.Person).Include(x => x.ProviderDriver))).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(request.ProfilePhoto))
            {
                entity.License = request.ProfilePhoto;
            }
            entity.LicenseExp = request.LicenseExp;

            if (!string.IsNullOrWhiteSpace(request.BackgroundCheck))
            {
                entity.BackgroundCheck = request.BackgroundCheck;
            }
            entity.BackgroundCheckExp = request.BackgroundCheckExp;

            if (!string.IsNullOrWhiteSpace(request.ProfilePhoto))
                entity.ProfilePhoto = request.ProfilePhoto;

            if (!string.IsNullOrWhiteSpace(request.FirstName))
                entity.Person.FirstName = request.FirstName;
            if (!string.IsNullOrWhiteSpace(request.LastName))
                entity.Person.LastName = request.LastName;

            await _repository.UpdateAsync(entity);

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(entity) };
        }

    }
}
