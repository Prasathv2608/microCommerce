using System;

namespace microCommerce.Domain
{
    /// <summary>
    /// identity column attribute for database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IdentityAttribute : Attribute
    {
    }
}