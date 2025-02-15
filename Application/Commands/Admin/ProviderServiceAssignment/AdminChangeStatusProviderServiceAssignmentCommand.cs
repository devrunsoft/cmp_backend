using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminChangeStatusProviderServiceAssignmentCommand : IRequest<CommandResponse<ProviderServiceAssignment>>
    {
        public long Id { get; set; }
        public ProviderServiceAssignmentStatus Status { get; set; }
    }
}