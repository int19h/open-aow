using System.Diagnostics;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class ArmySpriteConstructionTests
{
    private sealed class ArmyOwner() : EngineFixture.Owner
    {
        internal readonly List<string> Events = [];

        public override int AddChild(TEObject child)
        {
            base.AddChild(child);
            Events.Add(OrderingRecording.Snapshot(child));
            return Members.Count - 1;
        }

        public override int RemoveChild(TEObject child)
        {
            Events.Add("remove");
            return base.RemoveChild(child) < 0 ? -1 : 0;
        }
    }

    [Fact]
    public void StateOwnerEventsAndOffsetsMatchBaseCommit()
    {
        var actual = new SortedDictionary<string, string>(StringComparer.Ordinal);
        for (int owned = 0; owned < 2; owned++)
            for (int route = 0; route < 4; route++)
            {
                using var fixture = new OrderingRecording.Fixture(typeof(TArmyHS));
                var owner = owned == 0 ? null : new ArmyOwner();
                TArmyHS? value = null;
                string state;
                int destroyEntries = 0;
                try
                {
                    value = Assert.IsType<TArmyHS>(OrderingRecording.Create(typeof(TArmyHS), owner, route, fixture.Engine));
                    state = OrderingRecording.Snapshot(value);
                }
                catch (System.Exception error) when (error is not Xunit.Sdk.XunitException)
                {
                    state = error.GetType().FullName + ":" + error.Message;
                    destroyEntries = new StackTrace(error).GetFrames().Count(x => x.GetMethod()?.DeclaringType == typeof(TArmyHS) && x.GetMethod()?.Name == "Dispose");
                }
                Assert.NotNull(value);
                actual.Add($"owner{owned}.route{route}", JsonSerializer.Serialize(new { State = state, owner?.Events, Count = owner?.Members.Count, DestroyEntries = destroyEntries }));
                Game.AoWHSMap = EngineFixture.Configure(new TAoWHSMap(null!), value => { value.FStatus = TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying); });
                value?.SetOwner(null!);
                value?.Dispose();
            }
        OrderingRecording.Check("ArmySpriteConstruction", actual);
    }

    [Fact]
    public void SavedArmyReturnsThroughCreateEObjectAndReadWrite()
    {
        using var fixture = new OrderingRecording.Fixture(typeof(TArmyHS));
        TEObject source = new TArmyHS(null!);
        TEObject loaded = null!;
        using var memory = new TMemoryStream();
        try
        {
            ((TArmyHS)source).FArmy.FPlayer = 2;
            using (var writer = new TEWriteStorageStream(memory, fixture.Engine, false)) { source.ReadWrite(writer); }
            memory.Position = 0;
            loaded = fixture.Engine.CreateEObject(TArmyHS.ClassID, null!);
            using (var reader = new TEReadStorageStream(memory, fixture.Engine, false)) { loaded.ReadWrite(reader); }
            Assert.IsType<TArmyHS>(loaded);
            Assert.Equal(OrderingRecording.Snapshot(source), OrderingRecording.Snapshot(loaded));
        }
        finally
        {
            Game.AoWHSMap = EngineFixture.Configure(new TAoWHSMap(null!), value => { value.FStatus = TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying); });
            loaded?.Dispose();
            source.Dispose();
        }
    }
}
