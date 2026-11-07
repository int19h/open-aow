using Aow.Rtl;

namespace Aow.Platform;

public sealed class SdlHost : IRtlHost, IDisposable
{
    public SdlHost(string gameRoot, string? userDirectory = null, int scale = 1, bool? fullscreen = null, bool hidden = false, IDialogService? dialogs = null, bool? vsync = null)
    {
        GameRoot = Path.GetFullPath(gameRoot);
        var files = new LayeredFileSystem(GameRoot, userDirectory ?? DefaultUserDirectory());
        Files = files;
        Settings = new JsonSettingsStore(files.UserDirectory);
        Display = new SdlDisplayHost("OpenAoW", scale: scale, hidden: hidden);
        try
        {
            Display.Fullscreen = fullscreen ?? (Settings.Read(@"\Software\Triumph Studios\Age of Wonders\Video\Display Mode") is int mode ? mode != 0 : true);
            // The port's own setting, absent from the original: on unless it reads 0.
            Display.VSync = vsync ?? (Settings.Read(@"\Software\Triumph Studios\Age of Wonders\Video\VSync") is int interval ? interval != 0 : true);
        }
        catch { Display.Dispose(); throw; }
        Dialogs = dialogs ?? new UnavailableDialogs();
    }
    public static string DefaultUserDirectory()
    {
        if (OperatingSystem.IsWindows()) return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OpenAoW");
        if (OperatingSystem.IsMacOS()) return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Application Support", "OpenAoW");
        string? xdg = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
        return Path.Combine(string.IsNullOrEmpty(xdg) || !Path.IsPathRooted(xdg) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "share") : xdg, "openaow");
    }
    public IFileSystem Files { get; }
    public ISettingsStore Settings { get; }
    public IClock Clock { get; } = new SdlClock();
    public IDialogService Dialogs { get; }
    public SdlDisplayHost Display { get; }
    public string GameRoot { get; }
    public string ExeName => ((LayeredFileSystem)Files).ExpandPath(Path.Combine(GameRoot, "AoW.exe"));
    public void Dispose() => Display.Dispose();
    private sealed class UnavailableDialogs : IDialogService
    {
        public int Show(DialogRequest request) => throw new NotSupportedException("Game dialogs are part of milestone 5.3: " + request.Text);
    }
}
