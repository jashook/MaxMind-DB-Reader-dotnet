namespace MaxMind.Db;

/// <summary>
/// IResettable interface, allows allowing callback for reseting the underlying
/// type.
/// </summary>
public interface IResettable
{
    /// <summary>
    /// Reset method.
    /// </summary>
    public void Reset();
}