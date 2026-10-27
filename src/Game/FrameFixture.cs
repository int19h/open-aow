using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Aow.Units.AoWE;
using GameState = Aow.Units.AoWE.AoWE;
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.AoWUnit;

namespace Aow.Game;

internal sealed class FrameFixture
{
    private string? fileLocalTime;
    internal static DateTime FileStampUtc(DateTime localTime, TimeSpan offset) =>
        new DateTimeOffset(DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified), offset).UtcDateTime;

    private readonly Dictionary<string, string> defaults = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> values = new(StringComparer.Ordinal);
    private readonly Dictionary<string, object> settings = new(StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> booleans = ["Skip Intro", "Debug Mode", "Allow Resolution Switches", "Music Enabled", "Sound Effects Enabled", "Shadows Enabled", "Surface In VMem"];

    public static int Run(string path, string name, string gameRoot, string output, int? frameLimit = null)
    {
        gameRoot = Path.GetFullPath(gameRoot);
        output = Path.GetFullPath(output);
        var fixture = Load(path, name);
        string scene = fixture.Get("scene");
        bool presentationClock = fixture.defaults.GetValueOrDefault("frame_clock") == "presentation" || fixture.values.GetValueOrDefault("frame_clock") == "presentation";
        bool worldCapture = fixture.values.GetValueOrDefault("capture_clock") == "world";
        bool dayCapture = fixture.values.GetValueOrDefault("capture_clock") == "day";
        bool titleCapture = fixture.values.GetValueOrDefault("capture_clock") == "title";
        bool combatCapture = fixture.values.GetValueOrDefault("capture_clock") == "combat";
        bool afterCombatCapture = fixture.values.GetValueOrDefault("capture_clock") == "after-combat";
        bool tactical = fixture.values.GetValueOrDefault("action") == "tactical";
        var clicks = fixture.Clicks();
        int frame = frameLimit ?? int.Parse(fixture.Get("frame"), CultureInfo.InvariantCulture);
        if (frame < 1) throw new ArgumentOutOfRangeException(nameof(frameLimit));
        var size = fixture.Pair("size");
        var mouse = fixture.Pair("mouse");
        var now = DateTime.Parse(fixture.Get("local_time"), CultureInfo.InvariantCulture);
        DateTime? fileStampUtc = fixture.fileLocalTime is null ? null :
            FileStampUtc(DateTime.Parse(fixture.fileLocalTime, CultureInfo.InvariantCulture), DateTimeOffset.Now.Offset);
        var display = new HeadlessDisplayHost(output, size.X, size.Y, frame) { Mouse = mouse, FixtureFrame = frame };
        Directory.CreateDirectory(output);
        using var trace = new StreamWriter(Path.Combine(output, "exceptions.log")) { NewLine = "\n", AutoFlush = true };
        void Record(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs error) => trace.WriteLine(error.Exception + "\n" + new System.Diagnostics.StackTrace(1, true));
        AppDomain.CurrentDomain.FirstChanceException += Record;
        string? saveDirectory = fixture.values.GetValueOrDefault("save_directory");
        if (saveDirectory is not null)
        {
            string directory = Path.Combine(output, "user", saveDirectory);
            if (Directory.Exists(directory))
                foreach (string saved in Directory.EnumerateFiles(directory))
                {
                    if (Path.GetFileName(saved) != "roundtrip.asg") throw new InvalidDataException("Unexpected fixture save product " + saved);
                    File.Delete(saved);
                }
        }
        var host = new FixtureHost(gameRoot, Path.Combine(output, "user"), fixture.settings, new FixtureClock(now), saveDirectory);
        Aow.Rtl.Rtl.InstallHost(host);
        Aow.Rtl.Rtl.SuppressRandomize = true;
        Aow.Rtl.Rtl.RandSeed = int.Parse(fixture.Get("seed"), CultureInfo.InvariantCulture);
        Aow.Rtl.Rtl.RandomizeSeed = Aow.Rtl.Rtl.RandSeed;
        FrameFixtureInputs.AddressAnimationPhase = int.Parse(fixture.Get("address_animation_phase"), CultureInfo.InvariantCulture);
        FrameFixtureInputs.WaterVariantPhase = fixture.values.TryGetValue("water_variant_phase", out string? waterPhase) ? int.Parse(waterPhase, CultureInfo.InvariantCulture) : null;
        FrameFixtureInputs.Tactical = tactical;
        // The native capture uses positive FPS below 15 at its six strategic moves.
        // Reproduce that movement tier without making animation depend on host speed.
        FrameFixtureInputs.MeasuredFrameRate = fixture.defaults.TryGetValue("measured_frame_rate", out var measuredRate) ? int.Parse(measuredRate, CultureInfo.InvariantCulture) : tactical ? 9 : null;
        string argument = "";
        string? input = null;
        if (scene != "title")
        {
            input = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path))!, "../../tools/oracle/obj/frames/inputs", scene + ".asg");
            if (!File.Exists(input)) throw new FileNotFoundException("Prepared fixture input missing", input);
            File.Copy(input, Path.Combine(output, "user", scene + ".asg"), true);
            argument = Path.Combine(gameRoot, scene + ".asg");
        }
        int applied = 0;
        bool complete = false;
        bool tacticalReady = false;
        int observedDay = -1, dayOrigin = 0;
        long titleOrigin = 0;
        bool sawCombat = false;
        int lastCombatWorldFrame = 0;
        void ObserveDay()
        {
            if (GameState.AoWHSMap is not null) titleOrigin = display.Frame;
            if (GameState.AoWHSMap?.FCombatMap is not null)
            {
                sawCombat = true;
                lastCombatWorldFrame = GameState.AoWHSMap.Frame;
            }
            if (GameState.AoWHSMap is { } map && observedDay != map.FDay)
            {
                observedDay = map.FDay;
                dayOrigin = map.Frame;
            }
        }
        int DayFrame() => GameState.AoWHSMap is { } map ? map.Frame - dayOrigin + 1 : -1;
        int AfterCombatFrame() => sawCombat && GameState.AoWHSMap is { FCombatMap: null } map ? map.Frame - lastCombatWorldFrame : -1;
        long ScriptFrame(Click click)
        {
            string clock = click.Clock ?? fixture.defaults.GetValueOrDefault("frame_clock", "world");
            var map = GameState.AoWHSMap;
            return clock switch
            {
                "presentation" => display.Frame,
                "after-combat" => AfterCombatFrame(),
                "combat" => map?.FCombatMap?.Frame ?? -1,
                "title" => map is null ? display.Frame - titleOrigin : -1,
                _ => map?.FDay == (click.Day ?? int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture))
                    ? clock == "day" ? DayFrame() : map.Frame : -1
            };
        }
        using var script = new StreamWriter(Path.Combine(output, "script.jsonl")) { NewLine = "\n", AutoFlush = true };
        try
        {
            using var form = Aow.Units.AoWUnit.TMainForm.Create(display, argument);
            if (saveDirectory is not null)
                Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FDirectory = Path.Combine(gameRoot, saveDirectory).Replace('/', '\\') + "\\";
            FrameFixtureInputs.TacticalInitialized = () =>
            {
                if (GameState.AoWHSMap?.FCombatMap is not Aow.Units.AoWTC.TAoWCombatMap combat) return;
                combat.FPlaySounds = true;
                combat.SetSeatedCombatPlayer(0);
                Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.MinClick();
                tacticalReady = true;
            };
            bool AtTarget()
            {
                ObserveDay();
                if (applied != clicks.Count) return false;
                return afterCombatCapture ? AfterCombatFrame() == frame :
                combatCapture ? GameState.AoWHSMap?.FCombatMap?.Frame == frame :
                titleCapture ? GameState.AoWHSMap is null && display.Frame - titleOrigin == frame :
                dayCapture ? GameState.AoWHSMap?.FDay == int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture) && DayFrame() == frame :
                !worldCapture && (scene == "title" || presentationClock) ? display.Frame == frame :
                tactical ? tacticalReady && GameState.AoWHSMap.FCombatMap.Frame == frame :
                GameState.AoWHSMap is { } map && map.FDay == int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture) && map.Frame == frame;
            }
            display.CaptureWhen = AtTarget;
            display.AfterPresent = () =>
            {
                if (AtTarget()) { complete = true; form.Display.StopUpdateFrameLoop(); return; }
                if (applied < clicks.Count)
                {
                    var click = clicks[applied];
                    long scriptFrame = ScriptFrame(click);
                    if (scriptFrame > click.At) throw new InvalidOperationException("Missed scripted frame " + click.At);
                    if (scriptFrame == click.At)
                    {
                        if (saveDirectory is not null)
                            foreach (string saved in Directory.EnumerateFiles(Path.Combine(output, "user", saveDirectory)))
                                if (fileStampUtc is DateTime stamp) File.SetLastWriteTimeUtc(saved, stamp);
                                else File.SetLastWriteTime(saved, now);
                        var manager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
                        if (click.KeyDown is int keyDown)
                            for (int i = 0; i < click.Repeat; i++) form.KeyDown((ushort)keyDown, default);
                        else if (click.Key is int key) { byte character = (byte)key; manager.CheckKeyPress(ref character); }
                        else
                        {
                            manager.CheckMouseMove(default, click.X, click.Y);
                            manager.CheckMouseDown((TMouseButton)click.Button, default, click.X, click.Y);
                            manager.CheckMouseUp((TMouseButton)click.Button, default, click.X, click.Y);
                        }
                        if (click.Double) form.DoubleClick();
                        script.WriteLine(JsonSerializer.Serialize(new { script_frame = scriptFrame, click.X, click.Y, click.Button, click.Double, click.Key, click.KeyDown, click.Repeat, click.Clock, click.Day }));
                        applied++;
                    }
                }
                if (display.Frame >= 10000) throw new TimeoutException("Fixture target not reached");
            };
            form.Run(scene == "title" && !worldCapture && !dayCapture ? frame : 0);
            if (scene == "title" && !worldCapture && !dayCapture) complete = display.Frame == frame;
            if (!complete || applied != clicks.Count) throw new InvalidOperationException("Fixture ended before all inputs and the target frame");
            var taskbar = Aow.Units.AoWExeForms.AoWExeForms.TBWindow;
            var controls = Aow.Units.AOWControlWin.AOWControlWin.ControlWin;
            File.WriteAllText(Path.Combine(output, "controls.json"), JsonSerializer.Serialize(new[] { controls.Unit1Chk.Text.ToString(), controls.Unit2Chk.Text.ToString(), controls.MovesLbl.Text.ToString() }) + "\n");
            var selected = GameState.AoWHSMap?.FSelectedArmy?.GetArmyHS();
            if (GameState.AoWHSMap?.FCombatMap is Aow.Units.AoWTC.TAoWCombatMap combatMap)
            {
                var viewer = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer;
                var units = new List<object>();
                var data = combatMap.FCombat.FCombatData;
                for (int i = 0; i < data.GetCount(); i++)
                    if (data.GetObjects(i) is Aow.Units.AoWTC.TTacticalCombatUnit unit && unit.FUnitHS is { } sprite)
                    {
                        int x = 0, y = 0;
                        Aow.Units.HSEngine.HSEngine.HXtoHP(sprite.GetXhx(), sprite.GetYhx(), ref x, ref y);
                        units.Add(new { unit.FID, unit.FCombatPlayer, unit.FMovePoints, status = unit.FStatus.Image,
                            hits = unit.FUnit.GetHits(), hit_points = unit.FUnit.GetHitPoints(),
                            hx = sprite.GetXhx(), hy = sprite.GetYhx(),
                            screen_x = x - viewer.SceneX + 12, screen_y = y - viewer.SceneY + 12 });
                    }
                File.WriteAllText(Path.Combine(output, "combat.json"), JsonSerializer.Serialize(new
                {
                    combatMap.FRound, combatMap.FCurrentCombatPlayer, combatMap.FSeatedCombatPlayer,
                    units
                }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            }
            File.WriteAllText(Path.Combine(output, "selection.json"), JsonSerializer.Serialize(new
            {
                count = GameState.AoWHSMap?.FSelectedArmy?.FMoveSettingsList?.Count,
                x = selected?.GetXhx(), y = selected?.GetYhx(), level = selected?.GetLhx(), units = selected?.FArmy?.Count
            }) + "\n");
            if (fixture.values.GetValueOrDefault("save_state") == "true")
            {
                (GameState.AoWHSMap ?? throw new InvalidOperationException("State capture requires a map")).FPlayerControl.AutoSave();
                File.Copy(Path.Combine(output, "user", "Save", "autosave.asg"), Path.Combine(output, "state.asg"), true);
            }
            if (fixture.values.TryGetValue("saved_files", out string? savedFiles))
                foreach (string saved in JsonSerializer.Deserialize<string[]>(savedFiles)!)
                    File.Copy(Path.Combine(output, "user", saved), Path.Combine(output, "saved-" + Path.GetFileName(saved)), true);
            File.WriteAllText(Path.Combine(output, "taskbar.json"), JsonSerializer.Serialize(
                new[] { taskbar.WGTLbl, taskbar.WGILbl, taskbar.WMTLbl, taskbar.WMILbl, taskbar.TGTLbl, taskbar.TGILbl, taskbar.TMTLbl, taskbar.TMILbl }
                    .Select(c => new { name = c.Name.ToString(), text = c.Text.ToString(), c.WinLeft, c.WinTop, c.WinWidth, c.WinHeight, c.FVisible,
                        owner = c.FAOWOwner?.Name.ToString(), window = c.FAOWWindow?.Name.ToString() }),
                new JsonSerializerOptions { WriteIndented = true }) + "\n");
            File.WriteAllText(Path.Combine(output, "fixture.json"), JsonSerializer.Serialize(new
            {
                fixture = name, complete, display_presentations = display.Frame,
                map_frame = GameState.AoWHSMap?.Frame, day = GameState.AoWHSMap?.FDay,
                combat_frame = GameState.AoWHSMap?.FCombatMap?.Frame, clicks_applied = applied,
                input_sha256 = input is null ? null : Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(input))),
                local_time = now, file_local_time = fixture.fileLocalTime, file_stamp_utc = fileStampUtc, address_animation_phase = FrameFixtureInputs.AddressAnimationPhase,
                water_variant_phase = FrameFixtureInputs.WaterVariantPhase,
                measured_frame_rate = FrameFixtureInputs.MeasuredFrameRate,
                tactical_ready = tacticalReady, no_window = true, silent = true
            }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            Console.WriteLine($"fixture {name}: map frame {GameState.AoWHSMap?.Frame}, combat frame {GameState.AoWHSMap?.FCombatMap?.Frame}, {display.Frame} presentations, {applied} clicks, {size.X}x{size.Y} RGB555LE, no window, silent");
        }
        finally
        {
            FrameFixtureInputs.AddressAnimationPhase = null;
            FrameFixtureInputs.WaterVariantPhase = null;
            FrameFixtureInputs.Tactical = false;
            FrameFixtureInputs.MeasuredFrameRate = null;
            FrameFixtureInputs.TacticalInitialized = null;
            Aow.Rtl.Rtl.RandomizeSeed = null;
            AppDomain.CurrentDomain.FirstChanceException -= Record;
        }
        return 0;
    }
    private string Get(string key) => values.TryGetValue(key, out string? value) ? value : defaults.TryGetValue(key, out value) ? value : throw new InvalidDataException("Missing fixture field " + key);
    private (int X, int Y) Pair(string key)
    {
        using var json = JsonDocument.Parse(Get(key));
        return (json.RootElement[0].GetInt32(), json.RootElement[1].GetInt32());
    }
    private static FrameFixture Load(string path, string name)
    {
        var result = new FrameFixture();
        string section = "";
        Dictionary<string, string>? current = null;
        var fixtures = new List<Dictionary<string, string>>();
        foreach (string raw in File.ReadLines(path))
        {
            string line = raw.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;
            if (line == "[[fixture]]") { section = "fixture"; current = new(StringComparer.Ordinal); fixtures.Add(current); continue; }
            if (line.StartsWith('[')) { section = line.Trim('[', ']'); continue; }
            int equals = line.IndexOf('=');
            if (equals < 0) throw new InvalidDataException("Invalid fixture line: " + line);
            string key = line[..equals].Trim().Trim('"');
            string value = line[(equals + 1)..].Trim();
            if (section == "defaults") result.defaults.Add(key, value.StartsWith('"') ? JsonSerializer.Deserialize<string>(value)! : value);
            else if (section == "file_clock")
            {
                if (key != "local_time") throw new InvalidDataException("Unknown file clock field " + key);
                result.fileLocalTime = JsonSerializer.Deserialize<string>(value)!;
            }
            else if (section.StartsWith("registry.", StringComparison.Ordinal))
            {
                int integer = int.Parse(value, CultureInfo.InvariantCulture);
                result.settings.Add(@"\Software\Triumph Studios\Age of Wonders\" + section[9..] + "\\" + key, booleans.Contains(key) ? integer != 0 : integer);
            }
            else if (section == "fixture") current!.Add(key, value.StartsWith('"') ? JsonSerializer.Deserialize<string>(value)! : value);
            else throw new InvalidDataException("Unknown fixture section " + section);
        }
        var match = fixtures.SingleOrDefault(f => f.GetValueOrDefault("name") == name) ?? throw new InvalidDataException("Unknown fixture " + name);
        foreach (var item in match) result.values.Add(item.Key, item.Value);

        return result;
    }
    private sealed record Click(int At, int X, int Y, int Button, bool Double, int? Key, string? Clock, int? Day, int? KeyDown, int Repeat);
    private List<Click> Clicks()
    {
        if (!values.TryGetValue("clicks", out string? text)) return [];
        string json = Regex.Replace(text, @"(\w+)\s*=", "\"$1\":");
        using var document = JsonDocument.Parse(json);
        return document.RootElement.EnumerateArray().Select(c => new Click(c.GetProperty("at").GetInt32(),
            c.TryGetProperty("x", out var x) ? x.GetInt32() : 0, c.TryGetProperty("y", out var y) ? y.GetInt32() : 0,
            c.TryGetProperty("button", out var button) ? button.GetInt32() : 0,
            c.TryGetProperty("double", out var twice) && twice.GetBoolean(),
            c.TryGetProperty("key", out var key) ? key.GetInt32() : null,
            c.TryGetProperty("clock", out var clock) ? clock.GetString() : null,
            c.TryGetProperty("day", out var day) ? day.GetInt32() : null,
            c.TryGetProperty("key_down", out var keyDown) ? keyDown.GetInt32() : null,
            c.TryGetProperty("repeat", out var repeat) ? repeat.GetInt32() : 1)).ToList();
    }
    private sealed class FixtureHost : IRtlHost, IDialogService
    {
        public FixtureHost(string root, string user, Dictionary<string, object> settings, IClock clock, string? saveDirectory)
        {
            GameRoot = Path.GetFullPath(root);
            if (saveDirectory is not null) Directory.CreateDirectory(Path.Combine(user, saveDirectory));
            Files = new LayeredFileSystem(GameRoot, user, saveDirectory is null ? [] : [saveDirectory]);
            Clock = clock;
            foreach (var item in settings) Settings.Write(item.Key, item.Value);
        }
        public IFileSystem Files { get; }
        public IDialogService Dialogs => this;
        public IClock Clock { get; }
        public ISettingsStore Settings { get; } = new MemorySettingsStore();
        public string ExeName => Path.Combine(GameRoot, "AoW.exe");
        public string GameRoot { get; }
        public int Show(DialogRequest request) => throw new InvalidOperationException("Fixture dialog: " + request.Text);
    }
}
