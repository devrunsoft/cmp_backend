using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminSendCompanyContractHandler : IRequestHandler<AdminSendCompanyContractCommand, CommandResponse<CompanyContract>>
    {
        private readonly ICompanyContractRepository _repository;
        public AdminSendCompanyContractHandler(ICompanyContractRepository repository)
        {
            _repository = repository;
        }
        public async Task<CommandResponse<CompanyContract>> Handle(AdminSendCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            entity.Status = CompanyContractStatus.Send;
            await _repository.UpdateAsync(entity);

            return new Success<CompanyContract>() { Data = entity };
        }
    }
}

