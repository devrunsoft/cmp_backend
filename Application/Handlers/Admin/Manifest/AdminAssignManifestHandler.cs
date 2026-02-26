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
using System.Collections.Generic;
using CMPNatural.Application.Handlers;
using CMPNatural.Core.Models;

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

        private readonly IRequestRepository _invoiceRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IProviderContractRepository _companyContractRepository;
        private readonly IAppInformationRepository _appRepository;
        private readonly AppSetting _appSetting;

        public AdminAssignManifestHandler(IManifestRepository _repository, IProviderReposiotry _providerRepository, IMediator _mediator, ICompanyRepository _companyrepository,
             IDriverManifestRepository _driverManifestRepository, IDriverRepository _driverRepository,
             IRequestRepository invoiceRepository,
             IContractRepository contractRepository,
             IProviderContractRepository companyContractRepository, IAppInformationRepository _appRepository, AppSetting appSetting
             )
        {
            this._repository = _repository;
            this._providerRepository = _providerRepository;
            this._mediator = _mediator;
            this._companyrepository = _companyrepository;
            this._driverManifestRepository = _driverManifestRepository;
            this._driverRepository = _driverRepository;

            _invoiceRepository = invoiceRepository;
            _contractRepository = contractRepository;
            _companyContractRepository = companyContractRepository;
            this._appRepository = _appRepository;
            this._appSetting = appSetting;
        }
        
        public async Task<CommandResponse<Manifest>> Handle(AdminAssignManifestCommand request, CancellationToken cancellationToken)
        {
            var e = (await _repository.GetAsync(p=> p.Id == request.Id && (p.Status == ManifestStatus.Draft || p.Status == ManifestStatus.Scaduled),query=>query.Include(x=>x.Request))).FirstOrDefault();
            var allentity = (await _repository.GetAsync(p => p.ContractId == e.ContractId && (p.Status == ManifestStatus.Draft || p.Status == ManifestStatus.Scaduled), query => query.Include(x => x.Request))).ToList();
            var provider = (await _providerRepository.GetAsync(p => p.Id == request.ProviderId && p.Status == ProviderStatus.Approved)).FirstOrDefault();
            var company = (await _companyrepository.GetAsync(p => p.Id == e.Request.CompanyId)).FirstOrDefault();
            var drivers = (await _driverRepository.GetAsync(p => p.ProviderId == request.ProviderId)).ToList();

            var contract = (await _companyContractRepository.GetAsync(p => p.ProviderId == request.ProviderId && p.RequestId == e.RequestId)).Any();

            List<Manifest> manifests = new List<Manifest>() { e };

            if (request.AssignAll)
            {
                manifests = allentity;
            }

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
            if (provider == null)
            {
                return new NoAcess<Manifest>
                {
                    Message = "The manifest cannot be assigned because the associated provider is currently blocked."
                };
            }


            foreach (var entity in manifests)
            {


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
            var result =await _mediator.Send(new AdminSetInvoiceProviderCommand() { ProviderId = provider.Id , ManifestId = entity.Id});

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
           else
            {
                    throw new Exception("");
            }
           }
            if (!contract)
            {
                var resultContract = await new AdminCreateProviderContractHandler(_companyContractRepository, _contractRepository, _invoiceRepository, _appRepository, _appSetting, _providerRepository)
                    .Create(e.Request, manifests, request.ProviderId);

                if (!resultContract.IsSucces())
                {
                    return new NoAcess<Manifest>() { Data = e, Message = resultContract.Message };
                }
            }

            return new Success<Manifest>() { Data = e };
        }
    }
}

