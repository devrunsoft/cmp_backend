using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Models;
using CMPNatural.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Handlers
{
    public class AdminCreateProviderContractHandler
    {
        private readonly IProviderContractRepository _repository;
        private readonly IContractRepository _contractrepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IRequestRepository _baseServicerepository;
        private readonly AppSetting _appSetting;
        public AdminCreateProviderContractHandler(IProviderContractRepository _repository, IContractRepository _contractrepository,
            IRequestRepository baseServicerepository, IAppInformationRepository _apprepository, AppSetting appSetting)
        {
            this._repository = _repository;
            this._contractrepository = _contractrepository;
            this._baseServicerepository = baseServicerepository;
            this._apprepository = _apprepository;
            this._appSetting = appSetting;
        }

        public async Task<CommandResponse<ProviderContract>> Create(RequestEntity request, List<Manifest> models, long providerId)
        {
            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            if (information == null)
            {
                return new NoAcess<ProviderContract>() { Message = "Please complete the information" };
            }
            var contracts = await _contractrepository.GetAsync(x => x.Active && x.Type == ContractType.Provider);
            var contract = contracts.FirstOrDefault(c => c.IsDefault);
            contract ??= contracts.LastOrDefault();

            if (contract == null)
            {
                return new NoAcess<ProviderContract>() { Message = "No active contract found!" };
            }

            var company = request.Company;
            var mainInvoice = (await _baseServicerepository.GetAsync(x => x.Id == request.Id, query => query
            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.Product)
            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ProductPrice)
            .Include(x => x.Provider)
            )).ToList();

            var entity = new ProviderContract()
            {
                ManifestIsd = string.Join(",", models.Select(x=>x.Id)),
                CompanyId = company.Id,
                Status = CompanyContractStatus.Created,
                CreatedAt = DateTime.Now,
                ContractId = contract.Id,
                ContractNumber = "",
                OperationalAddressId = request.OperationalAddressId,
                ProviderId = providerId,
                RequestId = request.Id
            };

            var result = await _repository.AddAsync(entity);

            var dbContent = AdminContractProviderContractHandler.Create(mainInvoice, information, contract, company, result, _appSetting);
            //update
            entity.Content = dbContent.ToString();
            entity.ContractNumber = entity.Number;
            entity.OperationalAddressId = request.OperationalAddressId;
            await _repository.UpdateAsync(entity);
            return new Success<ProviderContract>() { Data = result };
        }




    }
}
