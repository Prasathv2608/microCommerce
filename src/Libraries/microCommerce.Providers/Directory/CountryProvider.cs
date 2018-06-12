using microCommerce.Caching;
using microCommerce.Dapper;
using microCommerce.Domain.Directory;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Providers.Directory
{
    public class CountryProvider : ICountryProvider
    {
        private readonly IDataContext _dataContext;
        private readonly ICacheManager _cacheManager;

        public CountryProvider(IDataContext dataContext,
            ICacheManager cacheManager)
        {
            _dataContext = dataContext;
            _cacheManager = cacheManager;
        }

        public virtual Country GetCountryById(int countryId)
        {
            if (countryId == 0)
                return null;

            return _cacheManager.Get(string.Format(ProviderCacheKey.COUNTRY_BY_IDENTITY_KEY, countryId), () =>
            {
                return _dataContext.First<Country>("SELECT * FROM Country WHERE Id = @countryId LIMIT 1", new { countryId });
            });
        }

        public virtual Country GetCountryByTwoLetterIsoCode(string twoLetterIsoCode)
        {
            if (string.IsNullOrEmpty(twoLetterIsoCode))
                return null;

            return _cacheManager.Get(string.Format(ProviderCacheKey.COUNTRY_BY_CODE_KEY, twoLetterIsoCode), () =>
            {
                return _dataContext.First<Country>("SELECT * FROM Country WHERE TwoLetterIsoCode = @twoLetterIsoCode LIMIT 1", new { twoLetterIsoCode });
            });
        }

        public virtual IList<Country> GetAllCountries(bool showHidden = false)
        {
            return _cacheManager.Get(string.Format(ProviderCacheKey.CURRENCIES_ALL_KEY, showHidden), () =>
            {
                if (showHidden)
                    return _dataContext.Query<Country>("SELECT * FROM Country").ToList();

                return _dataContext.Query<Country>("SELECT * FROM Country WHERE Published = 1").ToList();
            });
        }

        public virtual void InsertCountry(Country country)
        {
            _dataContext.Insert(country);
        }

        public virtual void UpdateCountry(Country country)
        {
            _dataContext.Update(country);
        }

        public virtual void DeleteCountry(Country country)
        {
            _dataContext.Delete(country);
        }
    }
}