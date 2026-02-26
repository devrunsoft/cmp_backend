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
using CMPNatural.Core.Extentions;

namespace CMPNatural.Application
{
    public class SignProviderContractHandler : IRequestHandler<SignProviderContractCommand, CommandResponse<ProviderContract>>
    {
        private readonly IProviderContractRepository _repository;
        private readonly IRequestRepository _invoiceRepository;
        public SignProviderContractHandler(IProviderContractRepository repository, IRequestRepository invoiceRepository)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<CommandResponse<ProviderContract>> Handle(SignProviderContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.ProviderContractId);
            entity.Status = CompanyContractStatus.Needs_Admin_Signature;
            entity.Sign = request.Sign;
            entity.ClientSignDate = DateTime.Now;
            var content = entity.Content;
            string formattedTime = entity.ClientSignDate.Value.ToString("hh:mm tt");
            content = CompanyContractHelper.ShowByKey(ContractProviderKeysEnum.ProviderSign.GetDescription(), content, CompanyContractHelper.SignFont);
            content = CompanyContractHelper.ShowByKey(ContractProviderKeysEnum.ProviderSignDateTime.GetDescription(), content);
            content = content.Replace(ContractProviderKeysEnum.ProviderSign.GetDescription(), request.Sign);
            content = content.Replace(ContractProviderKeysEnum.ProviderSignDateTime.GetDescription(), $"{entity.ClientSignDate.Value.ToDateString()} {formattedTime}");
            entity.Content = content;

            await _repository.UpdateAsync(entity);

            //update Invoice
            //var invoice = await _invoiceRepository.GetAsync(x => x.Id == entity.RequestId && x.Status == InvoiceStatus.Pending_Signature &&
            //x.CompanyId == request.ProviderId
            //);

            //foreach (var item in invoice)
            //{
            //    item.Status = InvoiceStatus.Needs_Admin_Signature;
            //    await _invoiceRepository.UpdateAsync(item);
            //}

            return new Success<ProviderContract>() { Data = entity };
        }
    }
}

