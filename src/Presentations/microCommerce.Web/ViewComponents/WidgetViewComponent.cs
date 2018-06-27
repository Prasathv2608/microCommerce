using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Module.Core.Widgets;
using microCommerce.Mvc.UI;
using microCommerce.Web.Infrastructure;
using microCommerce.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Linq;

namespace microCommerce.Web.ViewComponents
{
    public class WidgetViewComponent : BaseViewComponent
    {
        private readonly IWidgetModuleProvider _widgetModuleProvider;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;

        public WidgetViewComponent(IWidgetModuleProvider widgetModuleProvider,
            ICacheManager cacheManager,
            IWorkContext workContext)
        {
            _widgetModuleProvider = widgetModuleProvider;
            _cacheManager = cacheManager;
            _workContext = workContext;
        }

        public virtual IViewComponentResult Invoke(string widgetZone, object additionalData = null)
        {
            var cacheKey = string.Format(PublicCacheKeys.WidgetCacheKey, widgetZone, _workContext.CurrentTheme.ThemeName);
            var widgets = _cacheManager.Get(cacheKey, () =>
            {
                return _widgetModuleProvider.LoadWidgetModulesByZone(widgetZone);
            });

            var componentArguments = new RouteValueDictionary
            {
                { "widgetZone", widgetZone },
                { "additionalData", additionalData }
            };

            var model = widgets.Select(widget => new WidgetViewModel
            {
                ViewComponentName = widget.ViewComponentName,
                ViewComponentArguments = componentArguments
            }).ToList();
            
            return View(model);
        }
    }
}