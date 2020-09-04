using System;

namespace CodeTiger
{
    /// <summary>
    /// Used to indicate that an argument passed in to a method is verified to not be null if the method completes
    /// without throwing an exception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
