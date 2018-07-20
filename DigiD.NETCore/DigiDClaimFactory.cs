using ComponentSpace.Saml2.Assertions;
using ComponentSpace.Saml2.Claims;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DigiD.NETCore
{
    /// <summary>
    /// Custom claim factory to create some additional claims (besides the default NameID claim)
    /// </summary>
    public class DigiDClaimFactory : SamlClaimFactory
    {
        public override IList<Claim> CreateClaims(string userID, IList<SamlAttribute> attributes)
        {
            var claims = new List<Claim>();

            // Add default claims.
            claims.AddRange(base.CreateClaims(userID, attributes));

            // Add additional claims.
            claims.Add(new Claim("LoginDateUtc", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime));

            return claims;
        }
    }
}
