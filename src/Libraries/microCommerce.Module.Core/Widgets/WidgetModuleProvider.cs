using microCommerce.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Module.Core.Widgets
{
    public class WidgetModuleProvider : IWidgetModuleProvider
    {
        private readonly IModuleProvider _moduleProvider;
        private readonly WidgetSettings _widgetSettings;

        public WidgetModuleProvider(IModuleProvider moduleProvider,
            WidgetSettings widgetSettings)
        {
            _moduleProvider = moduleProvider;
            _widgetSettings = widgetSettings;
        }

        public virtual IList<IWidgetModule> LoadWidgetModules(bool showHidden = false)
        {
            var widgetModules = _moduleProvider.LoadModules<IWidgetModule>();

            if (!showHidden)
                widgetModules = widgetModules.Where(pm => _widgetSettings.ActiveWidgetModules
                     .Contains(pm.ModuleInfo.SystemName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return widgetModules;
        }

        public virtual IList<IWidgetModule> LoadWidgetModulesByZone(string widgetZone, bool showHidden = false)
        {
            if (string.IsNullOrEmpty(widgetZone))
                return new List<IWidgetModule>();

            return LoadWidgetModules(showHidden)
                .Where(wm => wm.WidgetZones.Contains(widgetZone, StringComparer.InvariantCultureIgnoreCase)).ToList();
        }

        public virtual IWidgetModule LoadWidgetModuleBySystemName(string systemName)
        {
            return _moduleProvider.LoadModules<IWidgetModule>()
                .FirstOrDefault(pm => pm.ModuleInfo.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}