using Currencies.Models;
using Currencies.Services.Interfaces;
using Currencies.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Utils;

namespace Currencies.ViewModels
{
    public sealed class MainWindowViewModel : Observable, IDisposable
    {
        private readonly ICurrencyManager currencyManager;
        private readonly IStorageService storageService;

        private string moneyValue;
        private CancellationTokenSource cancellationTokenSource;
        private ObservableCollection<CurrencyItemViewModel> currencies;
        private string lastUpdatedValue;
        private bool isDollarBaseCurrencySelected;
        private bool isInProgress;
        private string errorMessage;

        public MainWindowViewModel(ICurrencyManager currencyManager,IStorageService storageService)
        {
            this.currencyManager = currencyManager;
            this.storageService = storageService;
            cancellationTokenSource = new CancellationTokenSource();
            currencies = new ObservableCollection<CurrencyItemViewModel>();
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsInProgress
        {
            get => isInProgress;
            set
            {
                isInProgress = value;
                OnPropertyChanged();
            }
        }

        public bool IsDollarBaseCurrencySelected
        {
            get => isDollarBaseCurrencySelected;
            set
            {
                isDollarBaseCurrencySelected = value;
                OnPropertyChanged();
            }
        }

        public string MoneyValue
        {
            get => moneyValue;
            set
            {
                moneyValue = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CurrencyItemViewModel> Currencies
        {
            get => currencies;
            set
            {
                currencies = value;
                OnPropertyChanged();
            }
        }

        public string LastUpdatedValue
        {
            get => lastUpdatedValue;
            set
            {
                lastUpdatedValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand Save => new AsyncCommand(SaveCurrentStatusAsync, _ => IsInProgress == false);

        public ICommand Cancel => new Command(_ => cancellationTokenSource.Cancel());

        public ICommand Exit => new Command(_ => CloseApplication(), _ => IsInProgress == false);

        public ICommand DeleteCurrency => new Command<CurrencyItemViewModel>(item => Currencies.Remove(item));

        public ICommand AddCurrency => new Command(_ => Currencies.Add(CurrencyItemViewModel.CreateNewDefault()));

        public ICommand Refresh => new AsyncCommand(() => UpdateCurrenciesAsync(cancellationTokenSource.Token), _ => Currencies.Any(),
            onException: exc => ErrorMessage = exc.Message, continueOnCapturedContext: false);

        public async Task LoadSavedFile()
        {
            var loadedDatas = await storageService.LoadSavedSettings().ConfigureAwait(false);

            if (loadedDatas.InvalidStorageData) return;

            Currencies = new ObservableCollection<CurrencyItemViewModel>(loadedDatas.Currencies);

            IsDollarBaseCurrencySelected = loadedDatas.IsDollarBaseCurrencySelected;

            MoneyValue = loadedDatas.MoneyValue;

            LastUpdatedValue = loadedDatas.LastUpdatedDate;
        }

        public void Dispose() => cancellationTokenSource?.Dispose();

        private async Task UpdateCurrenciesAsync(CancellationToken cancellationToken)
        {
            try
            {
                IsInProgress = true;
                ErrorMessage = string.Empty;

                var isSuccessful = await currencyManager.GetDataAsync(MoneyValue, IsDollarBaseCurrencySelected, cancellationToken);
                if (isSuccessful == false) throw new InvalidOperationException("Downloading has failed!");

                UpdateCurrencies(currencyManager.GetModelsList);

                LastUpdatedValue = currencyManager.GetLastUpdateDateTime == default(DateTime) ? " - - " : currencyManager.GetLastUpdateDateTime.ToString();
            }
            catch(OperationCanceledException)
            {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
                cancellationTokenSource = new CancellationTokenSource();

                throw new Exception("The downloading has been cancelled!");
            }
            catch
            {
                //Do not handle the exceptions yet...
                throw;
            }
            finally
            {
                IsInProgress = false;
            }
        }

        private void UpdateCurrencies(List<CurrencyModel> currencyModels)
        {
            foreach (var currencyItem in currencyModels)
            {
                var currentCurrencies = Currencies.Where(x => x.SelectedCurrency == currencyItem.Name);

                foreach (var c in currentCurrencies)
                {
                    c.Rate = currencyItem.Rate.ToString();

                    c.ConvertedValue = currencyItem.Value == 0 ? " - " : currencyItem.Value.ToString();
                }
            }
        }

        private async Task SaveCurrentStatusAsync()
        {
            try
            {
                IsInProgress = true;
                var model = new StorageModel
                {
                    Currencies = this.Currencies.ToArray(),
                    IsDollarBaseCurrencySelected = this.IsDollarBaseCurrencySelected,
                    LastUpdatedDate = this.LastUpdatedValue,
                    MoneyValue = this.MoneyValue
                };

                await storageService.SaveCurrentSettings(model).ConfigureAwait(false);
            }
            finally
            {
                IsInProgress = false;
            }
        }
          


        private void CloseApplication()
        {
            Dispose();
            Environment.Exit(0);
        }
    }
}