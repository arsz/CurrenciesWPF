using Currencies.Models;
using Currencies.Services.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Currencies.Services
{
    public class StorageService : IStorageService
    {
        public StorageService(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public async Task SaveCurrentSettings(StorageModel storageModel)
        {
          var json = JsonConvert.SerializeObject(storageModel);

            using (StreamWriter writer = File.CreateText(Path))
            {
               await writer.WriteAsync(json).ConfigureAwait(false);
            }
        }

        public async Task<StorageModel> LoadSavedSettings()
        {
            string json = string.Empty;

            if (File.Exists(Path) == false) return StorageModel.DefaultEmptyModel;

            using (StreamReader reader = new StreamReader(Path))
            {
                json = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            return JsonConvert.DeserializeObject<StorageModel>(json);
        }

    }
}
