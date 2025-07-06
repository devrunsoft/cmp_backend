using System;
namespace CMPNatural.Core.Entities
{
	public partial class LocationDateTime
	{
		public long Id { get; set; }

        public string DayName { get; set; } = null!;

		public long CompanyId { get; set; }

		public long OperationalAddressId { get; set; }

        public int FromTime { get; set; }

        public int ToTime { get; set; }

        public virtual OperationalAddress OperationalAddress { get; set; }
    }
}

