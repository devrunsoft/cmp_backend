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
using CMPNatural.Core.Repositories.Chat;

namespace CMPNatural.Application
{

    public class ClientMenuRepresentationHandler : IRequestHandler<ClientMenuRepresentationCommand, CommandResponse<ClientRepresentationResponse>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly ICompanyContractRepository _companyContract;
        private readonly IChatMessageRepository chatMessageRepository;

        public ClientMenuRepresentationHandler(IinvoiceRepository iinvoiceRepository, ICompanyContractRepository companyContract,
            IManifestRepository _manifestRepository, IRequestRepository _requestRepository, IChatMessageRepository chatMessageRepository
            )
        {
            this._requestRepository = _requestRepository;
            _invoiceRepository = iinvoiceRepository;
            _companyContract = companyContract;
            this._manifestRepository = _manifestRepository;
            this.chatMessageRepository = chatMessageRepository;
        }

        public async Task<CommandResponse<ClientRepresentationResponse>> Handle(ClientMenuRepresentationCommand request, CancellationToken cancellationToken)
        {
            var manifest = (await _manifestRepository.GetAsync(x =>
                (x.Status != ManifestStatus.Completed || x.Status != ManifestStatus.Canceled || x.Status != ManifestStatus.Scaduled)
                && x.ProviderId != null &&
                x.CompanyId == request.CompanyId &&
                (request.OperationalAddressId == 0 || x.OperationalAddressId == request.OperationalAddressId)
              )).Count();

            var invoices = (await _invoiceRepository.GetAsync(x =>
                 x.Status == InvoiceStatus.Send_Payment && x.CompanyId == request.CompanyId && ( request.OperationalAddressId==0 || x.OperationalAddressId == request.OperationalAddressId)
                )).Count();

            var requests = (await _requestRepository.GetAsync(x =>
                ( x.Status == InvoiceStatus.Draft) && x.CompanyId == request.CompanyId && (request.OperationalAddressId == 0 || x.OperationalAddressId == request.OperationalAddressId)
                )).Count();

            var ContractsCount = (await _companyContract.GetAsync(x =>
              x.Status == CompanyContractStatus.Send && x.CompanyId == request.CompanyId && (request.OperationalAddressId == 0 || x.OperationalAddressId == request.OperationalAddressId)
              )).Count();

            var Messages = (await chatMessageRepository.GetAsync(x =>
                x.SenderType != ParticipantType.Client && x.ClientId == request.CompanyId && !x.IsSeen && 
                (request.OperationalAddressId == 0 || x.OperationalAddressId == request.OperationalAddressId)
                )).Count();

            var model = new ClientRepresentationResponse() { Contract = ContractsCount , Invoice = invoices , Requests = requests, Manifests = manifest, Messages = Messages };
            return new Success<ClientRepresentationResponse>() { Data = model };

        }
    }
}

