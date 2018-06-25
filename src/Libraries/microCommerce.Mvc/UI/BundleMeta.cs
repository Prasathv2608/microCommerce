namespace microCommerce.Mvc.UI
{
    /// <summary>
    /// JS file meta data
    /// </summary>
    internal class BundleMeta
    {
        /// <summary>
        /// A value indicating whether to exclude the script from bundling
        /// </summary>
        public bool ExcludeFromBundle { get; set; }

        /// <summary>
        /// Src for production
        /// </summary>
        public string Path { get; set; }
    }
}