using Bazaro.Application.Responses.Base;
using MediatR;
using System;

namespace Bazaro.Application.Commands
{
    public class SetPersonBirthDateCommand : IRequest<CommandResponse>
    {
        public long PersonId { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
