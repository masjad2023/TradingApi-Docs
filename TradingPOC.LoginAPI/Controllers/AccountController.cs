using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using TradingPOC.LoginAPI.Models;
using TradingPOC.LoginAPI.Entities;
using TradingPOC.LoginAPI.Models;

namespace TradingPOC.LoginAPI.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<FacebookAccountController> _logger;
        private readonly TradingdatabaseContext _dbContext;

        public AccountController(ILogger<FacebookAccountController> logger, TradingdatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost("/email-login")]
        public IActionResult Login([FromBody] LoginRequestModel request)
        {
            var user = _dbContext.Users.Where(x => x.EmailId == request.UserName && x.Password == request.Password).FirstOrDefault();
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new UserInfo()
            {
                UserId = user.Id,
                Email = user.EmailId,
                FirstName = user.FirstName,
                lastName = user.LastName,
                Type = "E",
                Username = user.EmailId
            });
        }
    }
}