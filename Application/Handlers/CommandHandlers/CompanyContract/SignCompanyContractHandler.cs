using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Helper;
using Microsoft.EntityFrameworkCore;
using System;

namespace CMPNatural.Application
{
    public class SignCompanyContractHandler : IRequestHandler<SignCompanyContractCommand, CommandResponse<CompanyContract>>
    {
        private readonly ICompanyContractRepository _repository;
        private readonly IinvoiceRepository _invoiceRepository;
        public SignCompanyContractHandler(ICompanyContractRepository repository, IinvoiceRepository invoiceRepository)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<CommandResponse<CompanyContract>> Handle(SignCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.CompanyContractId);
            entity.Status = (int)CompanyContractStatus.Needs_Admin_Signature;
            entity.Sign = request.Sign;
            entity.ClientSignDate = DateTime.Now;
            var content = entity.Content;
            string formattedTime = entity.ClientSignDate.Value.ToString("hh:mm tt");
            content = CompanyContractHelper.ShowByKey(ContractKeysEnum.ClientSign.GetDescription(), content, CompanyContractHelper.SignFont);
            content = CompanyContractHelper.ShowByKey(ContractKeysEnum.ClientSignDateTime.GetDescription(), content);
            content = content.Replace(ContractKeysEnum.ClientSign.GetDescription(), request.Sign);
            content = content.Replace(ContractKeysEnum.ClientSignDateTime.GetDescription(), $"{entity.ClientSignDate.Value.ToString("MM/dd/yyyy")} {formattedTime}");
            entity.Content = content;

            await _repository.UpdateAsync(entity);

            //update Invoice
            var invoice = await _invoiceRepository.GetAsync(x => x.InvoiceId == entity.InvoiceId && x.Status == InvoiceStatus.Pending_Signature &&
            x.CompanyId == request.CompanyId
            );

            foreach (var item in invoice)
            {
                item.Status = InvoiceStatus.Needs_Admin_Signature;
                await _invoiceRepository.UpdateAsync(item);
            }

            return new Success<CompanyContract>() { Data = entity };
        }
    }
}

