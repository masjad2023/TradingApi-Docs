using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TradingPOC.TradingAPI.Models;
using TradingPOC.TradingAPI.Service;
using TradingPOC.TradingAPI.ServiceBusExtention;

namespace TradingPOC.TradingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradingController : ControllerBase
    {
        private readonly ITradingService _tradingService;
		private readonly RabbitMQPublisher _rabbitMQPublisher;

		public TradingController(ITradingService tradingService, RabbitMQPublisher rabbitMQPublisher)
        {
            _tradingService = tradingService;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpPost(Name = "SaveTradeData")]
        public async Task<object> Post(TradeRequestModel tradeData)
        {
            //Post Trading in database
            var tradeResponse = await _tradingService.SaveTradeDataAsync(tradeData);
            if (_tradingService.HasError)
            {
                return BadRequest();
            }

            //send Email for successful tradings
            if(tradeResponse.OrderId > 0)
            {
                _rabbitMQPublisher.PublishOrder(JsonSerializer.Serialize(tradeResponse));
            }

            return Ok(tradeResponse.OrderId);
        }
	}
}
