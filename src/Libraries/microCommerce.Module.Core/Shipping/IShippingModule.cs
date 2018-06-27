namespace microCommerce.Module.Core.Shipping
{
    public interface IShippingModule : IModule
    {
        ShippingMethodInfo GetShippingInfo();

        ShippingResponse CreateShipment(ShippingRequest shippingRequest);

        ShippingResponse DeleteShipment(ShippingRequest shippingRequest);

        IShippingTracker ShippingTracker { get; }
    }
}