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
using CMPNatural.Application.Commands.Client.Representation;
using CMPNatural.Application.Responses.Client;

namespace CMPNatural.Application
{

    public class ClientMenuRepresentationHandler : IRequestHandler<ClientMenuRepresentationCommand, CommandResponse<ClientRepresentationResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly ICompanyContractRepository _companyContract;

        public ClientMenuRepresentationHandler(IinvoiceRepository iinvoiceRepository, ICompanyContractRepository companyContract
            )
        {
            _invoiceRepository = iinvoiceRepository;
            _companyContract = companyContract;
        }

        public async Task<CommandResponse<ClientRepresentationResponse>> Handle(ClientMenuRepresentationCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetAsync(x =>
                 x.Status == InvoiceStatus.Send_Payment && x.CompanyId == request.CompanyId
                )).Count();

            var requests = (await _invoiceRepository.GetAsync(x =>
                ( x.Status == InvoiceStatus.Draft) && x.CompanyId == request.CompanyId
                )).Count();

            var ContractsCount = (await _companyContract.GetAsync(x =>
              x.Status == CompanyContractStatus.Send && x.CompanyId == request.CompanyId
              )).Count();

            var model = new ClientRepresentationResponse() { Contract = ContractsCount , Invoice = invoices , Requests = requests };
            return new Success<ClientRepresentationResponse>() { Data = model };

        }
    }
}

