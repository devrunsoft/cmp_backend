using Barbara.Application.Responses.Base; 
using MediatR;

namespace Bazaro.Application.Commands
{
    public class CreateAddressCommand : InputAddressModel, IRequest<CommandResponse>
    {
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
