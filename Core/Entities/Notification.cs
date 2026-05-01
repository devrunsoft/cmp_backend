using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Notification
	{
		public long Id { get; set; }

		public string Title { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public NotificationType type { get; set; }

		public string? Payload { get; set; }

		public DateTime? Seen { get; set; }

        public DateTime CreateAt { get; set; }

    }
}

