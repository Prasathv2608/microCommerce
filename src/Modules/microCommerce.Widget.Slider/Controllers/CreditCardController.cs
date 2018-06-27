using microCommerce.Mvc.Controllers;
using microCommerce.Widget.Slider.Models;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Widget.Slider.Controllers
{
    public class CreditCardController : BaseModuleController
    {
        public IActionResult Configure()
        {
            var model = new ConfigureViewModel();
            return View("~/Modules/Widget.Slider/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigureViewModel model)
        {
            return View("~/Modules/Widget.Slider/Views/Configure.cshtml", model);
        }
    }
}