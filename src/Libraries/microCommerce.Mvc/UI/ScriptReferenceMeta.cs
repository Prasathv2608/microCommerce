using System;

namespace microCommerce.Mvc.UI
{
    /// <summary>
    /// JS file meta data
    /// </summary>
    internal class ScriptReferenceMeta : IEquatable<ScriptReferenceMeta>
    {
        /// <summary>
        /// A value indicating whether to exclude the script from bundling
        /// </summary>
        public bool ExcludeFromBundle { get; set; }

        /// <summary>
        /// A value indicating whether to load the script asynchronously 
        /// </summary>
        public bool IsAsync { get; set; }

        /// <summary>
        /// Src for production
        /// </summary>
        public string Src { get; set; }
        
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="item">Other item</param>
        /// <returns>Result</returns>
        public bool Equals(ScriptReferenceMeta item)
        {
            if (item == null)
                return false;

            return Src.Equals(item.Src);
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Src == null ? 0 : Src.GetHashCode();
        }
    }
}