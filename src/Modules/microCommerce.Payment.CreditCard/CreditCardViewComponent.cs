using microCommerce.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Payment.CreditCard
{
    public class CreditCardViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Modules/Payment.CreditCard/Views/Public.cshtml");
        }
    }
}