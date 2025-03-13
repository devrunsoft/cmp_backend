using System;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Model
{
	public class AdminInput
	{
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; } = true;

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

