using TradingPOC.TradingAPI.Models;

namespace TradingPOC.TradingAPI.Service
{
    public interface IReportService
    {
        bool HasError { get; }
        string ErrorDescription { get; }

        /// <summary>
        /// Get Profit and Loss Report
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        Task<IEnumerable<ProfitLossResponseModel>> GetProfitandLossReport(int userId, DateTime from, DateTime to);
    }
}
