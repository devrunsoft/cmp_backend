using System;
namespace CMPNatural.Core.Entities
{
	public partial class Person
	{
		public Person()
		{
		}

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}

