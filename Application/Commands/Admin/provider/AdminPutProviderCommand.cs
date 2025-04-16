using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminPutProviderCommand : ProviderInput , IRequest<CommandResponse<Provider>>
    {
        public AdminPutProviderCommand(ProviderInput input , long Id , string BaseVirtualPath)
        {
            this.Id = Id;
            this.Name = input.Name;
            this.Rating = input.Rating;
            this.Status = input.Status;

            this.Lat = input.Lat;
            this.Long = input.Long;
            this.City = input.City;
            this.Address = input.Address;
            this.County = input.County;
            this.AreaLocation = input.AreaLocation;

            this.Email = input.Email;
            this.PhoneNumber = input.PhoneNumber;

            this.ProductIds = input.ProductIds;

            this.BusinessLicense = input.BusinessLicense;
            this.BusinessLicenseExp = input.BusinessLicenseExp;

            this.HealthDepartmentPermit = input.HealthDepartmentPermit;
            this.HealthDepartmentPermitExp = input.HealthDepartmentPermitExp;

            this.WasteHaulerPermit = input.WasteHaulerPermit;

            this.EPACompliance = input.EPACompliance;
            this.EPAComplianceExp = input.EPAComplianceExp;

            this.Insurance = input.Insurance;
            this.InsuranceExp = input.InsuranceExp;

            this.ManagerFirstName = input.ManagerFirstName;
            this.ManagerLastName = input.ManagerLastName;
            this.ManagerPhoneNumber = input.ManagerPhoneNumber;
            this.BaseVirtualPath = BaseVirtualPath;
        }

		public long Id { get; set; }
        public string BaseVirtualPath { get; set; }
    }
}

