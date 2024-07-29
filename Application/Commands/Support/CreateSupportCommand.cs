using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class CreateSupportCommand : IRequest<CommandResponse>
    {
        public int SupportCategoryId { get; set; }
        public string Description { get; set; }
        public long? PersonId { get; set; }
    }
}
