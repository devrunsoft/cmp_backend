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
using CMPFile;

namespace CMPNatural.Application
{
    public class RegisterProviderDocumentHandler : IRequestHandler<RegisterProviderDocumentCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _repository;
        private readonly IFileStorage fileStorage;

        public RegisterProviderDocumentHandler(IProviderReposiotry repository, IFileStorage fileStorage)
        {
            _repository = repository;
            this.fileStorage = fileStorage;
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

