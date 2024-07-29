using Barbara.Application.Responses.Base; 
using MediatR; 

namespace Barbara.Application.Queries
{
    public class DeleteRelatedInboxChacheCommand : IRequest<CommandResponse>
    {
        public int? ShopId { get; set; }
        public long? PersonId { get; set; }
        public long? ShopUserPersonId { get; set; } 
    }
}
