using System;
namespace CMPNatural.Core.Entities
{
	 public partial class AdminEntity
	{

		public long Id { get; set; }

		public string Email { get; set; } = null!;

        //LimitedAdmin or SuperAdmin
        public string Role { get; set; } = "LimitedAdmin";

        public bool? IsActive { get; set; } = true;

        public Guid PersonId { get; set; }

		public virtual Person Person { get; set; }

        public string Password { get; set; }

		public bool TwoFactor { get; set; }

        public string? Code { get; set; } = "";

        public DateTime? CodeTime { get; set; }

        public string FullName { get { return $"{Person.FirstName} {Person.LastName}"; } }


    }
}

