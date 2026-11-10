using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;

namespace Aow.Units.Tests;

public sealed class CombatDataTests
{
    private sealed class ObservedNode() : ENode(null!)
    {
        internal int DestroyCount;

        public override void Dispose() { DestroyCount++; base.Dispose(); }
    }

    private sealed class ObservedList : ByteList
    {
        internal int DestroyCount;

        public override void Dispose() { DestroyCount++; base.Dispose(); }
    }

    private sealed class ObservedChild(ECustomNode owner) : CombatObject(owner)
    {
        internal int DestroyCount;

        public override void Dispose() { DestroyCount++; base.Dispose(); }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DestroyFreesCombatObjectsAndBothLists(bool countCalls)
    {
        var data = new CombatData(null!);
        if (countCalls)
        {
            data.FCombatObjects?.Dispose();
            data.FDefeatedLeaders?.Dispose();
            data.FInvolvedLeaders?.Dispose();
            data.FCombatObjects = new ObservedNode();
            data.FDefeatedLeaders = new ObservedList();
            data.FInvolvedLeaders = new ObservedList();
        }
        var node = data.FCombatObjects;
        var defeated = data.FDefeatedLeaders;
        var involved = data.FInvolvedLeaders;
        var child = new ObservedChild(node);
        defeated.Add(3);
        involved.Add(4);
        Assert.Same(child, Assert.Single(node.FChildren.List.Take(node.Count)));
        data.Dispose();
        Assert.Equal(1, child.DestroyCount);
        Assert.Null(child.Owner);
        Assert.Equal(0, node.Count);
        Assert.Equal(0, defeated.Count);
        Assert.Equal(0, involved.Count);
        if (countCalls)
        {
            Assert.Equal(1, ((ObservedNode)node).DestroyCount);
            Assert.Equal(1, ((ObservedList)defeated).DestroyCount);
            Assert.Equal(1, ((ObservedList)involved).DestroyCount);
        }
    }

    private static byte[] Save(CombatData data)
    {
        using var memory = new MemoryStream();
        using (var writer = new EWriteStorageStream(memory, null!, false)) { writer.rwEObject(data, 4); }
        return memory.ToArray();
    }

    [Fact]
    public void BinaryStateMatchesBeforeTypedFieldsAndRoundTrips()
    {
        var data = new CombatData(null!)
        {
            FX = 12,
            FY = 34,
            FL = 2,
            FAttackerX = 9,
            FAttackerY = 8,
            FAttackerL = 1,
            FAttackerPlayer = -1,
            FDefenderPlayer = 3,
            FRound = 7,
            FHeader = "Combat",
            FWallType = 2,
            FWallBreached = true
        };
        data.FDefeatedLeaders.Add(3);
        data.FDefeatedLeaders.Add(5);
        data.FTurnOrder.Add(2);
        data.FTurnOrder.Add(1);
        data.FInvolvedLeaders.Add(4);
        try
        {
            byte[] before = Save(data);
            // Captured through TEWriteStorageStream before the object fields were typed.
            Assert.Equal(Convert.FromHexString(
                "010400140A000B040C080D0C0E0E0F0F1010111112151319141D15211628172C183019341A3B1B421C431D43" +
                "0C00000022000000020000000000FF03000700000001010000010100000101000006436F6D62617409000000" +
                "0800000001000000000200000003050002000000020102000100000004"), before);
            using var memory = new MemoryStream();
            memory.Write(before);
            memory.Position = 0;
            using var loaded = new CombatData(null!);
            using var reader = new EReadStorageStream(memory, null!, false);
            reader.rwEObject(loaded, 4);
            Assert.Equal(before, Save(loaded));
        }
        finally { data.Dispose(); }
    }
}
