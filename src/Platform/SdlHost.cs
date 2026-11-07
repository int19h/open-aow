using Aow.Rtl;
using Aow.Rtl.Host;

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
        Dialogs = dialogs ?? new DisplayDialogs(Display);
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
    public IShellService Shell { get; } = new SdlShell();
    public SdlDisplayHost Display { get; }
    public string GameRoot { get; }
    public string ExeName => ((LayeredFileSystem)Files).ExpandPath(Path.Combine(GameRoot, "AoW.exe"));

    public void Dispose() => Display.Dispose();

    /// <summary>The default browser through SDL_OpenURL. A failure goes to the error stream, as
    /// the original ignored the result of ShellExecute.</summary>
    private sealed class SdlShell : IShellService
    {
        public void OpenUrl(string url)
        {
            if (!SDL.SDL3.SDL_OpenURL(url)) Console.Error.WriteLine("Open URL failed: " + url + ": " + SDL.SDL3.SDL_GetError());
        }
    }

    /// <summary>Game message boxes on the window. Each text also goes to the error stream, so a
    /// run's log keeps what the player saw.</summary>
    private sealed class DisplayDialogs : IDialogService
    {
        // The game's catch blocks drop the exception and show only a message (as the original
        // did), so the host keeps the last one thrown on each thread for the log.
        [ThreadStatic] private static System.Exception? last;
        private readonly SdlDisplayHost display;

        public DisplayDialogs(SdlDisplayHost display)
        {
            this.display = display;
            AppDomain.CurrentDomain.FirstChanceException += (_, e) => last = e.Exception;
        }

        public int Show(DialogRequest request)
        {
            Console.Error.WriteLine("Message: " + request.Text);
            if (request.Text.StartsWith("Exception", StringComparison.Ordinal) && last is { } error) Console.Error.WriteLine(error);
            return display.ShowMessageBox(request);
        }
    }
}
