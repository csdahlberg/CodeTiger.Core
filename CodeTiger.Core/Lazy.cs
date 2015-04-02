using System;
using System.Threading;

namespace CodeTiger
{
    /// <summary>
    /// Provides static methods for creating <see cref="Lazy{T}"/> objects.
    /// </summary>
    public static class Lazy
    {
        /// <summary>
        /// Creates a new <see cref="Lazy{T}"/> object which will use the default constructor of
        /// <typeparamref name="T"/> and a thread-safety mode of
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <returns>A new <see cref="Lazy{T}"/>.</returns>
        public static Lazy<T> Create<T>()
        {
            return new Lazy<T>();
        }

        /// <summary>
        /// Creates a new <see cref="Lazy{T}"/> object which will use the default constructor of
        /// <typeparamref name="T"/> and a thread-safety mode determined by <paramref name="isThreadSafe"/> (
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/> if <c>true</c> or
        /// <see cref="LazyThreadSafetyMode.None"/> if <c>false</c>).
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="isThreadSafe">Indicates whether less-performant but thread-safe operations should be used
        /// when creating or reading the lazy-initialized value.</param>
        /// <returns>A new <see cref="Lazy{T}"/>.</returns>
        public static Lazy<T> Create<T>(bool isThreadSafe)
        {
            return new Lazy<T>(isThreadSafe);
        }

        /// <summary>
        /// Creates a new <see cref="Lazy{T}"/> object which will use the default constructor of
        /// <typeparamref name="T"/> and a specified thread-safety mode.
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="mode">Specifies the thread-safety mode to use when creating or reading the
        /// lazy-initialized value.</param>
        /// <returns>A new <see cref="Lazy{T}"/>.</returns>
        public static Lazy<T> Create<T>(LazyThreadSafetyMode mode)
        {
            return new Lazy<T>(mode);
        }

        /// <summary>
        /// Creates a new <see cref="Lazy{T}"/> object which will use a provided initialization
        /// function and a thread-safety mode of <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// <typeparamref name="T"/> and a thread-safety mode of
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        /// <returns>A new <see cref="Lazy{T}"/>.</returns>
        public static Lazy<T> Create<T>(Func<T> valueFactory)
        {
            return new Lazy<T>(valueFactory);
        }

        /// <summary>
        /// Creates a new <see cref="Lazy{T}"/> object which will use a provided initialization function and a
        /// thread-safety mode determined by <paramref name="isThreadSafe"/> (
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/> if <c>true</c> or
        /// <see cref="LazyThreadSafetyMode.None"/> if <c>false</c>).
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        /// <param name="isThreadSafe">Indicates whether less-performant but thread-safe operations should be used
        /// when creating or reading the lazy-initialized value.</param>
        /// <returns>A new <see cref="Lazy{T}"/>.</returns>
        public static Lazy<T> Create<T>(Func<T> valueFactory, bool isThreadSafe)
        {
            return new Lazy<T>(valueFactory, isThreadSafe);
        }

        /// <summary>
        /// Creates a new <see cref="Lazy{T}"/> object which will use a provided initialization function and
        /// specified thread-safety mode.
        /// </summary>
        /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        /// <param name="mode">Specifies the thread-safety mode to use when creating or reading the
        /// lazy-initialized value.</param>
        /// <returns>A new <see cref="Lazy{T}"/>.</returns>
        public static Lazy<T> Create<T>(Func<T> valueFactory, LazyThreadSafetyMode mode)
        {
            return new Lazy<T>(valueFactory, mode);
        }
    }
}