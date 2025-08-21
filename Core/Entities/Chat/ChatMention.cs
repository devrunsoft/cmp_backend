using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class ChatMention
    {
        public long Id { get; set; }
        public long ChatMessageId { get; set; }
        public long ClientId { get; set; }
        public long OperationalAddressId { get; set; }
        public MentionedType MentionedType { get; set; }
        public long MentionedId { get; set; }

        public ChatMessage ChatMessage { get; set; } = null!;
        public ChatNotification? Notification { get; set; }
    }
}

