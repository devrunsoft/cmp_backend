using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class TermsConditions
	{
		public long Id { get; set; }
		public long? TenantId { get; set; }
		public DateTime CreateAt { get; set; }
        public TermsConditionsEnum Type { get; set; }
        public bool Enable { get; set; }
		public string Content { get; set; } = "";
		public virtual Tenant? Tenant { get; set; }
	}
}
