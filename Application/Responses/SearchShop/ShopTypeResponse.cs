using Bazaro.Application.Responses.Base;
using System;

namespace Bazaro.Application.Responses
{
    public class ShopTypeResponse : BaseResponse<int>
    { 
        public string Title { get; set; }  
        public string Name { get; set; } 
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
