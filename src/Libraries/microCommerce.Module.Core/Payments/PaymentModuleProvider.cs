using microCommerce.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Module.Core.Payments
{
    public class PaymentModuleProvider : IPaymentModuleProvider
    {
        private readonly IModuleProvider _moduleProvider;
        private readonly PaymentSettings _paymentSettings;

        public PaymentModuleProvider(IModuleProvider moduleProvider,
            PaymentSettings paymentSettings)
        {
            _moduleProvider = moduleProvider;
            _paymentSettings = paymentSettings;
        }

        public virtual IList<IPaymentModule> LoadPaymentModules(bool showHidden = false)
        {
            var paymentModules = _moduleProvider.LoadModules<IPaymentModule>();

            if (!showHidden)
                paymentModules = paymentModules.Where(pm => _paymentSettings.ActivePaymentModules
                     .Contains(pm.ModuleInfo.SystemName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return paymentModules;
        }

        public virtual IPaymentModule LoadPaymentModuleBySystemName(string systemName)
        {
            return _moduleProvider.LoadModules<IPaymentModule>()
                .FirstOrDefault(pm => pm.ModuleInfo.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}