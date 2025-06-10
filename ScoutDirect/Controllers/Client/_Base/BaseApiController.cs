
using CMPEmail.Email;
using CMPNatural.Api.Controllers.Service;
using CMPNatural.Core.Models;
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
        public void SendToProvider(string subject, string body, string email, string buttonText)
        {
            emailSender.SendToProvider(_appSetting,subject, body, email, buttonText);
        }

        [NonAction]
        public void SendToClient(string subject, string body, string email, string buttonText)
        {
            emailSender.SendToClient(_appSetting , subject, body, email, buttonText);
        }

        [NonAction]
        public void sendEmail(string subject, string body, string email, string link="", string buttonText="")
        {
            emailSender.Send(subject, body, email, link , buttonText);
        }

        [NonAction]
        public void sendEmailToAdmin(string subject, string body)
        {
           emailSender.SendAdmin(_appSetting, subject, body, serviceScopeFactory , null , null);
        }

        [NonAction]
        public void sendEmailToClient(long companyId ,string subject, string body)
        {
             emailSender.SendClient(_appSetting,companyId, subject, body, serviceScopeFactory, null , null);
        }

        [NonAction]
        public void sendEmailToAdmin(string subject, string body,string Link, string buttonText)
        {
             emailSender.SendAdmin(_appSetting, subject, body, serviceScopeFactory, Link , buttonText);
        }

        [NonAction]
        public void sendEmailToClient(long companyId, string subject, string body,string Link, string buttonText)
        {
            emailSender.SendClient(_appSetting,companyId, subject, body, serviceScopeFactory, Link, buttonText);
        }


        private IEmailSender _emailSender;
        protected IEmailSender emailSender =>
            _emailSender ??= HttpContext.RequestServices.GetRequiredService<IEmailSender>();

        public AppSetting _appSetting;
        public AppSetting appSetting =>
            _appSetting ??= HttpContext.RequestServices.GetRequiredService<AppSetting>();

        private IServiceScopeFactory _serviceScopeFactory;
        protected IServiceScopeFactory serviceScopeFactory =>
            _serviceScopeFactory ??= HttpContext.RequestServices.GetRequiredService<IServiceScopeFactory>();

    }
}
