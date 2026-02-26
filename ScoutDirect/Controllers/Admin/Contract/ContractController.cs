using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Application.Model;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Admin.Contract
{
    [MenuAuthorize(MenuEnum.Contract)]
    public class ContractController : BaseAdminApiController
    {
        public ContractController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllContractCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("DropDown")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetList([FromQuery] AdminDropDownContractActiveCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ContractInput request)
        {
            var result = await _mediator.Send(new AdminAddContractCommand(request));
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminDeleteContractCommand()
            {
                Id = Id
            });
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromBody] ContractInput request)
        {
            var result = await _mediator.Send(new AdminEditContractCommand(request,Id));
            return Ok(result);
        }

        [HttpGet("AllKeys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public ActionResult GetAllKeys()
        {
            var data = Enum.GetValues(typeof(ContractKeysEnum)).Cast<ContractKeysEnum>()
                .Select(x=> new NameAndValue<string>() {
                    name = x.ToString(),
                    value = x.GetDescription()
                }).ToList();

            return Ok(new Success<List<NameAndValue<string>>>() { Data = data });
        }

        [HttpGet("ProviderAllKeys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public ActionResult GetAllProviderKeys()
        {
            var data = Enum.GetValues(typeof(ContractProviderKeysEnum)).Cast<ContractProviderKeysEnum>()
                .Select(x => new NameAndValue<string>()
                {
                    name = x.ToString(),
                    value = x.GetDescription()
                }).ToList();

            return Ok(new Success<List<NameAndValue<string>>>() { Data = data });
        }

    }
}
