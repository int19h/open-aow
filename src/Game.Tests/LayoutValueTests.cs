using Aow.Engine.Collections;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Tests;

// The typed value lists and move point tables of issue 192 (LY-3 and LY-5): the values the byte
// views read, without the byte views.
public sealed class LayoutValueTests
{
    [Fact]
    public void DoubleItemsAboveTwoToTheFiftyThirdCompareExactly()
    {
        // Two items whose 64-bit images differ only in the lowest bit, above 2^53. The port compared
        // them through double, which rounds both to the same value. The Delphi Comp compare is exact.
        var low = new DoubleItemList.DoubleItem(First: 0, Second: 0x00400000);
        var high = new DoubleItemList.DoubleItem(First: 1, Second: 0x00400000);
        long lowImage = (long)low.Second << 32 | (uint)low.First;
        long highImage = (long)high.Second << 32 | (uint)high.First;
        Assert.True(lowImage > 1L << 53);
        Assert.Equal((double)lowImage, (double)highImage);
        Assert.NotEqual(low, high);

        using var list = new DoubleItemList();
        list.Add(low);
        Assert.Equal(-1, list.IndexOf(high));
        Assert.Equal(0, list.IndexOf(low));
        list.Default = high;
        list.Pack();
        Assert.Equal(1, list.Count);
        list.Default = low;
        list.Pack();
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void ValueListsMoveCompareAndFillTypedItems()
    {
        using var list = new DoubleItemList();
        for (int i = 0; i < 5; i++) list.Add(new DoubleItemList.DoubleItem(First: i, Second: -i));
        list.Insert(1, new DoubleItemList.DoubleItem(First: 9, Second: 9));
        list.Delete(3);
        Assert.Equal([0, 9, 1, 3, 4], Enumerable.Range(0, list.Count).Select(i => list[i].First));
        using var copy = new DoubleItemList();
        copy.Assign(list);
        Assert.True(list.Compare(copy));
        copy[4] = new DoubleItemList.DoubleItem(First: 4, Second: 0);
        Assert.False(list.Compare(copy));

        using var quads = new QuadItemList();
        for (int i = 0; i < 4; i++) quads.AddABCD(i, i + 1, i + 2, i + 3);
        quads.Insert(0, new QuadItemList.QuadInteger(First: 7, Second: 7, Third: 7, Fourth: 7));
        quads.Delete(2);
        Assert.Equal([7, 0, 2, 3], Enumerable.Range(0, quads.Count).Select(i => quads[i].First));

        using var bytes = new SByteList { Default = -3 };
        bytes.Count = 3;
        Assert.Equal([-3, -3, -3], bytes.List.Take(3));
    }

    [Fact]
    public void AFrameTableFillsEachByteWithTheDefault()
    {
        // ImageLib.pas:9517 fills new items with FillChar, so each byte of a new item is the low
        // byte of the default.
        using var frames = new FrameTableList { DefaultValue = 0x1234 };
        frames.Count = 2;
        Assert.Equal(0x34343434, frames[0]);
        Assert.Equal(0x34343434, frames[1]);
    }

    [Fact]
    public void TerrainPairsKeepTheWordView()
    {
        var terrain = new TerrainList();
        terrain.SetTandO(0, 0x05FE);
        Assert.Equal(-2, terrain.GetTerrain(0));
        Assert.Equal(5, terrain.GetOverlay(0));
        Assert.True(terrain.GetUsed(0));
        terrain.SetTerrain(1, 3);
        Assert.Equal(0xFE03, terrain.GetTandO(1));
        Assert.Equal(0xFEFE, terrain.GetTandO(2));
        terrain.SetTandO(2, 0x8080);
        Assert.False(terrain.GetUsed(2));
        terrain.SetTandO(2, 0x807F);
        Assert.True(terrain.GetUsed(2));
    }

    [Fact]
    public void TornadoReadsTheArmyTableOneByteShifted()
    {
        // GlobalTargetSpells.pas:1834 passes a TSpellMoveTable (Flags, then Cost[0..15, 0..15]) as
        // the untyped table of TArmy.CreateMovePointTable, which fills its first 256 bytes. Cost[t, o]
        // is then byte 1 + 16 t + o of the army table, and Cost[15, 15] is the byte after it.
        MovePointTable army = default;
        var image = new byte[257];
        for (int offset = 0; offset < MovePointTable.EntryCount; offset++)
        {
            sbyte value = (sbyte)(offset * 7 - 100);
            army[offset / 16][offset % 16] = value;
            image[offset] = (byte)value;
        }

        for (int terrain = 0; terrain < 16; terrain++)
        {
            for (int overlay = -1; overlay < 16; overlay++)
            {
                int costOffset = 16 * terrain + overlay;
                if (costOffset < 0) continue;
                Assert.Equal((sbyte)image[1 + costOffset], Tornado.SpellTableCost(army, terrainType: terrain, overlayType: overlay));
            }
        }

        // The shift cancels the overlay base of -1: every overlay -1 to 14 reads the army's own cost.
        for (int terrain = 0; terrain < 16; terrain++)
        {
            for (int overlay = -1; overlay <= 14; overlay++)
            {
                Assert.Equal(army[terrain][overlay - MovementTables.FirstOverlay], Tornado.SpellTableCost(army, terrainType: terrain, overlayType: overlay));
            }
        }
    }

    [Fact]
    public void MovePointTablesFillAndMaximize()
    {
        MovePointTable table = default;
        table.Fill(1);
        MovePointTable other = default;
        other.Fill(4);
        other[2][3] = -1;
        other[5][13] = 9;
        table[6][0] = -1;
        table.Maximize(other);
        Assert.Equal(4, table[0][0]);
        Assert.Equal(-1, table[2][3]);
        Assert.Equal(-1, table[6][0]);
        Assert.Equal(1, table[5][13]);
        Assert.Equal(4, table[15][12]);
    }
}
