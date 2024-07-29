using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using System;
using System.Collections.Generic;

namespace Bazaro.Application.Responses
{
    public class ChatResponse : BaseResponse<long>
    {
        public long InboxId { get; set; }
        public int TypeId { get; set; }
        public string Body { get; set; } 
        public string File { get; set; }
        public long? ShopUserId { get; set; }
        public bool Seen { get; set; }
        public DateTime CreatedAt { get; set; }
        public InvoiceResponse Invoice { get; set; }
    }
}
