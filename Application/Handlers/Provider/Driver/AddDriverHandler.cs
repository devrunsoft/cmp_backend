using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Services;
using ScoutDirect.Core.Entities.Base;

namespace CMPNatural.Application
{
    public class AddDriverHandler : IRequestHandler<AddDriverCommand, CommandResponse<Driver>>
    {
        private readonly IDriverRepository _repository;

        public AddDriverHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Driver>> Handle(AddDriverCommand request, CancellationToken cancellationToken)
        {
            var path = Guid.NewGuid().ToString();
            var License = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.License, "License", request.ProviderId, path);
            var BackgroundCheck = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.BackgroundCheck, "BackgroundCheck", request.ProviderId, path);
            var ProfilePhoto = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.ProfilePhoto, "ProfilePhoto", request.ProviderId, path);

            var entity = new Driver() {
                License= License,
                LicenseExp = request.LicenseExp,
                BackgroundCheck = BackgroundCheck,
                BackgroundCheckExp = request.BackgroundCheckExp,
                ProfilePhoto= ProfilePhoto,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ProviderId = request.ProviderId,
            };

            var result = await _repository.AddAsync(entity);

            return new Success<Driver>() { Data = result };
        }
    }
}

