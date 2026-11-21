#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Text.Json.Nodes;

using Aow.Game.Magic.Abilities;
using Xunit.Sdk;

namespace Aow.Game.Tests;

public sealed class EngineStartupTests
{
    // These fields were untyped DelphiList. The startup walk still enters them so the recorded element paths stay.
    private static readonly HashSet<string> ConvertedListFields =
    [
        with(StringComparer.Ordinal),
        "Aow.Game.AI.Groups.AIGroupManager.RegisteredClasses",
        "Aow.Game.Combat.CombatMap.Effects",
        "Aow.Game.Combat.CombatMap.FleeLocations",
        "Aow.Game.Combat.CombatMap.Walls",
        "Aow.Game.Combat.BattleAI._allies",
        "Aow.Game.Combat.BattleAI._enemies",
        "Aow.Game.Combat.BattleAI._moves",
        "Aow.Game.Combat.BattleAI._units",
        "Aow.Game.Combat.BattleAI._walls",
        "Aow.Game.Commands.CombatMoveTokenEvent._objects",
        "Aow.Game.Draw.BlendTableList._list",
        "Aow.Game.Draw.DisplayModeList._list",
        "Aow.Game.Draw.Images.PaletteManager._palettes",
        "Aow.Game.Draw.TacticalBattleUnitHexSprite.Abilities",
        "Aow.Game.Draw.Point3DList._list",
        "Aow.Game.GameEngine.EngineClasses",
        "Aow.Game.Magic.Abilities.AbilityControl.Abilities",
        "Aow.Game.Magic.Spells.SpellControl.RegisteredSpells",
        "Aow.Game.Net.PackageDataList._list",
        "Aow.Game.Net.TokenExecutor.TacticalBattleList",
        "Aow.Game.Net.TokenManager._tokenControls",
        "Aow.Game.UI.Controls.Memo.ColorList",
        "Aow.Game.UI.Controls.MenuBar.Menus",
        "Aow.Game.UI.Controls.TabPanel.Panels",
        "Aow.Game.UI.Controls.UIComponent.BlitList",
        "Aow.Game.UI.Controls.UIComponent.DrawList",
        "Aow.Game.UI.Dialogs.General.Abilities",
        "Aow.Game.UI.Dialogs.ResultsWindow.Controls",
        "Aow.Game.UI.Dialogs.TitleScene._credits",
        "Aow.Game.UI.Windows.Menu.SubMenus",
        "Aow.Game.UI.Windows.HtmlMemo.Changes",
        "Aow.Game.UI.Windows.HtmlMemo._fonts",
        "Aow.Game.Units.ItemControl._items",
        "Aow.Game.Units.UnitControl.Units",
        "Aow.Game.World.Map.StrategicMap.Effects",
        "Aow.Game.World.Map.HexSpriteSelection._locationHexSprite",
        "Aow.Game.World.Map.MapObjectResourceTerrainControl.Resources",
        "Aow.Game.World.PadControl.PadResources",
        "Aow.Game.World.Terrain.FireControl._resources",
    ];

