using System;
namespace CMPNatural.Application.Model
{
	public class CheckResetPasswordInput
	{
		public Guid forgotPasswordLink { get; set; }
		public string email { get; set; }
	}
}


