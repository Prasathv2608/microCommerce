namespace microCommerce.Providers
{
    public class ProviderCacheKey
    {
        //Setting
        public const string SETTINGS_ALL_KEY = "Setting.all-{0}";
        public const string SETTINGS_PATTERN_KEY = "Setting.";

        //Currency
        public const string CURRENCY_BY_IDENTITY_KEY = "Currency.by.identity-{0}";
        public const string CURRENCY_BY_CODE_KEY = "Currency.by.code-{0}";
        public const string CURRENCIES_ALL_KEY = "Currency.all-{0}";
        public const string CURRENCIES_PATTERN_KEY = "Currency.";

        //Country
        public const string COUNTRY_BY_IDENTITY_KEY = "Country.by.identity-{0}";
        public const string COUNTRY_BY_CODE_KEY = "Country.by.code-{0}";
        public const string COUNTRIES_ALL_KEY = "Country.all-{0}";
        public const string COUNTRIES_PATTERN_KEY = "Country.";

        //Language
        public const string LANGUAGE_BY_IDENTITY_KEY = "Language.by.identity-{0}";
        public const string LANGUAGE_BY_CODE_KEY = "Language.by.code-{0}";
        public const string LANGUAGES_ALL_KEY = "Language.all-{0}";
        public const string LANGUAGES_PATTERN_KEY = "Language.";
    }
}