using System.Globalization;
using Aow.Rtl;

namespace Aow.Game.World.Map;

/// <summary>
/// A map location stored packed in one <c>rwInteger</c> slot (Delphi <c>TXYL</c> read as an
/// integer): X in the low byte, then Y and the map level, and 0 in the fourth byte. Every Delphi
/// writer stores 0 in the fourth byte. Only load, save and the headless trace use it.
/// </summary>
[StoredLayout]
public readonly struct StoredCoordinateImage
{
    private readonly int _value;

    private StoredCoordinateImage(int value) => _value = value;

    public static StoredCoordinateImage FromInt32(int value) => new(value);

    public static StoredCoordinateImage From(MapCoordinate location) =>
        new((byte)location.X | ((byte)location.Y << 8) | ((byte)location.MapLevel << 16));

    public int ToInt32() => _value;

    public MapCoordinate Location => new(X: (sbyte)_value, Y: (sbyte)(_value >> 8), MapLevel: (sbyte)(_value >> 16));

    public MapCoordinate ReadCheckedLocation(string memberName) =>
        (_value & unchecked((int)0xFF00_0000)) != 0
            ? throw new InvalidOperationException(memberName + " is " + _value.ToString(CultureInfo.InvariantCulture) + ", outside its storage range.")
            : Location;
}
