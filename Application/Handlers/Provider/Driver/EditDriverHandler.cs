using System;
using CMPNatural.Application.Services;
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
        private readonly IFileStorage fileStorage;

        public EditDriverHandler(IDriverRepository repository, IFileStorage fileStorage)
        {
            _repository = repository;
            this.fileStorage = fileStorage;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(EditDriverCommand request, CancellationToken cancellationToken)
        {
            var existingDriver = (await _repository.GetAsync(x => x.Email == request.Email && x.Id != request.Id)).Any();
            if (existingDriver)
            {
                return new NoAcess<DriverResponse>() { Message = "A driver with this email already exists." };
            }


            var entity = (await _repository.GetAsync(x=>x.Id == request.Id, query => query.Include(x=>x.Person))).FirstOrDefault();

            if(entity.ProviderId != request.ProviderId)
            {
                return new NoAcess<DriverResponse>() { Message = "No Access to Edit!" };
            }

            var path = Guid.NewGuid().ToString();

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
            entity.IsDefault = request.IsDefault;

            await _repository.UpdateAsync(entity);

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(entity) };
        }
    }
}

