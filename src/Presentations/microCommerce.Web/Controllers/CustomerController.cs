using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class CustomerController : WebBaseController
    {
        public virtual IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public virtual IActionResult Login(IFormCollection form)
        {
            return View();
        }

        public virtual IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public virtual IActionResult Register(IFormCollection form)
        {
            return View();
        }

        public virtual IActionResult RecoveryPassword()
        {
            return View();
        }
    }
}