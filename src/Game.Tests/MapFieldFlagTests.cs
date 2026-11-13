using System.Buffers.Binary;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// The map field flags without word views (issue 192, LY-4). The Delphi read the fog count as
// WordRec(LongRec(FFieldFlags).Hi).Lo, byte 2 of the little-endian integer, and tested the
// message bits through the same views.
public sealed class MapFieldFlagTests
{
    private static byte ByteOf(int flags, int index)
    {
        Span<byte> bytes = stackalloc byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(bytes, flags);
        return bytes[index];
    }

    [Fact]
    public void TheFogCountIsByteTwoOfTheFlags()
    {
        var field = new MapField(null!) { FieldFlags = unchecked((int)0xA5C3_0F1E) };
        Assert.Equal(ByteOf(field.FieldFlags, index: 2), field.FogCount);
        Assert.Equal(0xC3, field.FogCount);
        for (int value = 0; value <= 255; value++)
        {
            field.SetFogCount((byte)value);
            Assert.Equal(value, field.FogCount);
            Assert.Equal(value, ByteOf(field.FieldFlags, index: 2));
            Assert.Equal(unchecked((int)0xA500_0F1E), field.FieldFlags & ~0x00FF0000);
        }
    }

    [Fact]
    public void TheFlagBitsAreTheBitsOfTheByteViews()
    {
        // WordRec(LongRec(F).Lo).Hi and $10, WordRec(LongRec(F).Hi).Hi and 1, 2 and 4.
        Assert.Equal(0x10 << 8, MapField.ViewerFlag);
        Assert.Equal(1 << 24, MapField.PlayerChangeListenerFlag);
        Assert.Equal(2 << 24, MapField.OwnerFlag);
        Assert.Equal(4 << 24, MapField.BattleFieldInfoFlag);
        Assert.Equal(0x08000000, MapField.AITargetFlag);
        Assert.Equal(0x2000, MapField.MoveOverFlag);
        Assert.Equal(0x4000, MapField.MoveOnFlag);
        Assert.Equal(0x8000, MapField.MoveExclusiveFlag);
        Assert.Equal(unchecked((int)0xFFFF1FFF), ~MapField.MoveFlags);
        Assert.Equal(unchecked((int)0xFFFFF000), ~MapField.ExploredFlags);
        for (int player = 1; player <= 12; player++)
        {
            var field = new MapField(null!);
            field.SetExplored(player, value: true);
            Assert.Equal(MapField.ExploredFlag(player), field.FieldFlags);
            Assert.Equal(field.FieldFlags, field.FieldFlags & MapField.ExploredFlags);
        }
    }
}
