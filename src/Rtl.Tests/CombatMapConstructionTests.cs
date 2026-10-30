using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aow.Units.AoWE;
using Aow.Units.AoWTC;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using Combat = Aow.Units.AoWTC.AoWTC;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

public sealed class CombatMapConstructionTests
{
    private sealed class Owner(bool reject) : TECustomNode
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<bool> PublishedAtInsertion = [];
        public override int AddChild(TEObject child)
        {
            Members.Add(child);
            PublishedAtInsertion.Add(ReferenceEquals(Combat.AoWCombatMap, child));
            if (reject) throw new InvalidDataException("owner rejects after insertion");
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child) => Members.Remove(child) ? 0 : -1;
    }

    // On the base commit set AOW_COMBAT_MAP_RECORD to the baseline's full path to regenerate.
    // Regeneration is rejected after conversion to keep the original failure evidence.
    [Fact]
    public void RoutesStateOwnerPublicationOffsetsAndFixtureFailuresMatchBaseCommit()
    {
        var previous = Combat.AoWCombatMap;
        var actual = new JsonObject();
        bool plain = typeof(TAoWCombatMap).GetConstructor([typeof(TECustomNode)]) is not null;
        try
        {
            for (int failure = 0; failure < 3; failure++)
            for (int owned = 0; owned < 2; owned++)
            for (int route = 0; route < 4; route++)
            {
                using var fixture = new OrderingRecording.Fixture(typeof(TAoWCombatMap));
                var entry = fixture.Manager.FDiskList.AddILFile((AnsiString)"TargHex.ILB");
                entry.FImageLibrary = TImageLibrary.Create(null!);
                var set = Game.AoWHSSet;
                if (failure == 2) Game.AoWHSSet = null!;
                Combat.AoWCombatMap = null!;
                var owner = owned == 1 || failure == 1 ? new Owner(failure == 1) : null;
                int origin = new TObject().InstanceId;
                TAoWCombatMap? value = null;
                System.Exception? caught = null;
                try
                {
                    value = Assert.IsType<TAoWCombatMap>(OrderingRecording.Create(typeof(TAoWCombatMap), owner, route, fixture.Engine));
                }
                catch (System.Exception error) when (error is not Xunit.Sdk.XunitException) { caught = error; }
                finally { Game.AoWHSSet = set; }
                Assert.Equal(failure == 0, value is not null);
                var published = Combat.AoWCombatMap;
                Assert.NotNull(published);
                Assert.Equal(origin + 1, published.InstanceId);
                if (owner is not null) Assert.Equal([true], owner.PublishedAtInsertion);
                int destroyEntries = caught is null ? 0 : new StackTrace(caught).GetFrames().Count(x => x.GetMethod()?.DeclaringType == typeof(TAoWCombatMap) && x.GetMethod()?.Name == "Destroy");
                var record = new JsonObject
                {
                    ["GlobalType"] = published.GetType().FullName,
                    ["GlobalOffset"] = published.InstanceId - origin,
                    ["OwnerMembership"] = owner?.Members.Contains(published),
                    ["PublishedAtInsertion"] = JsonSerializer.SerializeToNode(owner?.PublishedAtInsertion),
                    ["ExceptionType"] = caught?.GetType().FullName,
                    ["ExceptionMessage"] = caught?.Message,
                    ["DestroyEntries"] = destroyEntries,
                    ["FieldGraph"] = failure == 0 ? JsonNode.Parse(OrderingRecording.Snapshot(value!, origin)) : null
                };
                string key = $"failure{failure}.owner{owned}.route{route}";
                actual.Add(key, record);
                if (failure == 0)
                {
                    Assert.Same(value, published);
                    var mouse = Assert.IsType<TAoWCombatMapMouse>(value!.Mouse);
                    Assert.Equal(256, mouse.CursorMode);
                    Assert.All(Enumerable.Range(0, 8), i => Assert.Equal(2, value.FPositionStatus[i]));
                    Assert.Contains("RectangleSelectEvent", record["FieldGraph"]!.ToJsonString());
                    value.SetOwner(null!);
                    value.Destroy();
                    Assert.Null(Combat.AoWCombatMap);
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
                File.WriteAllText(target, actual.ToJsonString(new JsonSerializerOptions { WriteIndented = true }).Replace("\r\n", "\n", StringComparison.Ordinal) + "\n", new System.Text.UTF8Encoding(false));
                return;
            }
            using var stream = typeof(CombatMapConstructionTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.CombatMapConstructionBaseline.json")!;
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
        finally { Combat.AoWCombatMap = previous; }
    }
}
