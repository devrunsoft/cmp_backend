using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Entities.Base;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Models;

namespace CMPNatural.Application
{
    public class AdminUpdateCompanyContractHandler : IRequestHandler<AdminUpdateCompanyContractCommand, CommandResponse<CompanyContract>>
    {
        private readonly ICompanyContractRepository _repository;
        private readonly IContractRepository _contractrepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IinvoiceRepository _baseServicerepository;
        private readonly AppSetting _appSetting;

        public AdminUpdateCompanyContractHandler(ICompanyContractRepository _repository, IContractRepository _contractrepository,
            IinvoiceRepository baseServicerepository, IAppInformationRepository _apprepository, AppSetting appSetting)
        {
            this._repository = _repository;
            this._contractrepository = _contractrepository;
            this._baseServicerepository = baseServicerepository;
            this._apprepository = _apprepository;
            this._appSetting = appSetting;
        }

        public async Task<CommandResponse<CompanyContract>> Handle(AdminUpdateCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(x => x.Id == request.CompanyContractId)).FirstOrDefault();

            if(entity.Status != CompanyContractStatus.Created)
            {
                return new NoAcess<CompanyContract>() { Message = "This Contract is Already sent" };
            }

            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            if (information == null)
            {
                return new NoAcess<CompanyContract>() { Message = "Please complete the information" };
            }
            var contract = (await _contractrepository.GetAsync(x => x.Active && x.Id == request.ContractId)).FirstOrDefault();
            if (contract == null)
            {
                return new NoAcess<CompanyContract>() { Message = "No active contract found!" };
            }

            var invoice = (await _baseServicerepository.GetAsync(x => x.InvoiceId == entity.InvoiceId, query => query
            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.Product)
            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ProductPrice)
            .Include(x => x.Company)
            )).FirstOrDefault();
            var company = invoice.Company;


            var dbContent = AdminContractCompanyContractHandler.Create(invoice, information, contract, company, entity, _appSetting);
            entity.Content = dbContent.ToString();
            entity.ContractId = contract.Id;
            await _repository.UpdateAsync(entity);

            return new Success<CompanyContract>() { Data = entity };
        }
    }
}

