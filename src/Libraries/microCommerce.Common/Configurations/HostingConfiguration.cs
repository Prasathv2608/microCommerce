﻿namespace microCommerce.Common.Configurations
{
    public class HostingConfiguration
    {
        /// <summary>
        /// Gets or sets custom forwarded HTTP header (e.g. CF-Connecting-IP, X-FORWARDED-PROTO, etc)
        /// </summary>
        public string ForwardedHttpHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use HTTP_CLUSTER_HTTPS
        /// </summary>
        public bool UseHttpClusterHttps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use HTTP_X_FORWARDED_PROTO
        /// </summary>
        public bool UseHttpXForwardedProto { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we compress response
        /// </summary>
        public bool UseResponseCompression { get; set; }

        /// <summary>
        /// Gets or sets the application root path e.g ~/images
        /// </summary>
        public string ApplicationRootPath { get; set; }

        /// <summary>
        /// Gets or sets the content folder path e.g ~/wwwwroot/images
        /// </summary>
        public string ContentRootPath { get; set; }

        /// <summary>
        /// Gets or sets the modules folder path e.g ~/Modules
        /// </summary>
        public string ModulesRootPath { get; set; }
    }
}