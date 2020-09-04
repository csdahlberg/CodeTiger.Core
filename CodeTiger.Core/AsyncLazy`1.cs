using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CodeTiger
{
    /// <summary>
    /// Provides support for asynchronous lazy initialization.
    /// </summary>
    /// <typeparam name="T">The type of object to be lazily initialized.</typeparam>
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses the default constructor of
        /// <typeparamref name="T"/> and a thread-safety mode of
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// </summary>
        public AsyncLazy()
            : base(() => Task.FromResult(Activator.CreateInstance<T>()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses the default constructor of
        /// <typeparamref name="T"/> and a thread-safety mode determined by <paramref name="isThreadSafe"/> (
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/> if <c>true</c> or
        /// <see cref="LazyThreadSafetyMode.None"/> if <c>false</c>).
        /// </summary>
        /// <param name="isThreadSafe">Indicates whether less-performant but thread-safe operations should be used
        /// when creating or reading the lazy-initialized value.</param>
        public AsyncLazy(bool isThreadSafe)
            : base(() => Task.FromResult(Activator.CreateInstance<T>()), isThreadSafe)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses the default constructor of
        /// <typeparamref name="T"/> and a specified thread-safety mode.
        /// </summary>
        /// <param name="mode">Specifies the thread-safety mode to use when creating or reading the
        /// lazy-initialized value.</param>
        public AsyncLazy(LazyThreadSafetyMode mode)
            : base(() => Task.FromResult(Activator.CreateInstance<T>()), mode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a provided initialization
        /// function and a thread-safety mode of <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// <typeparamref name="T"/> and a thread-safety mode of
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>.
        /// </summary>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is ")]
        public AsyncLazy(Func<Task<T>> valueFactory)
            : base(valueFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a provided initialization
        /// function and a thread-safety mode determined by <paramref name="isThreadSafe"/> (
        /// <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/> if <c>true</c> or
        /// <see cref="LazyThreadSafetyMode.None"/> if <c>false</c>).
        /// </summary>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        /// <param name="isThreadSafe">Indicates whether less-performant but thread-safe operations should be used
        /// when creating or reading the lazy-initialized value.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Nesting of a generic type within a Func or Action type is acceptable.")]
        public AsyncLazy(Func<Task<T>> valueFactory, bool isThreadSafe)
            : base(valueFactory, isThreadSafe)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a provided initialization
        /// function and specified thread-safety mode.
        /// </summary>
        /// <param name="valueFactory">The function to use to produce the lazily initialized value.</param>
        /// <param name="mode">Specifies the thread-safety mode to use when creating or reading the
        /// lazy-initialized value.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Nesting of a generic type within a Func or Action type is acceptable.")]
        public AsyncLazy(Func<Task<T>> valueFactory, LazyThreadSafetyMode mode)
            : base(valueFactory, mode)
        {
        }

        /// <summary>
        /// Gets an awaiter used to await <see cref="Lazy{T}.Value"/>.
        /// </summary>
        /// <returns>An awaiter instance.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "It is not appropriate to use a property here.")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Awaiter",
            Justification = "Awaiter is spelled correctly.")]
        public TaskAwaiter<T> GetAwaiter()
        {
            return Value.GetAwaiter();
        }
    }
}
