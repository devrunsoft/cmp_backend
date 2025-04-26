using CMPNatural.Application;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Provider
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ProviderController : BaseAdminApiController
    {
        private readonly IWebHostEnvironment Environment;
        public ProviderController(IMediator mediator,
            IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllProviderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> get([FromRoute] int Id)
        {
            var result = await _mediator.Send(new AdminGetProviderCommand() { Id = Id });
            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromForm] ProviderInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new AdminPutProviderCommand(input, Id, wwwPath));

            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromForm] ProviderInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new AdminPostProviderCommand(input, wwwPath));

            if (result.IsSucces())
            {
                SendToProvider("Your Account Credentials", $"<p style=\"margin: 5px 0;\"> <strong>Username/Email:</strong> <span style=\"color: #16a085; font-family: monospace;\">{result.Data.Email}</span> </p> <p style=\"margin: 5px 0;\"> <strong>Password:</strong> <span style=\"color: #c0392b; font-family: monospace;\">{result.Data.Password}</span> </p>", input.Email,"Login");
            }

            return Ok(result);
        }

        [HttpPut("ChangeStatus/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ChangeStatus([FromRoute] long Id, [FromBody] AdminChangeStatusProviderCommand command)
        {
            command.ProviderId = Id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

