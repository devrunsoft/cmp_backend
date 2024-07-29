using Bazaro.Application.Commands;
using Bazaro.Application.Mapper;
using Bazaro.Application.Responses;
using Bazaro.Application.Responses.Base;
using Bazaro.Application.Services;
using Bazaro.Core.Entities;
using Bazaro.Core.Enums;
using Bazaro.Core.Repositories;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class CheckDeferredOrderHandler : IRequestHandler<CheckDeferredOrderCommand, CommandResponse>
    {
        private readonly IInboxRepository _inboxRepository;

        public CheckDeferredOrderHandler(IInboxRepository inboxRepository)
        {
            _inboxRepository = inboxRepository;
        }

        public async Task<CommandResponse> Handle(CheckDeferredOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _inboxRepository.GetDeferredOrder(request.InboxId, request.ChatId);

            if (order != null)
            {
                var body = WebUtility.UrlEncode($"شما یک سفارش جدید از {order.Customer.IdNavigation.Name} دارید ، لطفا در بازارُو به آن رسیدگی فرمایید");
                await KavehNegarService.Call(order.Shop.ShopInfo.PhoneOne, body);

                return new Success() { };
            }

            return new CommandResponse() { Success = false };
        }

    }
}
