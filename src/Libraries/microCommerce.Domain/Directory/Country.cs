namespace microCommerce.Domain.Directory
{
    public class Country : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the two letter ISO code
        /// </summary>
        public string TwoLetterIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the three letter ISO code
        /// </summary>
        public string ThreeLetterIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the numeric ISO code
        /// </summary>
        public int NumericIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the dial code
        /// </summary>
        public string DialCode { get; set; }

        /// <summary>
        /// Gets or sets the default currency identifier
        /// </summary>
        public int? CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the default language identifier
        /// </summary>
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the allow billing for order
        /// </summary>
        public bool AllowBilling { get; set; }

        /// <summary>
        /// Gets or sets the allow shipping for order
        /// </summary>
        public bool AllowShipping { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

    }
}