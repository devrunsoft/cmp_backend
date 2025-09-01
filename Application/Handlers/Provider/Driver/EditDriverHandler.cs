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

namespace CMPNatural.Application
{
    public class EditDriverHandler : IRequestHandler<EditDriverCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;

        public EditDriverHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(EditDriverCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(x=>x.Id == request.Id, query => query.Include(x=>x.Person))).FirstOrDefault();

            if(entity.ProviderId != request.ProviderId)
            {
                return new NoAcess<DriverResponse>() { Message = "No Access to Edit!" };
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

            entity.Person.FirstName = request.FirstName;
            entity.Person.LastName = request.LastName;
            entity.Email = request.Email;
            entity.IsDefault = request.IsDefault;

            await _repository.UpdateAsync(entity);

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(entity) };
        }
    }
}

