using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class AppLog
	{
		public long Id { get; set; }
		public Guid PersonId { get; set; }
		public string FullName { get; set; }
		public LogTypeEnum LogType { get; set; }
        public string Action { get; set; }
		public DateTime CreatedAt { get; set; }

	}
}

