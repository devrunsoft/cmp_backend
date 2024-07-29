using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class FixedTariffResponse : BaseResponse<int>
    {
        public int Day { get; set; }
        public int Price { get; set; }
        public TariffResponse Tariff { get; set; }
    }
}
