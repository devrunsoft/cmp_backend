using System;
using CMPNatural.Core.Enums;
using Newtonsoft.Json;

namespace CMPNatural.Core.Entities
{
    public partial class ChatMessage
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public SenderType SenderType { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsInternalNote { get; set; } = false;
        public MessageType Type { get; set; }
        public bool IsSeen { get; set; } = false;

        [JsonIgnore]
        public ChatSession ChatSession { get; set; } = null!;
        [JsonIgnore]
        public List<ChatMention> Mentions { get; set; } = new();
    }
}

