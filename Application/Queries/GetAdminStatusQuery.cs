using Barbara.Application.Responses;
using MediatR;
using System;

namespace ScoutDirect.Application.Queries
{
    public class GetAdminStatusQuery : IRequest<ClientStatusResponse>
    {
        public Guid PersonId { get; set; }
    }
}
