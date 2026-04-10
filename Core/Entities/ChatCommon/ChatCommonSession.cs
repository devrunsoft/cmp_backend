using System;
using System.ComponentModel.DataAnnotations.Schema;
using CMPNatural.Core.Enums;
using Newtonsoft.Json;

namespace  CMPNatural.Core.Entities
{
	public partial class ChatCommonSession
	{
        public long Id { get; set; }
        public long ParticipantId { get; set; }
        public Guid PersonId { get; set; }
        public bool IsActive { get; set; } = true;
        public ParticipantType ParticipantType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

        [JsonIgnore]
        public virtual ICollection<ChatCommonMessage> Messages { get; set; }


        [NotMapped]
        public int UnRead { get; set; } = 0;
    }
}
