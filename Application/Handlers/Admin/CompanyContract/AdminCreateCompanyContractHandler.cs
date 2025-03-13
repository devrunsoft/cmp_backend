using System;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Handlers
{
	public class AdminCreateCompanyContractHandler
	{
        private readonly ICompanyContractRepository _repository;
        private readonly IContractRepository _contractrepository;
        public AdminCreateCompanyContractHandler(ICompanyContractRepository _repository, IContractRepository _contractrepository)
		{
            this._repository = _repository;
            this._contractrepository = _contractrepository;
        }

        public async Task<CommandResponse<CompanyContract>> Create(string invoiceId, long companyId)
        {
            var contract = (await _contractrepository.GetAsync(x => x.Active)).LastOrDefault();
            if (contract == null)
            {
                return new NoAcess<CompanyContract>() {  Message = "No active contract found!" };
            }
            var entity = new CompanyContract()
            {
                CompanyId = companyId,
                InvoiceId = invoiceId,
                Status = (int)CompanyContractStatus.Created,
                CreatedAt = DateTime.Now,
                ContractId = contract.Id,
                Content = contract.Content
            };
            var result = await _repository.AddAsync(entity);
            return new Success<CompanyContract>() { Data = result, Message = "Successfull!" };
        }
	}
}

