using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class TariffResponse : BaseResponse<int>
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public int TariffTypeId { get; set; } 

        public virtual TarrifCommissionResponse Commission { get; set; }
        public virtual FixedTariffResponse FixedTariff { get; set; }
    }
}
