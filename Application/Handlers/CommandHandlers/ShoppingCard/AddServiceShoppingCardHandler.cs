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

namespace CMPNatural.Application.Handlers
{

    public class AddServiceShoppingCardHandler : IRequestHandler<AddServiceShoppingCardCommand, CommandResponse<ShoppingCard>>
    {
        private readonly IShoppingCardRepository _shoppingCardRepository;
        private readonly IServiceAppointmentRepository _serviceAppointmentRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly ILocationCompanyRepository _locationCompanyRepository;

        public AddServiceShoppingCardHandler(IShoppingCardRepository shoppingCardRepository , IServiceAppointmentRepository billingInformationRepository,
            IProductPriceRepository productPriceRepository, IOperationalAddressRepository operationalAddressRepository, ILocationCompanyRepository locationCompanyRepository)
        {
            _shoppingCardRepository = shoppingCardRepository;
            _serviceAppointmentRepository = billingInformationRepository;
            _productPriceRepository = productPriceRepository;
            _operationalAddressRepository = operationalAddressRepository;
            _locationCompanyRepository = locationCompanyRepository;
        }

        public async Task<CommandResponse<ShoppingCard>> Handle(AddServiceShoppingCardCommand request, CancellationToken cancellationToken)
        {
            var price =(await _productPriceRepository.GetAsync(p=>p.Id == request.ProductPriceId && p.Enable!=false, query => query.Include(i => i.Product))).FirstOrDefault();
            var address = (await _operationalAddressRepository.GetAsync(p => p.Id == request.OperationalAddressId)).FirstOrDefault();
            var locations = (await _locationCompanyRepository.GetAsync(p => request.LocationCompanyIds.Any(e=>e==p.Id),
                p=>p.Include(p=>p.CapacityEntity))).ToList();


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
                ServiceId = price.Product.ServiceType?? (int)ServiceType.Other,
                LocationCompanyIds = string.Join(",", request.LocationCompanyIds),
                Qty = locations.Sum(x=>x.CapacityEntity.Qty) ,
                ProductPriceId = request.ProductPriceId,
                ProductId = request.ProductId

            };
            var result = await _shoppingCardRepository.AddAsync(entity);

            return new Success<ShoppingCard>() { Data = result, Message = "Service Enrolled Successfully!" };

        }

    }
}

