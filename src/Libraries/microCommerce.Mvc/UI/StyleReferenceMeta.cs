using System;

namespace microCommerce.Mvc.UI
{
    internal class StyleReferenceMeta : IEquatable<StyleReferenceMeta>
    {
        public bool ExcludeFromBundle { get; set; }

        /// <summary>
        /// Src for production
        /// </summary>
        public string Src { get; set; }
        
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="item">Other item</param>
        /// <returns>Result</returns>
        public bool Equals(StyleReferenceMeta item)
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