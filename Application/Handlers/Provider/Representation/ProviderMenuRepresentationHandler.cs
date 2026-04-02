

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

namespace CMPNatural.Application
{

    public class ProviderMenuRepresentationHandler : IRequestHandler<ProviderMenuRepresentationCommand, CommandResponse<ProviderMenuRepresentationResponse>>
    {
        private readonly IRouteRepository routeRepository;
        private readonly IProviderContractRepository _companyContract;
        private readonly IManifestRepository _manifestRepository;

        public ProviderMenuRepresentationHandler(IRouteRepository routeRepository, IProviderContractRepository companyContract,
             IManifestRepository _manifestRepository
            )
        {
            this.routeRepository = routeRepository;
            _companyContract = companyContract;
            this._manifestRepository = _manifestRepository;
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

            var model = new ProviderMenuRepresentationResponse() { RouteCount = invoices,  ContractsCount = ContractsCount, ManifestCount = ManifestCount };

            return new Success<ProviderMenuRepresentationResponse>() { Data = model };

        }
    }
}

