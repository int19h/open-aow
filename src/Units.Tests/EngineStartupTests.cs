using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aow.Rtl.Localize;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Game = Aow.Units.AoWE.AoWE;
using Core = Aow.Units.Engine.Engine;
using Hex = Aow.Units.HSEngine.HSEngine;
using Images = Aow.Units.ImageLib.ImageLib;
using SoundApi = Aow.Units.Sound.Sound;

namespace Aow.Units.Tests;

public sealed class EngineStartupTests
{
    private static readonly FieldInfo Counter = typeof(TObject).GetField("nextId", BindingFlags.Static | BindingFlags.NonPublic)!;

    [Fact]
    public async Task RealStartupSideEffectsStateAndFourFailuresMatchBaseCommit()
    {
        // The base recording starts in a fresh process. Earlier tests populate lazy class caches.
        if (Environment.GetEnvironmentVariable("AOW_ENGINE_STARTUP_ISOLATED") is null)
        {
            var start = new ProcessStartInfo("dotnet") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
            start.ArgumentList.Add("vstest");
            start.ArgumentList.Add(typeof(EngineStartupTests).Assembly.Location);
            start.ArgumentList.Add("/TestCaseFilter:FullyQualifiedName~EngineStartupTests");
            start.Environment["AOW_ENGINE_STARTUP_ISOLATED"] = "1";
            using var process = Process.Start(start)!;
            var stdout = process.StandardOutput.ReadToEndAsync();
            var stderr = process.StandardError.ReadToEndAsync();
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            try { await process.WaitForExitAsync(timeout.Token); }
            catch (OperationCanceledException) { process.Kill(entireProcessTree: true); throw; }
            Assert.True(process.ExitCode == 0, await stdout + await stderr);
            return;
        }
        var actual = new JsonObject();
        foreach (string route in new[] { "AoW", "HS", "Engine", "debug", "dictionary", "images", "plugin" })
        {
            using var fixture = new EngineFixture.RecordingHost(route);
            Runtime.InstallHost(fixture);
            var events = fixture.Events;
            TEngine? value = null;
            System.Exception? caught = null;
            void Thrown(object? sender, FirstChanceExceptionEventArgs args)
            {
                if (args.Exception is Xunit.Sdk.XunitException) return;
                string[] destroy = new StackTrace(args.Exception).GetFrames()
                    .Where(x => x.GetMethod()?.Name == "Destroy")
                    .Select(x => x.GetMethod()!.DeclaringType!.Name).ToArray();
                events.Add(fixture.Event("throw", args.Exception.GetType().FullName + ": " + fixture.Relative(args.Exception.Message), destroy));
            }
            AppDomain.CurrentDomain.FirstChanceException += Thrown;
            try
            {
                try
                {
                    value = route switch
                    {
                        "Engine" => new TEngine(null!),
                        "HS" => THSEngine.Create(null!),
                        _ => TAoWEngine.Create(null!)
                    };
                }
                catch (System.Exception error) { caught = error; }
                var engine = value ?? Game.AoWEngine;
                Assert.NotNull(engine);
                fixture.Origin = engine.InstanceId;
                Assert.Equal(route is "AoW" or "HS" or "Engine", caught is null);
                var result = new JsonObject
                {
                    ["ExceptionType"] = caught?.GetType().FullName,
                    ["ExceptionMessage"] = caught is null ? null : fixture.Relative(caught.Message),
                    ["StateAtCaller"] = fixture.State(),
                    ["ObjectsAtCaller"] = Objects(engine, fixture.Origin),
                    ["DestroyExceptionFrames"] = caught is null ? null : JsonSerializer.SerializeToNode(new StackTrace(caught).GetFrames()
                        .Where(x => x.GetMethod()?.Name == "Destroy").Select(x => x.GetMethod()!.DeclaringType!.Name).ToArray())
                };
                if (route == "AoW")
                {
                    int before = (int)Counter.GetValue(null)!;
                    Assert.True(Game.ValidateAoWEngine());
                    Assert.Equal(before, (int)Counter.GetValue(null)!);
                    Assert.Same(value, Game.AoWEngine);
                    result["ValidateAgain"] = true;
                    result["FinalSnapshot"] = Snapshot((TAoWEngine)value!, fixture);
                    result["MovePointTables"] = MovePoints();
                }
                else if (caught is not null)
                {
                    Assert.Null(value);
                    Assert.Same(engine, Game.AoWEngine);
                    Assert.True(Game.ValidateAoWEngine());
                    result["ValidateAfterFailure"] = true;
                }
                if (value is not null)
                {
                    events.Add(fixture.Event("Destroy.begin", value.GetType().Name));
                    value.Destroy();
                    events.Add(fixture.Event("Destroy.end", value.GetType().Name));
                    Assert.Equal(-1, Core.Engines.IndexOf(value));
                }
                result["StateAfterDestruction"] = fixture.State();
                // Failed early destructors leave partial objects. Free their independent resources
                // after the recording without retrying virtual engine destruction.
                result["Events"] = JsonSerializer.SerializeToNode(events.Select(x => new
                {
                    x.Call, x.Argument, Engine = x.Engine is null ? (int?)null : x.Engine - fixture.Origin,
                    Set = x.Set is null ? (int?)null : x.Set - fixture.Origin,
                    Counter = x.Counter - fixture.Origin, x.Engines, x.DestroyFrames
                }));
                actual.Add(route, result);
            }
            finally { AppDomain.CurrentDomain.FirstChanceException -= Thrown; }
        }
        string? target = Environment.GetEnvironmentVariable("AOW_ENGINE_STARTUP_RECORD");
        if (target is not null) throw new InvalidOperationException("Startup baseline recording ended with constructor conversion.");
        using var baseline = typeof(EngineStartupTests).Assembly.GetManifestResourceStream("Aow.Units.Tests.EngineStartupSnapshot.json")!;
        var expected = JsonNode.Parse(baseline)!.AsObject();
        foreach (var pair in actual)
            Assert.True(JsonNode.DeepEquals(expected[pair.Key], pair.Value), pair.Key + " startup recording differs from the base commit.");
        Assert.Equal(expected.Count, actual.Count);
    }



