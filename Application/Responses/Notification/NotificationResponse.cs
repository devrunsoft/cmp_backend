using Bazaro.Application.Responses.Base;
using System;

namespace Bazaro.Application.Responses
{
    public class NotificationResponse : BaseResponse<long>
    {
        public string Title { get; set; } 
        public string Body { get; set; } 
        public DateTime Date { get; set; }
        public int NotificationType { get; set; }
        public long ReciverPersonId { get; set; }
        public DateTime SentTime { get; set; }
        public DateTime? SeenTime { get; set; }
        public DateTime? ReciveTime { get; set; }
        public string Logo { get; set; }
        public string Banner { get; set; }
    }
}
