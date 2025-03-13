using System;
namespace CMPNatural.Core.Entities
{
	public partial class AdminMenuAccess
	{
		public AdminMenuAccess()
		{
		}
		public long Id { get; set; }
        public long MenuId { get; set; }
        public long AdminId { get; set; }

        public virtual Menu Menu { get; set; }
    }
}

