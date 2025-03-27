using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Extentions;
using CMPNatural.Core.Helper;
using CMPNatural.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Handlers
{
	public class AdminCreateCompanyContractHandler
	{
        private readonly ICompanyContractRepository _repository;
        private readonly IContractRepository _contractrepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IinvoiceRepository _baseServicerepository;
        public AdminCreateCompanyContractHandler(ICompanyContractRepository _repository, IContractRepository _contractrepository,
            IinvoiceRepository baseServicerepository, IAppInformationRepository _apprepository)
		{
            this._repository = _repository;
            this._contractrepository = _contractrepository;
            this._baseServicerepository = baseServicerepository;
            this._apprepository = _apprepository;
        }

        public async Task<CommandResponse<CompanyContract>> Create(Invoice invoice, long companyId)
        {
            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            var contract = (await _contractrepository.GetAsync(x => x.Active)).LastOrDefault();
            var company = invoice.Company;
            if (contract == null)
            {
                return new NoAcess<CompanyContract>() {  Message = "No active contract found!" };
            }

            var services = (await _baseServicerepository.GetAsync(x => x.InvoiceId == invoice.InvoiceId, query => query
            .Include(x=>x.BaseServiceAppointment)
            .ThenInclude(x=>x.Product)
            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ProductPrice)
            )).FirstOrDefault();

            var entity = new CompanyContract()
            {
                CompanyId = companyId,
                InvoiceId = invoice.InvoiceId,
                Status = (int)CompanyContractStatus.Created,
                CreatedAt = DateTime.Now,
                ContractId = contract.Id,
            };

            var result = await _repository.AddAsync(entity);


            // Service list
            List<string> serviceList = services.BaseServiceAppointment
                .Select(x => $"<strong>{x.Product.Name}</strong>  - <strong>{x.ProductPrice.Name} </strong> "
                           + $"- <strong>Number of Payments:</strong> {x.ProductPrice.NumberofPayments}, "
                           + $"<strong>Billing Period:</strong> {x.ProductPrice.BillingPeriod}" +
                           $"<br> <strong>Start Date:</strong> {x.DueDate.ToDateString()}" +
                           $" - <strong>Preferred Days:</strong> {x.DayOfWeek} ({(x.FromHour.ConvertTimeToString())} until {(x.ToHour.ConvertTimeToString())})" +
                           $"<br> <strong>Total:</strong> ${x.TotalAmount}"
                           )
                .ToList();

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
            string cleanedNumber = Regex.Replace(information.CompanyPhoneNumber, @"^\+1\s*|\D", "");

            dbContent = dbContent.Replace(ContractKeysEnum.ManagmentCompanyLogo.GetDescription(), managementCompany.Logo);
          dbContent=  dbContent.Replace(ContractKeysEnum.ManagmentCompanyName.GetDescription(), information.CompanyTitle);
          dbContent=  dbContent.Replace(ContractKeysEnum.ManagmentCompanyEmail.GetDescription(), information.CompanyEmail);
          dbContent=  dbContent.Replace(ContractKeysEnum.ManagmentCompanyPhoneNumber.GetDescription(), cleanedNumber);
          dbContent=  dbContent.Replace(ContractKeysEnum.ManagmentCompanyFirstName.GetDescription(), information.CompanyCeoFirstName);
          dbContent=  dbContent.Replace(ContractKeysEnum.ManagmentCompanyLastName.GetDescription(), information.CompanyCeoLastName);
          dbContent=  dbContent.Replace(ContractKeysEnum.ManagmentCompanyAddress.GetDescription(), information.CompanyAddress);
             ///////
          dbContent=  dbContent.Replace(ContractKeysEnum.ClientFirstName.GetDescription(), company.PrimaryFirstName);
          dbContent=  dbContent.Replace(ContractKeysEnum.ClientLastName.GetDescription(), company.PrimaryLastName);
          dbContent=  dbContent.Replace(ContractKeysEnum.ClientAddress.GetDescription(), invoice.Address);
          dbContent=  dbContent.Replace(ContractKeysEnum.ClientCompanyName.GetDescription(), company.CompanyName);
          dbContent=  dbContent.Replace(ContractKeysEnum.ServiceItems.GetDescription(), managementCompany.Services);
          dbContent=  dbContent.Replace(ContractKeysEnum.ContractNumber.GetDescription(), result.ContractNumber);

          dbContent = CompanyContractHelper.HideByKey(ContractKeysEnum.ClientSign.GetDescription(), dbContent);
          dbContent = CompanyContractHelper.HideByKey(ContractKeysEnum.ManagmentCompanySign.GetDescription(), dbContent);
          dbContent = CompanyContractHelper.HideByKey(ContractKeysEnum.ClientSignDateTime.GetDescription(), dbContent);

            //update
            entity.Content = dbContent.ToString();
            await _repository.UpdateAsync(entity);
            return new Success<CompanyContract>() { Data = result, Message = "Successfull!" };
        }




    }
}

