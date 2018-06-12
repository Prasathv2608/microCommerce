namespace microCommerce.Common.Configurations
{
    public interface IAppConfiguration
    {
        /// <summary>
        /// Gets or sets data caching enabled for database performance
        /// </summary>
        bool CachingEnabled { get; set; }
        
        /// <summary>
        /// Logging error, info, warning messages enabled
        /// </summary>
        bool LoggingEnabled { get; set; }
    }
}