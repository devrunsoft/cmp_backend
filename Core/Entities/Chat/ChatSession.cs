using System;
namespace CMPNatural.Core.Entities
{
    public partial class ChatSession
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public Company Company { get; set; }
        public List<ChatParticipant> Participants { get; set; } = new();
        public List<ChatMessage> Messages { get; set; } = new();
    }
}


