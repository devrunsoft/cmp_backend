using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
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

        public async Task<CommandResponse<CompanyContract>> Create(string invoiceId, long companyId)
        {
            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            var contract = (await _contractrepository.GetAsync(x => x.Active)).LastOrDefault();
            if (contract == null)
            {
                return new NoAcess<CompanyContract>() {  Message = "No active contract found!" };
            }

            var services = (await _baseServicerepository.GetAsync(x => x.InvoiceId == invoiceId, query => query
            .Include(x=>x.BaseServiceAppointment)
            .ThenInclude(x=>x.Product)
            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ProductPrice)
            )).FirstOrDefault();

            var entity = new CompanyContract()
            {
                CompanyId = companyId,
                InvoiceId = invoiceId,
                Status = (int)CompanyContractStatus.Created,
                CreatedAt = DateTime.Now,
                ContractId = contract.Id,
            };

            var result = await _repository.AddAsync(entity);


            var managementCompany = new
            {
                Logo = $"<img width={40} height={40}  src='https://api.app-cmp.com{information.CompanyIcon}' alt='Company Logo' />",
                Name = $"{information.CompanyCeoFirstName}",
                Address = $"{information.CompanyAddress}",
                Phone = $"{information.CompanyAddress}",
                TotalAmount = $"{services.Amount}",
                AgreementTitle = $"SERVICE AGREEMENT #{result.ContractNumber}",
                //AgreementText = "This Agreement is made and entered into by and between..."
            };

            // Service list
            List<string> serviceList = services.BaseServiceAppointment
                .Select(x => $"<strong>{x.Product.Name}</strong>  - <strong>{x.ProductPrice.Name} </strong> "
                           + $"- Number of Payments: {x.ProductPrice.NumberofPayments}, "
                           + $"Billing Period: {x.ProductPrice.BillingPeriod}")
                .ToList();

            StringBuilder fullContract = new StringBuilder();

            fullContract.AppendLine("<div>");
            fullContract.AppendLine(managementCompany.Logo);
            fullContract.AppendLine($"<p>{managementCompany.Name}</p>");
            fullContract.AppendLine($"<p>{managementCompany.Address}</p>");
            fullContract.AppendLine($"<p>{managementCompany.Phone}</p>");
            fullContract.AppendLine($"<p><strong>{managementCompany.AgreementTitle}</strong></p>");
            //fullContract.AppendLine($"<p>{managementCompany.AgreementText}</p>");
            fullContract.AppendLine("<br>");
            fullContract.AppendLine("<hr>");
            fullContract.AppendLine("<br>");
            fullContract.AppendLine("<br>");
            fullContract.AppendLine(contract.Content);
            fullContract.AppendLine("<br>");
            fullContract.AppendLine("<hr>");
            fullContract.AppendLine("<br>");
            fullContract.AppendLine("<br>");
            fullContract.AppendLine("<h3>Items:</h3>");
            fullContract.AppendLine("<ul>");
            foreach (var service in serviceList)
            {
                fullContract.AppendLine($"<li>{service}</li>");
            }
            fullContract.AppendLine("</ul>");

            fullContract.AppendLine($"<p>Total: <strong>${managementCompany.TotalAmount}</strong></p>");

            fullContract.AppendLine("</div>");


            //update
            entity.Content = fullContract.ToString();
            await _repository.UpdateAsync(entity);
            return new Success<CompanyContract>() { Data = result, Message = "Successfull!" };
        }
	}
}

