using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminReviewWhiteLabelRequestCommand : IRequest<CommandResponse<WhiteLabelRequest>>
    {
        public long WhiteLabelRequestId { get; set; }
        public WhiteLabelStatus Status { get; set; }
        public string? AdminReviewNote { get; set; }
    }
}
