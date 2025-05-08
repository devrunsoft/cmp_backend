using System;
using CMPNatural.Application.Commands.Company;
using System.ComponentModel.Design;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Logger;
using infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ScoutDirect.Core.Caching;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.infrastructure.Logger
{
    public class CustomDbLogger : ICustomDbLogger
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CustomDbLogger(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void LogAsync(Guid PersonId, string FullName, LogTypeEnum LogType, string Action)
        {
            Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<ScoutDBContext>();
                var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>()(CacheTech.Memory);
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                try
                {
                    var log = new AppLog
                    {
                        PersonId = PersonId,
                        FullName = FullName,
                        LogType = LogType,
                        Action = Action,
                        CreatedAt = DateTime.Now
                    };

                    context.AppLog.Add(log);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Log to console or ignore — avoid crashing
                }
            });
        }
    }

}