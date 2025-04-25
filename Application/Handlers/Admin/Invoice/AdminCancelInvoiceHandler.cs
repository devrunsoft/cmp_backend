using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application.Handlers
{
    public class AdminCancelInvoiceHandler : IRequestHandler<AdminCancelInvoiceCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly ICompanyContractRepository _companyContractRepository;

        public AdminCancelInvoiceHandler(IinvoiceRepository invoiceRepository, ICompanyContractRepository companyContractRepository)
        {
            _invoiceRepository = invoiceRepository;
            _companyContractRepository = companyContractRepository;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(AdminCancelInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId)).FirstOrDefault();

            //todo
            if (entity.Status != InvoiceStatus.Draft)
            {
                return new NoAcess<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(entity), Message = "Your service is currently being processed." };
            }

            entity.Status = InvoiceStatus.Deleted;
            await _invoiceRepository.UpdateAsync(entity);

            if (entity.ContractId != null)
            {
                var contract = (await _companyContractRepository.GetAsync(p => p.CompanyId == entity.CompanyId && p.Id == entity.ContractId)).FirstOrDefault();
                await _companyContractRepository.DeleteAsync(contract);
            }

            return new Success<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(entity), Message = "Successfull!" };

        }

    }
}

