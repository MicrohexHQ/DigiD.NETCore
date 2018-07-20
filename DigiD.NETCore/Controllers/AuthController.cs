using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComponentSpace.Saml2.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DigiD.NETCore.Controllers
{
    /// <summary>
    /// MVC controller to allow client-side code to invoke the login/logout sequence
    /// </summary>
    [Route("auth")]
    public class AuthController : Controller
    {
        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            var properties = new AuthenticationProperties { RedirectUri = returnUrl };
            return new ChallengeResult(properties);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Cookie first
            await HttpContext.SignOutAsync(SamlAuthenticationDefaults.AuthenticationScheme); // Also for saml
            return Redirect(returnUrl);
        }
    }
}