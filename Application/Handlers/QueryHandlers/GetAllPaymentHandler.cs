using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllPaymentHandler : IRequestHandler<GetAllPaymentQuery, PaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetAllPaymentHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentResponse> Handle(GetAllPaymentQuery request, CancellationToken cancellationToken)
        {
            var Result = await _paymentRepository.GetPaymentByIdAsync((int)request.Id);
            return PaymentMapper.Mapper.Map<PaymentResponse>(Result);
        } 
    }
}
