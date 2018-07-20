using DigiD.NETCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DigiD.NETCore.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {

        /// <summary>
        /// Raadplegen van gegevens van ingelogde gebruiker
        /// </summary>
        /// <returns></returns>
        [Route("me")]
        public IActionResult Me()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var nameId = this.User.Identity.Name;
                var nameIdParts = nameId.Split(':');
                var gebruikerID = nameIdParts[1];

                var gebruikerIDType = GebruikerIDType.Unspecified;
                switch(nameIdParts[0].ToLowerInvariant())
                {
                    case "s00000000":
                        gebruikerIDType = GebruikerIDType.BSN;
                        break;
                    case "s00000001":
                        gebruikerIDType = GebruikerIDType.SOFI;
                        break;
                }

                var loginDateUtc = DateTime.Parse(this.User.Claims.First(c => c.Type == "LoginDateUtc").Value);

                var user = new DigiDUser
                {
                    GebruikerID = gebruikerID,
                    GebruikerIDType = gebruikerIDType,
                    GeldigTmUtc = loginDateUtc.AddMinutes(60),
                    InlogDatumUtc = loginDateUtc,

                    // TODO: Ergens vandaan halen
                    BetrouwbaarheidsNiveau = BetrouwbaarheidsNiveau.Basis,
                    IP = "1.1.1.1"
                };

                return Ok(user);
            }

            return NotFound();
        }

    }
}