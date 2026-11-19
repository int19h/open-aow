using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Tests;

// The map coordinate and the meanings of the Delphi flag byte (issue 191). MapCoordinate is X, Y
// and level. A path step adds the move cost, and StoredCoordinateImage packs X, Y and level with
// 0 in the fourth byte. A cleared AI target is null and differs from MapCoordinate.None.
public sealed class MapCoordinateEqualityTests
{
    [Fact]
    public void NoneIsTheDelphiNoneValue()
    {
        Assert.Equal(0x00FF_FFFF, StoredCoordinateImage.From(MapCoordinate.None).ToInt32());
        Assert.Equal(MapCoordinate.None, new MapCoordinate(X: -1, Y: -1, MapLevel: -1));
    }

    [Fact]
    public void AStepComparesItsCost()
    {
        MapCoordinate location = new(X: 3, Y: 4, MapLevel: 1);
        PathStep step = new(Location: location, MoveCost: 7);
        Assert.True(location == HexGeometry.PackMapCoordinate(x: 3, y: 4, mapLevel: 1));
        Assert.True(step != new PathStep(Location: location, MoveCost: 0));
        Assert.Equal(0x0701_0403, PathStepImage.From(step).ToInt32());
        Assert.Equal(step, PathStepImage.From(step).ToValue());
        Assert.Equal(new PathStep(Location: MapCoordinate.None, MoveCost: 0), PathStep.None);
    }

    [Fact]
    public void TheClearedTargetDiffersFromNone()
    {
        MapCoordinate? cleared = null;
        Assert.NotEqual(expected: cleared, actual: MapCoordinate.None);
    }

    [Fact]
    public void AStoredSlotPacksWithZeroFourthByteAndChecksItWhenAsked()
    {
        foreach (MapCoordinate location in new[] { new MapCoordinate(0, 0, 0), MapCoordinate.None, new MapCoordinate(5, -2, 1), new MapCoordinate(127, -128, 2) })
        {
            int packed = StoredCoordinateImage.From(location).ToInt32();
            Assert.Equal(0, (byte)((uint)packed >> 24));
            Assert.Equal(location, StoredCoordinateImage.FromInt32(packed).Location);
            Assert.Equal(location, StoredCoordinateImage.FromInt32(packed).ReadCheckedLocation("Slot"));
        }

        StoredCoordinateImage nonZero = StoredCoordinateImage.FromInt32(unchecked((int)0xA501_FE05));
        Assert.Equal(new MapCoordinate(5, -2, 1), nonZero.Location);
        Assert.Equal(0x0001_FE05, StoredCoordinateImage.From(nonZero.Location).ToInt32());
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(() => nonZero.ReadCheckedLocation("Slot"));
        Assert.Contains("Slot", error.Message, StringComparison.Ordinal);
        Assert.Contains("outside its storage range", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void AMapPositionPacksAsTheDelphiArithmetic()
    {
        // HexSprite.GetMapCoordinate and MapField.GetMapCoordinate computed HexX + HexY shl 8 +
        // HexLevel shl 16. For a map position, whose components are not negative, those are the bytes.
        foreach ((sbyte x, sbyte y, sbyte level) in new (sbyte, sbyte, sbyte)[] { (0, 0, 0), (113, 97, 2), (127, 127, 4) })
        {
            Assert.Equal(x + (y << 8) + (level << 16), StoredCoordinateImage.From(HexGeometry.PackMapCoordinate(x, y, level)).ToInt32());
        }
    }
}
