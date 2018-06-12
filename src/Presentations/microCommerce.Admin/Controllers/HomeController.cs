using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}