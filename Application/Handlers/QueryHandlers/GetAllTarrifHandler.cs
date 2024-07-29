using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Enums;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllTarrifHandler : IRequestHandler<GetAllTarrifQuery, List<TariffResponse>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITariffRepository _tariffRepository;

        public GetAllTarrifHandler(ITariffRepository tariffRepository, IPaymentRepository paymentRepository)
        {
            _tariffRepository = tariffRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<List<TariffResponse>> Handle(GetAllTarrifQuery request, CancellationToken cancellationToken)
        {
            //var Result = await _tariffRepository.GetAllPagedAsync(request);
            //return Result.Select(p => TariffMapper.Mapper.Map<TariffResponse>(p)).ToList();

            var payment = await _paymentRepository.GetLastPaymentByShopIdAsync(request.ShopId);

            List<TariffResponse> list = new List<TariffResponse>();

            if (payment != null && payment.Tariff != null && payment.Tariff.TariffTypeId == (int)TariffType.Fixed)
            {
                list.Add(TariffMapper.Mapper.Map<TariffResponse>(payment.Tariff));
            }
            else
            {
                list.Add(new TariffResponse()
                {
                    Title = "بر اساس پرداخت ثابت دوره ای",
                    Name = "تعرفه ثابت",
                    TariffTypeId = (int)TariffType.Fixed,
                    Commission = null,
                    FixedTariff = null,
                });
            }

            if (payment != null && payment.Tariff != null && payment.Tariff.TariffTypeId == (int)TariffType.Commission)
            {
                list.Add(TariffMapper.Mapper.Map<TariffResponse>(payment.Tariff));
            }
            else
            {
                list.Add(new TariffResponse()
                {
                    Title = "بر اساس درصدی از مبلغ سفارش موفق",
                    Name = "کمیسیون",
                    TariffTypeId = (int)TariffType.Commission,
                    Commission = null,
                    FixedTariff = null,
                });
            } 

            return list;
        }
    }
}