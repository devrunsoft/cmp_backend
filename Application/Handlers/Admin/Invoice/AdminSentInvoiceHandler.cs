using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Models;

namespace CMPNatural.Application.Handlers
{
    public class AdminSentInvoiceHandler : IRequestHandler<AdminSentRequestCommand, CommandResponse<RequestEntity>>
    {
        private readonly IRequestRepository _invoiceRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ICompanyContractRepository _companyContractRepository;
        private readonly IAppInformationRepository _appRepository;
        private readonly AppSetting _appSetting;
        public AdminSentInvoiceHandler(IRequestRepository invoiceRepository,
             IContractRepository contractRepository,
             ICompanyContractRepository companyContractRepository, IAppInformationRepository _appRepository, AppSetting appSetting)
        {
            _invoiceRepository = invoiceRepository;
            _contractRepository = contractRepository;
            _companyContractRepository = companyContractRepository;
            this._appRepository = _appRepository;
            this._appSetting = appSetting;
        }

        public async Task<CommandResponse<RequestEntity>> Handle(AdminSentRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.RequestId, query=>query.Include(x=>x.Company))).FirstOrDefault();
            entity.Status = InvoiceStatus.Pending_Signature;

            if (entity.ContractId != null)
            {
                return new NoAcess<RequestEntity>() { Message = "You cannot edit an invoice that is linked to a contract." };
            }
            //if (requests.CreateContract)
            //{
                var result = await new AdminCreateCompanyContractHandler(_companyContractRepository, _contractRepository , _invoiceRepository , _appRepository, _appSetting)
                    .Create(entity, entity.CompanyId);

                if (!result.IsSucces())
                {
                    return new NoAcess<RequestEntity>() { Data = entity, Message = result.Message };
                }
                entity.ContractId = result.Data.Id;
                //entity.InvoiceNumber = entity.Number;
            //}

            entity.CalculateAmount();

            await _invoiceRepository.UpdateAsync(entity);

            return new Success<RequestEntity>() { Data = entity };

        }
    }
}

