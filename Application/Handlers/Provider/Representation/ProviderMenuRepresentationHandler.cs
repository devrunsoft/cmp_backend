

using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Responses.AdminMenuRepresentation;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.Provider.Representation;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Repositories.ChatCommon;

namespace CMPNatural.Application
{

    public class ProviderMenuRepresentationHandler : IRequestHandler<ProviderMenuRepresentationCommand, CommandResponse<ProviderMenuRepresentationResponse>>
    {
        private readonly IRouteRepository routeRepository;
        private readonly IProviderContractRepository _companyContract;
        private readonly IManifestRepository _manifestRepository;
        private readonly IChatCommonMessageRepository chatCommonMessageRepository;
        private readonly IChatCommonSessionRepository chatCommonSessionRepository;

        public ProviderMenuRepresentationHandler(IRouteRepository routeRepository, IProviderContractRepository companyContract,
             IManifestRepository _manifestRepository, IChatCommonMessageRepository chatCommonMessageRepository, IChatCommonSessionRepository chatCommonSessionRepository
            )
        {
            this.routeRepository = routeRepository;
            _companyContract = companyContract;
            this._manifestRepository = _manifestRepository;
            this.chatCommonMessageRepository = chatCommonMessageRepository;
            this.chatCommonSessionRepository = chatCommonSessionRepository;
        }

        public async Task<CommandResponse<ProviderMenuRepresentationResponse>> Handle(ProviderMenuRepresentationCommand request, CancellationToken cancellationToken)
        {

            var invoices = (await routeRepository.GetAsync(x =>
                (x.Status == RouteStatus.InProcess || x.Status == RouteStatus.Draft)  && x.ProviderId == request.ProviderId
                )).Count();

            var ContractsCount = (await _companyContract.GetAsync(x =>
              x.Status == CompanyContractStatus.Send && x.ProviderId == request.ProviderId
              )).Count();

            var ManifestCount = (await _manifestRepository.GetAsync(x =>
             x.Status == ManifestStatus.Assigned && x.ProviderId == request.ProviderId
                 )).Count();

            var session = (await chatCommonSessionRepository.GetAsync(x => x.ParticipantId == (request.IsDriver ? request.DriverId : request.ProviderId)
                )).FirstOrDefault();

            int MessagesCount = 0;

            if (session != null)
            {
                MessagesCount = (await chatCommonMessageRepository.GetAsync(x =>
                    x.SenderType == ParticipantType.Admin && x.ChatCommonSessionId == session.Id
                    )).Count();
            }

            var model = new ProviderMenuRepresentationResponse() { RouteCount = invoices,  ContractsCount = ContractsCount, ManifestCount = ManifestCount, MessagesCount = MessagesCount };

            return new Success<ProviderMenuRepresentationResponse>() { Data = model };

        }
    }
}

