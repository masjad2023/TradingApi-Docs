using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TradingPOC.TradingAPI.Service;
using TradingPOC.TradingAPI.ServiceBusExtention;

namespace TradingPOC.TradingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("ProfitAndLoss/{userId:int}/{from}/{to}")]
        public IActionResult ProfitAndLossReport(int userId, DateTime from, DateTime to)
        {
            var reportResponse = _reportService.GetProfitandLossReport(userId, from, to);
            if (_reportService.HasError)
            {
                return BadRequest();
            }
            return Ok(reportResponse);
        }
    }
}
