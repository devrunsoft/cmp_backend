using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Admin.Manifest
{
    [MenuAuthorize(MenuEnum.Manifests)]
    public class ManifestController : BaseAdminApiController
    {
        public ManifestController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet("AllManifestRoute/{ProviderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> AllAssign([FromRoute] long ProviderId)
        {
            var result = await _mediator.Send(new ProviderGetAllRouteManifestCommand() { ProviderId = ProviderId });
            return Ok(result);
        }


        [HttpGet("Completing/Company/{CompanyId}/OperationalAddress/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> getCompleting([FromRoute] long CompanyId, [FromRoute] long OperationalAddressId )
        {
            var result = await _mediator.Send(new AdminGetCompletingManifestCommand() { CompanyId = CompanyId , OperationalAddressId = OperationalAddressId });
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetId([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminGetManifestCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPut("Assign/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long Id, [FromBody] AdminAssignManifestCommand command)
        {
            command.Id = Id;
            var result = await _mediator.Send(command);

            if (result.IsSucces())
            {
                sendNote(MessageNoteType.ManifestAssigned, result.Data.CompanyId , result.Data.OperationalAddressId, result.Data.NoteTitle);
            }

            return Ok(result);
        }

        [HttpPut("ChangeAssign/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ChangeAssign([FromRoute] long Id, [FromBody] AdminChangeAssignManifestCommand command)
        {
            command.Id = Id;
            var result = await _mediator.Send(command);

            if (result.IsSucces())
            {
                sendNote(MessageNoteType.ManifestAssigned, result.Data.CompanyId, result.Data.OperationalAddressId, result.Data.NoteTitle);
            }
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminGetAllManifestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Completed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetComplete([FromQuery] AdminGetAllManifestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Completed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutComplete([FromQuery] AdminGetAllManifestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Cancel/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CancelManifest([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminCancelManifestCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPut("ReActivate/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ReActivate([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminReActivatelManifestCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPut("Processing/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutProcessing([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminProcessingManifestCommand() { Id = Id });
            return Ok(result);
        }


        [HttpPut("Submited")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> SubmitInvoice([FromQuery] AdminGetAllManifestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}

