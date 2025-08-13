using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories.Chat;
using CMPNatural.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ScoutDirect.Core.Caching;
using System;
using CMPNatural.Application.Hub;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminGetPaginateMessageHandler : IRequestHandler<AdminGetPaginateMessageCommand, CommandResponse<PagesQueryResponse<ChatMessage>>>
    {
        private readonly IChatMessageRepository _repository;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public AdminGetPaginateMessageHandler(IChatMessageRepository _repository, IServiceScopeFactory serviceScopeFactory)
        {
            this._repository = _repository;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<CommandResponse<PagesQueryResponse<ChatMessage>>> Handle(AdminGetPaginateMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p =>
                request.OperationalAddressId==0 ? (p.ChatSession.ClientId == request.ClientId) :
                (p.ChatSession.OperationalAddressId == request.OperationalAddressId),
            query => query.Include(x=>x.ChatSession)
            ));

            if (result != null && result.elements.Any())
            {
                Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope()) // Create a new DI scope
                {
                    var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var _chatService = scope.ServiceProvider.GetRequiredService<IChatService>();
                    var IChatMessageRepository = scope.ServiceProvider.GetRequiredService<IChatMessageRepository>();

                    var unseenList = result.elements.Where(x => !x.IsSeen && x.SenderType != SenderType.Admin);
                    foreach (var item in unseenList)
                    {
                        item.IsSeen = true;
                        await IChatMessageRepository.UpdateAsync(item);
                        await _chatService.SendMessageToClient(item.ClientId, item, ChatEnum.seen);
                    }
                }
            });
           }

            return new Success<PagesQueryResponse<ChatMessage>>() { Data = result };
        }
    }
}

