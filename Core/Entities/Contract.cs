using System;
namespace CMPNatural.Core.Entities
{
	public partial class Contract
	{
		public long Id { get; set; }
		public bool Active { get; set; }
		public string Content { get; set; } = "";
        public string Title { get; set; } = "";
		public DateTime CreatedAt { get; set; }
	}
}

