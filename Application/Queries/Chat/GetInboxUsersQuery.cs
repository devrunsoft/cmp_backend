using Bazaro.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetInboxUsersQuery : IRequest<List<Tuple<string, InboxUserModel>>>
    { 
        public long InboxId { get; set; } 
    }
}
