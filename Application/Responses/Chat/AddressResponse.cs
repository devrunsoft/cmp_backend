using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class AddressResponse : BaseResponse<long>
    {
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string BuildingNumber { get; set; }
        public string Street { get; set; }
        public string Unit { get; set; }
        public string Address { get; set; }
        public decimal? Long { get; set; }
        public decimal? Lat { get; set; }
    }
}
