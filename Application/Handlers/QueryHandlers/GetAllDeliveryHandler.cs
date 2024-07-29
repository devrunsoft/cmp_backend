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
    public class GetAllDeliveryHandler : IRequestHandler<GetAllDeliveryQuery, List<DeliveryResponse>>
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public GetAllDeliveryHandler(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<List<DeliveryResponse>> Handle(GetAllDeliveryQuery request, CancellationToken cancellationToken)
        {
            var deliveries = await _deliveryRepository.GetPagedAsync(request, p => p.IsActive == true);
            return deliveries.Select(p => DeliveryMapper.Mapper.Map<DeliveryResponse>(p)).ToList();
        }
    }
}

