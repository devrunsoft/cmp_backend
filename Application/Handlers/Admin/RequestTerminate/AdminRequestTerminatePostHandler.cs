using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Command;
using System.Linq;
using CMPNatural.Application.Commands.Client;
using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminRequestTerminatePostHandler : IRequestHandler<AdminRequestTerminatePostCommand, CommandResponse<RequestTerminate>>
    {
        private readonly IRequestTerminateRepository terminateRepository;
        private readonly ICompanyContractRepository companyContractRepository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IManifestRepository _manifestRepository;

        public AdminRequestTerminatePostHandler(IinvoiceRepository invoiceRepository, IManifestRepository _manifestRepository, IRequestTerminateRepository terminateRepository,
           ICompanyContractRepository companyContractRepository )
        {
            _invoiceRepository = invoiceRepository;
            this._manifestRepository = _manifestRepository;
            this.terminateRepository = terminateRepository;
            this.companyContractRepository = companyContractRepository;
        }

        public async Task<CommandResponse<RequestTerminate>> Handle(AdminRequestTerminatePostCommand request, CancellationToken cancellationToken)
        {
            var terminaterequest = (await terminateRepository.GetAsync(x=>x.Id == request.Id)).FirstOrDefault();

            var invoices = (await _invoiceRepository.GetAsync(p => p.InvoiceId == terminaterequest.InvoiceNumber && p.CompanyId == terminaterequest.CompanyId &&
            (p.Status != InvoiceStatus.Complete && p.Status != InvoiceStatus.Deleted),
                query => query
                .Include(i => i.BaseServiceAppointment)
                )).ToList();

            foreach (var item in invoices)
            {

                foreach (var service in item.BaseServiceAppointment)
                {
                    service.Status = ServiceStatus.Canceled;
                }
                item.Status = InvoiceStatus.Canceled;
                //////
                var manifest = (await _manifestRepository.GetAsync(x => x.InvoiceId == item.Id)).FirstOrDefault();
                if (manifest != null)
                {
                    manifest.Status = ManifestStatus.Canceled;
                    await _manifestRepository.UpdateAsync(manifest);
                }
                var contract = (await companyContractRepository.GetAsync(x => x.InvoiceId == item.InvoiceId)).FirstOrDefault();
                if (manifest != null)
                {
                    contract.Status = CompanyContractStatus.Canceld;
                    await companyContractRepository.UpdateAsync(contract);
                }

                await _invoiceRepository.UpdateAsync(item);
            }

            terminaterequest.RequestTerminateStatus = RequestTerminateProcessEnum.Terminated;
            await terminateRepository.UpdateAsync(terminaterequest);

            return new Success<RequestTerminate>() { Data = terminaterequest };
        }
    }
}

