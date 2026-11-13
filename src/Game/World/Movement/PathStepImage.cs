using System.Runtime.InteropServices;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Movement;

/// <summary>
/// The stored image of a <see cref="PathStep"/>: the four bytes of a move path item (Delphi
/// <c>TXYL</c>), X, Y, map level and the move cost in the flag byte. The record keeps the stored
/// name <c>MapCoordinate</c> and the field name <c>Tag</c>, so own documents do not change.
/// </summary>
[StoredLayout, StoredRecord("MapCoordinate")]
[StructLayout(LayoutKind.Explicit, Size = 4)]
public readonly struct PathStepImage : IRecordImage<PathStepImage, PathStep>
{
    [FieldOffset(0)] public readonly sbyte X;
    [FieldOffset(1)] public readonly sbyte Y;
    [FieldOffset(2)] public readonly sbyte MapLevel;
    [FieldOffset(3)] public readonly sbyte Tag;

    private PathStepImage(in PathStep value)
    {
        X = value.Location.X;
        Y = value.Location.Y;
        MapLevel = value.Location.MapLevel;
        Tag = value.MoveCost;
    }

    public static PathStepImage From(PathStep value) => new(value);

    public PathStep ToValue() => new(Location: new MapCoordinate(X, Y, MapLevel), MoveCost: Tag);

    /// <summary>Reads the four bytes of a stored item.</summary>
    public static PathStepImage Read(Stream stream)
    {
        PathStepImage image = default;
        stream.ReadExactly(Bytes.Of(ref image, 4));
        return image;
    }

    /// <summary>Writes the four bytes of a stored item.</summary>
    public void Write(Stream stream)
    {
        var image = this;
        stream.Write(Bytes.Of(ref image, 4));
    }

    /// <summary>The four bytes as one little-endian integer, the number the headless trace writes.</summary>
    public int ToInt32() => (byte)X | ((byte)Y << 8) | ((byte)MapLevel << 16) | ((byte)Tag << 24);
}
