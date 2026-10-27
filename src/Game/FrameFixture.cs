using System.Globalization;
using System.Text.Json;
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
        if (fixture.Get("scene") != "title") throw new NotSupportedException("This rendering mode supports title-scene fixtures.");
        int frame = frameLimit ?? int.Parse(fixture.Get("frame"), CultureInfo.InvariantCulture);
        if (frame < 1) throw new ArgumentOutOfRangeException(nameof(frameLimit));
        var size = fixture.Pair("size");
        var mouse = fixture.Pair("mouse");
        var now = DateTime.Parse(fixture.Get("local_time"), CultureInfo.InvariantCulture);
        var display = new HeadlessDisplayHost(output, size.X, size.Y, frame) { Mouse = mouse };
        Directory.CreateDirectory(output);
        using var trace = new StreamWriter(Path.Combine(output, "exceptions.log")) { NewLine = "\n", AutoFlush = true };
        void Record(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs error) => trace.WriteLine(error.Exception + "\n" + new System.Diagnostics.StackTrace(1, true));
        AppDomain.CurrentDomain.FirstChanceException += Record;
        var host = new FixtureHost(gameRoot, Path.Combine(output, "user"), fixture.settings, new FixtureClock(now));
        Aow.Rtl.Rtl.InstallHost(host);
        Aow.Rtl.Rtl.SuppressRandomize = true;
        Aow.Rtl.Rtl.RandSeed = int.Parse(fixture.Get("seed"), CultureInfo.InvariantCulture);
        try
        {
            using var form = Aow.Units.AoWUnit.TMainForm.Create(display);
            form.Run(frame);
            Console.WriteLine($"title fixture {name}: {form.Layouts.Modules.Count} modules, {form.Layouts.PendingFixups} pending fixups, {display.Frame} frames, {size.X}x{size.Y} RGB555LE, no window, silent");
        }
        finally { AppDomain.CurrentDomain.FirstChanceException -= Record; }
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
        if (result.values.ContainsKey("clicks") || result.values.ContainsKey("action") || result.values.ContainsKey("save")) throw new NotSupportedException("Scripted game fixtures belong to the game-frame milestone.");
        return result;
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
