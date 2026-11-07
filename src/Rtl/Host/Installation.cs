using Aow.Rtl.Host;

namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static IFileSystem Files { get; set; } = new SystemIoFileSystem();
    public static IClock Clock { get; set; } = new SystemClock();

    public static ISettingsStore? Settings { get; set; }
    public static IDialogService? Dialogs { get; set; }
    public static IShellService? Shell { get; set; }

    /// <summary>The installed host's choice of written extensions (IRtlHost.WritesOwnExtensions).</summary>
    public static bool WritesOwnExtensions { get; set; } = true;

    public static void InstallHost(IRtlHost host)
    {
        Files = host.Files;
        Clock = host.Clock;
        Settings = host.Settings;
        Dialogs = host.Dialogs;
        WritesOwnExtensions = host.WritesOwnExtensions;
        Shell = host.Shell;
        Application.ExeName = host.ExeName;
        MessageHandler = message => host.Dialogs.Show(new(message, "", 0, 1));
    }

    public static void OpenUrl(string url) => (Shell ?? throw new EInvalidOperation("No shell service opens " + url)).OpenUrl(url);

    public static int MessageBox(int handle, string text, string caption, int flags)
    {
        int[] buttons = (flags & 15) switch
        {
            1 => [1, 2],
            2 => [3, 4, 5],
            3 => [6, 7, 2],
            4 => [6, 7],
            5 => [4, 2],
            _ => [1]
        };
        int selected = Math.Min((flags >> 8) & 3, buttons.Length - 1);
        return (Dialogs ?? throw new EInvalidOperation(text)).Show(new(text, caption, flags, buttons[selected]));
    }
}
