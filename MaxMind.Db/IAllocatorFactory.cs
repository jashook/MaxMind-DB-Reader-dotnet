using System;

namespace MaxMind.Db;

/// <summary>
/// Gets the allocator for a specific type.
/// </summary>
public interface IAllocatorFactory
{
#if !(NETSTANDARD2_0 || NETSTANDARD2_1)
    /// <summary>
    /// Gets the allocator for a specific type.
    /// </summary>
    public static abstract IAllocator<T> GetAllocatorForType<T>(Type type) where T : IResettable;
#endif // NETSTANDARD2_0
}