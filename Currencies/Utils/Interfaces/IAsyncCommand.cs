using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp.Utils.Interfaces
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
    }
}