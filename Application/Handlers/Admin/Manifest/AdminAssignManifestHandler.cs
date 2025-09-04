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

namespace CMPNatural.Application
{
    public class AdminAssignManifestHandler : IRequestHandler<AdminAssignManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _repository;
        private readonly IDriverManifestRepository _driverManifestRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IProviderReposiotry _providerRepository;
        private readonly IMediator _mediator;

        public AdminAssignManifestHandler(IManifestRepository _repository, IProviderReposiotry _providerRepository, IMediator _mediator, ICompanyRepository _companyrepository,
             IDriverManifestRepository _driverManifestRepository, IDriverRepository _driverRepository)
        {
            this._repository = _repository;
            this._providerRepository = _providerRepository;
            this._mediator = _mediator;
            this._companyrepository = _companyrepository;
            this._driverManifestRepository = _driverManifestRepository;
            this._driverRepository = _driverRepository;
        }
        
        public async Task<CommandResponse<Manifest>> Handle(AdminAssignManifestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(p=> p.Id == request.Id && (p.Status == ManifestStatus.Draft || p.Status == ManifestStatus.Scaduled),query=>query.Include(x=>x.Invoice))).FirstOrDefault();
            var provider = (await _providerRepository.GetAsync(p => p.Id == request.ProviderId && p.Status == ProviderStatus.Approved)).FirstOrDefault();
            var company = (await _companyrepository.GetAsync(p => p.Id == entity.Invoice.CompanyId)).FirstOrDefault();
            var drivers = (await _driverRepository.GetAsync(p => p.ProviderId == request.ProviderId)).ToList();

            if (!drivers.Any())
            {
                return new NoAcess<Manifest>
                {
                    Message = "The manifest cannot be assigned because no drivers are available for this provider."
                };
            }

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
            if (entity.Status != ManifestStatus.Draft && entity.Status != ManifestStatus.Scaduled)
            {
                return new NoAcess<Manifest>
                {
                    Message = "The manifest cannot be assigned because the status must be Draft or Scaduled."
                };
            }
            if (entity.ProviderId != null)
            {
                return new NoAcess<Manifest>
                {
                    Message = "This manifest has already been assigned to a provider and cannot be reassigned."
                };
            }

            

            entity.ServiceDateTime = request.ServiceDateTime.ToLocalTime();
            entity.ProviderId = provider.Id;

            if (entity.Status != ManifestStatus.Scaduled)
            {
                entity.Status = ManifestStatus.Assigned;
            }

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

            if (result.IsSucces())
            {
                await _repository.UpdateAsync(entity);
                #region assignToDriver
                var dirver = drivers.FirstOrDefault(x => x.IsDefault);
                if (dirver == null)
                {
                    dirver = drivers.FirstOrDefault();
                }
                var driverManifest = new DriverManifest()
                {
                    ProviderId = request.ProviderId,
                    ManifestId = entity.Id,
                    DriverId = dirver.Id,
                    CreateAt = DateTime.Now,
                };
                await _driverManifestRepository.AddAsync(driverManifest);
                #endregion
            }

            return new Success<Manifest>() { Data = entity };
        }
    }
}

