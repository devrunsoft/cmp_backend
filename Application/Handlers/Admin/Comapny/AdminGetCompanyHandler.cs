using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using ScoutDirect.Core.Repositories;
using CMPNatural.Application.Commands.Admin.Company;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminGetCompanyHandler : IRequestHandler<AdminGetCompanyCommand, CommandResponse<CompanyResponse>>
    {
        private readonly ICompanyRepository _invoiceRepository;

        public AdminGetCompanyHandler(ICompanyRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<CompanyResponse>> Handle(AdminGetCompanyCommand request, CancellationToken cancellationToken)
        {
            var result = (await _invoiceRepository.GetByIdAsync(request.Id));

            return new Success<CompanyResponse>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(result) };

        }

    }
}

