namespace Aow.Rtl;

public interface IRtlHost
{
    IFileSystem Files { get; }
    IDialogService Dialogs { get; }
    IClock Clock { get; }
    ISettingsStore Settings { get; }
    string ExeName { get; }
    string GameRoot { get; }
}

public readonly record struct DialogRequest(string Text, string Caption, int Flags, int DefaultResult);
public interface IDialogService
{
    int Show(DialogRequest request);
}

public interface ISettingsStore
{
    object? Read(string key);
    void Write(string key, object value);
}

public sealed class MemorySettingsStore : ISettingsStore
{
    private readonly Dictionary<string, object> values = new(StringComparer.OrdinalIgnoreCase);
    public object? Read(string key) => values.GetValueOrDefault(key);
    public void Write(string key, object value) => values[key] = value;
}

public static partial class RtlUnit
{
    public static bool SuppressRandomize { get; set; }
    public static ISettingsStore? Settings { get; set; }
    public static IDialogService? Dialogs { get; set; }

    public static void InstallHost(IRtlHost host)
    {
        Files = host.Files;
        Clock = host.Clock;
        Settings = host.Settings;
        Dialogs = host.Dialogs;
        Application.ExeName = host.ExeName;
        MessageHandler = message => host.Dialogs.Show(new(message, "", 0, 1));
    }

    public static int MessageBox(int handle, string text, string caption, int flags)
    {
        int[] buttons = (flags & 15) switch
        {
            1 => [1, 2], 2 => [3, 4, 5], 3 => [6, 7, 2],
            4 => [6, 7], 5 => [4, 2], _ => [1]
        };
        int selected = Math.Min((flags >> 8) & 3, buttons.Length - 1);
        return (Dialogs ?? throw new EInvalidOperation(text)).Show(new(text, caption, flags, buttons[selected]));
    }
}
