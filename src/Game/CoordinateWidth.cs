namespace Aow.Game;

/// <summary>The original width of a stored map coordinate.</summary>
public enum CoordinateWidth
{
    /// <summary>An unsigned byte, the two's complement byte of an sbyte.</summary>
    Byte,

    /// <summary>A signed 32-bit integer in the range -128 to 127.</summary>
    Int32,
}
