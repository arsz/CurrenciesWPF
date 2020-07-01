using Currencies.Models;
using Currencies.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Currencies.Services
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly ICurrencyDataProvider conversionDataProvider;

        private CurrencyResultModel currencyResultModel;

        private IEnumerable<CurrencyModel> currencyModels;

        public CurrencyManager(ICurrencyDataProvider conversionDataProvider)
        {
            this.conversionDataProvider = conversionDataProvider;
        }

        public List<CurrencyModel> GetModelsList => currencyModels.ToList();

        public DateTime GetLastUpdateDateTime { get; set; }

        public async Task<bool> GetDataAsync(string currentMoneyValue, bool isDollarSelected, CancellationToken cancellationToken)
        {
            var baseCurrency = isDollarSelected ? BaseMoneyCurrency.Dollar : BaseMoneyCurrency.Euro;

            currencyResultModel = await conversionDataProvider.DownloadCurrenciesAsync<CurrencyResultModel>(baseCurrency, cancellationToken).ConfigureAwait(false);

            if (currencyResultModel.result == "error") return false;

            var currenciesWithRates = currencyResultModel.conversion_rates.GetCurrenciesWithRates();

            currencyModels = CreateModels(currenciesWithRates, currentMoneyValue);

            GetLastUpdateDateTime = DateTime.Now;

            return true;
        }

        private IEnumerable<CurrencyModel> CreateModels(Dictionary<string, double> currencyRatePairs, string currentMoneyValue)
        {
            foreach (var valuePair in currencyRatePairs)
            {
                yield return new CurrencyModel(valuePair.Key, valuePair.Value, CalculateConvertedValue(valuePair.Value, currentMoneyValue));
            }
        }

        private double CalculateConvertedValue(double rate, string moneyValue)
        {
            double realValue;

            if (string.IsNullOrEmpty(moneyValue)) return 0;

            if (double.TryParse(moneyValue, out realValue) == false) throw new InvalidCastException("This value cannot be converted!");

            return rate * realValue;
        }
    }
}