using Bazaro.Application.Responses.Base;
using System;

namespace Bazaro.Application.Responses
{
    public class DeliveryResponse : BaseResponse<int>
    {  
        public string Title { get; set; } 
        public string Name { get; set; }  
        public int Price { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}
