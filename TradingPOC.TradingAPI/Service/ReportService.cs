using TradingPOC.TradingAPI.Entities;
using TradingPOC.TradingAPI.Models;

namespace TradingPOC.TradingAPI.Service
{
    public class ReportService : IReportService
    {
        private readonly TradingDbContext _dbContext;
        public bool HasError { get; private set; }
        public string ErrorDescription { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ReportService(TradingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ProfitLossResponseModel>> GetProfitandLossReport(int userId, DateTime startDate, DateTime endDate)
        {
            List<ProfitLossResponseModel> responseData = new List<ProfitLossResponseModel>();
            _ResetError();
            try
            {
                IEnumerable<Trade> trades = _dbContext.Trades.Where(a => a.UserId == userId && a.TradeTimeStamp >= startDate && a.TradeTimeStamp <= endDate);
                var totalSoldDetails = trades.Where(a => a.TradeType == "S")
                    .GroupBy(a => a.Symbol)
                    .Select(a => new
                    {
                        Symbol = a.Key,
                        TotalSellQuantity = a.Sum(b => b.TradeQuantity),
                        TotalSellValue = a.Sum(b => b.TradePrice * b.TradeQuantity)
                    });

                var totalBuyDetails = trades.Where(a => a.TradeType == "B")
                   .GroupBy(a => a.Symbol)
                   .Select(a => new
                   {
                       Symbol = a.Key,
                       TotalBuyQuantity = a.Sum(b => b.TradeQuantity),
                       TotalBuyValue = a.Sum(b => (b.TradePrice * b.TradeQuantity))
                   });

                IEnumerable<ProfitLossResponseModel> result = totalBuyDetails.Join(totalSoldDetails, b => b.Symbol, s => s.Symbol,
                    (b, s) => new ProfitLossResponseModel
                    {
                        ScriptName = b.Symbol,
                        Quantity = (s.TotalSellQuantity),
                        TotalSellValue = s.TotalSellValue,
                        AvgSellRate = (s.TotalSellValue / s.TotalSellQuantity),
                        TotalBuyValue = b.TotalBuyValue,
                        AvgBuyRate = (b.TotalBuyValue / b.TotalBuyQuantity),
                        RealizedGainOrLoss = (s.TotalSellValue - b.TotalBuyValue),
                        LongOrShort = ""
                    });
                return result;
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorDescription = ex.Message != null ? ex.Message : (ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : "Exception ocurred while excecution of api endpoint.");
            }
            return responseData;
        }


        private void _ResetError()
        {
            HasError = false;
            ErrorDescription = string.Empty;
        }
    }
}
