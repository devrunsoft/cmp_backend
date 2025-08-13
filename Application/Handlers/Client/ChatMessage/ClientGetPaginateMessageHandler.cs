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
using CMPNatural.Application.Hub;
using Microsoft.Extensions.DependencyInjection;
using ScoutDirect.Core.Caching;
using System;
using System.Linq;

namespace CMPNatural.Application
{
    public class ClientGetPaginateMessageHandler : IRequestHandler<ClientGetPaginateMessageCommand, CommandResponse<PagesQueryResponse<ChatMessage>>>
    {
        private readonly IChatMessageRepository _repository;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ClientGetPaginateMessageHandler(IChatMessageRepository _repository, IServiceScopeFactory serviceScopeFactory)
        {
            this._repository = _repository;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<CommandResponse<PagesQueryResponse<ChatMessage>>> Handle(ClientGetPaginateMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p => p.ChatSession.ClientId == request.CompanyId && p.ChatSession.OperationalAddressId == request.OperationalAddressId,
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

                        var unseenList = result.elements.Where(x => !x.IsSeen && x.SenderType != SenderType.Client);
                        foreach (var item in unseenList)
                        {
                            item.IsSeen = true;
                            await _repository.UpdateAsync(item);
                            await _chatService.SendMessageToClient(item.ClientId, item, ChatEnum.seen);
                        }
                    }
                });
            }

            return new Success<PagesQueryResponse<ChatMessage>>() { Data = result };
        }
    }
}

