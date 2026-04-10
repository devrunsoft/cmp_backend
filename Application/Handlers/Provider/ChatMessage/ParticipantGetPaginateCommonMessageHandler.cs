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
using CMPNatural.Core.Repositories.ChatCommon;

namespace CMPNatural.Application
{
    public class ParticipantGetPaginateCommonMessageHandler : IRequestHandler<ParticipantGetPaginateCommonMessageCommand, CommandResponse<PagesQueryResponse<ChatCommonMessage>>>
    {
        private readonly IChatCommonMessageRepository _repository;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ParticipantGetPaginateCommonMessageHandler(IChatCommonMessageRepository _repository, IServiceScopeFactory serviceScopeFactory)
        {
            this._repository = _repository;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<CommandResponse<PagesQueryResponse<ChatCommonMessage>>> Handle(ParticipantGetPaginateCommonMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p => p.ChatSession.ParticipantId == request.ParticipantId,
                 query => query.Include(x=>x.ChatSession)
                ));

            if (result != null && result.elements.Any())
            {
                Task.Run(async () =>
                {
                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                        var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var _chatService = scope.ServiceProvider.GetRequiredService<IChatService>();
                        var IChatMessageRepository = scope.ServiceProvider.GetRequiredService<IChatCommonMessageRepository>();

                        var unseenList = result.elements.Where(x => !x.IsSeen && x.SenderType != x.ChatSession.ParticipantType);
                        foreach (var item in unseenList)
                        {
                            item.IsSeen = true;
                            await IChatMessageRepository.UpdateAsync(item);
                            await _chatService.SendCommonMessageToPerson(item.PersonId.ToString(), item, ChatEnum.seen);
                        }
                    }
                });
            }

            return new Success<PagesQueryResponse<ChatCommonMessage>>() { Data = result };
        }
    }
}
