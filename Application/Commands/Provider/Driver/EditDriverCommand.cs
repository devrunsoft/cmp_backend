using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class EditDriverCommand : DriverInput, IRequest<CommandResponse<DriverResponse>>
    {
        public EditDriverCommand()
        {
        }
        public long ProviderId { get; set; }
        public string BaseVirtualPath { get; set; }
        public long Id { get; set; }
    }
}

