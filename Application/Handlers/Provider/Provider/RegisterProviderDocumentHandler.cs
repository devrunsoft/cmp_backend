using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Linq;
using System.Collections.Generic;
using CMPNatural.Application.Services;
using System;

namespace CMPNatural.Application
{
    public class RegisterProviderDocumentHandler : IRequestHandler<RegisterProviderDocumentCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _repository;

        public RegisterProviderDocumentHandler(IProviderReposiotry repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Provider>> Handle(RegisterProviderDocumentCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(x=>x.Id == request.ProviderId && x.Status != ProviderStatus.Blocked)).FirstOrDefault();


            var path = Guid.NewGuid().ToString();
            string BusinessLicense = null;
            string HealthDepartmentPermit = null;
            string WasteHaulerPermit = null;
            string EPACompliance = null;
            string Insurance = null;

            if (request.BusinessLicense != null)
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
            result.BusinessLicenseExp = request.BusinessLicenseExp;
            //
            result.HealthDepartmentPermit = HealthDepartmentPermit;
            result.HealthDepartmentPermitExp = request.HealthDepartmentPermitExp;
            result.WasteHaulerPermit = WasteHaulerPermit;
            result.EPACompliance = EPACompliance;
            result.EPAComplianceExp = request.EPAComplianceExp;
            result.Insurance = Insurance;
            result.InsuranceExp = request.InsuranceExp;

            await _repository.UpdateAsync(result);

            await _repository.UpdateAsync(result);

            return new Success<Provider>() { Data = result };
        }
    }
}

