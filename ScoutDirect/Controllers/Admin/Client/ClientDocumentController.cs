using System;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Document;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class ClientDocumentController : BaseAdminClientApiController
    {
        private readonly IWebHostEnvironment Environment;
        public ClientDocumentController(IMediator mediator, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(mediator, httpContextAccessor)
        {
            Environment = _environment;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {

            var result = await _mediator.Send(new GetDocumentCommand()
            {
                CompanyId = rCompanyId,
            });
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromForm] DocumentInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new AddDocumentCommand()
            {
                CompanyId = rCompanyId,
                BusinessLicense = input.BusinessLicense,
                HealthDepartmentCertificate = input.HealthDepartmentCertificate,
                BaseVirtualPath = Path.Combine(wwwPath, $"FileContent/Client/{rCompanyId}")
            });

            return Ok(result);
        }

        [HttpPut("Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromQuery] long Id, [FromForm] DocumentInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new EditDocumentCommand()
            {
                Id = Id,
                CompanyId = rCompanyId,
                BusinessLicense = input.BusinessLicense,
                HealthDepartmentCertificate = input.HealthDepartmentCertificate,
                BaseVirtualPath = Path.Combine(wwwPath, $"FileContent/Client/{rCompanyId}")
            });


            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {

            var result = await _mediator.Send(new DeleteDocumentCommand()
            {
                Id = Id,
                CompanyId = rCompanyId
            });
            return Ok(result);
        }

    }
}

