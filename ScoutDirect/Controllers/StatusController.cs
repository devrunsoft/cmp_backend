using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Billing;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.Document;
using CMPNatural.Application.Handlers.CommandHandlers;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers
{
    [Route("api/[controller]")]
    public class RegisterStatusController : BaseApiController
    {
        public RegisterStatusController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            //var resultlocation = await _mediator.Send(new GetLocationCompanyCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (!resultlocation.Data.Any())
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.ProfessionalInformation.ToString() });

            //}
            ////
            //var resultdocument = await _mediator.Send(new GetDocumentCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (resultdocument.Data == null)
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.DocumentSubmission.ToString() });

            //}

            //var resultbiling = await _mediator.Send(new GetBilingInformationCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (resultbiling.Data == null)
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.BillingDetails.ToString() });

            //}
            var resultCompany = await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = rCompanyId,
            })!;

            if (resultCompany.Data == null)
            {
                return Ok(new NoAcess() { Data = null , Success = false });
            }

            if(!((CompanyResponse)resultCompany.Data).Registered)
            {
                return Ok(new Success<String>() { Data = RegisterType.NotActivate.ToString() });
            }

            return Ok(new Success<String>() { Data = RegisterType.Registered.ToString() });
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> RegistartionStatusLogin()
        {
            var resultlocation = await _mediator.Send(new GetLocationCompanyCommand()
            {
                CompanyId = rCompanyId,
            })!;

            if (!resultlocation.Data.Any())
            {
                return Ok(new Success<String>() { Data = RegisterType.ProfessionalInformation.ToString() });

            }
            //
            //var resultdocument = await _mediator.Send(new GetDocumentCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (resultdocument.Data == null)
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.DocumentSubmission.ToString() });

            //}

            //var resultbiling = await _mediator.Send(new GetBilingInformationCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (resultbiling.Data == null)
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.BillingDetails.ToString() });

            //}
            var resultCompany = await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = rCompanyId,
            })!;

            if (!((CompanyResponse)resultCompany.Data).Registered)
            {
                return Ok(new Success<String>() { Data = RegisterType.NotActivate.ToString() });
            }

            return Ok(new Success<String>() { Data = RegisterType.Registered.ToString() });
        }

    }
}

