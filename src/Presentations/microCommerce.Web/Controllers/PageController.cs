using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class PageController : FrontBaseController
    {
        public IActionResult Detail(int pageId)
        {
            return View();
        }
    }
}