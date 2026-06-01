using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Provider.WhiteLabel;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ProviderWhiteLabelRequestHandler :
        IRequestHandler<ProviderWhiteLabelRequestCommand, CommandResponse<WhiteLabelRequest>>,
        IRequestHandler<ProviderGetWhiteLabelRequestCommand, CommandResponse<WhiteLabelRequest>>
    {
        private readonly IWhiteLabelRequestRepository _repository;
        private readonly IProviderReposiotry _providerRepository;

        public ProviderWhiteLabelRequestHandler(IWhiteLabelRequestRepository repository, IProviderReposiotry providerRepository)
        {
            _repository = repository;
            _providerRepository = providerRepository;
        }

        public async Task<CommandResponse<WhiteLabelRequest>> Handle(ProviderWhiteLabelRequestCommand request, CancellationToken cancellationToken)
        {
            var provider = await _providerRepository.GetByIdAsync(request.ProviderId);
            if (provider == null)
            {
                return new NoAcess<WhiteLabelRequest>() { Message = "Provider not found." };
            }

            var entity = await _repository.GetByProviderIdAsync(request.ProviderId);
            if (entity == null)
            {
                entity = new WhiteLabelRequest
                {
                    ProviderId = request.ProviderId,
                    TenantId = provider.TenantId,
                    CreatedAt = DateTime.UtcNow,
                    Status = WhiteLabelStatus.Pending
                };
            }
            else if(entity.Status == WhiteLabelStatus.Accepted)
            {
                return new NoAcess<WhiteLabelRequest>() { Data = entity };
            }

            entity.ManageClientsDirectly = request.ManageClientsDirectly;
            entity.WantsClientPortal = request.WantsClientPortal;
            entity.WantsDispatchManagement = request.WantsDispatchManagement;
            entity.WantsInvoicing = request.WantsInvoicing;
            entity.WantsWhiteLabelBranding = request.WantsWhiteLabelBranding;
            entity.WantsCustomDomain = request.WantsCustomDomain;
            entity.CustomDomain = request.CustomDomain;
            entity.SubDomain = request.SubDomain;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.Status = WhiteLabelStatus.Pending;
            entity.AdminReviewNote = null;

            if (entity.Id == 0)
            {
                entity = await _repository.AddAsync(entity);
            }
            else
            {
                await _repository.UpdateAsync(entity);
            }

            return new Success<WhiteLabelRequest>() { Data = entity };
        }

        public async Task<CommandResponse<WhiteLabelRequest>> Handle(ProviderGetWhiteLabelRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(x=>x.ProviderId == request.ProviderId , query => query.Include(x=>x.Tenant).ThenInclude(x=>x.Domains))).FirstOrDefault();
            //if (entity == null)
            //{
            //    entity = new WhiteLabelRequest()
            //    {
            //        ProviderId = request.ProviderId,
            //    };
            //}

            return new Success<WhiteLabelRequest>() { Data = entity };
        }
    }
}
