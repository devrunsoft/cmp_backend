using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Invoice;
using ScoutDirect.Core.Entities.Base;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers
{

    public class DeleteInvoiceHandler : IRequestHandler<DeleteInvoiceCommand, CommandResponse<object>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly ICompanyContractRepository _companyContractRepository;

        public DeleteInvoiceHandler(IinvoiceRepository invoiceRepository, ICompanyContractRepository companyContractRepository)
        {
            _invoiceRepository = invoiceRepository;
            _companyContractRepository = companyContractRepository;
        }

        public async Task<CommandResponse<object>> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.CompanyId == request.CompanyId && p.Id == request.InvoiceId)).FirstOrDefault();

            if(entity.Status != (int)InvoiceStatus.Draft)
            {
                return new NoAcess<object>() { Data = entity, Message = "Your service is currently being processed." };
            }

            entity.Status = (int)InvoiceStatus.Canceled;
            await _invoiceRepository.UpdateAsync(entity);

            if (entity.ContractId != null)
            {
                var contract = (await _companyContractRepository.GetAsync(p => p.CompanyId == entity.CompanyId && p.Id == entity.ContractId)).FirstOrDefault();
                await _companyContractRepository.DeleteAsync(contract);
            }

            return new Success<object>() { Data = entity, Message = "Successfull!" };

        }

    }
}

