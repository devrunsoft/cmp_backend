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

        //LimitedAdmin or SuperAdmin
        public string Role { get; set; } = "LimitedAdmin";

        public bool? IsActive { get; set; } = true;

        public Guid PersonId { get; set; }

		public virtual Person Person { get; set; }

        public string? Password { get; set; }

    }
}

