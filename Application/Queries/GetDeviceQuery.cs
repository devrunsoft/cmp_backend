using Barbara.Application.Responses;
using Barbara.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace Barbara.Application.Queries
{
    public class GetDeviceQuery : IRequest<List<Device>>
    {
        public Guid PersonId { get; set; }
    }
}
