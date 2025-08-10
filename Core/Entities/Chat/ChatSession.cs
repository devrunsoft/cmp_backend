
namespace CMPNatural.Core.Entities
{
    public partial class ChatSession
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public long OperationalAddressId { get; set; }
        public long ChatClientSessionId { get; set; }

        public ChatClientSession ChatClientSession { get; set; }
        public Company Company { get; set; }
        public OperationalAddress OperationalAddress { get; set; }
        public virtual ICollection<ChatParticipant> Participants { get; set; }
        public virtual ICollection<ChatMessage> Messages { get; set; }
    }
}


