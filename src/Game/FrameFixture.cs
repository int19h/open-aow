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
    private readonly Dictionary<string, string> defaults = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> values = new(StringComparer.Ordinal);
    private readonly Dictionary<string, object> settings = new(StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> booleans = ["Skip Intro", "Debug Mode", "Allow Resolution Switches", "Music Enabled", "Sound Effects Enabled", "Shadows Enabled", "Surface In VMem"];

    public static int Run(string path, string name, string gameRoot, string output, int? frameLimit = null)
    {
        var fixture = Load(path, name);
        string scene = fixture.Get("scene");
        bool presentationClock = fixture.defaults.GetValueOrDefault("frame_clock") == "presentation" || fixture.values.GetValueOrDefault("frame_clock") == "presentation";
        bool worldCapture = fixture.values.GetValueOrDefault("capture_clock") == "world";
        bool tactical = fixture.values.GetValueOrDefault("action") == "tactical";
        var clicks = fixture.Clicks();
        int frame = frameLimit ?? int.Parse(fixture.Get("frame"), CultureInfo.InvariantCulture);
        if (frame < 1) throw new ArgumentOutOfRangeException(nameof(frameLimit));
        var size = fixture.Pair("size");
        var mouse = fixture.Pair("mouse");
        var now = DateTime.Parse(fixture.Get("local_time"), CultureInfo.InvariantCulture);
        var display = new HeadlessDisplayHost(output, size.X, size.Y, frame) { Mouse = mouse, FixtureFrame = frame };
        Directory.CreateDirectory(output);
        using var trace = new StreamWriter(Path.Combine(output, "exceptions.log")) { NewLine = "\n", AutoFlush = true };
        void Record(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs error) => trace.WriteLine(error.Exception + "\n" + new System.Diagnostics.StackTrace(1, true));
        AppDomain.CurrentDomain.FirstChanceException += Record;
        var host = new FixtureHost(gameRoot, Path.Combine(output, "user"), fixture.settings, new FixtureClock(now));
        Aow.Rtl.Rtl.InstallHost(host);
        Aow.Rtl.Rtl.SuppressRandomize = true;
        Aow.Rtl.Rtl.RandSeed = int.Parse(fixture.Get("seed"), CultureInfo.InvariantCulture);
        Aow.Rtl.Rtl.RandomizeSeed = Aow.Rtl.Rtl.RandSeed;
        FrameFixtureInputs.AddressAnimationPhase = int.Parse(fixture.Get("address_animation_phase"), CultureInfo.InvariantCulture);
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
            File.Copy(input, Path.Combine(output, "user", "fixture.asg"), true);
            argument = Path.Combine(gameRoot, "fixture.asg");
        }
        int applied = 0;
        bool complete = false;
        bool tacticalReady = false;
        using var script = new StreamWriter(Path.Combine(output, "script.jsonl")) { NewLine = "\n", AutoFlush = true };
        try
        {
            using var form = Aow.Units.AoWUnit.TMainForm.Create(display, argument);
            FrameFixtureInputs.TacticalInitialized = () =>
            {
                if (GameState.AoWHSMap?.FCombatMap is not Aow.Units.AoWTC.TAoWCombatMap combat) return;
                combat.FPlaySounds = true;
                combat.SetSeatedCombatPlayer(0);
                Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.MinClick();
                tacticalReady = true;
            };
            bool AtTarget() => !worldCapture && (scene == "title" || presentationClock) ? display.Frame == frame :
                tactical ? tacticalReady && GameState.AoWHSMap.FCombatMap.Frame == frame :
                GameState.AoWHSMap is { } map && map.FDay == int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture) && map.Frame == frame;
            display.CaptureWhen = AtTarget;
            display.AfterPresent = () =>
            {
                if (AtTarget()) { complete = true; form.Display.StopUpdateFrameLoop(); return; }
                if ((presentationClock || GameState.AoWHSMap is not null) && applied < clicks.Count)
                {
                    var click = clicks[applied];
                    bool worldInput = (click.Clock ?? (presentationClock ? "presentation" : "world")) != "presentation";
                    long scriptFrame = !worldInput ? display.Frame : GameState.AoWHSMap?.FDay == int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture) ? GameState.AoWHSMap.Frame : -1;
                    if (scriptFrame > click.At) throw new InvalidOperationException("Missed scripted frame " + click.At);
                    if (scriptFrame == click.At)
                    {
                        var manager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
                        if (click.Key is int key) { byte character = (byte)key; manager.CheckKeyPress(ref character); }
                        else
                        {
                            manager.CheckMouseMove(default, click.X, click.Y);
                            manager.CheckMouseDown((TMouseButton)click.Button, default, click.X, click.Y);
                            manager.CheckMouseUp((TMouseButton)click.Button, default, click.X, click.Y);
                        }
                        if (click.Double) form.DoubleClick();
                        script.WriteLine(JsonSerializer.Serialize(new { script_frame = scriptFrame, click.X, click.Y, click.Button, click.Double }));
                        applied++;
                    }
                }
                if (display.Frame >= 10000) throw new TimeoutException("Fixture target not reached");
            };
            form.Run(scene == "title" && !worldCapture ? frame : 0);
            if (scene == "title" && !worldCapture) complete = display.Frame == frame;
            if (!complete || applied != clicks.Count) throw new InvalidOperationException("Fixture ended before all inputs and the target frame");
            var taskbar = Aow.Units.AoWExeForms.AoWExeForms.TBWindow;
            var controls = Aow.Units.AOWControlWin.AOWControlWin.ControlWin;
            File.WriteAllText(Path.Combine(output, "controls.json"), JsonSerializer.Serialize(new[] { controls.Unit1Chk.Text.ToString(), controls.Unit2Chk.Text.ToString(), controls.MovesLbl.Text.ToString() }) + "\n");
            var selected = GameState.AoWHSMap?.FSelectedArmy?.GetArmyHS();
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
                local_time = now, address_animation_phase = FrameFixtureInputs.AddressAnimationPhase,
                measured_frame_rate = FrameFixtureInputs.MeasuredFrameRate,
                tactical_ready = tacticalReady, no_window = true, silent = true
            }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            Console.WriteLine($"fixture {name}: map frame {GameState.AoWHSMap?.Frame}, combat frame {GameState.AoWHSMap?.FCombatMap?.Frame}, {display.Frame} presentations, {applied} clicks, {size.X}x{size.Y} RGB555LE, no window, silent");
        }
        finally
        {
            FrameFixtureInputs.AddressAnimationPhase = null;
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
    private sealed record Click(int At, int X, int Y, int Button, bool Double, int? Key, string? Clock);
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
            c.TryGetProperty("clock", out var clock) ? clock.GetString() : null)).ToList();
    }
    private sealed class FixtureHost : IRtlHost, IDialogService
    {
        public FixtureHost(string root, string user, Dictionary<string, object> settings, IClock clock)
        {
            GameRoot = Path.GetFullPath(root);
            Files = new LayeredFileSystem(GameRoot, user);
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
