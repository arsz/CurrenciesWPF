using Currencies.Services.Interfaces;
using Currencies.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Currencies.ViewModels
{
    public class CurrencyItemViewModel : Observable
    {
        private List<string> currencies;
        private string selectedCurrency;
        private string rate;
        private string convertedValue;

        private CurrencyItemViewModel()
        {
            Currencies = new List<string>(CurrencyRateModel.GetAllCurrencysName());
            SelectedCurrency = Currencies.FirstOrDefault();
        }

        public static CurrencyItemViewModel CreateNewDefault() => new CurrencyItemViewModel();

        public string ConvertedValue
        {
            get => convertedValue;
            set
            {
                convertedValue = value;
                OnPropertyChanged();
            }
        }

        public string Rate
        {
            get => rate;
            set
            {
                rate = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCurrency
        {
            get => selectedCurrency;
            set
            {
                selectedCurrency = value;
                OnPropertyChanged();
            }
        }

        public List<string> Currencies
        {
            get => currencies;
            set
            {
                currencies = value;
                OnPropertyChanged();
            }
        }
    }
}