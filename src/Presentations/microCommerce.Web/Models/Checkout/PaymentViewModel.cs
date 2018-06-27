using microCommerce.Mvc.Models;
using System.Collections.Generic;

namespace microCommerce.Web.Models.Checkout
{
    public class PaymentViewModel : BaseModel
    {
        public PaymentViewModel()
        {
            PaymentMethods = new List<PaymentMethodViewModel>();
        }

        public IList<PaymentMethodViewModel> PaymentMethods { get; set; }
    }

    public class PaymentMethodViewModel : BaseModel
    {
        public string ViewComponentName { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodSystemName { get; set; }
    }
}