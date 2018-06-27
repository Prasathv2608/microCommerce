using microCommerce.Mvc.Controllers;
using microCommerce.Payment.CreditCard.Models;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Payment.CreditCard.Controllers
{
    public class CreditCardController : BaseModuleController
    {
        public IActionResult Configure()
        {
            var model = new ConfigureViewModel();
            return View("~/Modules/Payment.CreditCard/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigureViewModel model)
        {
            return View("~/Modules/Payment.CreditCard/Views/Configure.cshtml", model);
        }
    }
}