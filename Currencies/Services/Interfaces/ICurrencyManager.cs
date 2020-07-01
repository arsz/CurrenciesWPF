using Currencies.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Currencies.Services.Interfaces
{
    public interface ICurrencyManager
    {
        Task<bool> GetDataAsync(string currentMoneyValue, bool isDollarSelected, CancellationToken cancellationToken);

        List<CurrencyModel> GetModelsList { get; }

        DateTime GetLastUpdateDateTime { get; }
    }
}