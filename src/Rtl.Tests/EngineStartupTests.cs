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
using Sound = Aow.Units.Sound.Sound;

namespace Aow.Rtl.Tests;

public sealed class EngineStartupTests
{
    private static readonly FieldInfo Counter = typeof(TObject).GetField("nextId", BindingFlags.Static | BindingFlags.NonPublic)!;
    private static readonly JsonSerializerOptions Json = new() { WriteIndented = true };

    // On the base commit set AOW_ENGINE_STARTUP_RECORD to the baseline's full path.
    // Regeneration is rejected once the engine initializer declaration is retired.
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
            using var fixture = new RecordingHost(route);
            Rtl.InstallHost(fixture);
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
                        "Engine" => CreateEngine(),
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
        if (target is not null)
        {
            Assert.NotNull(typeof(TAoWEngine).GetMethod("Create__init", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            File.WriteAllText(target, actual.ToJsonString(Json).Replace("\r\n", "\n", StringComparison.Ordinal) + "\n", new System.Text.UTF8Encoding(false));
            return;
        }
        using var baseline = typeof(EngineStartupTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.EngineStartupBaseline.json")!;
        var expected = JsonNode.Parse(baseline)!.AsObject();
        foreach (var pair in actual)
            Assert.True(JsonNode.DeepEquals(expected[pair.Key], pair.Value), pair.Key + " startup recording differs from the base commit.");
        Assert.Equal(expected.Count, actual.Count);
    }

    private static TEngine CreateEngine()
    {
        // Reflection lets the same recording run before and after the constructor conversion.
        if (typeof(TEngine).GetConstructor([typeof(TECustomNode)]) is { } ctor)
            return (TEngine)ctor.Invoke([null]);
        var value = (TEngine)Activator.CreateInstance(typeof(TEngine))!;
        typeof(TEngine).GetMethod("Create__init", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)!.Invoke(value, [null]);
        return value;
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

    private static JsonObject Snapshot(TAoWEngine engine, RecordingHost fixture) => new()
    {
        ["Version"] = engine.FVersion, ["User"] = engine.FUser.Mask, ["Options"] = engine.Options.Mask,
        ["Debug"] = engine.FDebug, ["Detail"] = engine.FAnimationDetail,
        ["Language"] = engine.FLanguage, ["DictionaryLanguage"] = engine.FDictionary.Reader.Language,
        ["Demo"] = engine.FDemo, ["StartupDirectory"] = fixture.Relative(engine.StartupDirectory.ToString()),
        ["DebugIDs"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, engine.DebugIDs.Count).Select(i => new { Name = engine.DebugIDs[i].ToString(), Id = Rtl.ObjectToInt(engine.DebugIDs.Objects[i]) })),
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

    private sealed record Entry(string Call, string Argument, int? Engine, int? Set, int Counter, int Engines, string[] DestroyFrames);

    private sealed class RecordingHost : IRtlHost, IFileSystem, ISettingsStore, IDialogService, IDisposable
    {
        private readonly string fault;
        private readonly string root = Path.Combine(Path.GetTempPath(), "open-aow-engine-" + Guid.NewGuid().ToString("N"));
        private readonly SystemIoFileSystem files = new();
        private readonly Dictionary<FieldInfo, object?> globals = [];
        private readonly IFileSystem previousFiles = Rtl.Files;
        private readonly ISettingsStore? previousSettings = Rtl.Settings;
        private readonly IDialogService? previousDialogs = Rtl.Dialogs;
        private readonly IClock previousClock = Rtl.Clock;
        private readonly AnsiString previousExe = Rtl.Application.ExeName;
        private readonly Action<AnsiString> previousMessage = Rtl.MessageHandler;
        private readonly string previousLocalization = LocalizationRuntime.StartupDirectory;
        internal readonly List<Entry> Events = [];
        internal int Origin;
        internal RecordingHost(string fault)
        {
            this.fault = fault;
            foreach (Type type in new[] { typeof(Game), typeof(Core), typeof(Hex), typeof(Images), typeof(Sound) })
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    if (!field.IsLiteral && !field.IsInitOnly) globals.Add(field, field.GetValue(null));
            Directory.CreateDirectory(root);
            foreach (string directory in new[] { "IMAGES", "SFX", "PLUGINS", "Dict" }) Directory.CreateDirectory(Path.Combine(root, directory));
            if (fault == "dictionary") File.WriteAllBytes(Path.Combine(root, "Dict", "ResStr.mld"), [0]);
            if (fault == "plugin") File.WriteAllBytes(Path.Combine(root, "PLUGINS", "X.DPL"), []);
            files.SetCurrentDirectory(root);
            Core.Engines = new TList();
            Core.PlugIns = new TList();
            Core.Packages = new TStringList();
            Core.VerifyEObjectEnabled = true;
            Game.AoWEngine = null!;
            Game.AoWHSSet = null!;
            Game.AoWHSMap = null!;
            Hex.HSEngineDID = Game.CombatDID = Game.AIDID = Game.TMPDID = Game.MAGICDID = 0;
            Images.BaseImageLibraryPath = Sound.BaseSoundEnginePath = default;
            LocalizationRuntime.StartupDirectory = "";
        }
        internal string Relative(string value) => value.Replace(root, "$root", StringComparison.OrdinalIgnoreCase).Replace('\\', '/');
        internal Entry Event(string call, string argument, string[]? destroy = null) => new(call, Relative(argument), Game.AoWEngine?.InstanceId, Game.AoWHSSet?.InstanceId, (int)Counter.GetValue(null)!, Core.Engines.Count, destroy ?? []);
        private void Record(string call, string argument) => Events.Add(Event(call, argument));
        internal JsonObject State() => new()
        {
            ["AoWEngine"] = Game.AoWEngine is null ? null : Game.AoWEngine.InstanceId - Origin,
            ["AoWHSSet"] = Game.AoWHSSet is null ? null : Game.AoWHSSet.InstanceId - Origin,
            ["HSEngineDID"] = Hex.HSEngineDID, ["CombatDID"] = Game.CombatDID, ["AIDID"] = Game.AIDID,
            ["TMPDID"] = Game.TMPDID, ["MAGICDID"] = Game.MAGICDID,
            ["BaseImageLibraryPath"] = Relative(Images.BaseImageLibraryPath.ToString()),
            ["BaseSoundEnginePath"] = Relative(Sound.BaseSoundEnginePath.ToString()),
            ["LocalizationStartupDirectory"] = Relative(LocalizationRuntime.StartupDirectory),
            ["Engines"] = JsonSerializer.SerializeToNode(Enumerable.Range(0, Core.Engines.Count).Select(i => ((TObject)Core.Engines[i]!).InstanceId - Origin)),
            ["Counter"] = (int)Counter.GetValue(null)! - Origin
        };
        public IFileSystem Files => this;
        public ISettingsStore Settings => this;
        public IDialogService Dialogs => this;
        public IClock Clock => previousClock;
        public string ExeName => Path.Combine(root, "AoW.exe");
        public string GameRoot => root;
        public string CurrentDirectory { get { Record("CurrentDirectory", ""); return files.CurrentDirectory; } }
        public string ExpandPath(string path) { Record("ExpandPath", path); return Checked(path); }
        private string Checked(string path)
        {
            string full = files.ExpandPath(path);
            Assert.True(full.Equals(root, StringComparison.OrdinalIgnoreCase) || full.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Startup escaped temporary root: " + full);
            return full;
        }
        public bool SetCurrentDirectory(string path) { Record("SetCurrentDirectory", path); return files.SetCurrentDirectory(Checked(path)); }
        public Stream Open(string path, int mode) { Record("Open:" + mode, path); return files.Open(Checked(path), mode); }
        public bool FileExists(string path) { Record("FileExists", path); return files.FileExists(Checked(path)); }
        public bool DirectoryExists(string path) { Record("DirectoryExists", path); return files.DirectoryExists(Checked(path)); }
        public bool DeleteFile(string path) { Record("DeleteFile", path); return files.DeleteFile(Checked(path)); }
        public bool RenameFile(string source, string target) { Record("RenameFile", source + " -> " + target); return files.RenameFile(Checked(source), Checked(target)); }
        public bool CreateDirectory(string path) { Record("CreateDirectory", path); return files.CreateDirectory(Checked(path)); }
        public bool RemoveDirectory(string path) { Record("RemoveDirectory", path); return files.RemoveDirectory(Checked(path)); }
        public int GetAttributes(string path) { Record("GetAttributes", path); return files.GetAttributes(Checked(path)); }
        public int SetAttributes(string path, int attributes) { Record("SetAttributes:" + attributes, path); return files.SetAttributes(Checked(path), attributes); }
        public DateTime? LastWriteTime(string path) { Record("LastWriteTime", path); return files.LastWriteTime(Checked(path)); }
        public IEnumerable<RtlFileEntry> Enumerate(string directory)
        {
            Record("Enumerate", directory);
            if (fault == "images" && Checked(directory).TrimEnd('\\', '/').Equals(Path.Combine(root, "IMAGES"), StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("injected IMAGES search failure");
            return files.Enumerate(Checked(directory)).ToArray();
        }
        public object? Read(string key)
        {
            Record("Settings.Read", key);
            if (fault == "debug" && key.EndsWith("\\Debug Mode", StringComparison.Ordinal)) throw new InvalidDataException("injected debug-mode settings failure");
            return null;
        }
        public void Write(string key, object value) => Record("Settings.Write", key + "=" + value);
        public int Show(DialogRequest request) { Record("Dialog", request.Text); return request.DefaultResult; }
        public void Dispose()
        {
            var partial = Game.AoWEngine;
            if (partial is not null && partial.FGameSet is null)
            {
                partial.FDictionary.Free();
                partial.FEmailGames.Free();
                partial.FRegistry.Free();
            }
            Core.Engines.Free(); Core.PlugIns.Free(); Core.Packages.Free();
            foreach (var pair in globals) pair.Key.SetValue(null, pair.Value);
            LocalizationRuntime.StartupDirectory = previousLocalization;
            Rtl.Files = previousFiles; Rtl.Settings = previousSettings; Rtl.Dialogs = previousDialogs;
            Rtl.Clock = previousClock; Rtl.Application.ExeName = previousExe; Rtl.MessageHandler = previousMessage;
            Directory.Delete(root, true);
        }
    }
}
