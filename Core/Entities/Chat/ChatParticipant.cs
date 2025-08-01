using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class ChatParticipant
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public long ParticipantId { get; set; } // Could be AdminId or ProviderId
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public bool IsRemoved { get; set; } = false;

        public ChatSession ChatSession { get; set; } = null!;
    }

}

