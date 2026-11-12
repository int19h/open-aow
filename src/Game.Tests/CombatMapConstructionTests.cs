using System.Diagnostics;

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Xunit.Sdk;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class CombatMapConstructionTests
{
    private sealed class MapPublicationOwner(bool reject) : EngineFixture.Owner
    {
        internal List<bool> PublishedAtInsertion { get; } = [];

        public override int AddChild(EngineObject child)
        {
            base.AddChild(child);
            PublishedAtInsertion.Add(ReferenceEquals(CombatMap.Current, child));
            if (reject) throw new InvalidDataException("owner rejects after insertion");
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) => base.RemoveChild(child) < 0 ? -1 : 0;
    }

    // On the base commit set AOW_COMBAT_MAP_RECORD to the baseline's full path to regenerate.
    // Regeneration is rejected after conversion to keep the original failure evidence.
    [Fact]
    public void RoutesStateOwnerPublicationOffsetsAndFixtureFailuresMatchBaseCommit()
    {
        var previous = CombatMap.Current;
        var actual = new JsonObject();
        bool plain = typeof(CombatMap).GetConstructor([typeof(ECustomNode)]) is not null;
        try
        {
            for (int failure = 0; failure < 3; failure++)
                for (int owned = 0; owned < 2; owned++)
                    for (int route = 0; route < 4; route++)
                    {
                        using var fixture = new OrderingRecording.Fixture(typeof(CombatMap));
                        var entry = fixture.Manager.DiskList.AddImageLibraryFile("TargHex.ILB");
                        entry.ImageLibrary = new ImageLibrary(null!);
                        var set = AoWHSSet.Current;
                        if (failure == 2) AoWHSSet.Current = null!;
                        CombatMap.Current = null!;
                        var owner = owned == 1 || failure == 1 ? new MapPublicationOwner(failure == 1) : null;
                        CombatMap? value = null;
                        Exception? caught = null;
                        try
                        {
                            value = Assert.IsType<CombatMap>(OrderingRecording.Create(typeof(CombatMap), owner, route, fixture.Engine));
                        }
                        catch (Exception error) when (error is not XunitException) { caught = error; }
                        finally { AoWHSSet.Current = set; }
                        Assert.Equal(failure == 0, value is not null);
                        var published = CombatMap.Current;
                        Assert.NotNull(published);
                        if (owner is not null) Assert.Equal([true], owner.PublishedAtInsertion);
                        int destroyEntries = caught is null ? 0 : new StackTrace(caught).GetFrames().Count(x => x.GetMethod()?.DeclaringType == typeof(CombatMap) && x.GetMethod()?.Name == "Dispose");
                        var record = new JsonObject
                        {
                            ["GlobalType"] = published.GetType().FullName,
                            ["OwnerMembership"] = owner?.Members.Contains(published),
                            ["PublishedAtInsertion"] = JsonSerializer.SerializeToNode(owner?.PublishedAtInsertion),
                            ["ExceptionType"] = caught?.GetType().FullName,
                            ["ExceptionMessage"] = caught?.Message,
                            ["DestroyEntries"] = destroyEntries,
                            ["FieldGraph"] = failure == 0 ? JsonNode.Parse(OrderingRecording.Snapshot(value!)) : null
                        };
                        string key = $"failure{failure}.owner{owned}.route{route}";
                        actual.Add(key, record);
                        if (failure == 0)
                        {
                            Assert.Same(value, published);
                            var mouse = Assert.IsType<CombatMapMouse>(value!.Mouse);
                            Assert.Equal(256, mouse.CursorMode);
                            Assert.All(Enumerable.Range(0, 8), i => Assert.Equal(2, value.PositionStatus[i]));
                            Assert.Contains("RectangleSelectEvent", record["FieldGraph"]!.ToJsonString());
                            value.SetOwner(null!);
                            value.Dispose();
                            Assert.Null(CombatMap.Current);
                            Assert.Equal(0, entry.RefCount);
                        }
                        else
                        {
                            Assert.Equal(plain ? 0 : 1, destroyEntries);
                            Assert.IsType(plain && failure == 1 ? typeof(InvalidDataException) : typeof(NullReferenceException), caught);
                            owner?.Members.Clear();
                        }
                    }
            string? target = Environment.GetEnvironmentVariable("AOW_COMBAT_MAP_RECORD");
            if (target is not null)
            {
                Assert.False(plain, "Regenerate on the base commit before constructor edits.");
                File.WriteAllText(target, actual.ToJsonString(new JsonSerializerOptions { WriteIndented = true }).Replace("\r\n", "\n", StringComparison.Ordinal) + "\n", new UTF8Encoding(false));
                return;
            }
            using var stream = typeof(CombatMapConstructionTests).Assembly.GetManifestResourceStream("Aow.Game.Tests.CombatMapConstructionSnapshot.json")!;
            var expected = JsonNode.Parse(stream)!.AsObject();
            Assert.Equal(expected.Count, actual.Count);
            foreach (var pair in actual)
            {
                var baseline = expected[pair.Key]!.DeepClone();
                if (plain && !pair.Key.StartsWith("failure0", StringComparison.Ordinal))
                {
                    // D1 accepts no destructor on fixture-only failures. Owner errors now survive.
                    baseline["DestroyEntries"] = 0;
                    if (pair.Key.StartsWith("failure1", StringComparison.Ordinal))
                    {
                        baseline["ExceptionType"] = typeof(InvalidDataException).FullName;
                        baseline["ExceptionMessage"] = "owner rejects after insertion";
                    }
                }
                Assert.True(JsonNode.DeepEquals(baseline, pair.Value), pair.Key + "\n" + pair.Value);
            }
        }
        finally { CombatMap.Current = previous; }
    }
}
