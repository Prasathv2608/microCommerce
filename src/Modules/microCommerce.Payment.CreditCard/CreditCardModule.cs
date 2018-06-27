using microCommerce.Domain.Basket;
using microCommerce.Module.Core;
using microCommerce.Module.Core.Payments;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace microCommerce.Payment.CreditCard
{
    public class CreditCardModule : BaseModule, IPaymentModule
    {
        public virtual PaymentConfirmResponse PaymentConfirm(PaymentConfirmRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual void PaymentProcess(PaymentProcessRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual IList<string> ValidateForm(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public virtual PaymentConfirmRequest GetPaymentConfirmRequest(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public virtual decimal GetAdditionalFee(IList<BasketItem> basketItems)
        {
            throw new NotImplementedException();
        }

        public virtual string ViewComponentName
        {
            get
            {
                return "CreditCard";
            }
        }

        public override string ConfigurationUrl
        {
            get
            {
                return "/Admin/CreditCard/Configure";
            }
        }

        public override void Install()
        {
            base.Install();
        }

        public override void Uninstall()
        {
            base.Uninstall();
        }
    }
}