namespace microCommerce.Common.Configurations
{
    public class ServiceConfiguration : IAppConfiguration
    {
        /// <summary>
        /// Gets or sets the application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the aplication description
        /// </summary>
        public string ApplicationDescription { get; set; }

        /// <summary>
        /// Gets or sets the api current working version
        /// </summary>
        public int CurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the connection strings
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database provider
        /// </summary>
        public string DatabaseProviderName { get; set; }

        /// <summary>
        /// Gets or sets data caching enabled for database performance
        /// </summary>
        public bool CachingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the cache database index
        /// </summary>
        public int CacheDatabaseIndex { get; set; }

        /// <summary>
        /// Logging error, info, warning messages enabled
        /// </summary>
        public bool LoggingEnabled { get; set; }
    }
}