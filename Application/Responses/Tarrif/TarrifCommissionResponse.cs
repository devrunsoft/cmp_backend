using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Responses
{
    public class TarrifCommissionResponse : BaseResponse<int>
    {
        public int Percent { get; set; }
        public int MaxPrice { get; set; }
        public string Descrtiption { get; set; }
        public TariffResponse Tarrif { get; set; }
    }
}
