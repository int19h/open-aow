using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Game.Session;

namespace Aow.Game.Tests;

public sealed class PlayerIdTests
{
    [Fact]
    public void NamedValuesAndQueriesKeepTheirNumbers()
    {
        Assert.Equal(0, PlayerId.Independent.Index);
        Assert.True(PlayerId.Independent.IsIndependent);
        Assert.False(PlayerId.Player1.IsIndependent);
        Assert.Equal(12, PlayerId.Player12.Index);
        Assert.Equal(13, PlayerId.Ids.Count);
        Assert.Equal(12, PlayerId.GameIds.Count);
        Assert.Equal(PlayerId.Player1, PlayerId.GameIds[0]);
        Assert.Equal(PlayerId.Player12, PlayerId.GameIds[^1]);
    }

    [Theory]
    [InlineData(int.MinValue, 0)]
    [InlineData(-129, 127)]
    [InlineData(-128, -128)]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(127, 127)]
    [InlineData(128, -128)]
    [InlineData(255, -1)]
    [InlineData(int.MaxValue, -1)]
    public void FromInt32WrapsLikeTheOriginalByte(int value, int expected)
    {
        PlayerId player = PlayerId.FromInt32(value);
        Assert.Equal((sbyte)expected, player.Value);
        Assert.Equal(expected, player.OriginalValue);
        Assert.Equal(expected.ToString(System.Globalization.CultureInfo.InvariantCulture), player.ToString());
    }

    [Fact]
    public void AbsenceUsesTheOriginalSentinelOnlyAtTheBoundary()
    {
        PlayerId? none = null;
        Assert.Equal(-1, none.ToOriginal());
        Assert.Null(PlayerId.FromOriginal(-1));
        Assert.Null(PlayerId.FromOriginal(255));
        Assert.Equal((PlayerId?)PlayerId.Independent, PlayerId.FromOriginal(0));
        Assert.Equal((PlayerId?)PlayerId.FromInt32(-56), PlayerId.FromOriginal(200));
        Assert.Equal(-56, PlayerId.FromOriginal(200).ToOriginal());
    }

    [Fact]
    public void MessageArgumentsRoundTripEveryByte()
    {
        for (int stored = 0; stored <= 255; stored++)
        {
            MsgArg argument = MsgArg.FromPlayer(PlayerId.FromOriginal(stored));
            Assert.Equal(stored == 255 ? -1 : unchecked((sbyte)stored), argument.Int);
            Assert.Equal(PlayerId.FromOriginal(stored), argument.Player);
        }
    }

    [Fact]
    public void SnapshotRenderingUsesTheOriginalNumber()
    {
        Assert.True(OriginalValueSnapshot.TryRender(PlayerId.Player3, declaredType: typeof(PlayerId), rendered: out object? present));
        Assert.Equal(3, present);
        Assert.True(OriginalValueSnapshot.TryRender(null, declaredType: typeof(PlayerId?), rendered: out object? absent));
        Assert.Equal(-1, absent);
        Assert.False(OriginalValueSnapshot.TryRender(null, declaredType: typeof(string), rendered: out _));
    }

    [Fact]
    public void CollectionSnapshotsUseTheElementType()
    {
        Assert.Equal("[-1,3]", OrderingRecording.Snapshot(new PlayerId?[] { null, PlayerId.Player3 }));
        Assert.Equal("[-1,4]", OrderingRecording.Snapshot(new List<PlayerId?> { null, PlayerId.Player4 }));
        TwoPlayers inline = new();
        inline[0] = null;
        inline[1] = PlayerId.Player5;
        Assert.Equal("[-1,5]", OrderingRecording.Snapshot(inline));
    }

    [InlineArray(2)]
    private struct TwoPlayers
    {
        public PlayerId? Element0;
    }
}
