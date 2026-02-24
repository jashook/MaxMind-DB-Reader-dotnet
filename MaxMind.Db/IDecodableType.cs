using System;

namespace MaxMind.Db;

#if NET10_0_OR_GREATER

/// <summary>
/// State
/// </summary>
public enum DecodeState : byte
{
    /// <summary>
    /// Not found.
    /// </summary>
    NotFound,

    /// <summary>
    /// Initialized
    /// </summary>
    Initialized,

    /// <summary>
    /// Assigned
    /// </summary>
    Assigned
}

/// <summary>
/// Decodable type
/// </summary>
[CLSCompliant(false)]
public interface IDecodableType<T> where T : allows ref struct
{
    /// <summary>
    /// Delegate method
    /// </summary>
    public delegate void ParameterAssignmentDelegate(Span<byte> route, ref T lhs, ReadOnlyMemory<byte> input);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="route"></param>
    /// <param name="written"></param>
    /// <param name="returnValue"></param>
    /// <returns>bool</returns>
    public DecodeState TryAssignToParameter(ReadOnlySpan<byte> name, Span<byte> route, out int written, out ParameterAssignmentDelegate? returnValue);

    /// <summary>
    ///     Read a long from the buffer.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>bool</returns>
    public static long ReadLong(ReadOnlySpan<byte> input)
    {
        long val = 0;
        for (int index = 0; index < input.Length; ++index)
        {
            val = (val << 8) | input[index];
        }

        return val;
    }

    /// <summary>
    ///     Read a double from the buffer.
    /// </summary>
    /// <param name="input"></param>
    public static double ReadDouble(ReadOnlySpan<byte> input)
    {
        return BitConverter.Int64BitsToDouble(ReadLong(input));
    }

    /// <summary>
    ///     Read a float from the buffer.
    /// </summary>
    /// <param name="input"></param>
    public static float ReadFloat(ReadOnlySpan<byte> input)
    {
        return BitConverter.Int32BitsToSingle(ReadInt(input));
    }

    /// <summary>
    ///     Read an int from the buffer.
    /// </summary>
    /// <param name="input"></param>
    public static int ReadInt(ReadOnlySpan<byte> input)
    {
        return input[0] << 24 |
               input[1] << 16 |
               input[2] << 8 |
               input[3];
    }

    /// <summary>
    ///     Read a uint64 from the buffer.
    /// </summary>
    /// <param name="input"></param>
    [CLSCompliant(false)]
    public static ulong ReadULong(ReadOnlySpan<byte> input)
    {
        ulong val = 0;
        for (int index = 0; index < input.Length; ++index)
        {
            val = (val << 8) | input[index];
        }

        return val;
    }

    /// <summary>
    ///     Read a variable-sized int from the buffer.
    /// </summary>
    /// <param name="input"></param>
    public static int ReadVarInt(ReadOnlySpan<byte> input)
    {
        return input.Length switch
        {
            0 => 0,
            1 => input[0],
            2 => input[0] << 8 | input[1],
            3 => input[0] << 16 | input[1] << 8 | input[2],
            4 => ReadInt(input),
            _ => throw new InvalidDatabaseException($"Unexpected int32 of size {input.Length}"),
        };
    }
}

#endif