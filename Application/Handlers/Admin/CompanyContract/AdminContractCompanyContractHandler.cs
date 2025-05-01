using System;
using ScoutDirect.Application.Responses;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using CMPNatural.Core.Extentions;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Helper;



namespace CMPNatural.Application
{
	public class AdminContractCompanyContractHandler
	{

		public static  string Create(Invoice invoice, AppInformation information, Contract contract,Company company, CompanyContract result)
		{
            List<string> serviceList = invoice.BaseServiceAppointment
             .Select(x => $"<strong>{x.Product.Name}</strong>  - <strong>{x.ProductPrice.Name} </strong> "
               + $"- <strong>Number of Payments:</strong> {x.ProductPrice.NumberofPayments}, "
               +
               //$"<strong>Billing Period:</strong> {x.ProductPrice.BillingPeriod}" +
               $"<br> <strong>Start Date:</strong> {x.StartDate.ToDateString()}" +
               $" - <strong>Preferred Days:</strong> {x.DayOfWeek} ({(x.FromHour.ConvertTimeToString())} until {(x.ToHour.ConvertTimeToString())})" +
               $"<br> <strong>Total:</strong> ${x.TotalAmount}"
               ).ToList();

            var managementCompany = new
            {
                Logo = $"<img width={40} height={40}  src='https://api.app-cmp.com{information.CompanyIcon}' alt='Company Logo' />",
                //Name = $"{information.CompanyCeoFirstName}",
                //Address = $"{information.CompanyAddress}",
                //Phone = $"{information.CompanyAddress}",
                //TotalAmount = $"{services.Amount}",
                //AgreementTitle = $"SERVICE AGREEMENT #{result.ContractNumber}",
                Services = $"<ul>{string.Join("", serviceList.Select(service => $"<li>{service}</li>"))}</ul>"
            };


            var dbContent = contract.Content.ToString();
            string cleanedNumber = information.CompanyPhoneNumber.FormatPhoneNumber();

            dbContent = dbContent.Replace(ContractKeysEnum.ManagmentCompanyLogo.GetDescription(), managementCompany.Logo);
            dbContent = dbContent.Replace(ContractKeysEnum.ManagmentCompanyName.GetDescription(), information.CompanyTitle);
            dbContent = dbContent.Replace(ContractKeysEnum.ManagmentCompanyEmail.GetDescription(), information.CompanyEmail);
            dbContent = dbContent.Replace(ContractKeysEnum.ManagmentCompanyPhoneNumber.GetDescription(), cleanedNumber);
            dbContent = dbContent.Replace(ContractKeysEnum.ManagmentCompanyFirstName.GetDescription(), information.CompanyCeoFirstName);
            dbContent = dbContent.Replace(ContractKeysEnum.ManagmentCompanyLastName.GetDescription(), information.CompanyCeoLastName);
            dbContent = dbContent.Replace(ContractKeysEnum.ManagmentCompanyAddress.GetDescription(), information.CompanyAddress);
            ///////
            dbContent = dbContent.Replace(ContractKeysEnum.ClientFirstName.GetDescription(), company.PrimaryFirstName);
            dbContent = dbContent.Replace(ContractKeysEnum.ClientLastName.GetDescription(), company.PrimaryLastName);
            dbContent = dbContent.Replace(ContractKeysEnum.ClientAddress.GetDescription(), invoice.Address);
            dbContent = dbContent.Replace(ContractKeysEnum.ClientCompanyName.GetDescription(), company.CompanyName);
            dbContent = dbContent.Replace(ContractKeysEnum.ServiceItems.GetDescription(), managementCompany.Services);
            dbContent = dbContent.Replace(ContractKeysEnum.ContractNumber.GetDescription(), result.ContractNumber);

            dbContent = CompanyContractHelper.HideByKey(ContractKeysEnum.ClientSign.GetDescription(), dbContent);
            dbContent = CompanyContractHelper.HideByKey(ContractKeysEnum.ManagmentCompanySign.GetDescription(), dbContent);
            dbContent = CompanyContractHelper.HideByKey(ContractKeysEnum.ClientSignDateTime.GetDescription(), dbContent);

            return dbContent;
        }
    }
}

