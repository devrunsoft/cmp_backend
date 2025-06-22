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
using ScoutDirect.Core.Repositories;
using System.ServiceModel.Channels;
using Stripe;

namespace CMPNatural.Application
{
    public class AdminChangeAssignManifestHandler : IRequestHandler<AdminChangeAssignManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IinvoiceRepository _iinvoiceRepository;
        private readonly IManifestRepository _repository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IProviderReposiotry _providerRepository;
        private readonly IMediator _mediator;

        public AdminChangeAssignManifestHandler(IManifestRepository _repository, IProviderReposiotry _providerRepository, IMediator _mediator, ICompanyRepository _companyrepository, IinvoiceRepository _iinvoiceRepository)
        {
            this._repository = _repository;
            this._providerRepository = _providerRepository;
            this._mediator = _mediator;
            this._companyrepository = _companyrepository;
            this._iinvoiceRepository = _iinvoiceRepository;
        }

        public async Task<CommandResponse<Manifest>> Handle(AdminChangeAssignManifestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(p=> p.Id == request.Id && (p.Status == ManifestStatus.Assigned),query=>query.Include(x=>x.Invoice))).FirstOrDefault();
            var provider = (await _providerRepository.GetAsync(p => p.Id == request.ProviderId && p.Status == ProviderStatus.Approved)).FirstOrDefault();
            var company = (await _companyrepository.GetAsync(p => p.Id == entity.Invoice.CompanyId)).FirstOrDefault();

            if (company.Status == CompanyStatus.Pending)
            {
                return new NoAcess<Manifest>
                {
                    Message = "The manifest cannot be assigned because the associated company is currently pending"
                };
            }
            if (company.Status == CompanyStatus.Blocked)
            {
                return new NoAcess<Manifest>
                {
                    Message = "The manifest cannot be assigned because the associated company is currently blocked."
                };
            }
            if (provider.Status == ProviderStatus.Blocked)
            {
                return new NoAcess<Manifest>
                {
                    Message = "The manifest cannot be assigned because the associated provider is currently blocked."
                };
            }
            //if (entity.Status != ManifestStatus.Draft && entity.Status != ManifestStatus.Scaduled)
            //{
            //    return new NoAcess<Manifest>
            //    {
            //        Message = "The manifest cannot be assigned because the status must be Draft or Scaduled."
            //    };
            //}
            //if (entity.ProviderId != null)
            //{
            //    return new NoAcess<Manifest>
            //    {
            //        Message = "This manifest has already been assigned to a provider and cannot be reassigned."
            //    };
            //}



            entity.ServiceDateTime = request.ServiceDateTime.ToLocalTime();
            entity.ProviderId = provider.Id;

            //if (entity.Status != ManifestStatus.Scaduled)
            //{
            //    entity.Status = ManifestStatus.Assigned;
            //}

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
            entity.Invoice.ProviderId = request.ProviderId;
            //var result = await _mediator.Send(new AdminSetInvoiceProviderCommand() { ProviderId = provider.Id , InvoiceId = entity.InvoiceId});

            //if (result.IsSucces())
            //{
                await _repository.UpdateAsync(entity);
            //}

            return new Success<Manifest>() { Data = entity };
        }
    }
}

