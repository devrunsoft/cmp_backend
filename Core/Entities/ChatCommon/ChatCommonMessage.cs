using System;
using CMPNatural.Core.Enums;
using Newtonsoft.Json;

namespace CMPNatural.Core.Entities
{
	public partial class ChatCommonMessage
	{
        public long Id { get; set; }
        public long ChatCommonSessionId { get; set; }
        public ParticipantType SenderType { get; set; }
        public long SenderId { get; set; }
        public Guid PersonId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsInternalNote { get; set; } = false;
        public MessageType Type { get; set; }
        public bool IsSeen { get; set; } = false;

        public string? FileUrl { get; set; }

        public string? FileThumbnailUrl { get; set; }

        public string? FileExtension { get; set; }

        public long? FileSize { get; set; }

        public double? DurationSeconds { get; set; }

        [JsonIgnore]
        public List<ChatMention> Mentions { get; set; } = new();

        [JsonIgnore]
        public ChatCommonSession ChatSession { get; set; } = null!;
    }
}
