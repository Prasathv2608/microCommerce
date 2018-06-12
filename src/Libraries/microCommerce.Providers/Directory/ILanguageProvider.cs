using microCommerce.Domain.Directory;
using System.Collections.Generic;

namespace microCommerce.Providers.Directory
{
    public interface ILanguageProvider
    {
        Language GetLanguageById(int languageId);

        Language GetLanguageByCulture(string culture);

        IList<Language> GetAllLanguages(bool showHidden = false);

        void InsertLanguage(Language language);

        void UpdateLanguage(Language language);

        void DeleteLanguage(Language language);
    }
}