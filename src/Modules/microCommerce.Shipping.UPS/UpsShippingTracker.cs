using microCommerce.Module.Core.Shipping;
using System.Collections.Generic;

namespace microCommerce.Shipping.UPS
{
    public class UpsShippingTracker : IShippingTracker
    {
        public string GetTrackingUrl(string trackingNumber)
        {
            return string.Format("http://wwwapps.ups.com/WebTracking/track?trackNums={0}&track.x=Track", trackingNumber);
        }

        public IList<ShippingTrackerResponse> GetEvents(string trackingNumber)
        {
            if (string.IsNullOrEmpty(trackingNumber))
                return new List<ShippingTrackerResponse>();
            
            return new List<ShippingTrackerResponse>();
        }
    }
}