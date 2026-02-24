namespace MaxMind.Db;

/// <summary>
/// Simple type to allocate using the new keyword. Does not return.
/// </summary>
public class ManagedHeapAllocator<T> : IAllocator<T> where T : IResettable, new()
{
    /// <summary>
    /// Allocate the object
    /// </summary>
    /// <returns></returns>
    public T Allocate()
    {
        return new T();
    }

    /// <summary>
    /// Does not return. Will be optimized away.
    /// </summary>
    /// <param name="value"></param>
    public void Return(T value)
    {
        return;
    }

}