    private static bool IsGenericList(object value)
    {
        Type type = value.GetType();
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    private static bool ConvertedListField(FieldInfo field)
        => ConvertedListFields.Contains($"{field.DeclaringType?.FullName ?? ""}.{MemberNames.Of(field)}");

    // These fields were hash tables. The walk records the dictionary type and does not enter its entries.
    private static readonly HashSet<string> ConvertedDictionaryFields =
    [
        with(StringComparer.Ordinal),
        "Aow.Game.Draw.Images.ImageLibraryManagerList.HashTable",
        "Aow.Game.GameEngine.ClassTable",
        "Aow.Game.Units.UnitControl.UnitTable",
    ];

    private static bool IsDictionary(object value)
    {
        Type type = value.GetType();
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
    }

    private static bool ConvertedDictionaryField(FieldInfo field)
        => ConvertedDictionaryFields.Contains($"{field.DeclaringType?.FullName ?? ""}.{MemberNames.Of(field)}");

    [Fact]
    public async Task RealStartupSideEffectsStateAndThreeFailuresMatchBaseCommit()
    {
        // The base recording starts in a fresh process. Earlier tests populate lazy class caches.
        if (Environment.GetEnvironmentVariable("AOW_ENGINE_STARTUP_ISOLATED") is null)
        {
            ProcessStartInfo start = new("dotnet") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
            start.ArgumentList.Add("vstest");
            start.ArgumentList.Add(typeof(EngineStartupTests).Assembly.Location);
            start.ArgumentList.Add("/TestCaseFilter:FullyQualifiedName~EngineStartupTests");
            start.Environment["AOW_ENGINE_STARTUP_ISOLATED"] = "1";
            using Process process = Process.Start(start)!;
            Task<string> stdout = process.StandardOutput.ReadToEndAsync();
            Task<string> stderr = process.StandardError.ReadToEndAsync();
            using CancellationTokenSource timeout = new(TimeSpan.FromSeconds(60));
            try
            {
                await process.WaitForExitAsync(timeout.Token);
            }
            catch (OperationCanceledException)
            {
                process.Kill(entireProcessTree: true);
                throw;
            }
            Assert.True(process.ExitCode == 0, userMessage: await stdout + await stderr);
            return;
        }
        JsonObject actual = [];
        foreach (string route in new[] { "AoW", "HS", "Engine", "debug", "dictionary", "images" })
        {
            using EngineFixture.RecordingHost fixture = new(route);
            RtlHost.InstallHost(fixture);
            List<EngineFixture.Entry> events = fixture.Events;
            void Thrown(object? sender, FirstChanceExceptionEventArgs arguments)
            {
                if (arguments.Exception is XunitException)
                {
                    return;
                }

                string[] destroy = [.. new StackTrace(arguments.Exception).GetFrames()
                    .Where(x => x.GetMethod()?.Name == "Dispose")
                    .Select(x => x.GetMethod()!.DeclaringType!.Name)];
                events.Add(fixture.Event("throw", argument: $"{arguments.Exception.GetType().FullName}: {fixture.Relative(arguments.Exception.Message)}", destroy));
            }
            AppDomain.CurrentDomain.FirstChanceException += Thrown;
            try
            {
                GameEngine? value = null;
                Exception? caught = null;
                try
                {
                    value = route switch
                    {
                        "Engine" => new GameEngine(null!),
                        "HS" => GameEngine.Create(null!),
                        _ => GameEngine.Create(null!)
                    };
                }
                catch (Exception error)
                {
                    caught = error;
                }
                // A failed construction leaves no reference to the partial engine, so the caller
                // records its objects only for an engine that was built.
                Assert.Equal(route is "AoW" or "HS" or "Engine", actual: caught is null);
                JsonObject result = new()
                {
                    ["ExceptionType"] = caught?.GetType().FullName,
                    ["ExceptionMessage"] = caught is null ? null : fixture.Relative(caught.Message),
                    ["StateAtCaller"] = fixture.State(),
                    ["ObjectsAtCaller"] = value is null ? null : Objects(value),
                    ["DestroyExceptionFrames"] = caught is null ? null : JsonSerializer.SerializeToNode(new StackTrace(caught).GetFrames()
                        .Where(x => x.GetMethod()?.Name == "Dispose").Select(x => x.GetMethod()!.DeclaringType!.Name).ToArray())
                };
                if (route == "AoW")
                {
                    result["FinalSnapshot"] = Snapshot(value!, fixture);
                    result["MovePointTables"] = MovePoints();
                }
                else if (caught is not null)
                {
                    Assert.Null(value);
                }
                if (value is not null)
                {
                    events.Add(fixture.Event("Destroy.begin", argument: value.GetType().Name));
                    value.Dispose();
                    events.Add(fixture.Event("Destroy.end", argument: value.GetType().Name));
                }
                result["StateAfterDestruction"] = fixture.State();
                // Failed early destructors leave partial objects. Free their independent resources
                // after the recording without retrying virtual engine destruction.
                result["Events"] = JsonSerializer.SerializeToNode(events.Select(x => new
                {
                    x.Call,
                    x.Argument,
                    x.Set,
                    x.DestroyFrames
                }));
                actual.Add(route, result);
            }
            finally
            {
                AppDomain.CurrentDomain.FirstChanceException -= Thrown;
            }
        }
        string? target = Environment.GetEnvironmentVariable("AOW_ENGINE_STARTUP_RECORD");
        if (target is not null)
        {
            throw new InvalidOperationException("Startup baseline recording ended with constructor conversion.");
        }

        using Stream baseline = typeof(EngineStartupTests).Assembly.GetManifestResourceStream("Aow.Game.Tests.EngineStartupSnapshot.json")!;
        JsonObject expected = JsonNode.Parse(baseline)!.AsObject();
        foreach (KeyValuePair<string, JsonNode?> pair in actual)
        {
            Assert.True(JsonNode.DeepEquals(expected[pair.Key], pair.Value), userMessage: $"{pair.Key} startup recording differs from the base commit.");
        }

        Assert.Equal(expected.Count, actual.Count);
    }

    private static JsonObject Objects(object root)
    {
        JsonObject result = [];
        HashSet<object> seen = [with(ReferenceEqualityComparer.Instance)];
        void Visit(object value, string path)
        {
            if (!seen.Add(value))
            {
                return;
            }

            result[path] = new JsonObject { ["Type"] = value.GetType().FullName };
            if (DelphiItems(value) is { } list)
            {
                for (int index = 0; index < list.Length; index++)
                {
                    if (list[index] is object element && LegacyRoots.Contains(element.GetType()))
                    {
                        Visit(element, path: $"{path}[{index}]");
                    }
                }
            }
            else if (IsGenericList(value) && value is System.Collections.IList items)
            {
                for (int index = 0; index < items.Count; index++)
                {
                    if (items[index] is object element && LegacyRoots.Contains(element.GetType()))
                    {
                        Visit(element, path: $"{path}[{index}]");
                    }
                }
            }
            for (Type? type = value.GetType(); type is not null && type != typeof(object); type = type.BaseType)
            {
                foreach (FieldInfo? field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(MemberNames.Of, comparer: StringComparer.Ordinal))
                {
                    if (MemberNames.Of(field) is "_owner" or "Owner" || field.GetValue(value) is not object child)
                    {
                        continue;
                    }

                    if (LegacyRoots.Contains(child.GetType())
                        || (IsGenericList(child) && ConvertedListField(field))
                        || (IsDictionary(child) && ConvertedDictionaryField(field)))
                    {
                        Visit(child, path: $"{path}.{MemberNames.Of(field)}");
                    }
                }
            }
        }
        Visit(root, path: "Engine");
        return result;
    }

    // The live slots of a DelphiList of any element type, or null for another value.
    private static object?[]? DelphiItems(object value)
    {
        for (Type? type = value.GetType(); type is not null; type = type.BaseType)
        {
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(DelphiList<>))
            {
                continue;
            }

            Array items = (Array)type.GetProperty("List")!.GetValue(value)!;
            return [.. items.Cast<object?>().Take((int)type.GetProperty("Count")!.GetValue(value)!)];
        }
        return null;
    }

