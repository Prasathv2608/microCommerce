using System.Collections.Generic;

namespace microCommerce.Module.Core.Payments
{
    public interface IPaymentModuleProvider
    {
        IList<IPaymentModule> LoadPaymentModules(bool showHidden = false);

        IPaymentModule LoadPaymentModuleBySystemName(string systemName);
    }
}