    private static JsonObject Objects(TObject root, int origin)
    {
        var result = new JsonObject();
        var seen = new HashSet<TObject>(ReferenceEqualityComparer.Instance);
        void Visit(TObject value, string path)
        {
            if (!seen.Add(value)) return;
            result[path] = new JsonObject { ["Type"] = value.GetType().FullName, ["Id"] = value.InstanceId - origin };
            // The unchanged hash constructors allocate bucket[i] at table.Id + i + 1.
            // Assert every bucket offset against the table offset in the pinned snapshot.
            if (value.GetType().Name is "TIntegerHashTable256" or "TStringHashTable256")
            {
                object buckets = value.GetType().GetField("FLists", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(value)!;
                TObject Bucket(int i) => buckets switch
                {
                    Aow.Units.FixedArray_Aow_Units_Engine_TIntegerHashList_256 a => a[i],
                    Aow.Units.FixedArray_Aow_Units_Engine_TStringHashList_256 a => a[i],
                    Aow.Units.FixedArray_Aow_Units_HashTabU_TIntegerHashList_256 a => a[i],
                    Aow.Units.FixedArray_Aow_Units_HashTabU_TStringHashList_256 a => a[i],
                    _ => throw new InvalidOperationException("Unknown hash bucket layout")
                };
                for (int i = 0; i < 256; i++)
                    Assert.True(Bucket(i).InstanceId - origin == value.InstanceId - origin + i + 1,
                        path + $".FLists[{i}] allocation offset");
            }
            if (value is TList list)
                for (int i = 0; i < list.Count; i++)
                    if (list[i] is TObject child) Visit(child, path + $"[{i}]");
            for (Type? type = value.GetType(); type is not null && type != typeof(TObject); type = type.BaseType)
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name, StringComparer.Ordinal))
                    if (field.Name != "FOwner" && field.GetValue(value) is TObject child)
                        Visit(child, path + "." + field.Name);
        }
        Visit(root, "Engine");
        return result;
    }

    private static JsonObject Snapshot(TAoWEngine engine, EngineFixture.RecordingHost fixture) => new()
    {
        ["Version"] = engine.FVersion, ["User"] = engine.FUser.Mask, ["Options"] = engine.Options.Mask,
        ["Debug"] = engine.FDebug, ["Detail"] = engine.FAnimationDetail,
        ["Language"] = engine.FLanguage, ["DictionaryLanguage"] = engine.FDictionary.Reader.Language,
        ["Demo"] = engine.FDemo, ["StartupDirectory"] = fixture.Relative(engine.StartupDirectory.ToString()),
        ["DebugIDs"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, engine.DebugIDs.Count).Select(i => new { Name = engine.DebugIDs[i].ToString(), Id = Runtime.ObjectToInt(engine.DebugIDs.Objects[i]) })),
        ["ClassIDs"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, engine.EClasses.Count).Select(i => ((EClass)engine.EClasses[i]!).ClassID)),
        ["Children"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, engine.Count).Select(i => engine.Children[i].GetType().FullName)),
        ["Globals"] = fixture.State()
    };

    private static JsonObject MovePoints()
    {
        var result = new JsonObject();
        for (int mask = 0; mask < 256; mask++)
        {
            var terrain = new JsonObject();
            for (int t = 0; t < 16; t++)
            {
                var overlay = new JsonObject();
                for (int o = -1; o < 15; o++) overlay[$"Overlay[{o}]"] = (int)Game.MovePointTables[mask][t][o + 1];
                terrain[$"Terrain[{t}]"] = overlay;
            }
            result[$"Mask[{mask}]"] = terrain;
        }
        return result;
    }

}
