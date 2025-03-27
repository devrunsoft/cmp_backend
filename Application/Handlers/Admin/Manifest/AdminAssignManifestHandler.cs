using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Helper;
using CMPNatural.Core.Extentions;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminAssignManifestHandler : IRequestHandler<AdminAssignManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _repository;
        private readonly IProviderReposiotry _providerRepository;
        private readonly IMediator _mediator;

        public AdminAssignManifestHandler(IManifestRepository _repository, IProviderReposiotry _providerRepository, IMediator _mediator)
        {
            this._repository = _repository;
            this._providerRepository = _providerRepository;
            this._mediator = _mediator;
        }

        public async Task<CommandResponse<Manifest>> Handle(AdminAssignManifestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(p=> p.Id == request.Id && p.Status == ManifestStatus.Draft)).FirstOrDefault();
            var provider = (await _providerRepository.GetAsync(p => p.Id == request.ProviderId && p.Status == ProviderStatus.Approved)).FirstOrDefault();

            entity.ServiceDateTime = request.ServiceDateTime.ToLocalTime();
            entity.ProviderId = provider.Id;
            entity.Status = ManifestStatus.Assigned;
            var content = entity.Content;

            content = CompanyContractHelper.ShowByKey(ManifestKeyEnum.Date.GetDescription(), content);
            content = content.Replace(ManifestKeyEnum.Date.GetDescription(), entity.ServiceDateTime.Value.ToDateString());

            content = CompanyContractHelper.ShowByKey(ManifestKeyEnum.Time.GetDescription(), content);
            content = content.Replace(ManifestKeyEnum.Time.GetDescription(), entity.ServiceDateTime.Value.ToTimeString());

            content = CompanyContractHelper.ShowByKey(ManifestKeyEnum.ProviderAddress.GetDescription(), content);
            content = content.Replace(ManifestKeyEnum.ProviderAddress.GetDescription(), provider.Address);

            content = CompanyContractHelper.ShowByKey(ManifestKeyEnum.ProviderName.GetDescription(), content);
            content = content.Replace(ManifestKeyEnum.ProviderName.GetDescription(), provider.Name);

            entity.Content = content;
            var result =await _mediator.Send(new AdminSetInvoiceProviderCommand() { ProviderId = provider.Id , InvoiceId = entity.InvoiceId});

            if(result.IsSucces())
            await _repository.UpdateAsync(entity);

            return new Success<Manifest>() { Data = entity };
        }
    }
}

