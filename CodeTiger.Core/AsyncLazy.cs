using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace CodeTiger
{
    /// <summary>
    /// Provides static methods for creating <see cref="AsyncLazy{T}"/> objects.
    /// </summary>
    public static class AsyncLazy
    {
        /// <summary>
        /// Creates a new <see cref="AsyncLazy{T}"/> object which will use the default constructor of
        /// <typeparamref name="T"/> and a thread-safety mode of
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <returns>A new <see cref="AsyncLazy{T}"/>.</returns>
        public static AsyncLazy<T> Create<T>()
        {
            return new AsyncLazy<T>();
        }

        /// <summary>
        /// Creates a new <see cref="AsyncLazy{T}"/> object which will use the default constructor of
        /// <typeparamref name="T"/> and a thread-safety mode determined by <paramref name="isThreadSafe"/> (
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/> if <c>true</c> or
        /// <see cref="LazyThreadSafetyMode.None"/> if <c>false</c>).
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="isThreadSafe">Indicates whether less-performant but thread-safe operations should be used
        /// when creating or reading the lazy-initialized value.</param>
        /// <returns>A new <see cref="AsyncLazy{T}"/>.</returns>
        public static AsyncLazy<T> Create<T>(bool isThreadSafe)
        {
            return new AsyncLazy<T>(isThreadSafe);
        }

        /// <summary>
        /// Creates a new <see cref="AsyncLazy{T}"/> object which will use the default constructor of
        /// <typeparamref name="T"/> and a specified thread-safety mode.
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="mode">Specifies the thread-safety mode to use when creating or reading the
        /// lazy-initialized value.</param>
        /// <returns>A new <see cref="AsyncLazy{T}"/>.</returns>
        public static AsyncLazy<T> Create<T>(LazyThreadSafetyMode mode)
        {
            return new AsyncLazy<T>(mode);
        }

        /// <summary>
        /// Creates a new <see cref="AsyncLazy{T}"/> object which will use a provided initialization
        /// function and a thread-safety mode of <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// <typeparamref name="T"/> and a thread-safety mode of
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        /// <returns>A new <see cref="AsyncLazy{T}"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Nesting of a generic type within a Func or Action type is acceptable.")]
        public static AsyncLazy<T> Create<T>(Func<Task<T>> valueFactory)
        {
            return new AsyncLazy<T>(valueFactory);
        }

        /// <summary>
        /// Creates a new <see cref="AsyncLazy{T}"/> object which will use a provided initialization
        /// function and a thread-safety mode determined by <paramref name="isThreadSafe"/> (
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/> if <c>true</c> or
        /// <see cref="LazyThreadSafetyMode.None"/> if <c>false</c>).
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        /// <param name="isThreadSafe">Indicates whether less-performant but thread-safe operations should be used
        /// when creating or reading the lazy-initialized value.</param>
        /// <returns>A new <see cref="AsyncLazy{T}"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Nesting of a generic type within a Func or Action type is acceptable.")]
        public static AsyncLazy<T> Create<T>(Func<Task<T>> valueFactory, bool isThreadSafe)
        {
            return new AsyncLazy<T>(valueFactory, isThreadSafe);
        }

        /// <summary>
        /// Creates a new <see cref="AsyncLazy{T}"/> object which will use a provided initialization
        /// function and specified thread-safety mode.
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        /// <param name="mode">Specifies the thread-safety mode to use when creating or reading the
        /// lazy-initialized value.</param>
        /// <returns>A new <see cref="AsyncLazy{T}"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Nesting of a generic type within a Func or Action type is acceptable.")]
        public static AsyncLazy<T> Create<T>(Func<Task<T>> valueFactory, LazyThreadSafetyMode mode)
        {
            return new AsyncLazy<T>(valueFactory, mode);
        }
    }
}
