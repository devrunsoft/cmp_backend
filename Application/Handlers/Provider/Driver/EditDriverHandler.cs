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

namespace CMPNatural.Application
{
    public class EditDriverHandler : IRequestHandler<EditDriverCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        private readonly IProviderDriverRepository _providerDriverRepository;
        private readonly IFileStorage fileStorage;

        public EditDriverHandler(IDriverRepository repository, IProviderDriverRepository providerDriverRepository, IFileStorage fileStorage)
        {
            _repository = repository;
            _providerDriverRepository = providerDriverRepository;
            this.fileStorage = fileStorage;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(EditDriverCommand request, CancellationToken cancellationToken)
        {
            var existingDriver = (await _repository.GetAsync(x => x.Email == request.Email && x.Id != request.Id)).Any();
            if (existingDriver)
            {
                return new NoAcess<DriverResponse>() { Message = "A driver with this email already exists." };
            }

            var entity = (await _repository.GetAsync(
                x => x.Id == request.Id,
                query => query.Include(x => x.Person).Include(x => x.ProviderDriver))).FirstOrDefault();

            var providerDriver = entity?.ProviderDriver.FirstOrDefault(x => x.ProviderId == request.ProviderId && x.DriverId == request.Id);
            if (entity == null || providerDriver == null)
            {
                return new NoAcess<DriverResponse>() { Message = "No Access to Edit!" };
            }

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

            entity.Person.FirstName = request.FirstName;
            entity.Person.LastName = request.LastName;
            entity.Email = request.Email;
            providerDriver.IsDefault = request.IsDefault;

            await ResetOtherProviderDefaultsAsync(request.ProviderId, entity.Id, request.IsDefault);
            await _repository.UpdateAsync(entity);

            var updatedRelation = await LoadProviderDriverAsync(request.ProviderId, entity.Id);

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(updatedRelation) };
        }

        private async Task ResetOtherProviderDefaultsAsync(long providerId, long driverId, bool shouldReset)
        {
            if (!shouldReset)
            {
                return;
            }

            var otherRelations = await _providerDriverRepository.GetAsync(x => x.ProviderId == providerId && x.DriverId != driverId && x.IsDefault);
            foreach (var relation in otherRelations)
            {
                relation.IsDefault = false;
                await _providerDriverRepository.UpdateAsync(relation);
            }
        }

        private async Task<ProviderDriver> LoadProviderDriverAsync(long providerId, long driverId)
        {
            return (await _providerDriverRepository.GetAsync(
                x => x.ProviderId == providerId && x.DriverId == driverId,
                query => query.Include(x => x.Driver).ThenInclude(x => x.Person))).FirstOrDefault();
        }
    }
}
