using TradingPOC.TradingAPI.Entities;
using TradingPOC.TradingAPI.Models;

namespace TradingPOC.TradingAPI.Service
{
    public class TradingService : ITradingService
    {
        private readonly TradingDbContext _dbContext;
        public bool HasError { get; private set; }
        public string ErrorDescription { get; private set; }

        public TradingService(TradingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TradeResponseModel> SaveTradeDataAsync(TradeRequestModel tradeData)
        {
            TradeResponseModel tradeResponseModel = new TradeResponseModel();
            _ResetError();
            try
            {
                Trade tradeInfo = new Trade
                {
                    TradeType = tradeData.Type,
                    TradeTimeStamp = DateTime.Now,
                    UserId = tradeData.UserId,
                    TradePrice = (decimal)tradeData.Price,
                    TradeQuantity = tradeData.Quantity,
                    Symbol = tradeData.Symbol,
                };
                _dbContext.Trades.Add(tradeInfo);
                _dbContext.SaveChanges();

                User user = _dbContext.Users.Where(u => u.Id == tradeData.UserId).FirstOrDefault();
                tradeResponseModel.OrderId = tradeInfo.OrderId;
                tradeResponseModel.Price = tradeInfo.TradePrice;
                tradeResponseModel.Quantity = tradeInfo.TradeQuantity;
                tradeResponseModel.TradeType = tradeInfo.TradeType;
                tradeResponseModel.UserEmail = user.EmailId;
                tradeResponseModel.UserName = user.FirstName;
                tradeResponseModel.ScriptName = tradeInfo.Symbol;
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorDescription = ex.Message;
            }
            return tradeResponseModel;
        }

        private void _ResetError()
        {
            HasError = false;
            ErrorDescription = string.Empty;
        }
    }
}
