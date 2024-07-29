using Bazaro.Application.Responses.Base;
using Bazaro.Core.Models;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class UpdateAddressCommand : InputAddressModel,IRequest<CommandResponse>
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }

        //public long CustomerId { get; set; }
        //public string Name { get; set; }
        //public string BuildingNumber { get; set; }
        //public string Street { get; set; }
        //public string Unit { get; set; }
        //public decimal? Long { get; set; }
        //public decimal? Lat { get; set; }
    }
}
