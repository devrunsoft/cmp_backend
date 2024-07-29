namespace Barbara.Application.Responses.Base
{
    public class SendSmsResponse
    {
        public bool Success { get; set; } = false;
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
