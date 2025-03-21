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
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers
{
    public class AdminSentInvoiceHandler : IRequestHandler<AdminSentInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ICompanyContractRepository _companyContractRepository;
        private readonly IAppInformationRepository _appRepository;
        public AdminSentInvoiceHandler(IinvoiceRepository invoiceRepository,
             IContractRepository contractRepository,
             ICompanyContractRepository companyContractRepository, IAppInformationRepository _appRepository)
        {
            _invoiceRepository = invoiceRepository;
            _contractRepository = contractRepository;
            _companyContractRepository = companyContractRepository;
            this._appRepository = _appRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(AdminSentInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId,query=>query.Include(x=>x.Company))).FirstOrDefault();
            entity.Status = InvoiceStatus.Pending_Signature;

            //if (requests.CreateContract)
            //{
                var result = await new AdminCreateCompanyContractHandler(_companyContractRepository, _contractRepository , _invoiceRepository , _appRepository)
                    .Create(entity, entity.CompanyId);

                if (!result.IsSucces())
                {
                    return new NoAcess<Invoice>() { Data = entity, Message = result.Message };
                }
                entity.ContractId = result.Data.Id;
            //}

            entity.CalculateAmount();

            await _invoiceRepository.UpdateAsync(entity);

            return new Success<Invoice>() { Data = entity, Message = "Successfull!" };

        }
    }
}

