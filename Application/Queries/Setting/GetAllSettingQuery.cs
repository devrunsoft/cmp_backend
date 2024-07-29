using Bazaro.Core.Base;
using Bazaro.Core.Entities;
using MediatR;

namespace Bazaro.Application.Queries
{
    public class GetAllSettingQuery : IRequest<SettingResponse>
    {  
        public int ShopId { get; set; }
        public long PersonId { get; set; }
    }
}
