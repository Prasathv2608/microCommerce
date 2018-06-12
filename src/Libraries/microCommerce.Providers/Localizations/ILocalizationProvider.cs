using microCommerce.Domain.Localizations;
using System.Collections.Generic;

namespace microCommerce.Providers.Localizations
{
    public interface ILocalizationProvider
    {
        void InsertLocalizationResource(string name, string value, string languageCultureCode);
        void UpdateLocalizationResource(string name, string value, string languageCultureCode);
        void DeleteLocalizationResource(string name, string languageCultureCode);
        LocalizationResource GetLocalizationResourceByName(string name, string languageCultureCode);
        IList<LocalizationResource> GetAllResources(string languageCultureCode);
        string GetResourceValue(string name, string defaultValue = null, bool setEmptyIfNotFound = false);
        string GetResourceValue(string name, string languageCultureCode, string defaultValue = null, bool setEmptyIfNotFound = false);
    }
}