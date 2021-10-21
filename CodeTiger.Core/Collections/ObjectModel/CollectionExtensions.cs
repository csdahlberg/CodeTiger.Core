using System.Collections.Generic;

namespace CodeTiger.Collections.ObjectModel
{
    /// <summary>
    /// Contains extension methods for the <see cref="ICollection{T}"/> class.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a collection of values to a <see cref="ICollection{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to add values to.</param>
        /// <param name="values">The values to add to <paramref name="collection"/>.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            Guard.ArgumentIsNotNull(nameof(collection), collection);
            Guard.ArgumentIsNotNull(nameof(values), values);
            
            foreach (var value in values)
            {
                collection.Add(value);
            }
        }
    }
}
