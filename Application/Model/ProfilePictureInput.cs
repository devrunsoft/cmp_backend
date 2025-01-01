using System;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Model
{
	public class ProfilePictureInput
	{
		public ProfilePictureInput()
		{
		}
        public IFormFile ProfilePicture { get; set; }
    }
}

