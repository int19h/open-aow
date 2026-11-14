using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Xunit.Sdk;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class ArmySpriteConstructionTests
{
    private sealed class ArmyOwner() : EngineFixture.Owner
    {
        internal List<string> Events { get; } = [];

        public override int AddChild(EngineObject child)
        {
            base.AddChild(child);
            Events.Add(OrderingRecording.Snapshot(child));
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child)
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
                using var fixture = new OrderingRecording.Fixture(typeof(ArmyHexSprite));
                var owner = owned == 0 ? null : new ArmyOwner();
                ArmyHexSprite? value = null;
                string state;
                int destroyEntries = 0;
                try
                {
                    value = Assert.IsType<ArmyHexSprite>(OrderingRecording.Create(typeof(ArmyHexSprite), EngineFixture.OwnerFor(typeof(ArmyHexSprite), owner!), route, fixture.Engine));
                    state = OrderingRecording.Snapshot(value);
                }
                catch (Exception error) when (error is not XunitException)
                {
                    state = error.GetType().FullName + ":" + error.Message;
                    destroyEntries = new StackTrace(error).GetFrames().Count(x => x.GetMethod()?.DeclaringType == typeof(ArmyHexSprite) && x.GetMethod()?.Name == "Dispose");
                }
                Assert.NotNull(value);
                actual.Add($"owner{owned}.route{route}", JsonSerializer.Serialize(new { State = state, owner?.Events, Count = owner?.Members.Count, DestroyEntries = destroyEntries }));
                StrategicMap.Current = EngineFixture.Configure(new StrategicMap(null!, holder: EngineFixture.Engine), value => { value.Status = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying); });
                value?.SetOwner(null!);
                value?.Dispose();
            }
        OrderingRecording.Check("ArmySpriteConstruction", actual);
    }

    // The sprite and its army store through the tree since their ReadWrite is retired (PE-3-i), so
    // the storage streams write and read them through the tree, and a set the tree loads carries
    // explicit bounds where a constructed set has the default ones.
    // Both bound the same members and compare equal, so the snapshots leave the bounds out.
    private static string WithoutSetBounds(string snapshot) =>
        Regex.Replace(snapshot, pattern: @"""Set\\u00601\.(bounded|high|low)"":[^,}]*,?", replacement: "");

    [Fact]
    public void SavedArmyReturnsThroughCreateEngineObjectAndTheStorageStreams()
    {
        using var fixture = new OrderingRecording.Fixture(typeof(ArmyHexSprite));
        EngineObject source = new ArmyHexSprite(null!) { Holder = EngineFixture.Engine };
        EngineObject loaded = null!;
        using var memory = new MemoryStream();
        try
        {
            ((ArmyHexSprite)source).Army.Player = 2;
            using (var writer = new StorageWriter(memory, fixture.Engine, false)) { writer.PersistObject(source); }
            memory.Position = 0;
            loaded = fixture.Engine.CreateEngineObject(ArmyHexSprite.ClassId, null!);
            using (var reader = new StorageReader(memory, fixture.Engine, false)) { reader.PersistObject(loaded); }
            Assert.IsType<ArmyHexSprite>(loaded);
            Assert.Equal(WithoutSetBounds(OrderingRecording.Snapshot(source)), WithoutSetBounds(OrderingRecording.Snapshot(loaded)));
        }
        finally
        {
            StrategicMap.Current = EngineFixture.Configure(new StrategicMap(null!, holder: EngineFixture.Engine), value => { value.Status = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying); });
            loaded?.Dispose();
            source.Dispose();
        }
    }
}
