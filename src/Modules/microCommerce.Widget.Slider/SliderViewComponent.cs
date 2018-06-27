using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Widget.Slider
{
    public class SliderViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, object additionalData = null)
        {
            return View("~/Modules/Widget.Slider/Views/Public.cshtml");
        }
    }
}