using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class PageController : WebBaseController
    {
        public IActionResult Detail(int pageId)
        {
            return View();
        }
    }
}