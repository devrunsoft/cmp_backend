using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class InboxInfoResponse : BaseResponse<long>
    {
        public long CustomerId { get; set; }
        public long AddressId { get; set; }
    }
}
