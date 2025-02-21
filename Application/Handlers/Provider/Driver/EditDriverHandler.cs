using System;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class EditDriverHandler : IRequestHandler<EditDriverCommand, CommandResponse<Driver>>
    {
        private readonly IDriverRepository _repository;

        public EditDriverHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Driver>> Handle(EditDriverCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            if(entity.ProviderId != request.ProviderId)
            {
                return new NoAcess<Driver>() { Message = "No Access to Edit!" };
            }
            string License=null;
            string BackgroundCheck = null;
            string ProfilePhoto = null;

            var path = Guid.NewGuid().ToString();

            if(request.License!=null)
             License = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.License, "License", request.ProviderId, path);

            if (request.BackgroundCheck != null)
                BackgroundCheck = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.BackgroundCheck, "BackgroundCheck", request.ProviderId, path);

            if (request.ProfilePhoto != null)
                ProfilePhoto = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.ProfilePhoto, "ProfilePhoto", request.ProviderId, path);

            if (License != null)
                entity.License = License;
            entity.LicenseExp = request.LicenseExp;

            if (BackgroundCheck != null)
                entity.BackgroundCheck = BackgroundCheck;

            entity.BackgroundCheckExp = request.BackgroundCheckExp;

            if(ProfilePhoto!=null)
            entity.ProfilePhoto = ProfilePhoto;

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            //entity.ProviderId = request.ProviderId;

            await _repository.UpdateAsync(entity);

            return new Success<Driver>() { Data = entity };
        }
    }
}

