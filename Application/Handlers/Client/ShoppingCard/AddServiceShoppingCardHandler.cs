using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Enums;
using System.Linq;
using System.ServiceModel.Channels;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Repositories;
using Stripe;
using System.Collections.Generic;

namespace CMPNatural.Application.Handlers
{

    public class AddServiceShoppingCardHandler : IRequestHandler<AddServiceShoppingCardCommand, CommandResponse<ShoppingCard>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IShoppingCardRepository _shoppingCardRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly ILocationCompanyRepository _locationCompanyRepository;

        public AddServiceShoppingCardHandler(IShoppingCardRepository shoppingCardRepository , ICompanyRepository _companyRepository,
            IProductPriceRepository productPriceRepository, IOperationalAddressRepository operationalAddressRepository, ILocationCompanyRepository locationCompanyRepository)
        {
            this._companyRepository = _companyRepository;
            _shoppingCardRepository = shoppingCardRepository;
            _productPriceRepository = productPriceRepository;
            _operationalAddressRepository = operationalAddressRepository;
            _locationCompanyRepository = locationCompanyRepository;
        }

        public async Task<CommandResponse<ShoppingCard>> Handle(AddServiceShoppingCardCommand request, CancellationToken cancellationToken)
        {

            var company = (await _companyRepository.GetByIdAsync(request.CompanyId));
            if (string.IsNullOrEmpty(company.BusinessEmail))
            {
                company.Status = CompanyStatus.Incomplete_information;
            }
            if (company.Status != CompanyStatus.Approved)
            {
                switch (company.Status)
                {
                    case CompanyStatus.Incomplete_information:
                        return new NoAcess<ShoppingCard>()
                        {
                            Message = "Your company profile is incomplete."
                        };

                    case CompanyStatus.Blocked:
                        return new NoAcess<ShoppingCard>()
                        {
                            Message = "Your company account is currently blocked. Please contact support to resolve this issue."
                        };
                    case CompanyStatus.Pending:
                        return new NoAcess<ShoppingCard>()
                        {
                            Message = "Please confirm your email address to continue."
                        };
                }

            }

            int Qty = 1;
            var price =(await _productPriceRepository.GetAsync(p=>p.Id == request.ProductPriceId && p.Enable!=false, query => query.Include(i => i.Product))).FirstOrDefault();
            var address = (await _operationalAddressRepository.GetAsync(p => p.Id == request.OperationalAddressId)).FirstOrDefault();

            if (price.Product.ServiceType == (int)ServiceType.Other)
            {
                var loc = (await _locationCompanyRepository.GetAsync(x => x.Type == (int)LocationType.Other && x.OperationalAddressId == request.OperationalAddressId)).FirstOrDefault();
                request.LocationCompanyIds= new List<long>() { loc.Id };
            }



                if (price.Product.ServiceType == (int)ServiceType.Cooking_Oil_Collection || price.Product.ServiceType == (int)ServiceType.Grease_Trap_Management) {
                var locations = (await _locationCompanyRepository.GetAsync(p => request.LocationCompanyIds.Any(e => e == p.Id),
                    p => p.Include(p => p.CapacityEntity))).ToList();
                if(locations.Count == 0)
                {
                    return new NoAcess<ShoppingCard>() { Message = "Please at least select a location" };
                }

                Qty = locations.Sum(x => x.CapacityEntity.Qty);
            }
            else
            {
                Qty = request.qty;
            }
            var dof = request.DayOfWeek.Count == 0 ? request.DayOfWeek : Enum.GetValues(typeof(DayOfWeekEnum)).Cast<DayOfWeekEnum>().ToList();


            var entity = new ShoppingCard()
            {
                CompanyId = request.CompanyId,
                ServicePriceCrmId = "",
                ServiceCrmId = "",
                StartDate = request.StartDate ?? DateTime.Now,
                OperationalAddressId = request.OperationalAddressId,
                FrequencyType = request.FrequencyType,
                Name = price.Product.Name,
                AddressName = address.Name,
                Address = address.Address,
                PriceName = price.Name,
                ServiceKind = (int) request.ServiceKind,
                ServiceId = price.Product.ServiceType,
                LocationCompanyIds = string.Join(",", request.LocationCompanyIds),
                Qty = Qty,
                ProductPriceId = request.ProductPriceId,
                ProductId = request.ProductId,
                DayOfWeek = string.Join(",", dof.Select(x => x.GetDescription())),
                FromHour = request.FromHour,
                ToHour = request.ToHour,
                //BillingInformationId = request.BillingInformationId

            };
            var result = await _shoppingCardRepository.AddAsync(entity);

            return new Success<ShoppingCard>() { Data = result, Message = "Service Enrolled Successfully!" };

        }

    }
}

