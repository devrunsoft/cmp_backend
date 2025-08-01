using System.Security.Claims;
using CMPEmail.Email;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Core.Caching;

namespace ScoutDirect.Api.Controllers._Base
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseClientApiController : BaseApiController
    {
        protected readonly IMediator _mediator;

        public BaseClientApiController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;

        }

        [NonAction]
        public void sendNote(MessageNoteType Type , string Content = "")
        {
            Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope()) 
                {
                    var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                    var _cache = cache(CacheTech.Memory);
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                   await _mediator.Send(new ClientSendMessageNoteCommand() { ClientId = rCompanyId, Type = Type, Content = Content });
                }
            });
        }

        protected long rCompanyId => long.Parse(Request.HttpContext.User.FindFirstValue("CompanyId"));
        protected string rBusinessEmail => (Request.HttpContext.User.FindFirstValue("businessEmail"));

        protected string userName => Request.HttpContext.User.Identity.Name;

    }
}
