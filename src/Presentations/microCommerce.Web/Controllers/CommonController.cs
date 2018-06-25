using microCommerce.Common;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class CommonController : WebBaseController
    {
        public virtual IActionResult SetLanguage()
        {
            return View();
        }

        public virtual IActionResult SetCurrency()
        {
            return View();
        }

        public virtual IActionResult SetTheme()
        {
            return View();
        }

        public virtual IActionResult Sitemap()
        {
            return Content("", MimeTypes.TextXml);
        }

        public virtual IActionResult RobotsText()
        {
            return Content("", MimeTypes.TextPlain);
        }
    }
}