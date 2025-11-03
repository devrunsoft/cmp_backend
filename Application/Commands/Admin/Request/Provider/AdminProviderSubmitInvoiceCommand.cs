using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminProviderSubmitRequestCommand : IRequest<CommandResponse<RequestResponse>>
    {
        public AdminProviderSubmitRequestCommand()
        {
        }
        public long RequestId { get; set; }
        public string Comment { get; set; }
    }
}