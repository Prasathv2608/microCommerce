using microCommerce.Module.Core;
using microCommerce.Module.Core.Widgets;
using System.Collections.Generic;

namespace microCommerce.Widget.Slider
{
    public class SliderModule : BaseModule, IWidgetModule
    {
        public string ViewComponentName
        {
            get
            {
                return "Slider";
            }
        }

        public override string ConfigurationUrl
        {
            get
            {
                return "/Admin/Slider/Configure";
            }
        }

        public IList<string> WidgetZones
        {
            get
            {
                return new List<string> { "HomePage_Top" };
            }
        }

        public override void Install()
        {
            base.Install();
        }

        public override void Uninstall()
        {
            base.Uninstall();
        }
    }
}