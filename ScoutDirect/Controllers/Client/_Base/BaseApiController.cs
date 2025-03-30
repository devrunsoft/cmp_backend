
using CMPEmail.Email;
using CMPNatural.Api.Controllers.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Core.Caching;

namespace ScoutDirect.Api.Controllers._Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        protected readonly IMediator _mediator;

        public BaseApiController(IMediator mediator)
        {
            _mediator = mediator;

        }

        protected string userName => Request.HttpContext.User.Identity.Name;


        [NonAction]
        public void sendEmail(string subject, string body, string email)
        {
            emailSender.Send(subject, body, email);
        }

        [NonAction]
        public void sendEmailToAdmin(string subject, string body)
        {
           emailSender.SendAdmin(subject, body, serviceScopeFactory , null);
        }

        [NonAction]
        public void sendEmailToClient(long companyId ,string subject, string body)
        {
             emailSender.SendClient(companyId, subject, body, serviceScopeFactory, null);
        }

        [NonAction]
        public void sendEmailToAdmin(string subject, string body,string Link)
        {
             emailSender.SendAdmin(subject, body, serviceScopeFactory, Link);
        }

        [NonAction]
        public void sendEmailToClient(long companyId, string subject, string body,string Link)
        {
            emailSender.SendClient(companyId, subject, body, serviceScopeFactory, Link);
        }


        private IEmailSender _emailSender;
        protected IEmailSender emailSender =>
            _emailSender ??= HttpContext.RequestServices.GetRequiredService<IEmailSender>();

        private IServiceScopeFactory _serviceScopeFactory;
        protected IServiceScopeFactory serviceScopeFactory =>
            _serviceScopeFactory ??= HttpContext.RequestServices.GetRequiredService<IServiceScopeFactory>();

    }
}
