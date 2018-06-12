using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Dapper;
using microCommerce.Domain.Localizations;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Providers.Localizations
{
    public class LocalizationProvider : ILocalizationProvider
    {
        //private readonly IDataContext _dataContext;
        //private readonly IWorkContext _workContext;
        //private readonly ICacheManager _cacheManager;

        //public LocalizationProvider(IDataContext dataContext,
        //    IWorkContext workContext,
        //ICacheManager cacheManager)
        //{
        //    _dataContext = dataContext;
        //    _workContext = workContext;
        //    _cacheManager = cacheManager;
        //}

        public virtual void InsertLocalizationResource(string name, string value, string languageCultureCode)
        {
            //if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(languageCultureCode))
            //    return;

            //var localizationResource = new LocalizationResource
            //{
            //    Name = name.Trim().ToLowerInvariant(),
            //    Value = value,
            //    LanguageCultureCode = languageCultureCode
            //};

            //_dataContext.InsertAsync(localizationResource);
        }

        public virtual void UpdateLocalizationResource(string name, string value, string languageCultureCode)
        {
            //if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(languageCultureCode))
            //    return;

            //var localizationResource = _dataContext.FirstProcedure<LocalizationResource>("Localization_Find", new { name, languageCultureCode });
            //if (localizationResource != null)
            //{
            //    localizationResource.Value = value;
            //    _dataContext.Update(localizationResource);
            //}
        }

        public virtual void DeleteLocalizationResource(string name, string languageCultureCode)
        {
            //if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(languageCultureCode))
            //    return;

            //var localizationResource = _dataContext.FirstProcedure<LocalizationResource>("Localization_Find", new { name, languageCultureCode });
            //if (localizationResource != null)
            //    _dataContext.Delete(localizationResource);
        }

        public virtual LocalizationResource GetLocalizationResourceByName(string name, string languageCultureCode)
        {
            //return _dataContext.FirstProcedure<LocalizationResource>("Localization_Find", new { name, languageCultureCode });
            return null;
        }

        public virtual IList<LocalizationResource> GetAllResources(string languageCultureCode)
        {
            //return _dataContext.Query<LocalizationResource>("Localization_FindAll", new { languageCultureCode }).ToList();
            return null;
        }

        public virtual string GetResourceValue(string name, string defaultValue = "", bool setEmptyIfNotFound = false)
        {
            //return GetResourceValue(name, _workContext.CurrentLanguage.LanguageCulture, defaultValue, setEmptyIfNotFound);
            return null;
        }

        public virtual string GetResourceValue(string name, string languageCultureCode, string defaultValue = "", bool setEmptyIfNotFound = false)
        {
            //if (name == null)
            //    name = string.Empty;

            //name = name.Trim().ToLowerInvariant();
            //string cacheKey = string.Format("localization.resource.{0}.{1}", name, languageCultureCode);
            //var localizationResource = _cacheManager.Get(cacheKey, () =>
            //{
            //    return _dataContext.FirstProcedure<LocalizationResource>("Localization_Find", new { name, languageCultureCode });
            //});

            //var value = string.Empty;
            //if (localizationResource != null)
            //    value = localizationResource.Value;

            //if (string.IsNullOrEmpty(value))
            //{
            //    if (!string.IsNullOrEmpty(defaultValue))
            //        value = defaultValue;
            //    else
            //    {
            //        if (!setEmptyIfNotFound)
            //            value = name;
            //    }
            //}

            //return value;
            return null;
        }
    }
}