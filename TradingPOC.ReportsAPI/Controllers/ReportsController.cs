using Microsoft.AspNetCore.Mvc;

namespace TradingPOC.ReportsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
       
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ILogger<ReportsController> logger)
        {
            _logger = logger;
        }

       

        [HttpGet(Name = "Reports")]
        public string Reports()
        {
            return "Hello";
        }
    }
}
