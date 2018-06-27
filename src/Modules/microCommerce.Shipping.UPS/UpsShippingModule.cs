using microCommerce.Module.Core;
using microCommerce.Module.Core.Shipping;
using System;

namespace microCommerce.Shipping.UPS
{
    public class UpsShippingModule : BaseModule, IShippingModule
    {
        public ShippingMethodInfo GetShippingInfo()
        {
            throw new NotImplementedException();
        }

        public ShippingResponse CreateShipment(ShippingRequest shippingRequest)
        {
            throw new NotImplementedException();
        }

        public ShippingResponse DeleteShipment(ShippingRequest shippingRequest)
        {
            throw new NotImplementedException();
        }

        public IShippingTracker ShippingTracker
        {
            get
            {
                return new UpsShippingTracker();
            }
        }

        public override string ConfigurationUrl
        {
            get
            {
                return "";
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