using Currencies.Models;
using System.Threading.Tasks;

namespace Currencies.Services.Interfaces
{
    public interface IStorageService
    {
        Task SaveCurrentSettings(StorageModel storageModel);

        Task<StorageModel> LoadSavedSettings();
    }
}
