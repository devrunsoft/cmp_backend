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

namespace CMPNatural.Application.Handlers
{

    public class AddServiceShoppingCardHandler : IRequestHandler<AddServiceShoppingCardCommand, CommandResponse<ShoppingCard>>
    {
        private readonly IShoppingCardRepository _shoppingCardRepository;
        private readonly IServiceAppointmentRepository _serviceAppointmentRepository;

        public AddServiceShoppingCardHandler(IShoppingCardRepository shoppingCardRepository , IServiceAppointmentRepository billingInformationRepository)
        {
            _shoppingCardRepository = shoppingCardRepository;

            _serviceAppointmentRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<ShoppingCard>> Handle(AddServiceShoppingCardCommand request, CancellationToken cancellationToken)
        {

            //var resultService = (await _serviceAppointmentRepository.GetList(
            //       (p) => p.OperationalAddressId == request.OperationalAddressId &&
            //      p.CompanyId == request.CompanyId &&
            //      p.Status != (int)ServiceStatus.canceled&&
            //       p.ServiceCrmId == request.ServiceCrmId
            //       )
            //        ).ToList();

            //if (resultService.Count() > 0)
            //{
            //    return new NoAcess<ShoppingCard>()
            //    {
            //        Message = resultService.FirstOrDefault().Status == (int)ServiceStatus.draft ?
            //        "you are already enrolled this item, check your invoices"
            //        : "you are already enrolled in repeating services, if you want to change, please cancel your existing service."
            //    };
            //}

            //var resultShoppingCard = (await _shoppingCardRepository.GetAsync(
            //(p) => p.OperationalAddressId == request.OperationalAddressId &&
            // p.CompanyId == request.CompanyId &&
            // p.ServiceCrmId == request.ServiceCrmId
            //                   )).ToList();

            //if (resultShoppingCard.Count() > 0)
            //{
            //    return new NoAcess<ShoppingCard>() {
            //     Message=   "you are already add this item in your cart"
            //    };
            //}

            var entity = new ShoppingCard()
            {
                CompanyId = request.CompanyId,
                ServicePriceCrmId = request.ServicePriceId,
                ServiceCrmId = request.ServiceCrmId,
                StartDate = request.StartDate ?? DateTime.Now,
                OperationalAddressId = request.OperationalAddressId,
                FrequencyType = request.FrequencyType,
                Name = request.Name,
                AddressName = request.AddressName,
                PriceName = request.PriceName,
                ServiceKind = (int) request.ServiceKind,
                ServiceId =(int)request.ServiceTypeId,
                LocationCompanyIds = string.Join(",", request.LocationCompanyIds),
                Address = request.Address,
                Qty = request.qty,
                ProductPriceId = request.ProductPriceId

            };
            var result = await _shoppingCardRepository.AddAsync(entity);

            return new Success<ShoppingCard>() { Data = result, Message = "Service Enrolled Successfully!" };

        }

    }
}

