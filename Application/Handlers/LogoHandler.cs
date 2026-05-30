using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Services;

namespace CMPNatural.Application
{
    public class LogoHandler : IRequestHandler<LogoCommand, CommandResponse<string>>
    {
        private readonly IAppInformationRepository _repository;
        public LogoHandler(IAppInformationRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }
        public async Task<CommandResponse<string>> Handle(LogoCommand request, CancellationToken cancellationToken)
        {
            var tenantContext = TenantExecutionContext.Current;
            var tenantId = tenantContext?.TenantId;
            var entity = (await _repository.GetAsync(x => x.TenantId == tenantId)).LastOrDefault();
            return new Success<string>() { Data = entity==null ? "" : entity.CompanyIcon };
        }
    }
}

