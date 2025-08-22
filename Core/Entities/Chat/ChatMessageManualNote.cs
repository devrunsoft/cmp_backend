using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class ChatMessageManualNote
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public long ClientId { get; set; }
        public long OperationalAddressId { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public ChatSession ChatSession { get; set; }
    }
}