    private static JsonObject Snapshot(GameEngine engine, EngineFixture.RecordingHost fixture) => new()
    {
        ["Version"] = engine.Version,
        ["User"] = (ulong)engine.User,
        ["Options"] = (ulong)engine.Options,
        ["Debug"] = engine.Debug,
        ["Detail"] = engine.AnimationDetail,
        ["Language"] = engine.Language,
        ["DictionaryLanguage"] = engine.Dictionary.Reader.Language,
        ["Demo"] = engine.Demo,
        ["StartupDirectory"] = fixture.Relative(engine.StartupDirectory),
        ["ClassIDs"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, engine.EngineClasses.Count).Select(i => engine.EngineClasses[i].ClassId)),
        ["Children"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, engine.Count).Select(i => engine.Children[i].GetType().FullName)),
        ["Globals"] = fixture.State()
    };

    private static JsonObject MovePoints()
    {
        JsonObject result = [];
        for (int mask = 0; mask < 256; mask++)
        {
            JsonObject terrain = [];
            for (int t = 0; t < 16; t++)
            {
                JsonObject overlay = [];
                for (int o = -1; o < 15; o++)
                {
                    overlay[$"Overlay[{o}]"] = (int)MovementTables.MovePointTables[mask][t][o + 1];
                }

                terrain[$"Terrain[{t}]"] = overlay;
            }
            result[$"Mask[{mask}]"] = terrain;
        }
        return result;
    }
}
