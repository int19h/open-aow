// Translated from HSEngine.pas.
using System.Runtime.InteropServices;
using Aow.Rtl;

namespace Aow.Game.World.Map;

// { A map location: hexagon column X, row Y, map level L and a flag byte (LocationsConnected
//     marks the locations it has reached with it); the items of TXYLList, made by PackXYL }
// StoredRecord binds the move path record (an int-list of MapCoordinate) until issue #191 moves it to an image.
[StoredRecord("MapCoordinate")]
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct MapCoordinate : IEquatable<MapCoordinate>
{
    [FieldOffset(0)]
    public sbyte X;
    [FieldOffset(1)]
    public sbyte Y;
    [FieldOffset(2)]
    public sbyte MapLevel;
    // The Delphi flag byte. Issue #191 splits it from the coordinate.
    [FieldOffset(3)]
    public sbyte Tag;
    // The four bytes as the stored integer. Only ReadWrite, PersistFields and the headless trace
    // read or write it. Game code compares coordinates with == and names the none value None.
    [FieldOffset(0)]
    public int Packed;

    // The none value: X, Y and level -1 and a zero flag byte, the Delphi $00FFFFFF.
    public static MapCoordinate None { get; } = new() { X = -1, Y = -1, MapLevel = -1 };

    // Equality covers all four bytes, flag byte included, as the Delphi integer comparisons did.
    public static bool operator ==(MapCoordinate left, MapCoordinate right) => left.Packed == right.Packed;

    public static bool operator !=(MapCoordinate left, MapCoordinate right) => left.Packed != right.Packed;

    public readonly bool Equals(MapCoordinate other) => Packed == other.Packed;

    public override readonly bool Equals(object obj) => obj is MapCoordinate other && Equals(other);

    public override readonly int GetHashCode() => Packed;
}
