using System.Security.Claims;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ScoutDirect.Api.Controllers._Base
{
    [Authorize]
    [Route("api/driver/[controller]")]
    [ApiController]
    public class BaseDriverApiController : BaseApiController
    {
        protected readonly IMediator _mediator;

        public BaseDriverApiController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;

        }

        //[NonAction]
        //public void sendNote(MessageNoteType Type, long OperationalAddressId, string Content = "")
        //{
        //    //Task.Run(async () =>
        //    //{
        //    //    using (var scope = serviceScopeFactory.CreateScope())
        //    //    {
        //    //        var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
        //    //        var _cache = cache(CacheTech.Memory);
        //    //        var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        //    //        await _mediator.Send(new ClientSendMessageNoteCommand()
        //    //        {
        //    //            ClientId = rDriverId,
        //    //            Type = Type,
        //    //            Content = Content,
        //    //            OperationalAddressId = OperationalAddressId
        //    //        });
        //    //    }
        //    //});
        //}

        protected Guid PersonId => Guid.Parse(Request.HttpContext.User.FindFirstValue("PersonId"));
        protected long rDriverId => long.Parse(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        protected long Email => long.Parse(Request.HttpContext.User.FindFirstValue("Email"));

    }
}
