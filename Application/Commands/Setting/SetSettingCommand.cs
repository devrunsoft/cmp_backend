using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class SetSettingCommand : IRequest<CommandResponse>
    {
        public SetSettingCommand()
        {

        }

        public int Id { get; set; }

        //public int OrderMinPriceId { get; set; }
        //public int DeliveriesModelId { get; set; }
        //public string Name { get; set; }
        //public string Logo { get; set; }
        //public string Number { get; set; }
        //public string Revenue { get; set; }
        public bool IsEnable { get; set; }
    }
}
