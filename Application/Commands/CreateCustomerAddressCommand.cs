using Bazaro.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaro.Application.Commands
{
    public class CreateCustomerAddressCommand : IRequest<CustomerAddressResponse>
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string UnitNumber { get; set; }
    }
}
