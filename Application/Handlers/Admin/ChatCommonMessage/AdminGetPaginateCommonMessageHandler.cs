using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using ScoutDirect.Core.Caching;
using System;
using CMPNatural.Application.Hub;
using System.Linq;
using CMPNatural.Core.Repositories.ChatCommon;

namespace CMPNatural.Application
{
    public class AdminGetPaginateCommonMessageHandler : IRequestHandler<AdminGetPaginateCommonMessageCommand, CommandResponse<PagesQueryResponse<ChatCommonMessage>>>
    {
        private readonly IChatCommonMessageRepository _repository;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public AdminGetPaginateCommonMessageHandler(IChatCommonMessageRepository _repository, IServiceScopeFactory serviceScopeFactory)
        {
            this._repository = _repository;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<CommandResponse<PagesQueryResponse<ChatCommonMessage>>> Handle(AdminGetPaginateCommonMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p => p.ChatCommonSessionId == request.ChatCommonSessionId));

            if (result != null && result.elements.Any())
            {
            Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope()) // Create a new DI scope
                {
                    var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var _chatService = scope.ServiceProvider.GetRequiredService<IChatService>();
                    var IChatMessageRepository = scope.ServiceProvider.GetRequiredService<IChatCommonMessageRepository>();

                    var unseenList = result.elements.Where(x => !x.IsSeen && x.SenderType != ParticipantType.Admin);
                    foreach (var item in unseenList)
                    {
                        item.IsSeen = true;
                        await IChatMessageRepository.UpdateAsync(item);
                        await _chatService.SendToPerson(item.PersonId.ToString(), item.Content, ChatEnum.seen);
                    }
                }
            });
           }

            return new Success<PagesQueryResponse<ChatCommonMessage>>() { Data = result };
        }
    }
}

