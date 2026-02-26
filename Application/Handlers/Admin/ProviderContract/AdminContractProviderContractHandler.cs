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
using CMPNatural.Core.Models;

namespace CMPNatural.Application
{
    public class AdminContractProviderContractHandler
    {
        public static string Create(List<RequestEntity> invoice, AppInformation information, Contract contract, Provider provider, ProviderContract result, AppSetting _appSetting)
        {
            var serviceList = invoice.Select(inv =>
            {
                var services = inv.BaseServiceAppointment.Where(x=>x.Status == ServiceStatus.Proccessing).Select(x =>
                    $"<strong>{x.Product.Name}</strong> - <strong>{x.ProductPrice.Name}</strong> " +
                    $"<br><strong>Start Date:</strong> {x.StartDate.ToDateString()}" +
                    $" - <strong>Preferred Days:</strong> {x.DayOfWeek} ({x.FromHour.ConvertTimeToString()} until {x.ToHour.ConvertTimeToString()})" +
                    $"<br><strong>Total:</strong> ${x.TotalAmount}"
                );

                return $"<div style='margin-bottom:20px'>" +
                       $"<ul>{string.Join("", services.Select(s => $"<li>{s}</li>"))}</ul>" +
                       $"</div>";
            }).ToList();

            var managementCompany = new
            {
                Logo = $"<img width={40} height={40}  src='{_appSetting.host}{information.CompanyIcon}' alt='Company Logo' />",
                Services = $"<ul>{string.Join("", serviceList.Select(service => $"<li>{service}</li>"))}</ul>"
            };

            var dbContent = contract.Content.ToString();
            var cleanedNumber = information.CompanyPhoneNumber.FormatPhoneNumber();

            dbContent = dbContent.Replace(ContractProviderKeysEnum.ManagmentCompanyLogo.GetDescription(), managementCompany.Logo);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ManagmentCompanyName.GetDescription(), information.CompanyTitle);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ManagmentCompanyEmail.GetDescription(), information.CompanyEmail);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ManagmentCompanyPhoneNumber.GetDescription(), cleanedNumber);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ManagmentCompanyFirstName.GetDescription(), information.CompanyCeoFirstName);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ManagmentCompanyLastName.GetDescription(), information.CompanyCeoLastName);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ManagmentCompanyAddress.GetDescription(), information.CompanyAddress);

            dbContent = dbContent.Replace(ContractProviderKeysEnum.ProviderName.GetDescription(), provider?.Name ?? string.Empty);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ProviderAddress.GetDescription(), provider?.Address ?? string.Empty);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ProviderCityStateZip.GetDescription(), provider?.City ?? string.Empty);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ProviderPhone.GetDescription(), provider?.PhoneNumber ?? string.Empty);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ProviderEmail.GetDescription(), provider?.Email ?? string.Empty);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.EffectiveDate.GetDescription(), DateTime.Now.ToDateString());
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ServiceItems.GetDescription(), managementCompany.Services);
            dbContent = dbContent.Replace(ContractProviderKeysEnum.ContractNumber.GetDescription(), result.Number);

            dbContent = CompanyContractHelper.HideByKey(ContractProviderKeysEnum.ManagmentCompanySign.GetDescription(), dbContent);
            dbContent = CompanyContractHelper.HideByKey(ContractProviderKeysEnum.ProviderSign.GetDescription(), dbContent);
            dbContent = CompanyContractHelper.HideByKey(ContractProviderKeysEnum.ProviderSignDateTime.GetDescription(), dbContent);
            dbContent = CompanyContractHelper.HideByKey(ContractProviderKeysEnum.ManagmentCompanySignDateTime.GetDescription(), dbContent);

            //foreach (var key in Enum.GetValues(typeof(ContractProviderKeysEnum)).Cast<ContractProviderKeysEnum>())
            //{
            //    dbContent = dbContent.Replace(key.GetDescription(), string.Empty);
            //}

            return dbContent;
        }
    }
}
