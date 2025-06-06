using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Application.Services;
using System.IO;
using ScoutDirect.Core.Caching;

namespace CMPNatural.Application
{
    public class AdminAppInformationPutHandler : IRequestHandler<AdminAppInformationPutCommand, CommandResponse<AppInformation>>
    {
        private readonly IAppInformationRepository _repository;
        private readonly ICacheService _cache;
        public AdminAppInformationPutHandler(IAppInformationRepository providerReposiotry, Func<CacheTech, ICacheService> _cacheService)
        {
            _repository = providerReposiotry;
            _cache = _cacheService(CacheTech.Memory);
        }
        public async Task<CommandResponse<AppInformation>> Handle(AdminAppInformationPutCommand request, CancellationToken cancellationToken)
        {

            string CompanyIcon = null;
            if (request.CompanyIcon != null)
                CompanyIcon = FileHandler.AppfileHandler(request.BaseVirtualPath, request.CompanyIcon, "CompanyIcon", "Admin/AppInfromation");

            //var entity = (await _repository.GetAllAsync()).FirstOrDefault();
            //if (entity == null)
            //{
               var entity = new AppInformation()
                {
                CompanyPhoneNumber = request.CompanyPhoneNumber,
                CompanyTitle = request.CompanyTitle,
                CompanyAddress = request.CompanyAddress,
                CompanyCeoFirstName = request.CompanyCeoFirstName,
                CompanyCeoLastName = request.CompanyCeoLastName,
                Sign = request.Sign,
                CompanyIcon = CompanyIcon,
                CompanyEmail = request.CompanyEmail,
                StripeApikey = request.StripeApikey,
                StripePaymentMethodConfiguration = request.StripePaymentMethodConfiguration
                };

            entity = await _repository.AddAsync(entity);
            //}
            //else
            //{

            //    entity.CompanyPhoneNumber = request.CompanyPhoneNumber;
            //    entity.CompanyTitle = request.CompanyTitle;
            //    entity.CompanyAddress = request.CompanyAddress;
            //    entity.CompanyCeoFirstName = request.CompanyCeoFirstName;
            //    entity.CompanyCeoLastName = request.CompanyCeoLastName;
            //    entity.Sign = request.Sign;
            //    entity.CompanyIcon = CompanyIcon;
            //    entity.CompanyEmail = request.CompanyEmail;
            //    entity.StripeApikey = request.StripeApikey;
            //    entity.StripePaymentMethodConfiguration = request.StripePaymentMethodConfiguration;

            //    await _repository.UpdateAsync(entity);
            //}

            var cacheKey = $"AppInformation";
            _cache.Set(cacheKey, entity);

            return new Success<AppInformation>() { Data = entity };

        }
    }
}

