using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class SearchShopProfileStatusResponse 
    {
        public int Comments { get; set; }
        public int SatisfiedCustomers { get; set; }
        public int DissatisfiedCustomers { get; set; }
    }
}
