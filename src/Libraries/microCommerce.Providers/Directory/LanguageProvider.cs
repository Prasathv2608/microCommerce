using microCommerce.Caching;
using microCommerce.Dapper;
using microCommerce.Domain.Directory;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Providers.Directory
{
    public class LanguageProvider : ILanguageProvider
    {
        private readonly IDataContext _dataContext;
        private readonly ICacheManager _cacheManager;

        public LanguageProvider(IDataContext dataContext,
            ICacheManager cacheManager)
        {
            _dataContext = dataContext;
            _cacheManager = cacheManager;
        }

        public virtual Language GetLanguageById(int languageId)
        {
            if (languageId == 0)
                return null;

            return _cacheManager.Get(string.Format(ProviderCacheKey.LANGUAGE_BY_IDENTITY_KEY, languageId), () =>
            {
                return _dataContext.First<Language>("SELECT * FROM Language WHERE Id = @languageId LIMIT 1", new { languageId });
            });
        }

        public virtual Language GetLanguageByCulture(string culture)
        {
            if (string.IsNullOrEmpty(culture))
                return null;

            return _cacheManager.Get(string.Format(ProviderCacheKey.LANGUAGE_BY_CODE_KEY, culture), () =>
            {
                return _dataContext.First<Language>("SELECT * FROM Language WHERE CultureCode = @culture LIMIT 1", new { culture });
            });
        }

        public virtual IList<Language> GetAllLanguages(bool showHidden = false)
        {
            return _cacheManager.Get(string.Format(ProviderCacheKey.LANGUAGES_ALL_KEY, showHidden), () =>
            {
                if (showHidden)
                    return _dataContext.Query<Language>("SELECT * FROM Language").ToList();

                return _dataContext.Query<Language>("SELECT * FROM Language WHERE Published = 1").ToList();
            });
        }

        public virtual void InsertLanguage(Language language)
        {
            _dataContext.Insert(language);
        }

        public virtual void UpdateLanguage(Language language)
        {
            _dataContext.Update(language);
        }

        public virtual void DeleteLanguage(Language language)
        {
            _dataContext.Delete(language);
        }
    }
}