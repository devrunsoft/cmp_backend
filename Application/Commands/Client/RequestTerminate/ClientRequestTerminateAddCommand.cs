using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Commands.Client
{
    public class ClientRequestTerminateAddCommand : IRequest<CommandResponse<RequestTerminate>>
    {
        public string InvoiceNumber { get; set; }
        public string Message { get; set; }
        public RequestTerminateEnum Status { get; set; } = RequestTerminateEnum.Terminate;
    }
}

