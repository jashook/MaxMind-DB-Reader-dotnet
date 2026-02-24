namespace MaxMind.Db;

/// <summary>
/// Implementable allocator which can be used for decoding
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAllocator<T>
{
    /// <summary>
    /// Allocate the method
    /// </summary>
    /// <returns>Object being allocated</returns>
    public T Allocate();

    /// <summary>
    /// Return the value back to the allocator
    /// </summary>
    /// <param name="value"></param>
    public void Return(T value);
}