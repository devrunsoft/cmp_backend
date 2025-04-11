using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Billing;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class ClientInformationController : BaseAdminClientApiController
    {
        public ClientInformationController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromBody] UpdateCompanyInput request)
        {
            var resultBillingAddress = await _mediator.Send(new AddOrUpdateBillingAddressCommand()
            {
                CompanyId = rCompanyId,
                Address = request.BillingAddress,
                City = request.City,
                ZIPCode = request.ZIPCode,
                State = request.State
            });

            var result = await _mediator.Send(new UpdateCompanyCommand()
            {
                CompanyId = rCompanyId,
                CompanyName = request.CompanyName,
                Position = request.Position,
                PrimaryFirstName = request.PrimaryFirstName,
                PrimaryLastName = request.PrimaryLastName,
                PrimaryPhonNumber = request.PrimaryPhonNumber,
                SecondaryFirstName = request.SecondaryFirstName,
                SecondaryLastName = request.SecondaryLastName,
                SecondaryPhoneNumber = request.SecondaryPhoneNumber,
            });
            return Ok(result);
        }
    }
}

