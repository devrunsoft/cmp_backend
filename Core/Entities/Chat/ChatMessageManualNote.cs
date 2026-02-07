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

        public MessageType Type { get; set; }

        public string? FileUrl { get; set; }

        // Optional thumbnail (for images/videos)
        public string? FileThumbnailUrl { get; set; }

        // File extension e.g. "jpg", "png", "mp4", "pdf"
        public string? FileExtension { get; set; }

        // File size in bytes
        public long? FileSize { get; set; }

        // Optional: video/audio duration in seconds
        public double? DurationSeconds { get; set; }

        public ChatSession ChatSession { get; set; }
    }
}

