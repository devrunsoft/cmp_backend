using System;
namespace CMPNatural.Core.Entities
{
	 public partial class AdminEntity
	{
		public AdminEntity()
		{
		}
		public long Id { get; set; }

		public string Email { get; set; }

		public bool? IsActive { get; set; } = true;

        public Guid PersonId { get; set; }

		public Person Person { get; set; }

        public string? Password { get; set; }
    }
}

