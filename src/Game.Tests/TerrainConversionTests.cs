using Aow.Engine;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// The original-number boundary of the terrain and overlay types (issue 331, step 9): the field values
// take -1 and the members, the footprint answers take -2, -1 and the members, and every other number
// above -1 throws.
public sealed class TerrainConversionTests
{
    [Fact]
    public void FieldValuesTakeNoneAndTheMembers()
    {
        Assert.Null(TerrainType.FromOriginal(-1));
        Assert.Equal(TerrainType.Dirt, TerrainType.FromOriginal(12));
        Assert.Equal(TerrainType.Border, TerrainType.FromOriginal(15));
        Assert.Null(OverlayType.FromOriginal(-1));
        Assert.Equal(OverlayType.Ooze, OverlayType.FromOriginal(11));
    }

    [Theory]
    [InlineData(16)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(255)]
    [InlineData(-2)]
    public void TerrainNumbersOutsideTheMembersThrow(int value)
        => Assert.Throws<ArgumentOutOfRangeException>(() => TerrainType.FromOriginal(value));

    [Theory]
    [InlineData(12)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(255)]
    [InlineData(-2)]
    public void OverlayNumbersOutsideTheMembersThrow(int value)
        => Assert.Throws<ArgumentOutOfRangeException>(() => OverlayType.FromOriginal(value));

    [Fact]
    public void FootprintAnswersTakeOutsideKeepAndTheMembers()
    {
        Assert.Equal(FootprintCell<TerrainType>.Outside, FootprintCell<TerrainType>.FromOriginal(-2));
        Assert.Equal(FootprintCell<TerrainType>.Keep, FootprintCell<TerrainType>.FromOriginal(-1));
        Assert.Equal(TerrainType.Dirt, FootprintCell<TerrainType>.FromOriginal(12).Value);
        Assert.Equal(OverlayType.Ooze, FootprintCell<OverlayType>.FromOriginal(11).Value);
        Assert.Equal(-2, FootprintCell<OverlayType>.FromOriginal(-2).ToOriginal());
        Assert.Equal(-1, FootprintCell<OverlayType>.FromOriginal(-1).ToOriginal());
        Assert.Equal(11, FootprintCell<OverlayType>.FromOriginal(11).ToOriginal());
    }

    [Theory]
    [InlineData(16)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(255)]
    public void TerrainAnswersAboveTheMembersThrow(int value)
        => Assert.Throws<ArgumentOutOfRangeException>(() => FootprintCell<TerrainType>.FromOriginal(value));

    [Theory]
    [InlineData(12)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(255)]
    public void OverlayAnswersAboveTheMembersThrow(int value)
        => Assert.Throws<ArgumentOutOfRangeException>(() => FootprintCell<OverlayType>.FromOriginal(value));

    [Fact]
    public void FieldTerrainThrowsUntilSet()
    {
        MapField field = new(owner: null);
        Assert.False(field.TryGetTerrain(out _));
        _ = Assert.Throws<InvalidOperationException>(() => field.Terrain);
        field.SetTerrain(TerrainType.Grass);
        Assert.True(field.TryGetTerrain(out TerrainType terrain));
        Assert.Equal(TerrainType.Grass, terrain);
        Assert.Equal(TerrainType.Grass, field.Terrain);
        field.SetTerrain(null);
        Assert.False(field.TryGetTerrain(out _));
    }

    [Fact]
    public void NullFieldValuesRenderAsTheOriginalNone()
    {
        Assert.True(OriginalValueSnapshot.TryRender(null, declaredType: typeof(TerrainType?), rendered: out object? terrain));
        Assert.Equal(-1, terrain);
        Assert.True(OriginalValueSnapshot.TryRender(null, declaredType: typeof(OverlayType?), rendered: out object? overlay));
        Assert.Equal(-1, overlay);
    }
}
