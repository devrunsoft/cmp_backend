using System;
using CMPNatural.Application.Commands.Admin.provider;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;

namespace CMPNatural.Application
{
    public class AdminPutProviderHandler : IRequestHandler<AdminPutProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminPutProviderHandler(IProviderReposiotry providerReposiotry)
        {
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminPutProviderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _providerReposiotry.GetByIdAsync(request.Id);

            entity.Lat = request.Lat;
            entity.Long = request.Long;
            entity.Name = request.Name;
            entity.Rating = request.Rating;
            entity.Address = request.Address;
            entity.County = request.County;
            entity.City = request.City;
            entity.Status = (int)request.Status;
            entity.BusinessLicenseExp = request.BusinessLicenseExp;
            entity.HealthDepartmentPermitExp = request.HealthDepartmentPermitExp;
            entity.EPAComplianceExp = request.EPAComplianceExp;
            entity.InsuranceExp = request.InsuranceExp;
            entity.AreaLocation = request.AreaLocation;


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

