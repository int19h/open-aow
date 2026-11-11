using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Xunit.Sdk;

namespace Aow.Game.Tests;

public sealed class EngineStartupTests
{
    // These fields were untyped DelphiList. The startup walk still enters them so the recorded element paths stay.
    private static readonly HashSet<string> ConvertedListFields = new(StringComparer.Ordinal)
    {
        "Aow.Engine.PlugIn.FEngines",
        "Aow.Game.Ai.Groups.AIGroupManager.FRegisteredClasses",
        "Aow.Game.Combat.AoWCombatMap.FEffects",
        "Aow.Game.Combat.AoWCombatMap.FFleeLocations",
        "Aow.Game.Combat.AoWCombatMap.FWalls",
        "Aow.Game.Combat.CAI.Allies",
        "Aow.Game.Combat.CAI.Enemies",
        "Aow.Game.Combat.CAI.Moves",
        "Aow.Game.Combat.CAI.Units",
        "Aow.Game.Combat.CAI.Walls",
        "Aow.Game.Commands.CombatMoveTE.FObjects",
        "Aow.Game.Draw.BlendTableList.FList",
        "Aow.Game.Draw.DisplayModeList.FList",
        "Aow.Game.Draw.Images.PaletteManager.FPalettes",
        "Aow.Game.Draw.TacticalCombatUnitHS.FAbilities",
        "Aow.Game.Draw.XYZList.FList",
        "Aow.Game.GameEngine.FEClasses",
        "Aow.Game.Magic.Abilities.AbilityControl.FAbilities",
        "Aow.Game.Magic.Spells.SpellControl.FRegisteredSpells",
        "Aow.Game.Net.PackageDataList.FList",
        "Aow.Game.Net.TokenExecuter.FTCList",
        "Aow.Game.Net.TokenManager.FTokenControls",
        "Aow.Game.Ui.Controls.AOWMemo.FColorList",
        "Aow.Game.Ui.Controls.AOWMenuBar.FMenus",
        "Aow.Game.Ui.Controls.AOWTabPanel.FPanels",
        "Aow.Game.Ui.Controls.AoWComponent.FBltList",
        "Aow.Game.Ui.Controls.AoWComponent.FDrawList",
        "Aow.Game.Ui.Dialogs.General.FAbilities",
        "Aow.Game.Ui.Dialogs.ResultsWin.FControls",
        "Aow.Game.Ui.Dialogs.TitleScene.FCredits",
        "Aow.Game.Ui.Windows.AOWMenu.FSubMenus",
        "Aow.Game.Ui.Windows.TaowHTMLMemo.FChanges",
        "Aow.Game.Ui.Windows.TaowHTMLMemo.FFonts",
        "Aow.Game.Units.ItemControl.FItems",
        "Aow.Game.Units.UnitControl.FUnits",
        "Aow.Game.World.Map.AoWHSMap.FEffects",
        "Aow.Game.World.Map.HexagonSpriteSelection.FLocationHS",
        "Aow.Game.World.Map.MORTerrainControl.FResources",
        "Aow.Game.World.PadControl.FPadResources",
        "Aow.Game.World.Terrain.FireControl.FResources",
    };

