using System;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Model
{
	public class DocumentInput
	{
		public DocumentInput()
		{
		}

        public IFormFile BusinessLicense { get; set; }
        public IFormFile HealthDepartmentCertificate { get; set; }
    }
}

