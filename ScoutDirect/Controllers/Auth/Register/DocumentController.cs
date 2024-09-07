using System;
using Barbara.Core.Models.GalleryFile;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Billing;
using CMPNatural.Application.Commands.Document;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ScoutDirect.Api.Controllers._Base;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CMPNatural.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : BaseApiController
    {
        private readonly IWebHostEnvironment Environment;
        public DocumentController(IMediator mediator, IWebHostEnvironment _environment) : base(mediator)
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
                CompanyId=rCompanyId,
                BusinessLicense=input.BusinessLicense,
                HealthDepartmentCertificate=input.HealthDepartmentCertificate,          
                BaseVirtualPath = Path.Combine(wwwPath, $"FileContent/{rCompanyId}")
            });

            return Ok(result);
        }

        [HttpPut("Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromQuery] long Id,[FromForm] DocumentInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new EditDocumentCommand()
            {
                Id= Id,
                CompanyId = rCompanyId,
                BusinessLicense = input.BusinessLicense,
                HealthDepartmentCertificate = input.HealthDepartmentCertificate,
                BaseVirtualPath = Path.Combine(wwwPath, $"FileContent/{rCompanyId}")
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

