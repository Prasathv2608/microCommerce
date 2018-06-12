namespace microCommerce.Domain.Localizations
{
    public class LocalizationResource : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string LanguageCultureCode { get; set; }
    }
}