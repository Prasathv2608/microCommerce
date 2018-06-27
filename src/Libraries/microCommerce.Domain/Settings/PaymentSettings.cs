namespace microCommerce.Domain.Settings
{
    public class PaymentSettings : ISettings
    {
        public string ActivePaymentModules { get; set; }
    }
}