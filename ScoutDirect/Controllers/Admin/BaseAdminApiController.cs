using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CMPNatural.Api.Controllers.Service;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Models;
using CMPNatural.Core.Repositories.Chat;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Core.Caching;

namespace CMPNatural.Api.Controllers.Admin
{
    [Authorize(Roles = "SuperAdmin,LimitedAdmin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class BaseAdminApiController : BaseApiController
    {
        protected readonly IMediator _mediator;
        public BaseAdminApiController(IMediator mediator): base(mediator)
        {
            _mediator = mediator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var isAdmin = bool.Parse(context.HttpContext.User.FindFirstValue("isAdmin"));

                if (!isAdmin)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }

        [NonAction]
        public void sendNote(MessageNoteType Type, long ClientId, long OperationalAddressId, string Content = "")
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
                        await _mediator.Send(new AdminSendMessageNoteCommand() { ClientId = ClientId, OperationalAddressId = OperationalAddressId, Type = Type, AdminId = adminId, Content = Content });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            });
        }

        private IChatMessageNoteRepository _chatMessageNoteRepository;
        protected IChatMessageNoteRepository chatMessageNoteRepository =>
            _chatMessageNoteRepository ??= HttpContext.RequestServices.GetRequiredService<IChatMessageNoteRepository>();

        protected Guid PersonId => Guid.Parse(Request.HttpContext.User.FindFirstValue("PersonId"));
        protected long AdminId => long.Parse(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        protected long Email => long.Parse(Request.HttpContext.User.FindFirstValue("Email"));

    }
}
