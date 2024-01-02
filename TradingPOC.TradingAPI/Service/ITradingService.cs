using TradingPOC.TradingAPI.Models;

namespace TradingPOC.TradingAPI.Service
{
    public interface ITradingService
    {
        bool HasError { get; }
        string ErrorDescription { get; }

        /// <summary>
        /// Saves Buy/Sell Records
        /// </summary>
        /// <param name="tradeData"></param>
        /// <returns></returns>
        Task<TradeResponseModel> SaveTradeDataAsync(TradeRequestModel tradeData);
    }
}
