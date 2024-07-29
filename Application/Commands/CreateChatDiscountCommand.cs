using Bazaro.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaro.Application.Commands
{
    public class CreateChatDiscountCommand : IRequest<ChatDiscountResponse>
    {
        public int Percent { get; set; }
        public int Price { get; set; }
        public int DeliveryCost { get; set; }
        public float TotalCost { get; set; }
        public bool DiscountStatus { get; set; }
    }
}
