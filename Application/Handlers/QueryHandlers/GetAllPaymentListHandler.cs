using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllPaymentListHandler : IRequestHandler<GetAllPaymentListQuery, List<PaymentListResponse>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetAllPaymentListHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
 
        public async Task<List<PaymentListResponse>> Handle(GetAllPaymentListQuery request, CancellationToken cancellationToken)
        {
            var Result = await _paymentRepository.GetPaymentsListByShopIdAsync((int)request.ShopId);
            return Result.Select(p => PaymentListMapper.Mapper.Map<PaymentListResponse>(p)).ToList();
        }
    }
}
