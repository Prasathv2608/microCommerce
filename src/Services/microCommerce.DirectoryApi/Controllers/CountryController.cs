using microCommerce.Caching;
using microCommerce.Dapper;
using microCommerce.Domain.Directory;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace microCommerce.DirectoryApi.Controllers
{
    public class CountryController : ServiceBaseController
    {
        #region Constants
        private const string COUNTRIES_ALL_CACHE_KEY = "COUNTRIES_ALL";
        private const string COUNTRY_BY_ID_CACHE_KEY = "COUNTRY_BY_ID_{0}";
        private const string COUNTRY_BY_ISO_CODE_CACHE_KEY = "COUNTRY_BY_ISO_CODE_{0}";
        #endregion

        #region Fields
        private readonly IDataContext _dataContext;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public CountryController(IDataContext dataContext,
            ICacheManager cacheManager)
        {
            _dataContext = dataContext;
            _cacheManager = cacheManager;
        }
        #endregion

        #region Methods
        [HttpGet("/countries")]
        public virtual IActionResult Countries()
        {
            var countries = _cacheManager.Get(COUNTRIES_ALL_CACHE_KEY, () =>
            {
                return _dataContext
                   .Query<Country>(
                   "SELECT Name, TwoLetterIsoCode, ThreeLetterIsoCode, NumericIsoCode, DialCode, CurrencyId, LanguageId, AllowBilling, AllowShipping, DisplayOrder, Published FROM Country"
                   ).ToList();
            });

            return Json(countries);
        }

        [HttpGet("/countries/{Id:int}")]
        public virtual IActionResult CountryById(int Id)
        {
            if (Id == 0)
                return BadRequest();

            var country = _cacheManager.Get(string.Format(COUNTRY_BY_ID_CACHE_KEY, Id), () =>
            {
                return _dataContext.Find<Country>(Id);
            });

            return Json(country);
        }

        [HttpGet("/countries/{twoLetterIsoCode}")]
        public virtual IActionResult CountryByTwoLetterIsoCode(string twoLetterIsoCode)
        {
            if (string.IsNullOrEmpty(twoLetterIsoCode))
                return BadRequest();

            var country = _cacheManager.Get(string.Format(COUNTRY_BY_ISO_CODE_CACHE_KEY, twoLetterIsoCode), () =>
            {
                return _dataContext
                    .First<Country>(
                    "SELECT Name, TwoLetterIsoCode, ThreeLetterIsoCode, NumericIsoCode, DialCode, CurrencyId, LanguageId, AllowBilling, AllowShipping, DisplayOrder, Published FROM Country WHERE TwoLetterIsoCode = @twoLetterIsoCode LIMIT 1",
                    new { twoLetterIsoCode });
            });

            return Json(country);
        }
        #endregion
    }
}