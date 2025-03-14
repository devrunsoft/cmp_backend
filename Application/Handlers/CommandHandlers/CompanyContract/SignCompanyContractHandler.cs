using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;
using CMPNatural.Core.Enums;

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
            entity.Status = (int)CompanyContractStatus.Signed;
            entity.Sign = request.Sign;
            await _repository.UpdateAsync(entity);

            //update Invoice
            var invoice = await _invoiceRepository.GetAsync(x => x.InvoiceId == entity.InvoiceId && x.Status == (int)InvoiceStatus.PendingSignature &&
            x.CompanyId == request.CompanyId
            );

            foreach (var item in invoice)
            {
                item.Status = (int)InvoiceStatus.Processing;
                await _invoiceRepository.UpdateAsync(item);
            }

            return new Success<CompanyContract>() { Data = entity };
        }
    }
}

