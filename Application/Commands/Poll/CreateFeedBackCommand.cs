using Bazaro.Api.Models;
using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class CreateFeedBackCommand : InputFeedBackModel, IRequest<CommandResponse>
    {
        public long CreatorPersonId { get; set; }
        public bool IsShop { get; set; } 
    }
}
