#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
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
            _ = base.AddChild(child);
            PublishedAtInsertion.Add(ReferenceEquals(EngineFixture.GlobalCombatMap, child));
            return reject ? throw new InvalidDataException("owner rejects after insertion") : Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) => base.RemoveChild(child) < 0 ? -1 : 0;
    }

    // On the base commit set AOW_COMBAT_MAP_RECORD to the baseline's full path to regenerate.
    // Regeneration is rejected after conversion to keep the original failure evidence.
    [Fact]
    public void RoutesStateOwnerPublicationOffsetsAndFixtureFailuresMatchBaseCommit()
    {
        IDisposable previousScope = EngineFixture.UseCombatMap(CombatMap.Current);
        JsonObject actual = [];
        bool plain = EngineClassCreation.OwnerConstructor(typeof(CombatMap)) is not null;
        try
        {
            for (int failure = 0; failure < 3; failure++)
            {
                for (int owned = 0; owned < 2; owned++)
                {
                    for (int route = 0; route < 4; route++)
                    {
                        using OrderingRecording.Fixture fixture = new(typeof(CombatMap));
                        ImageLibraryManagerObject entry = fixture.Manager.DiskList.AddImageLibraryFile("TargHex.ILB");
                        entry.ImageLibrary = new ImageLibrary(null!);
                        IDisposable setScope = EngineFixture.UsePack(GamePack.Current);
                        if (failure == 2)
                        {
                            EngineFixture.SetPack(null!);
                        }

                        EngineFixture.SetCombatMap(null!);
                        MapPublicationOwner? owner = owned == 1 || failure == 1 ? new MapPublicationOwner(failure == 1) : null;
                        CombatMap? value = null;
                        Exception? caught = null;
                        try
                        {
                            value = Assert.IsType<CombatMap>(OrderingRecording.Create(typeof(CombatMap), owner, route, fixture.Engine));
                        }
                        catch (Exception error) when (error is not XunitException)
                        {
                            caught = error;
                        }
                        finally
                        {
                            setScope.Dispose();
                        }
                        Assert.Equal(failure == 0, actual: value is not null);
                        CombatMap published = EngineFixture.GlobalCombatMap;
                        Assert.NotNull(published);
                        if (owner is not null)
                        {
                            Assert.Equal([true], owner.PublishedAtInsertion);
                        }

                        int destroyEntries = caught is null ? 0 : new StackTrace(caught).GetFrames().Count(x => x.GetMethod()?.DeclaringType == typeof(CombatMap) && x.GetMethod()?.Name == "Dispose");
                        JsonObject record = new()
                        {
                            ["GlobalType"] = published.GetType().FullName,
                            ["OwnerMembership"] = owner?.Members.Contains(published),
                            ["PublishedAtInsertion"] = JsonSerializer.SerializeToNode(owner?.PublishedAtInsertion),
                            ["ExceptionType"] = caught?.GetType().FullName,
                            ["ExceptionMessage"] = caught?.Message,
                            ["DestroyEntries"] = destroyEntries,
                            ["FieldGraph"] = failure == 0 ? JsonNode.Parse(OrderingRecording.Snapshot(value!)) : null
                        };
                        actual.Add($"failure{failure}.owner{owned}.route{route}", value: record);
                        if (failure == 0)
                        {
                            Assert.Same(value, published);
                            CombatMapMouse mouse = Assert.IsType<CombatMapMouse>(value!.Mouse);
                            Assert.Equal(256, mouse.CursorMode);
                            Assert.All(Enumerable.Range(0, count: 8), i => Assert.Equal(2, value.PositionStatus[i]));
                            Assert.Contains("RectangleSelectEvent", record["FieldGraph"]!.ToJsonString());
                            value.SetOwner(null!);
                            value.Dispose();
                            Assert.Null(EngineFixture.GlobalCombatMap);
                            Assert.Equal(0, entry.RefCount);
                        }
                        else
                        {
                            Assert.Equal(plain ? 0 : 1, destroyEntries);
                            Assert.IsType(plain && failure == 1 ? typeof(InvalidDataException) : typeof(NullReferenceException), @object: caught);
                            owner?.Members.Clear();
                        }
                    }
                }
            }

            string? target = Environment.GetEnvironmentVariable("AOW_COMBAT_MAP_RECORD");
            if (target is not null)
            {
                Assert.False(plain, userMessage: "Regenerate on the base commit before constructor edits.");
                File.WriteAllText(target, contents: $"{actual.ToJsonString(new JsonSerializerOptions { WriteIndented = true }).Replace("\r\n", "\n", comparisonType: StringComparison.Ordinal)}\n", encoding: new UTF8Encoding(false));
                return;
            }
            using Stream stream = typeof(CombatMapConstructionTests).Assembly.GetManifestResourceStream("Aow.Game.Tests.CombatMapConstructionSnapshot.json")!;
            JsonObject expected = JsonNode.Parse(stream)!.AsObject();
            Assert.Equal(expected.Count, actual.Count);
            foreach (KeyValuePair<string, JsonNode?> pair in actual)
            {
                JsonNode baseline = expected[pair.Key]!.DeepClone();
                if (plain && !pair.Key.StartsWith("failure0", comparisonType: StringComparison.Ordinal))
                {
                    // D1 accepts no destructor on fixture-only failures. Owner errors now survive.
                    baseline["DestroyEntries"] = 0;
                    if (pair.Key.StartsWith("failure1", comparisonType: StringComparison.Ordinal))
                    {
                        baseline["ExceptionType"] = typeof(InvalidDataException).FullName;
                        baseline["ExceptionMessage"] = "owner rejects after insertion";
                    }
                }
                Assert.True(JsonNode.DeepEquals(baseline, pair.Value), userMessage: $"{pair.Key}\n{pair.Value}");
            }
        }
        finally
        {
            previousScope.Dispose();
        }
    }
}
