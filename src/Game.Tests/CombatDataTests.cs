using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;

namespace Aow.Game.Tests;

// The storage streams store combat data through the tree and the engine's classes since issue 185
// PE-3-i part 4, so these tests share the engine fixture with the others that change its state.
[Collection("Engine")]
[EngineState]
public sealed class CombatDataTests
{
    private sealed class ObservedNode() : ENode(null!)
    {
        internal int DestroyCount { get; private set; }

        public override void Dispose() { DestroyCount++; base.Dispose(); }
    }

    private sealed class ObservedList : ByteList
    {
        internal int DestroyCount { get; private set; }

        public override void Dispose() { DestroyCount++; base.Dispose(); }
    }

    private sealed class ObservedChild(ECustomNode owner) : CombatObject(owner)
    {
        internal int DestroyCount { get; private set; }

        public override void Dispose() { DestroyCount++; base.Dispose(); }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DestroyFreesCombatObjectsAndBothLists(bool countCalls)
    {
        var data = new CombatData(null!) { Holder = EngineFixture.Engine };
        if (countCalls)
        {
            data.CombatObjects?.Dispose();
            data.DefeatedLeaders?.Dispose();
            data.InvolvedLeaders?.Dispose();
            data.CombatObjects = new ObservedNode();
            data.DefeatedLeaders = new ObservedList();
            data.InvolvedLeaders = new ObservedList();
        }
        var node = data.CombatObjects;
        var defeated = data.DefeatedLeaders;
        var involved = data.InvolvedLeaders;
        var child = new ObservedChild(node);
        defeated.Add(3);
        involved.Add(4);
        Assert.Same(child, Assert.Single(((IOwner)node).Children.Cast<object>()));
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
        using (var writer = new StorageWriter(memory, null!, false)) { writer.Object(data, 4); }
        return memory.ToArray();
    }

    [Fact]
    public void BinaryStateMatchesBeforeTypedFieldsAndRoundTrips()
    {
        var data = new CombatData(null!)
        {
            Holder = EngineFixture.Engine,
            X = 12,
            Y = 34,
            MapLevel = 2,
            AttackerX = 9,
            AttackerY = 8,
            AttackerMapLevel = 1,
            AttackerPlayer = -1,
            DefenderPlayer = 3,
            Round = 7,
            Header = "Combat",
            WallType = 2,
            WallBreached = true
        };
        data.DefeatedLeaders.Add(3);
        data.DefeatedLeaders.Add(5);
        data.TurnOrder.Add(2);
        data.TurnOrder.Add(1);
        data.InvolvedLeaders.Add(4);
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
            using var loaded = new CombatData(null!) { Holder = EngineFixture.Engine };
            using var reader = new StorageReader(memory, null!, false);
            reader.Object(loaded, 4);
            Assert.Equal(before, Save(loaded));
        }
        finally { data.Dispose(); }
    }
}
