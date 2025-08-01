using System;
namespace CMPNatural.Core.Entities
{
    public partial class ChatNotification
    {
        public long Id { get; set; }
        public long ChatMentionId { get; set; }
        public bool IsSeen { get; set; } = false;
        public DateTime NotifiedAt { get; set; } = DateTime.UtcNow;

        public ChatMention ChatMention { get; set; } = null!;
    }

}

