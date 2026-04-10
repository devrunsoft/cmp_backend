using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Responses.ChatCommon
{
    public class ChatCommonSessionEntity
    {
        public long Id { get; set; }
        public long ParticipantId { get; set; }
        public string PersonId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string? ClosedAt { get; set; }
        public List<ChatCommonMessageEntity>? Messages { get; set; }
        public int UnRead { get; set; }
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
