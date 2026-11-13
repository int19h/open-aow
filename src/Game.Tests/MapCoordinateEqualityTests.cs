using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// MapCoordinate without reinterpretation in game code (issue 192, LY-8). Game code compares
// coordinates with == and names the none value None. Both keep the meaning of the Delphi integer
// views: equality covers all four bytes, flag byte included, and None is $00FFFFFF.
public sealed class MapCoordinateEqualityTests
{
    [Fact]
    public void NoneIsTheDelphiNoneValue()
    {
        Assert.Equal(0x00FF_FFFF, MapCoordinate.None.Packed);
        Assert.Equal(MapCoordinate.None, new MapCoordinate { X = -1, Y = -1, MapLevel = -1 });
    }

    [Fact]
    public void EqualityComparesTheFourBytes()
    {
        var location = new MapCoordinate { X = 3, Y = 4, MapLevel = 1 };
        var step = location with { Tag = 7 };
        Assert.True(location == HexGeometry.PackMapCoordinate(X: 3, Y: 4, L: 1));
        Assert.True(location != step);
        Assert.False(location.Equals(step));
        Assert.True(step == new MapCoordinate { Packed = 0x0701_0403 });
        Assert.Equal(step.Packed, step.GetHashCode());
        var cleared = new MapCoordinate { X = -1, Y = -1, MapLevel = -1, Tag = -1 };
        Assert.True(cleared != MapCoordinate.None);
        Assert.Equal(-1, cleared.Packed);
    }

    [Fact]
    public void AMapPositionPacksAsTheDelphiArithmetic()
    {
        // HexSprite.GetMapCoordinate and MapField.GetMapCoordinate computed HexX + HexY shl 8 +
        // HexLevel shl 16. For a map position, whose components are not negative, those are the bytes.
        foreach (var (x, y, level) in new (sbyte, sbyte, sbyte)[] { (0, 0, 0), (113, 97, 2), (127, 127, 4) })
            Assert.Equal(x + (y << 8) + (level << 16), HexGeometry.PackMapCoordinate(x, y, level).Packed);
    }
}
