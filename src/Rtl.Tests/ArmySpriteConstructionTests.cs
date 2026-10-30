using System.Diagnostics;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

[Collection("Engine")]
[EngineState]
public sealed class ArmySpriteConstructionTests
{
    private sealed class Owner(bool reject) : TECustomNode(null!)
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<string> Events = [];
        internal int Origin;
        public override int AddChild(TEObject child)
        {
            Members.Add(child);
            Events.Add(OrderingRecording.Snapshot(child, Origin));
            if (reject) throw new InvalidDataException("owner rejects after insertion");
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child)
        {
            Events.Add("remove");
            return Members.Remove(child) ? 0 : -1;
        }
    }
    [Fact]
    public void StateOwnerEventsOffsetsAndFailureMatchBaseCommit()
    {
        var actual = new SortedDictionary<string, string>(StringComparer.Ordinal);
        for (int owned = 0; owned < 3; owned++)
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new OrderingRecording.Fixture(typeof(TArmyHS));
            var owner = owned == 0 ? null : new Owner(owned == 2);
            int origin = new TObject().InstanceId;
            if (owner is not null) owner.Origin = origin;
            TArmyHS? value = null;
            string state;
            int destroyEntries = 0;
            try
            {
                value = Assert.IsType<TArmyHS>(OrderingRecording.Create(typeof(TArmyHS), owner, route, fixture.Engine));
                state = OrderingRecording.Hash(OrderingRecording.Snapshot(value, origin));
            }
            catch (System.Exception error) when (error is not Xunit.Sdk.XunitException)
            {
                state = error.GetType().FullName + ":" + error.Message;
                destroyEntries = new StackTrace(error).GetFrames().Count(x => x.GetMethod()?.DeclaringType == typeof(TArmyHS) && x.GetMethod()?.Name == "Destroy");
            }
            Assert.Equal(owned != 2, value is not null);
            actual.Add($"owner{owned}.route{route}", JsonSerializer.Serialize(new { State = state, owner?.Events, Count = owner?.Members.Count, DestroyEntries = destroyEntries }));
            Game.AoWHSMap = EngineFixture.Configure(TAoWHSMap.Create(null!), value => { value.FStatus = TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying); });
            value?.SetOwner(null!);
            value?.Destroy();
            if (owned == 2) owner!.Members.Clear();
        }
        OrderingRecording.Check("ArmySpriteConstruction", actual);
    }
    [Fact]
    public void SavedArmyReturnsThroughCreateEObjectAndReadWrite()
    {
        using var fixture = new OrderingRecording.Fixture(typeof(TArmyHS));
        TEObject source = TArmyHS.Create(null!);
        TEObject loaded = null!;
        using var memory = new TMemoryStream();
        try
        {
            ((TArmyHS)source).FArmy.FPlayer = 2;
            var writer = new TEWriteStorageStream(memory, fixture.Engine, false);
            try { source.ReadWrite(writer); }
            finally { writer.Destroy(); }
            memory.Position = 0;
            loaded = fixture.Engine.CreateEObject(TArmyHS.ClassID, null!);
            var reader = new TEReadStorageStream(memory, fixture.Engine, false);
            try { loaded.ReadWrite(reader); }
            finally { reader.Destroy(); }
            Assert.IsType<TArmyHS>(loaded);
            Assert.Equal(OrderingRecording.Snapshot(source, source.InstanceId), OrderingRecording.Snapshot(loaded, loaded.InstanceId));
        }
        finally
        {
            Game.AoWHSMap = EngineFixture.Configure(TAoWHSMap.Create(null!), value => { value.FStatus = TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying); });
            loaded?.Destroy();
            source.Destroy();
        }
    }
}
