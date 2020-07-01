using Currencies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currencies.Models
{
    public class StorageModel
    {
        public bool InvalidStorageData { get; set; }

        public bool IsDollarBaseCurrencySelected { get; set; }

        public string LastUpdatedDate { get; set; }

        public string MoneyValue { get; set; }

        public CurrencyItemViewModel[] Currencies { get; set; }

        public static StorageModel DefaultEmptyModel => new StorageModel() { InvalidStorageData = true };
    }
}
