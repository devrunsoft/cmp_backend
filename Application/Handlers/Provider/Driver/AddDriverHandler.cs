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
using CMPFile;
using System.ServiceModel.Channels;
using System.Linq;

namespace CMPNatural.Application
{
    public class AddDriverHandler : IRequestHandler<AddDriverCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        private readonly IPersonRepository _personRepository;
        private readonly IFileStorage fileStorage;

        public AddDriverHandler(IDriverRepository repository, IPersonRepository _personRepository , IFileStorage fileStorage)
        {
            _repository = repository;
            this._personRepository = _personRepository;
            this.fileStorage = fileStorage;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(AddDriverCommand request, CancellationToken cancellationToken)
        {
            var existingDriver = (await _repository.GetAsync(x => x.Email == request.Email)).Any();
            if (existingDriver)
            {
                return new NoAcess<DriverResponse>() { Message = "A driver with this email already exists." };
            }

            var personId = Guid.NewGuid();
            var person = new Person() { FirstName = request.FirstName, LastName = request.LastName, Id = personId };
            //await _personRepository.AddAsync(person);

            var entity = new Driver() {
                License= request.License,
                LicenseExp = request.LicenseExp,
                BackgroundCheck = request.BackgroundCheck,
                BackgroundCheckExp = request.BackgroundCheckExp,
                ProfilePhoto= request.ProfilePhoto,
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

