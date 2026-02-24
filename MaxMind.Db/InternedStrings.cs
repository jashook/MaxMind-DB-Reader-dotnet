using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MaxMind.Db;

#if NET10_0_OR_GREATER
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public sealed class ReadOnlyMemoryByteComparer : IEqualityComparer<ReadOnlyMemory<byte>>

{
    public static ReadOnlyMemoryByteComparer Default { get; } = new ReadOnlyMemoryByteComparer();

    public bool Equals(ReadOnlyMemory<byte> x, ReadOnlyMemory<byte> y)
    {
        // Use the sequence equal method to compare the contents
        return x.Span.SequenceEqual(y.Span);
    }

    public int GetHashCode(ReadOnlyMemory<byte> obj)
    {
        // This is a simple, non-cryptographic hash code generation based on the content.
        // For production use, consider a more robust hashing algorithm for byte sequences.
        // The implementation below sums up bytes in chunks of int32 for performance.
        // This is a basic approach and might not be suitable for high-security scenarios
        // due to potential hash collisions (similar to string hashing behavior in dictionaries).
        
        unchecked
        {
            int hash = 17;
            ReadOnlySpan<byte> span = obj.Span;
            
            // Simple hashing of the sequence
            foreach (byte b in span)
            {
                hash = hash * 31 + b;
            }
            return hash;
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


/// <summary>
/// 
/// </summary>
public static class InternedStrings
{
    internal static Dictionary<ReadOnlyMemory<byte>, string> s_Dictionary = new (ReadOnlyMemoryByteComparer.Default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string GetString(ReadOnlyMemory<byte> bytes)
    {
        bool found = s_Dictionary.TryGetValue(bytes, out string? returnValue);

        if (!found)
        {
            returnValue = Encoding.UTF8.GetString(bytes.Span);
            s_Dictionary.TryAdd(bytes, returnValue);
        }

        Debug.Assert(returnValue is not null);
        return returnValue;
    }
}

#endif