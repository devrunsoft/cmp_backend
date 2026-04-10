using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Responses.AdminMenuRepresentation;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories.Chat;
using CMPNatural.Core.Repositories.ChatCommon;

namespace CMPNatural.Application
{

    public class AdminMenuRepresentationHandler : IRequestHandler<AdminMenuRepresentationCommand, CommandResponse<AdminMenuRepresentationResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly ICompanyContractRepository _companyContract;
        private readonly IManifestRepository _manifestRepository;
        private readonly IChatMessageRepository _repository;
        private readonly IChatCommonMessageRepository _chatCommonMessageRepository;


        public AdminMenuRepresentationHandler(IinvoiceRepository iinvoiceRepository, ICompanyContractRepository companyContract,
             IManifestRepository _manifestRepository, IRequestRepository _requestRepository, IChatMessageRepository _repository, IChatCommonMessageRepository _chatCommonMessageRepository
            )
        {
            this._repository = _repository;
            _invoiceRepository = iinvoiceRepository;
            this._requestRepository = _requestRepository;
            _companyContract = companyContract;
            this._manifestRepository = _manifestRepository;
            this._chatCommonMessageRepository = _chatCommonMessageRepository;
        }

        public async Task<CommandResponse<AdminMenuRepresentationResponse>> Handle(AdminMenuRepresentationCommand request, CancellationToken cancellationToken)
        {

            var invoices = (await _invoiceRepository.GetAsync(x => 
                x.Status == InvoiceStatus.Send_Payment && x.Type == InvoiceType.Final_Invoice
                )).Count();

              var requests = (await _requestRepository.GetAsync(x =>
                   x.Status == InvoiceStatus.Draft
                  )).Count();

            var ContractsCount = (await _companyContract.GetAsync(x =>
              x.Status == CompanyContractStatus.Needs_Admin_Signature || x.Status == CompanyContractStatus.Created
              )).Count();

            var ManifestCount = (await _manifestRepository.GetAsync(x =>
             x.Status == ManifestStatus.Draft
                 )).Count();

            var ConversationCount = (await _repository.GetAsync(x =>
                !x.IsSeen && x.SenderType == ParticipantType.Client
                  )).Count();

            var ProviderConversationCount = (await _chatCommonMessageRepository.GetAsync(x =>
                 !x.IsSeen && (x.SenderType == ParticipantType.Provider || x.SenderType == ParticipantType.Driver)
                 )).Count();

            var model = new AdminMenuRepresentationResponse() {
                ProviderConversationCount= ProviderConversationCount,
                ConversationCount = ConversationCount, InvoicesCount = invoices, RequestsCount = requests, ContractsCount = ContractsCount , ManifestCount = ManifestCount };

            return new Success<AdminMenuRepresentationResponse>() { Data = model };

        }
    }
}

