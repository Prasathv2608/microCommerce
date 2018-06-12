using microCommerce.Domain.Directory;
using System.Collections.Generic;

namespace microCommerce.Providers.Directory
{
    public interface ICurrencyProvider
    {
        Currency GetCurrencyById(int currencyId);

        Currency GetCurrencyByCode(string currencyCode);

        IList<Currency> GetAllCurrencies(bool showHidden = false);

        void InsertCurrency(Currency currency);

        void UpdateCurrency(Currency currency);

        void DeleteCurrency(Currency currency);
    }
}