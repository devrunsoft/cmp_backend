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

        public AdminMenuRepresentationHandler(IinvoiceRepository iinvoiceRepository, ICompanyContractRepository companyContract)
        {
            _invoiceRepository = iinvoiceRepository;
            _companyContract = companyContract;
        }

        public async Task<CommandResponse<AdminMenuRepresentationResponse>> Handle(AdminMenuRepresentationCommand request, CancellationToken cancellationToken)
        {

            var invoices = (await _invoiceRepository.GetAsync(x => 
                x.Status != InvoiceStatus.Draft && x.Status != InvoiceStatus.Needs_Admin_Signature &&
                x.Status != InvoiceStatus.Pending_Signature
                )).Count();

              var requests = (await _invoiceRepository.GetAsync(x =>
                   x.Status == InvoiceStatus.Draft
                  )).Count();

            var ContractsCount = (await _companyContract.GetAsync(x =>
              x.Status == (int)CompanyContractStatus.Needs_Admin_Signature
              )).Count();

            var model = new AdminMenuRepresentationResponse() { InvoicesCount = invoices, RequestsCount = requests, ContractsCount = ContractsCount };

            return new Success<AdminMenuRepresentationResponse>() { Data = model };

        }
    }
}

