using TradingPOC.Data.API.Models;

namespace TradingPOC.Data.API.Service
{
    public interface ILivePriceService
    {
        bool HasError { get; }
        string ErrorDescription { get; }
        Task<PriceAPIResponseModel[]?> GetLivePricesAsync();
    }
}
