using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using System;
using CMPNatural.Application.Services;
using System.Collections.Generic;
using CMPNatural.Core.Helper;
using System.Text.RegularExpressions;
using Stripe;
using ScoutDirect.Core.Repositories.Base;
using System.Linq;

namespace CMPNatural.Application
{

    public class AdminPostProviderHandler : IRequestHandler<AdminPostProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IPersonRepository _personRepository;
        
        public AdminPostProviderHandler(IProviderReposiotry providerReposiotry, IPersonRepository _personRepository)
        {
            _providerReposiotry = providerReposiotry;
           this._personRepository = _personRepository;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminPostProviderCommand request, CancellationToken cancellationToken)
        {

            var existingDriver = (await _providerReposiotry.GetAsync(x => x.Email == request.Email)).Any();
            if (existingDriver)
            {
                return new NoAcess<Provider>() { Message = "A provider with this email already exists." };
            }

            var emailPattern = @"^[\w\.\+-]+@([\w-]+\.)+[a-zA-Z]{2,7}$";
            if (!Regex.IsMatch(request.Email, emailPattern))
            {
                return new NoAcess<Provider>() { Message = "The email format is invalid. Please provide a valid email address." };
            }

            List<ProviderService> providerServices = new List<ProviderService>();
            foreach (var item in request.ProductIds)
            {
                providerServices.Add(new ProviderService() { ProductId = item });
            }

            var personId = Guid.NewGuid();
            var person = new Core.Entities.Person() { FirstName = request.ManagerFirstName, LastName = request.ManagerLastName, Id = personId };
            await _personRepository.AddAsync(person);

            var entity = new Provider()
            {
                Lat = request.Lat,
                Long = request.Long,
                Name = request.Name,
                Rating = request.Rating,
                Address = request.Address,
                County = request.County,
                City = request.City,
                Status = request.Status,
                BusinessLicenseExp = request.BusinessLicenseExp,
                HealthDepartmentPermitExp = request.HealthDepartmentPermitExp,
                EPAComplianceExp = request.EPAComplianceExp,
                InsuranceExp = request.InsuranceExp,
                AreaLocation = request.AreaLocation,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                ProviderService = providerServices,
                ManagerFirstName = request.ManagerFirstName,
                ManagerLastName = request.ManagerLastName,
                ManagerPhoneNumber = request.ManagerPhoneNumber,
                PersonId = personId

            };
            var result = await _providerReposiotry.AddAsync(entity);


            var path = Guid.NewGuid().ToString();
            string BusinessLicense = null;
            string HealthDepartmentPermit = null;
            string WasteHaulerPermit = null;
            string EPACompliance = null;
            string Insurance = null;

            if (request.BusinessLicense != null)
                BusinessLicense = request.BusinessLicense;

            if (request.HealthDepartmentPermit != null)
                HealthDepartmentPermit = request.HealthDepartmentPermit;

            if (request.WasteHaulerPermit != null)
                WasteHaulerPermit = request.WasteHaulerPermit;

            if (request.EPACompliance != null)
                EPACompliance = request.EPACompliance;

            if (request.Insurance != null)
                Insurance = request.Insurance;


            result.BusinessLicense = BusinessLicense;
            result.HealthDepartmentPermit = HealthDepartmentPermit;
            result.WasteHaulerPermit = WasteHaulerPermit;
            result.EPACompliance = EPACompliance;
            result.Insurance = Insurance;
            result.Password = PasswordGenerator.GenerateSecurePassword();

            await _providerReposiotry.UpdateAsync(result);


            return new Success<Provider>() { Data = result };

        }
    }
}

