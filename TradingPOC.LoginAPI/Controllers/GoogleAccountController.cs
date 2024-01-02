using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingPOC.LoginAPI.Models;

namespace TradingPOC.LoginAPI.Controllers
{
    [ApiController]
    [Route("account")]
    public class GoogleAccountController : ControllerBase
    {
        [HttpGet("/google-login")]
        [HttpGet]
        public IActionResult GoogleLogin()
        
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("/google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            var response = new UserInfo()
            {
                Type = "F",
                Email = claims.Where(x => x.Type.ToString().Contains("emailaddress") == true).FirstOrDefault().Value,
                FirstName = claims.Where(x => x.Type.ToString().Contains("givenname") == true).FirstOrDefault().Value,
                lastName = claims.Where(x => x.Type.ToString().Contains("surname") == true).FirstOrDefault().Value,
                Username = claims.Where(x => x.Type.ToString().Contains("emailaddress") == true).FirstOrDefault().Value,
            };

            return Redirect(string.Format(@"http://localhost:58799?firstname={0}&lastname={1}&emailaddress={2}", response.FirstName, response.lastName, response.Email));
        }
    }

}

