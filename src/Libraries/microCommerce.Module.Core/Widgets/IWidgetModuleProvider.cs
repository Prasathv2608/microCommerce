using System.Collections.Generic;

namespace microCommerce.Module.Core.Widgets
{
    public interface IWidgetModuleProvider
    {
        IList<IWidgetModule> LoadWidgetModules(bool showHidden = false);

        IList<IWidgetModule> LoadWidgetModulesByZone(string widgetZone, bool showHidden = false);

        IWidgetModule LoadWidgetModuleBySystemName(string systemName);
    }
}