using CMPNatural.Application;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Admin.Provider
{
    public class ProviderContractController : BaseAdminApiController
    {
        public ProviderContractController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ProviderGetPaginateProviderContractCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetById([FromRoute] long Id)
        {
            var result = await _mediator.Send(new ProviderGetProviderContractCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPut("sign/{providerId}/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sign([FromRoute] long Id, [FromRoute] long providerId)
        {
            var result = await _mediator.Send(new AdminSignProviderContractCommand() { ProviderId = providerId, ProviderContractId = Id });
            if (result.IsSucces())
            {
                sendNote(MessageNoteType.ContractSignedByAdmin, result.Data.CompanyId, result.Data.OperationalAddressId, result.Data.NoteTitle);
            }
            return Ok(result);
        }
    }
}
