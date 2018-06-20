using microCommerce.Mvc.Models;
using Microsoft.AspNetCore.Routing;

namespace microCommerce.Web.Models.Common
{
    public class WidgetViewModel : BaseModel
    {
        public string ViewComponentName { get; set; }
        public RouteValueDictionary ViewComponentArguments { get; set; }
    }
}