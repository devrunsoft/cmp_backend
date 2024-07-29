using System;
namespace ScoutDirect.Application.Model
{
	public class RegisterPlayerInput
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}

