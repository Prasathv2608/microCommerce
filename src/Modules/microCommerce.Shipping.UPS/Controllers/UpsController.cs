using microCommerce.Mvc.Controllers;
using microCommerce.Shipping.UPS.Models;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Shipping.UPS.Controllers
{
    public class UpsController : BaseModuleController
    {
        public IActionResult Configure()
        {
            var model = new ConfigureViewModel();
            return View("~/Modules/Shipping.UPS/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigureViewModel model)
        {
            return View("~/Modules/Shipping.UPS/Views/Configure.cshtml", model);
        }
    }
}