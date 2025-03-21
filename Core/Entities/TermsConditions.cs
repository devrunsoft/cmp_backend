using System;
namespace CMPNatural.Core.Entities
{
	public partial class TermsConditions
	{
		public long Id { get; set; }
		public DateTime CreateAt { get; set; }
		public bool Enable { get; set; }
		public string Content { get; set; } = "";
	}
}

