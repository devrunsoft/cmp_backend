using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using System;
using System.Collections.Generic;

namespace Bazaro.Application.Responses
{
    public class InboxResponse : BaseResponse<long>
    {
        public int ShopId { get; set; }
        public long CustomerId { get; set; }
        //public bool IsOnline { get; set; }

        //public string Mobile { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Unread { get; set; }

        public InboxStatusResponse StatusCode { get; set; }
        public ChatResponse LastChat { get; set; }
        public AddressResponse Address { get; set; }
    }
}
