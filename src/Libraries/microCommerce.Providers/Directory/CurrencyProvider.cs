using microCommerce.Caching;
using microCommerce.Dapper;
using microCommerce.Domain.Directory;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Providers.Directory
{
    public class CurrencyProvider : ICurrencyProvider
    {
        private readonly IDataContext _dataContext;
        private readonly ICacheManager _cacheManager;

        public CurrencyProvider(IDataContext dataContext,
            ICacheManager cacheManager)
        {
            _dataContext = dataContext;
            _cacheManager = cacheManager;
        }

        public virtual Currency GetCurrencyById(int currencyId)
        {
            if (currencyId == 0)
                return null;

            return _cacheManager.Get(string.Format(ProviderCacheKey.CURRENCY_BY_IDENTITY_KEY, currencyId), () =>
            {
                return _dataContext.First<Currency>("SELECT * FROM Currency WHERE Id = @currencyId LIMIT 1", new { currencyId });
            });
        }

        public virtual Currency GetCurrencyByCode(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
                return null;

            return _cacheManager.Get(string.Format(ProviderCacheKey.CURRENCY_BY_CODE_KEY, currencyCode), () =>
            {
                return _dataContext.First<Currency>("SELECT * FROM Currency WHERE CurrencyCode = @currencyCode LIMIT 1", new { currencyCode });
            });
        }

        public virtual IList<Currency> GetAllCurrencies(bool showHidden = false)
        {
            return _cacheManager.Get(string.Format(ProviderCacheKey.CURRENCIES_ALL_KEY, showHidden), () =>
            {
                if (showHidden)
                    return _dataContext.Query<Currency>("SELECT * FROM Currency").ToList();

                return _dataContext.Query<Currency>("SELECT * FROM Currency WHERE Published = 1").ToList();
            });
        }

        public virtual void InsertCurrency(Currency currency)
        {
            _dataContext.Insert(currency);
        }

        public virtual void UpdateCurrency(Currency currency)
        {
            _dataContext.Update(currency);
        }

        public virtual void DeleteCurrency(Currency currency)
        {
            _dataContext.Delete(currency);
        }
    }
}