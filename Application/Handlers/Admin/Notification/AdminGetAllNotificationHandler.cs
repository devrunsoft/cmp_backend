using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Application.Hub;
using Microsoft.Extensions.DependencyInjection;
using ScoutDirect.Core.Caching;
using System;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminGetAllNotificationHandler : IRequestHandler<AdminGetAllNotificationCommand, CommandResponse<PagesQueryResponse<Notification>>>
    {
        private readonly INotificationRepository _repository;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public AdminGetAllNotificationHandler(INotificationRepository _repository, IServiceScopeFactory serviceScopeFactory)
        {
            this._repository = _repository;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<CommandResponse<PagesQueryResponse<Notification>>> Handle(AdminGetAllNotificationCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request));

            if (result != null && result.elements.Any())
            {
                Task.Run(async () =>
                {
                    using (var scope = serviceScopeFactory.CreateScope()) // Create a new DI scope
                    {
                        var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                        var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var IChatMessageRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                        var unseenList = result.elements.Where(x => x.Seen  == null);
                        foreach (var item in unseenList)
                        {
                            item.Seen = DateTime.Now;
                            await IChatMessageRepository.UpdateAsync(item);
                        }
                    }
                });
            }

            return new Success<PagesQueryResponse<Notification>>() { Data = result };
        }
    }
}

