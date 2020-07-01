using Currencies.Properties;
using Currencies.Services.Interfaces;
using Currencies.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Currencies.Services
{
    /// <summary>
    /// using valuta exchange API of https://www.exchangerate-api.com/ 
    /// </summary>
    public class ExchangeRateDataProvider : ICurrencyDataProvider
    {
        private const string BaseApiEndpoint = "https://v6.exchangerate-api.com/v6/";

        public Uri GetHttpRequestUri(BaseMoneyCurrency baseCurrency) => new Uri(BaseApiEndpoint + Resources.Token + "/latest/" + supportedBaseCurrencyDictionary[baseCurrency]);

        public async Task<T> DownloadCurrenciesAsync<T>(BaseMoneyCurrency currentCurrency, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (cancellationToken.Register(() => client.CancelPendingRequests()))
            {
                var response = await client.GetAsync(GetHttpRequestUri(currentCurrency), cancellationToken).ConfigureAwait(false);

                if (response.Content == null) throw new ArgumentNullException("Content is missing");

                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
      
                cancellationToken.ThrowIfCancellationRequested();

                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        private static Dictionary<BaseMoneyCurrency, string> supportedBaseCurrencyDictionary = new Dictionary<BaseMoneyCurrency, string>()
        {
            { BaseMoneyCurrency.Dollar,"USD" },
            { BaseMoneyCurrency.Euro,"EUR" }
        };
    }
}