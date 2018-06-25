using microCommerce.Mvc.Attributes;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.Web.Controllers
{
    public class ShoppingController : WebBaseController
    {
        public virtual IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public virtual IActionResult AddProduct(int productId, int quantity, IFormCollection form)
        {
            return View();
        }

        [HttpPost, ActionName("shopping")]
        [FormValueRequired("updatecart")]
        public virtual IActionResult UpdateCart(IFormCollection form)
        {
            return View();
        }
        
        [HttpPost, ActionName("shopping")]
        [FormValueRequired("apply-coupon-code")]
        public virtual IActionResult ApplyCouponCode(string couponCode)
        {
            return View();
        }

        [HttpPost, ActionName("Cart")]
        [FormValueRequired(FormValueRequirement.StartsWith, "remove-coupon-code-")]
        public virtual IActionResult RemoveCouponCode(string couponCode)
        {
            return View();
        }

        [HttpPost, ActionName("shopping")]
        [FormValueRequired("checkout")]
        public virtual IActionResult StartCheckout(IFormCollection form)
        {
            return View();
        }
    }
}