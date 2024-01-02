using Newtonsoft.Json;
using System.Text.Json.Serialization;
using TradingPOC.Data.API.Models;
using TradingPOC.Data.API.Models.Settings;

namespace TradingPOC.Data.API.Service
{
    public class RapidAPILivePriceService : ILivePriceService
    {
        private DateTime? _LastRefreshTime = null;
        PriceAPIResponseModel[]? _LastPriceValue = null;
        int _RefreshTimeInMilliSeconds = 1000;

        private RapidAPISettings _Settings;

        public RapidAPILivePriceService(RapidAPISettings settings)
        {
            _Settings = settings;
        }

        public bool HasError { get; private set; }

        public string ErrorDescription { get; private set; }

        public async Task<PriceAPIResponseModel[]?> GetLivePricesAsync()
        {
            PriceAPIResponseModel[]? responseModels = null;
            _ResetError();
            try
            {
                if (_IsRefreshNeeded())
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(_Settings.APIRequestUri),
                        Headers = { 
                            { "X-RapidAPI-Key", _Settings.APIKey },
                            { "X-RapidAPI-Host", _Settings.HostURL },
                        },
                    };
                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        responseModels = JsonConvert.DeserializeObject<PriceAPIResponseModel[]>(body);
                    }
                    _LastRefreshTime = DateTime.Now;
                    _LastPriceValue = responseModels;
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorDescription = ex.Message;
            }
            return _LastPriceValue;
        }

        private void _ResetError()
        {
            HasError = false;
            ErrorDescription = string.Empty;
        }

        private bool _IsRefreshNeeded()
        {
            if (_LastRefreshTime == null)
            {
                return true;
            }

            return DateTime.Now > (_LastRefreshTime.Value.AddMilliseconds(_RefreshTimeInMilliSeconds));
        }
    }
}