    private static bool IsGenericList(object value)
    {
        Type type = value.GetType();
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    private static bool ConvertedListField(FieldInfo field) =>
        ConvertedListFields.Contains((field.DeclaringType?.FullName ?? "") + "." + field.Name);

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
            GameEngine? value = null;
            Exception? caught = null;
            void Thrown(object? sender, FirstChanceExceptionEventArgs args)
            {
                if (args.Exception is XunitException) return;
                string[] destroy = new StackTrace(args.Exception).GetFrames()
                    .Where(x => x.GetMethod()?.Name == "Dispose")
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
                        "Engine" => new GameEngine(null!),
                        "HS" => GameEngine.Create(null!),
                        _ => GameEngine.Create(null!)
                    };
                }
                catch (Exception error) { caught = error; }
                GameEngine engine = value ?? GameTables.AoWEngine;
                Assert.NotNull(engine);
                Assert.Equal(route is "AoW" or "HS" or "Engine", caught is null);
                var result = new JsonObject
                {
                    ["ExceptionType"] = caught?.GetType().FullName,
                    ["ExceptionMessage"] = caught is null ? null : fixture.Relative(caught.Message),
                    ["StateAtCaller"] = fixture.State(),
                    ["ObjectsAtCaller"] = Objects(engine),
                    ["DestroyExceptionFrames"] = caught is null ? null : JsonSerializer.SerializeToNode(new StackTrace(caught).GetFrames()
                        .Where(x => x.GetMethod()?.Name == "Dispose").Select(x => x.GetMethod()!.DeclaringType!.Name).ToArray())
                };
                if (route == "AoW")
                {
                    Assert.True(GameTables.ValidateAoWEngine());
                    Assert.Same(value, GameTables.AoWEngine);
                    result["ValidateAgain"] = true;
                    result["FinalSnapshot"] = Snapshot((GameEngine)value!, fixture);
                    result["MovePointTables"] = MovePoints();
                }
                else if (caught is not null)
                {
                    Assert.Null(value);
                    Assert.Same(engine, GameTables.AoWEngine);
                    Assert.True(GameTables.ValidateAoWEngine());
                    result["ValidateAfterFailure"] = true;
                }
                if (value is not null)
                {
                    events.Add(fixture.Event("Destroy.begin", value.GetType().Name));
                    value.Dispose();
                    events.Add(fixture.Event("Destroy.end", value.GetType().Name));
                    Assert.Equal(-1, EngineHost.Engines.IndexOf(value));
                }
                result["StateAfterDestruction"] = fixture.State();
                // Failed early destructors leave partial objects. Free their independent resources
                // after the recording without retrying virtual engine destruction.
                result["Events"] = JsonSerializer.SerializeToNode(events.Select(x => new
                {
                    x.Call,
                    x.Argument,
                    x.Engine,
                    x.Set,
                    x.Engines,
                    x.DestroyFrames
                }));
                actual.Add(route, result);
            }
            finally { AppDomain.CurrentDomain.FirstChanceException -= Thrown; }
        }
        string? target = Environment.GetEnvironmentVariable("AOW_ENGINE_STARTUP_RECORD");
        if (target is not null) throw new InvalidOperationException("Startup baseline recording ended with constructor conversion.");
        using var baseline = typeof(EngineStartupTests).Assembly.GetManifestResourceStream("Aow.Game.Tests.EngineStartupSnapshot.json")!;
        var expected = JsonNode.Parse(baseline)!.AsObject();
        foreach (var pair in actual)
            Assert.True(JsonNode.DeepEquals(expected[pair.Key], pair.Value), pair.Key + " startup recording differs from the base commit.");
        Assert.Equal(expected.Count, actual.Count);
    }

    private static JsonObject Objects(object root)
    {
        var result = new JsonObject();
        var seen = new HashSet<object>(ReferenceEqualityComparer.Instance);
        void Visit(object value, string path)
        {
            if (!seen.Add(value)) return;
            result[path] = new JsonObject { ["Type"] = value.GetType().FullName };
            if (value.GetType().Name is "IntegerHashTable256" or "StringHashTable256")
            {
                object buckets = value.GetType().GetField("FLists", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(value)!;
                object Bucket(int i) => buckets switch
                {
                    FixedArray_Aow_Units_Engine_TIntegerHashList_256 a => a[i],
                    FixedArray_Aow_Units_HashTabU_TStringHashList_256 a => a[i],
                    _ => throw new InvalidOperationException("Unknown hash bucket layout")
                };
                var lists = new HashSet<object>(ReferenceEqualityComparer.Instance);
                for (int i = 0; i < 256; i++)
                {
                    object bucket = Bucket(i);
                    Assert.NotNull(bucket);
                    Assert.True(lists.Add(bucket), path + " bucket " + i + " repeats an earlier list");
                }
            }
            if (value is DelphiList list)
            {
                for (int index = 0; index < list.Count; index++)
                    if (list[index] is object element && LegacyRoots.Contains(element.GetType())) Visit(element, path + $"[{index}]");
            }
            else if (IsGenericList(value) && value is System.Collections.IList items)
            {
                for (int index = 0; index < items.Count; index++)
                    if (items[index] is object element && LegacyRoots.Contains(element.GetType())) Visit(element, path + $"[{index}]");
            }
            for (Type? type = value.GetType(); type is not null && type != typeof(object); type = type.BaseType)
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name, StringComparer.Ordinal))
                {
                    if (field.Name == "FOwner" || field.GetValue(value) is not object child) continue;
                    if (LegacyRoots.Contains(child.GetType()) || (IsGenericList(child) && ConvertedListField(field)))
                        Visit(child, path + "." + field.Name);
                }
        }
        Visit(root, "Engine");
        return result;
    }

    private static JsonObject Snapshot(GameEngine engine, EngineFixture.RecordingHost fixture) => new()
    {
        ["Version"] = engine.FVersion,
        ["User"] = engine.FUser.Mask,
        ["Options"] = engine.Options.Mask,
        ["Debug"] = engine.FDebug,
        ["Detail"] = engine.FAnimationDetail,
        ["Language"] = engine.FLanguage,
        ["DictionaryLanguage"] = engine.FDictionary.Reader.Language,
        ["Demo"] = engine.FDemo,
        ["StartupDirectory"] = fixture.Relative(engine.StartupDirectory),
        ["DebugIDs"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, engine.DebugIDs.Count).Select(i => new { Name = engine.DebugIDs[i], Id = Runtime.ObjectToInt(engine.DebugIDs.Objects[i]) })),
        ["ClassIDs"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, engine.EClasses.Count).Select(i => (engine.EClasses[i]!).ClassID)),
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
                for (int o = -1; o < 15; o++) overlay[$"Overlay[{o}]"] = (int)MovementTables.MovePointTables[mask][t][o + 1];
                terrain[$"Terrain[{t}]"] = overlay;
            }
            result[$"Mask[{mask}]"] = terrain;
        }
        return result;
    }
}
