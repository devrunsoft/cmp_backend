using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPEmail.Email;
using CMPNatural.Application.Services;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Helper;
using CMPNatural.Core.Models;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminReviewWhiteLabelRequestHandler : IRequestHandler<AdminReviewWhiteLabelRequestCommand, CommandResponse<WhiteLabelRequest>>
    {
        private readonly IWhiteLabelRequestRepository _repository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantDomainRepository _tenantDomainRepository;
        private readonly IProviderReposiotry _providerRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly CloudflareDnsService _cloudflareDnsService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly AppSetting _appSetting;

        public AdminReviewWhiteLabelRequestHandler(
            IWhiteLabelRequestRepository repository,
            ITenantRepository tenantRepository,
            ITenantDomainRepository tenantDomainRepository,
            IProviderReposiotry providerRepository,
            IAdminRepository adminRepository,
            CloudflareDnsService cloudflareDnsService,
            IConfiguration configuration,
            IEmailSender emailSender,
            AppSetting appSetting)
        {
            _repository = repository;
            _tenantRepository = tenantRepository;
            _tenantDomainRepository = tenantDomainRepository;
            _providerRepository = providerRepository;
            _adminRepository = adminRepository;
            _cloudflareDnsService = cloudflareDnsService;
            _configuration = configuration;
            _emailSender = emailSender;
            _appSetting = appSetting;
        }

        public async Task<CommandResponse<WhiteLabelRequest>> Handle(AdminReviewWhiteLabelRequestCommand request, CancellationToken cancellationToken)
        {
            var backendip = _configuration["Cloudflare:BackendServerIp"];
            var frontentip = _configuration["Cloudflare:ServerIp"];
            var cloudflareDomain = _configuration["Cloudflare:Domain"];

            if (request.Status != WhiteLabelStatus.Accepted && request.Status != WhiteLabelStatus.Rejected)
            {
                return new NoAcess<WhiteLabelRequest>() { Message = "Only Accepted or Rejected status is allowed." };
            }

            var entity = await _repository.GetByIdAsync(request.WhiteLabelRequestId);
            if (entity == null)
            {
                return new NoAcess<WhiteLabelRequest>() { Message = "White-label request not found." };
            }

            entity.Status = request.Status;
            entity.AdminReviewNote = request.AdminReviewNote??"";
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);

            if (entity.Status == WhiteLabelStatus.Accepted)
            {


                var domain = (entity.WantsCustomDomain ? entity.CustomDomain : entity.SubDomain);
                var tenantSubDomain = entity.WantsCustomDomain? $"api.{domain}": $"{domain}-api";
                var tenent = new Tenant()
                {
                    SubDomain = entity.WantsCustomDomain? null : tenantSubDomain,
                    Host = BuildHost(tenantSubDomain, cloudflareDomain, entity.WantsCustomDomain),
                    AdminCanViewAllRecords = false,
                    AllowMainAdminAccess = false,
                    ProviderId = entity.ProviderId,
                    WhiteLabelRequestId = entity.Id,
                    TenantAccesses = entity.TenantId == null ?
                    new List<TenantAccess>() :
                    new List<TenantAccess>()
                    {
                        new TenantAccess()
                        {
                            AccessibleTenantId = entity.TenantId.Value
                        }
                    }
                };


                tenent = await _tenantRepository.AddAsync(tenent);
                if (!entity.WantsCustomDomain)
                {
                    await _cloudflareDnsService.CreateTenantWildcardDnsAsync(tenantSubDomain, backendip);
                }




                List<TenantDomain> domains = new List<TenantDomain>();
                if (entity.WantsClientPortal)
                {

                    var clientSubDomain = entity.WantsCustomDomain ? null : $"{domain}-client";
                    domains.Add(new TenantDomain()
                    {
                        TenantId = tenent.Id,
                        SubDomain = clientSubDomain,
                        Host = BuildHost(clientSubDomain, cloudflareDomain, entity.WantsCustomDomain),
                        PortalType = PortalType.Client
                    });


                }

                if (!entity.WantsDispatchManagement)
                {

                    var adminSubDomain = entity.WantsCustomDomain ? null : $"{domain}-admin";
                    var host = BuildHost(adminSubDomain, cloudflareDomain, entity.WantsCustomDomain);
                    domains.Add(new TenantDomain()
                    {
                        TenantId = tenent.Id,
                        SubDomain = adminSubDomain,
                        Host = host,
                        PortalType = PortalType.Admin
                    });

                    #region CreateAdmin

                    var provider = await _providerRepository.GetByIdAsync(entity.ProviderId);
                    if (provider == null)
                    {
                        return new NoAcess<WhiteLabelRequest>() { Message = "Provider not found." };
                    }

                    if (string.IsNullOrWhiteSpace(provider.Email))
                    {
                        return new NoAcess<WhiteLabelRequest>() { Message = "Provider email is required to create the white-label admin account." };
                    }

                    var adminExists = (await _adminRepository.GetAsync(x => x.Email == provider.Email)).Any();
                    if (adminExists)
                    {
                        return new NoAcess<WhiteLabelRequest>() { Message = "An admin account with the provider email already exists." };
                    }

                    var adminPassword = PasswordGenerator.GenerateSecurePassword();
                    var superAdmin = new AdminEntity
                    {
                        Email = provider.Email,
                        Password = adminPassword,
                        Role = "SuperAdmin",
                        IsActive = true,
                        TenantId = tenent.Id,
                        TwoFactor = false,
                        Person = new Person
                        {
                            Id = Guid.NewGuid(),
                            FirstName = GetProviderFirstName(provider),
                            LastName = GetProviderLastName(provider)
                        }
                    };

                    await _adminRepository.AddAsync(superAdmin);

                    _emailSender.SendEmail(new MailModel
                    {
                        toEmail = provider.Email,
                        Subject = "Your white-label admin account is ready",
                        Name = GetProviderFirstName(provider),
                        CompanyName = provider.Name,
                        Link = host,
                        buttonText = "Open Admin Portal",
                        Body =
                            "Your white-label request has been approved.\n\n" +
                            "Your admin account has been created with the following credentials:\n\n" +
                            $"Email: {provider.Email}\n" +
                            $"Password: <strong>{adminPassword}</strong><br/><br/>" +
                            "Use the button below to open the admin portal and sign in."
                    });

                    #endregion
                }

                var providerSubDomain = entity.WantsCustomDomain ? null : $"{domain}-provider";
                domains.Add(new TenantDomain()
                {
                    TenantId = tenent.Id,
                    SubDomain = providerSubDomain,
                    Host = BuildHost(providerSubDomain, cloudflareDomain, entity.WantsCustomDomain),
                    PortalType = PortalType.Provider
                });

                await _tenantDomainRepository.AddRangeAsync(domains);

                if (!entity.WantsCustomDomain) { 
                    foreach (var item in domains)
                    {
                        await _cloudflareDnsService.CreateTenantWildcardDnsAsync(item.SubDomain!, frontentip);
                    }
                 }


            }

            return new Success<WhiteLabelRequest>() { Data = entity };
        }

        private static string GetProviderFirstName(Provider provider)
        {
            if (!string.IsNullOrWhiteSpace(provider.ManagerFirstName))
            {
                return provider.ManagerFirstName;
            }

            return string.IsNullOrWhiteSpace(provider.Name) ? "Provider" : provider.Name;
        }

        private static string GetProviderLastName(Provider provider)
        {
            return provider.ManagerLastName ?? string.Empty;
        }

        private static string BuildHost(string? subDomain, string? domain, bool WantsCustomDomain)
        {
            if (WantsCustomDomain)
            {
                return subDomain;
            }
            var normalizedSubDomain = subDomain?.Trim().Trim('.').ToLowerInvariant();
            var normalizedDomain = domain?.Trim().Trim('.').ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(normalizedSubDomain))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(normalizedDomain))
            {
                return normalizedSubDomain;
            }

            return $"{normalizedSubDomain}.{normalizedDomain}";
        }
    }
}
