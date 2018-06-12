using microCommerce.Ioc;
using microCommerce.Providers.Localizations;
using Microsoft.AspNetCore.Mvc.Razor;

namespace microCommerce.Mvc.UI
{
    public abstract class CustomRazorPage<TModel> : RazorPage<TModel>
    {
        private Localizer _localizer;
        private ILocalizationProvider _localizationProvider;

        public virtual Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    if (_localizationProvider == null)
                        _localizationProvider = EngineContext.Current.Resolve<ILocalizationProvider>();

                    _localizer = (format, args) =>
                    {
                        string resFormat = _localizationProvider.GetResourceValue(format);
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }

                        return new LocalizedString((args == null || args.Length == 0)
                            ? resFormat
                            : string.Format(resFormat, args));
                    };
                }

                return _localizer;
            }
        }
    }

    public abstract class CustomRazorPage : CustomRazorPage<dynamic>
    {
    }
}