using System;
using System.Security.Claims;

namespace CMPNatural.Api.Controllers.Client
{
	public class GenerateToken
	{
        public Claim[] get_claims(string adminStatus, string businessEmail, string companyId, bool registered, string? ProfilePicture, string fullname, Guid PersonId, long? operationalAddressId = null)
        {
            List<Claim> claims = new List<Claim>() { new Claim("businessEmail", businessEmail), new Claim("CompanyId", companyId) };

            claims.Add(new Claim("Registered", registered.ToString()));
            claims.Add(new Claim("Type", adminStatus));
            claims.Add(new Claim("ProfilePicture", ProfilePicture ?? ""));
            claims.Add(new Claim("FullName", fullname));
            claims.Add(new Claim("PersonId", PersonId.ToString()));
            if (operationalAddressId.HasValue && operationalAddressId.Value > 0)
            {
                claims.Add(new Claim("OperationalAddressId", operationalAddressId.Value.ToString()));
            }

            return claims.ToArray();
        }


    }
}
