using microCommerce.Mvc.Attributes;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class CheckoutController : WebBaseController
    {
        public virtual IActionResult Address()
        {
            return View();
        }

        [HttpPost]
        [FormValueRequired("address-step")]
        public virtual IActionResult Address(IFormCollection form)
        {
            return View();
        }

        public virtual IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        [FormValueRequired("payment-step")]
        public virtual IActionResult Payment(IFormCollection form)
        {
            return View();
        }

        public virtual IActionResult Confirm()
        {
            return View();
        }

        [HttpPost]
        [FormValueRequired("confirm")]
        public virtual IActionResult Confirm(IFormCollection form)
        {
            return View();
        }

        public virtual IActionResult PreComplete()
        {
            return View();
        }

        public virtual IActionResult Completed(int orderId)
        {
            return View();
        }
    }
}