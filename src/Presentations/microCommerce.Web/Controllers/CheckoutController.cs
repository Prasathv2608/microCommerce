using microCommerce.Module.Core.Payments;
using microCommerce.Mvc.Attributes;
using microCommerce.Mvc.Controllers;
using microCommerce.Web.Models.Checkout;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace microCommerce.Web.Controllers
{
    public class CheckoutController : WebBaseController
    {
        private readonly IPaymentModuleProvider _paymentModuleProvider;

        public CheckoutController(IPaymentModuleProvider paymentModuleProvider)
        {
            _paymentModuleProvider = paymentModuleProvider;
        }

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
            var model = new PaymentViewModel
            {
                PaymentMethods = _paymentModuleProvider
                .LoadPaymentModules().Select(pm => new PaymentMethodViewModel
                {
                    ViewComponentName = pm.ViewComponentName,
                    PaymentMethodName = pm.ModuleInfo.FriendlyName,
                    PaymentMethodSystemName = pm.ModuleInfo.SystemName
                }).ToList()
            };

            return View(model);
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