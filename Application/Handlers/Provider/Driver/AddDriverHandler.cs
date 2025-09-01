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
using CMPNatural.Application.Mapper;
using CMPNatural.Core.Helper;

namespace CMPNatural.Application
{
    public class AddDriverHandler : IRequestHandler<AddDriverCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        private readonly IPersonRepository _personRepository;

        public AddDriverHandler(IDriverRepository repository, IPersonRepository _personRepository)
        {
            _repository = repository;
            this._personRepository = _personRepository;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(AddDriverCommand request, CancellationToken cancellationToken)
        {
            var path = Guid.NewGuid().ToString();
            var License = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.License, "License", request.ProviderId, path);
            var BackgroundCheck = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.BackgroundCheck, "BackgroundCheck", request.ProviderId, path);
            var ProfilePhoto = FileHandler.ProviderVehiclefileHandler(request.BaseVirtualPath, request.ProfilePhoto, "ProfilePhoto", request.ProviderId, path);


            var personId = Guid.NewGuid();
            var person = new Person() { FirstName = request.FirstName, LastName = request.LastName, Id = personId };
            //await _personRepository.AddAsync(person);

            var entity = new Driver() {
                License= License,
                LicenseExp = request.LicenseExp,
                BackgroundCheck = BackgroundCheck,
                BackgroundCheckExp = request.BackgroundCheckExp,
                ProfilePhoto= ProfilePhoto,
                ProviderId = request.ProviderId,
                Password = PasswordGenerator.GenerateSecurePassword(),
                Email = request.Email,
                IsDefault = request.IsDefault,
                Person = person
            };

            var result = await _repository.AddAsync(entity);

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(result) };
        }
    }
}

