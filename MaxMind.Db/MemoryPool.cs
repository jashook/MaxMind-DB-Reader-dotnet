using System;
using System.Buffers;

/// <summary>
/// MemoryPool
/// </summary>
/// <typeparam name="T"></typeparam>
public static class MemoryPool<T>
{
    /// <summary>
    /// The pool to allocate from
    /// </summary>
    public static ArrayPool<T> s_Pool = ArrayPool<T>.Create(maxArrayLength: 512, maxArraysPerBucket: 2048);
}

/// <summary>
/// Wrap a pooled array
/// </summary>
/// <typeparam name="T"></typeparam>
public ref struct PooledArray<T> : IDisposable
{
    private ArrayPool<T> _Pool;
    private int _Size;
    private T[] _Array;

    /// <summary>
    /// 
    /// </summary>
    public Span<T> Value => this._Array.AsSpan().Slice(0, this._Size);

    /// <summary>
    /// 
    /// </summary>
    public Memory<T> Memory => this._Array.AsMemory().Slice(0, this._Size);
    
    /// <summary>
    /// 
    /// </summary>
    public T[] Array => this._Array;

    /// <summary>
    /// 
    /// </summary>
    public int Length => this._Size;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="size"></param>
    /// <param name="pool"></param>
    public PooledArray(int size, ArrayPool<T>? pool = null)
    {
        this._Size = size;
        this._Pool = pool ?? MemoryPool<T>.s_Pool;

        this._Array = this._Pool.Rent(size);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        this._Pool.Return(this._Array);
    }
}