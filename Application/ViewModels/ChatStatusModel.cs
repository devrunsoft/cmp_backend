using Bazaro.Application.ViewModels.Base;
using System;

namespace Bazaro.Application.ViewModels
{
    public partial class ChatStatusModel
    {
        public ChatStatusModel()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string CustomerTitle { get; set; }
        public string ShopTitle { get; set; }
        public string Name { get; set; } = null!;
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
