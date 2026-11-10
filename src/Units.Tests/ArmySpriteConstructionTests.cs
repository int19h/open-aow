using System.Diagnostics;
using System.Text.Json;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Draw;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Map;
using Xunit.Sdk;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class ArmySpriteConstructionTests
{
    private sealed class ArmyOwner() : EngineFixture.Owner
    {
        internal readonly List<string> Events = [];

        public override int AddChild(EObject child)
        {
            base.AddChild(child);
            Events.Add(OrderingRecording.Snapshot(child));
            return Members.Count - 1;
        }

        public override int RemoveChild(EObject child)
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
                using var fixture = new OrderingRecording.Fixture(typeof(ArmyHS));
                var owner = owned == 0 ? null : new ArmyOwner();
                ArmyHS? value = null;
                string state;
                int destroyEntries = 0;
                try
                {
                    value = Assert.IsType<ArmyHS>(OrderingRecording.Create(typeof(ArmyHS), owner, route, fixture.Engine));
                    state = OrderingRecording.Snapshot(value);
                }
                catch (Exception error) when (error is not XunitException)
                {
                    state = error.GetType().FullName + ":" + error.Message;
                    destroyEntries = new StackTrace(error).GetFrames().Count(x => x.GetMethod()?.DeclaringType == typeof(ArmyHS) && x.GetMethod()?.Name == "Dispose");
                }
                Assert.NotNull(value);
                actual.Add($"owner{owned}.route{route}", JsonSerializer.Serialize(new { State = state, owner?.Events, Count = owner?.Members.Count, DestroyEntries = destroyEntries }));
                AoWHSMap.Current = EngineFixture.Configure(new AoWHSMap(null!), value => { value.FStatus = Set<HSMapStatusElement>.Of(HSMapStatusElement.Destroying); });
                value?.SetOwner(null!);
                value?.Dispose();
            }
        OrderingRecording.Check("ArmySpriteConstruction", actual);
    }

    [Fact]
    public void SavedArmyReturnsThroughCreateEObjectAndReadWrite()
    {
        using var fixture = new OrderingRecording.Fixture(typeof(ArmyHS));
        EObject source = new ArmyHS(null!);
        EObject loaded = null!;
        using var memory = new MemoryStream();
        try
        {
            ((ArmyHS)source).FArmy.FPlayer = 2;
            using (var writer = new EWriteStorageStream(memory, fixture.Engine, false)) { source.ReadWrite(writer); }
            memory.Position = 0;
            loaded = fixture.Engine.CreateEObject(ArmyHS.ClassID, null!);
            using (var reader = new EReadStorageStream(memory, fixture.Engine, false)) { loaded.ReadWrite(reader); }
            Assert.IsType<ArmyHS>(loaded);
            Assert.Equal(OrderingRecording.Snapshot(source), OrderingRecording.Snapshot(loaded));
        }
        finally
        {
            AoWHSMap.Current = EngineFixture.Configure(new AoWHSMap(null!), value => { value.FStatus = Set<HSMapStatusElement>.Of(HSMapStatusElement.Destroying); });
            loaded?.Dispose();
            source.Dispose();
        }
    }
}
