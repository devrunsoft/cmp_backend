namespace Bazaro.Core.Models
{
    public class InputAddressModel
    {
        //public long CustomerId { get; set; }
        public string Name { get; set; }
        public string BuildingNumber { get; set; }
        public string Street { get; set; }
        public string Unit { get; set; }
        public decimal? Long { get; set; }
        public decimal? Lat { get; set; }
    }
}
