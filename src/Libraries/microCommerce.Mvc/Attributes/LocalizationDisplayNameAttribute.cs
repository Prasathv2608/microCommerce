using microCommerce.Common;
using microCommerce.Ioc;
using microCommerce.Providers.Localizations;
using System.ComponentModel;

namespace microCommerce.Mvc.Attributes
{
    public class LocalizationDisplayNameAttribute : DisplayNameAttribute
    {
        #region Ctor
        /// <summary>
        /// Create instance of the attribute
        /// </summary>
        /// <param name="resourceKey">Key of the locale resource</param>
        public LocalizationDisplayNameAttribute(string resourceKey) : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets key of the locale resource 
        /// </summary>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Getss the display name
        /// </summary>
        public override string DisplayName
        {
            get
            {
                //get working language identifier
                var languageCulture = IocContainer.Current.Resolve<IWorkContext>().CurrentLanguage.LanguageCulture;

                //get locale resource value
                string _resourceValue = IocContainer.Current.Resolve<ILocalizationProvider>().GetResourceValue(ResourceKey, languageCulture, ResourceKey);

                return _resourceValue;
            }
        }

        /// <summary>
        /// Gets name of the attribute
        /// </summary>
        public string Name
        {
            get { return nameof(LocalizationDisplayNameAttribute); }
        }
        #endregion
    }
}