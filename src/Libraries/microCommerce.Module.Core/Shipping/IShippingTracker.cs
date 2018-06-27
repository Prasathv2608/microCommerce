using System.Collections.Generic;

namespace microCommerce.Module.Core.Shipping
{
    public interface IShippingTracker
    {
        string GetTrackingUrl(string trackingNumber);

        IList<ShippingTrackerResponse> GetEvents(string trackingNumber);
    }
}