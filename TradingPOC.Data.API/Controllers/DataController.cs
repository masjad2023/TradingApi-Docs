using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingPOC.Data.API.Service;

namespace TradingPOC.Data.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        ILivePriceService _LivePriceService;

        public DataController(ILivePriceService livePriceService)
        {
            _LivePriceService = livePriceService;
        }

        [HttpGet(Name = "GetPrice")]
        public async Task<object> GetPriceAsync()
        {
            var livePrice = await _LivePriceService.GetLivePricesAsync();
            if (_LivePriceService.HasError)
            {
                return BadRequest();
            }

            return Ok(livePrice);
        }
    }
}
