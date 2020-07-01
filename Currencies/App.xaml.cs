using Currencies.Services;
using Currencies.ViewModels;
using Currencies.Views;
using System.Threading.Tasks;
using System.Windows;

namespace Currencies
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        //crying cat because of async void :( 
        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await InitializeAsync();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        private async Task InitializeAsync()
        {
            var mainWindow = new MainWindow();

            //An DI container of an IoC service should this injections...
            var context = new MainWindowViewModel(new CurrencyManager(new ExchangeRateDataProvider()), new StorageService("storageFile.json"));
            mainWindow.DataContext = context;
            await context.LoadSavedFile();
            mainWindow.Show();
        }
    }
}