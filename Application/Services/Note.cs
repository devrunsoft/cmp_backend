using System;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ScoutDirect.Core.Caching;

namespace CMPNatural.Api.Service
{
	public class Note
	{
        protected long AdminId { get; set; }
        protected IServiceScopeFactory serviceScopeFactory { get; set; }
        protected IMediator _mediator { get; set; }
        public Note(long AdminId, IServiceScopeFactory serviceScopeFactory, IMediator mediator)
        {
            this.AdminId = AdminId;
            this.serviceScopeFactory = serviceScopeFactory;
            this._mediator = mediator;
        }

        public void adminSendNote(MessageNoteType Type, long ClientId, long OperationalAddressId, string Content = "" )
        {
            _adminSendNote(Type, ClientId, OperationalAddressId, null, Content);
        }

        public void adminSendNote(MessageNoteType Type, long ClientId, long OperationalAddressId, object? Payload, string Content = "")
        {
            _adminSendNote(Type, ClientId, OperationalAddressId, Payload, Content);
        }

        private void _adminSendNote(MessageNoteType Type, long ClientId, long OperationalAddressId, object? Payload, string Content = "")
        {
            var scopeFactory = serviceScopeFactory;
            var adminId = AdminId;

            _ = Task.Run(async () =>
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                    var _cache = cache(CacheTech.Memory);
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    try
                    {
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    try
                    {
                        var result=  await _mediator.Send(new AdminSendMessageNoteCommand() { Data = Payload, ClientId = ClientId, OperationalAddressId = OperationalAddressId, Type = Type, AdminId = adminId, Content = Content });

                        await _mediator.Send(new AdminAddNotificationCommand()
                        {
                            type = NotificationType.Note,
                            Title = Type.Description(),
                            Body = Content,
                            Payload = result.Data
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }
    }
}

