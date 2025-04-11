using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using ScoutDirect.Core.Repositories;
using CMPNatural.Application.Commands.Admin.Company;
using CMPNatural.Core.Repositories;
using System.Linq;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminGetCompanyHandler : IRequestHandler<AdminGetCompanyCommand, CommandResponse<CompanyResponse>>
    {
        private readonly ICompanyRepository _invoiceRepository;
        private readonly IBillingInformationRepository _billingInformationRepository;
        public AdminGetCompanyHandler(ICompanyRepository invoiceRepository, IBillingInformationRepository billingInformationRepository)
        {
            _invoiceRepository = invoiceRepository;
            _billingInformationRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<CompanyResponse>> Handle(AdminGetCompanyCommand request, CancellationToken cancellationToken)
        {
            var result = (await _invoiceRepository.GetByIdAsync(request.Id));
            var lastResult = (await _billingInformationRepository.GetAsync(p => p.CompanyId == request.Id)).LastOrDefault();
            var Data = CompanyMapper.Mapper.Map<CompanyResponse>(result);
            Data.BillingInformation = lastResult;

            return new Success<CompanyResponse>() { Data = Data };

        }

    }
}

