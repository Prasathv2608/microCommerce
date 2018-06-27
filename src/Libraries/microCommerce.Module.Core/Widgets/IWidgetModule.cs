using System.Collections.Generic;

namespace microCommerce.Module.Core.Widgets
{
    public interface IWidgetModule : IModule
    {
        string ViewComponentName { get; }

        IList<string> WidgetZones { get; }
    }
}