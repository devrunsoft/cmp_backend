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

namespace CMPNatural.Application
{

    public class AdminPostProviderHandler : IRequestHandler<AdminPostProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminPostProviderHandler(IProviderReposiotry providerReposiotry)
        {
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminPostProviderCommand request, CancellationToken cancellationToken)
        {
            var emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            if (!Regex.IsMatch(request.Email, emailPattern))
            {
                return new NoAcess<Provider>() { Message = "The email format is invalid. Please provide a valid email address." };
            }

            List<ProviderService> providerServices = new List<ProviderService>();
            foreach (var item in request.ProductIds)
            {
                providerServices.Add(new ProviderService() { ProductId = item });
            }

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

            };
            var result = await _providerReposiotry.AddAsync(entity);


            var path = Guid.NewGuid().ToString();
            string BusinessLicense = null;
            string HealthDepartmentPermit = null;
            string WasteHaulerPermit = null;
            string EPACompliance = null;
            string Insurance = null;

            if (request.BusinessLicense!=null)
            BusinessLicense = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.BusinessLicense, "BusinessLicense", result.Id, path);

            if (request.HealthDepartmentPermit != null)
            HealthDepartmentPermit = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.HealthDepartmentPermit, "HealthDepartmentPermit", result.Id, path);

            if (request.WasteHaulerPermit != null)
            WasteHaulerPermit = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.WasteHaulerPermit, "WasteHaulerPermit", result.Id, path);

            if (request.EPACompliance != null)
              EPACompliance = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.EPACompliance, "EPACompliance", result.Id, path);

            if (request.Insurance != null)
              Insurance = FileHandler.ProviderfileHandler(request.BaseVirtualPath, request.Insurance, "Insurance", result.Id, path);


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

