using System;
namespace CMPNatural.Core.Entities
{
	public class Admin
	{
		public Admin()
		{
		}

		public long PersonId { get; set; }

		public Person Person { get; set; }

        public string? Password { get; set; }
    }
}

