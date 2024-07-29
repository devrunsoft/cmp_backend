using Barbara.Application.Queries;
using Barbara.Application.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Application.Queries;
using ScoutDirect.Core.Repositories;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers.QueryHandlers
{
    public class GetRegistrationStatusHanlder : IRequestHandler<RegisterStatusCommand, CommandResponse<RegisterType>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILocationCompanyRepository _locationCompanyRepository;

        public GetRegistrationStatusHanlder(
            IBillingInformationRepository billingInformationRepository,
            ICompanyRepository companyRepository,
            IDocumentRepository documentRepository,
            ILocationCompanyRepository locationCompanyRepository
            )
        {
            _billingInformationRepository = billingInformationRepository;
            _companyRepository = companyRepository;
            _documentRepository = documentRepository;
            _locationCompanyRepository = locationCompanyRepository;
        }

        public async Task<CommandResponse<RegisterType>> Handle(RegisterStatusCommand request, CancellationToken cancellationToken)
        {

            return new Success<RegisterType>() { Success = true, Message = "" };
        }

    }
}
