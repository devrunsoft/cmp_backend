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
        public void sendNote(MessageNoteType Type, long OperationalAddressId, string Content = "")
        {
            var scopeFactory = serviceScopeFactory;
            var companyId = rCompanyId;

            _ = Task.Run(async () =>
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var cache = scope.ServiceProvider.GetRequiredService<Func<CacheTech, ICacheService>>();
                    var _cache = cache(CacheTech.Memory);
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    try
                    {
                        await _mediator.Send(new ClientSendMessageNoteCommand()
                        {
                            ClientId = companyId,
                            Type = Type,
                            Content = Content,
                            OperationalAddressId = OperationalAddressId
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            });
        }

        protected long rCompanyId => long.Parse(Request.HttpContext.User.FindFirstValue("CompanyId"));
        protected string rBusinessEmail => (Request.HttpContext.User.FindFirstValue("businessEmail"));

        protected string userName => Request.HttpContext.User.Identity.Name;

    }
}
