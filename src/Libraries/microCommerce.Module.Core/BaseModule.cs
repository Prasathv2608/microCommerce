namespace microCommerce.Module.Core
{
    public abstract class BaseModule
    {
        /// <summary>
        /// Gets or sets the plugin descriptor
        /// </summary>
        public virtual ModuleInfo ModuleInfo { get; set; }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public abstract string ConfigurationUrl { get; }

        /// <summary>
        /// Install plugin
        /// </summary>
        public virtual void Install()
        {
            ModuleManager.MarkAsInstalled(ModuleInfo.SystemName);
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public virtual void Uninstall()
        {
            ModuleManager.MarkAsUninstalled(ModuleInfo.SystemName);
        }
    }
}