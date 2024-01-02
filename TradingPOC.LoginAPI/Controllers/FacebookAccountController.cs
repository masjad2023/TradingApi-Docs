using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using TradingPOC.LoginAPI.Models;

namespace TradingPOC.LoginAPI.Controllers
{
    [ApiController]
    [Route("account")]
    public class FacebookAccountController : ControllerBase
    {
        private readonly ILogger<FacebookAccountController> _logger;

        public FacebookAccountController(ILogger<FacebookAccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/facebook-login")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("/facebook-response")]
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
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

            return Redirect(string.Format(@"http://localhost:3000?firstname={0}&lastname={1}&emailaddress={2}", response.FirstName, response.lastName, response.Email));
        }
    }
}