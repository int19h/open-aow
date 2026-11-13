using Aow.Rtl;

namespace Aow.Game.World.Map;

/// <summary>
/// A map location stored packed in one <c>rwInteger</c> slot (Delphi <c>TXYL</c> read as an
/// integer): X in the low byte, then Y and the map level, and a fourth byte that the owner keeps
/// as its opaque <c>UnknownBytes</c>. Every writer stores 0 there, and every corpus file holds 0,
/// but a loaded value round-trips. Only load, save and the headless trace use it.
/// </summary>
[StoredLayout]
public readonly struct StoredCoordinateImage
{
    private readonly int _value;

    private StoredCoordinateImage(int value) => _value = value;

    public static StoredCoordinateImage FromInt32(int value) => new(value);

    public static StoredCoordinateImage From(MapCoordinate location, byte unknownBytes) =>
        new((byte)location.X | ((byte)location.Y << 8) | ((byte)location.MapLevel << 16) | (unknownBytes << 24));

    public int ToInt32() => _value;

    public MapCoordinate Location => new(X: (sbyte)_value, Y: (sbyte)(_value >> 8), MapLevel: (sbyte)(_value >> 16));

    /// <summary>The fourth byte of the slot.</summary>
    public byte UnknownBytes => (byte)(_value >> 24);

    public void Deconstruct(out MapCoordinate location, out byte unknownBytes)
    {
        location = Location;
        unknownBytes = UnknownBytes;
    }
}
