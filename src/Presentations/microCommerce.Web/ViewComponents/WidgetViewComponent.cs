using microCommerce.Mvc.UI;
using microCommerce.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace microCommerce.Web.ViewComponents
{
    public class WidgetViewComponent : BaseViewComponent
    {
        public virtual IViewComponentResult Invoke(string widgetZone, object additionalData = null)
        {
            var model = new List<WidgetViewModel>();
            return View(model);
        }
    }
}