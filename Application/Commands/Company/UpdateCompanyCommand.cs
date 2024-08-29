using System;
namespace CMPNatural.Application.Commands
{
	public class UpdateCompanyCommand : RegisterCompanyCommand
    {
		public long CompanyId { get; set; }
	}
}

