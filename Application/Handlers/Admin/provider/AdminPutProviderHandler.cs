using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CMPNatural.Application
{
    public class AdminPutProviderHandler : IRequestHandler<AdminPutProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IProviderServiceRepository _providerServiceRepository;
        private readonly IPersonRepository _personRepository;

        public AdminPutProviderHandler(IProviderReposiotry providerReposiotry, IProviderServiceRepository providerServiceRepository , IPersonRepository _personRepository)
        {
            _providerReposiotry = providerReposiotry;
            _providerServiceRepository = providerServiceRepository;
            this._personRepository = _personRepository;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminPutProviderCommand request, CancellationToken cancellationToken)
        {
            var emailPattern = @"^[\w\.\+-]+@([\w-]+\.)+[a-zA-Z]{2,7}$";
            if (!Regex.IsMatch(request.Email, emailPattern))
            {
                return new NoAcess<Provider>() { Message = "The email format is invalid. Please provide a valid email address." };
            }

            var entity = await _providerReposiotry.GetByIdAsync(request.Id);
            var providerService = (await _providerServiceRepository.GetAsync(p=>p.ProviderId == request.Id)).ToList();

            if(providerService.Count>0)
            await _providerServiceRepository.DeleteRangeAsync(providerService);

            List<ProviderService> providerServices = new List<ProviderService>();
            foreach (var item in request.ProductIds)
            {
                providerServices.Add(new ProviderService() { ProductId = item });
            }

            #region addperson
            if (entity.PersonId == null)
            {
                var personId = Guid.NewGuid();
                var person = new Core.Entities.Person() { FirstName = request.ManagerFirstName, LastName = request.ManagerLastName, Id = personId };
                await _personRepository.AddAsync(person);
                entity.PersonId = personId;
            }
            #endregion


            entity.Lat = request.Lat;
            entity.Long = request.Long;
            entity.Name = request.Name;
            entity.Rating = request.Rating;
            entity.Address = request.Address;
            entity.County = request.County;
            entity.City = request.City;
            //entity.Status = (int)request.Status;
            entity.BusinessLicenseExp = request.BusinessLicenseExp;
            entity.HealthDepartmentPermitExp = request.HealthDepartmentPermitExp;
            entity.EPAComplianceExp = request.EPAComplianceExp;
            entity.InsuranceExp = request.InsuranceExp;
            entity.AreaLocation = request.AreaLocation;
            entity.ProviderService = providerServices;
            entity.PhoneNumber = request.PhoneNumber;
            entity.Email = request.Email;
            entity.ManagerFirstName = request.ManagerFirstName;
            entity.ManagerLastName = request.ManagerLastName;
            entity.ManagerPhoneNumber = request.ManagerPhoneNumber;


            #region file
            var path = Guid.NewGuid().ToString();
            string BusinessLicense = null;
            string HealthDepartmentPermit = null;
            string WasteHaulerPermit = null;
            string EPACompliance = null;
            string Insurance = null;

            if (request.BusinessLicense != null)
                BusinessLicense = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.BusinessLicense, "BusinessLicense", entity.Id, path);

            if (request.HealthDepartmentPermit != null)
                HealthDepartmentPermit = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.HealthDepartmentPermit, "HealthDepartmentPermit", entity.Id, path);

            if (request.WasteHaulerPermit != null)
                WasteHaulerPermit = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.WasteHaulerPermit, "WasteHaulerPermit", entity.Id, path);

            if (request.EPACompliance != null)
                EPACompliance = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.EPACompliance, "EPACompliance", entity.Id, path);

            if (request.Insurance != null)
                Insurance = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.Insurance, "Insurance", entity.Id, path);


            entity.BusinessLicense = BusinessLicense;
            entity.HealthDepartmentPermit = HealthDepartmentPermit;
            entity.WasteHaulerPermit = WasteHaulerPermit;
            entity.EPACompliance = EPACompliance;
            entity.Insurance = Insurance;
            #endregion


            await _providerReposiotry.UpdateAsync(entity);

           return new Success<Provider>() { Data = entity };

        }
    }
}

