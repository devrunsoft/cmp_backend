using Bazaro.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaro.Application.Commands
{
    public class CustomerProfileCommand : IRequest<CustomerProfileResponse>
    {
        public bool Blocked { get; set; }
    }
}
