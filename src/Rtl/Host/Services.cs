namespace Aow.Rtl.Host;

public interface IRtlHost
{
    IFileSystem Files { get; }
    IDialogService Dialogs { get; }
    IClock Clock { get; }
    ISettingsStore Settings { get; }
    string ExeName { get; }
    string GameRoot { get; }

    /// <summary>
    /// Whether the game names the documents it writes with the own extensions (own-formats.md
    /// 4.2). The desktop and headless hosts do. A host that replays the original game's scripts
    /// against its frames writes the original extensions.
    /// </summary>
    bool WritesOwnExtensions => true;

    // A host without a desktop refuses the request.
    IShellService Shell => RefusingShell.Instance;
}

/// <summary>The desktop around the game, where the original called ShellExecute.</summary>
public interface IShellService
{
    /// <summary>Opens the address in the user's default browser.</summary>
    void OpenUrl(string url);
}

public sealed class RefusingShell : IShellService
{
    public static readonly RefusingShell Instance = new();

    public void OpenUrl(string url) => throw new InvalidOperationException("No shell service opens " + url);
}

/// <summary>Records each request and opens nothing, for headless runs, fixtures and tests.</summary>
public sealed class RecordingShell : IShellService
{
    public List<string> Urls { get; } = [];

    public void OpenUrl(string url) => Urls.Add(url);
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
