using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Responses.ChatCommon
{
    public class ChatCommonMessageEntity
    {
        public long Id { get; set; }
        public long ChatCommonSessionId { get; set; }
        public ParticipantType SenderType { get; set; }
        public long SenderId { get; set; }
        public string PersonId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string SentAt { get; set; } = string.Empty;
        public bool IsInternalNote { get; set; }
        public MessageType Type { get; set; }
        public bool IsSeen { get; set; }
        public string? FileUrl { get; set; }
        public string? FileThumbnailUrl { get; set; }
        public string? FileExtension { get; set; }
        public long? FileSize { get; set; }
        public double? DurationSeconds { get; set; }
        public MessageNoteType? MessageNoteType { get; set; }
    }
}
