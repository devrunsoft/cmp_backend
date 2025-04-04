using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMPNatural.Application.Responses.AdminMenuRepresentation;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{

    public class AdminMenuRepresentationHandler : IRequestHandler<AdminMenuRepresentationCommand, CommandResponse<AdminMenuRepresentationResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly ICompanyContractRepository _companyContract;
        private readonly IManifestRepository _manifestRepository;

        public AdminMenuRepresentationHandler(IinvoiceRepository iinvoiceRepository, ICompanyContractRepository companyContract,
             IManifestRepository _manifestRepository
            )
        {
            _invoiceRepository = iinvoiceRepository;
            _companyContract = companyContract;
            this._manifestRepository = _manifestRepository;
        }

        public async Task<CommandResponse<AdminMenuRepresentationResponse>> Handle(AdminMenuRepresentationCommand request, CancellationToken cancellationToken)
        {

            var invoices = (await _invoiceRepository.GetAsync(x => 
                x.Status != InvoiceStatus.Draft && x.Status != InvoiceStatus.Needs_Admin_Signature &&
                x.Status != InvoiceStatus.Pending_Signature && x.Status != InvoiceStatus.Needs_Assignment &&
                x.Status != InvoiceStatus.Processing_Provider
                )).Count();

              var requests = (await _invoiceRepository.GetAsync(x =>
                   x.Status == InvoiceStatus.Draft
                  )).Count();

            var ContractsCount = (await _companyContract.GetAsync(x =>
              x.Status == (int)CompanyContractStatus.Needs_Admin_Signature
              )).Count();

            var ManifestCount = (await _manifestRepository.GetAsync(x =>
             x.Status == ManifestStatus.Draft
                 )).Count();

            var model = new AdminMenuRepresentationResponse() { InvoicesCount = invoices, RequestsCount = requests, ContractsCount = ContractsCount , ManifestCount = ManifestCount };

            return new Success<AdminMenuRepresentationResponse>() { Data = model };

        }
    }
}

