
namespace Barbara.Application.Responses
{
    public class ClientStatusResponse
    {
        public bool Registred { get; set; }
        public bool Accepted { get; set; }
        public string RegisterType { get; set; }
        public long? ScoutId { get; set; }
        public long? PlayerId { get; set; }
    }
}
