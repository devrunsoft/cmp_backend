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
using CMPNatural.Core.Models;
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
        private readonly AppSetting _appSetting;
        public AdminCreateCompanyContractHandler(ICompanyContractRepository _repository, IContractRepository _contractrepository,
            IinvoiceRepository baseServicerepository, IAppInformationRepository _apprepository, AppSetting appSetting)
		{
            this._repository = _repository;
            this._contractrepository = _contractrepository;
            this._baseServicerepository = baseServicerepository;
            this._apprepository = _apprepository;
            this._appSetting = appSetting;
        }

        public async Task<CommandResponse<CompanyContract>> Create(Invoice invoice, long companyId)
        {
            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            if (information == null)
            {
                return new NoAcess<CompanyContract>() { Message = "Please complete the information" };
            }
            var contracts = await _contractrepository.GetAsync(x => x.Active);
            var contract = contracts.FirstOrDefault(c => c.IsDefault);
            contract ??= contracts.LastOrDefault();

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
                Status = CompanyContractStatus.Created,
                CreatedAt = DateTime.Now,
                ContractId = contract.Id,
                ContractNumber = ""
            };

            var result = await _repository.AddAsync(entity);

            var dbContent = AdminContractCompanyContractHandler.Create(services, information, contract, company, result, _appSetting);
            //update
            entity.Content = dbContent.ToString();
            entity.ContractNumber = entity.Number;
            entity.OperationalAddressId = invoice.OperationalAddressId;
            await _repository.UpdateAsync(entity);
            return new Success<CompanyContract>() { Data = result, Message = "Successfull!" };
        }




    }
}

