using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Command
{
    public class ProviderUpdateServiceAndLocationCommand : IRequest<CommandResponse<Provider>>
    {
        public long ProviderId { get; set; }
        public double? Lat { get; set; } = 0;
        public double? Long { get; set; } = 0;
        public string? City { get; set; } = "";
        public string? Address { get; set; } = "";
        public string? County { get; set; } = "";
        public string? ManagerFirstName { get; set; }
        public string? ManagerLastName { get; set; }
        public string? ManagerPhoneNumber { get; set; }
        public List<int> ProductIds { get; set; }
    }
}

