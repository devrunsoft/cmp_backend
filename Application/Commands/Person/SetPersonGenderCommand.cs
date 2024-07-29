using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class SetPersonGenderCommand : IRequest<CommandResponse>
    {
        //public int ShopUserId { get; set; }
        public long PersonId { get; set; }
        public int Gender { get; set; }
    }
}
