using System.Text.Json;
using Aow.Platform;
using Aow.Rtl;
using Aow.Rtl.Host;

namespace Aow.Headless;

internal sealed class HeadlessHost : IRtlHost, IDialogService, IClock, IShellService
{
    private readonly string dialogFile;

    public HeadlessHost(string root, string output, string? settings, string movementSpeed = "normal", string? input = null)
    {
        GameRoot = Path.GetFullPath(root);
        FilesForRun = new RunFiles(GameRoot, output, input);
        dialogFile = Path.Combine(output, "dialogs.jsonl");
        File.WriteAllText(dialogFile, "");
        File.WriteAllText(Path.Combine(output, "urls.jsonl"), "");
        if (settings is not null)
            foreach (var property in JsonDocument.Parse(File.ReadAllText(settings)).RootElement.EnumerateObject())
                Settings.Write(property.Name.StartsWith('\\') ? property.Name : "\\Software\\Triumph Studios\\Age of Wonders\\" + property.Name.Replace('/', '\\'), ReadSetting(property.Name, property.Value));
        byte speed = movementSpeed switch
        {
            "normal" => 0,
            "fast" => 1,
            "turbo" => 2,
            _ => throw new ArgumentException("movement speed must be normal, fast or turbo")
        };
        Settings.Write(@"\Software\Triumph Studios\Age of Wonders\General\Movement Speed", new byte[] { speed });
    }

    private static object ReadSetting(string key, JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Object)
        {
            string kind = value.GetProperty("kind").GetString() ?? "";
            var data = value.GetProperty("value");
            if (kind == "rectangle" && data.ValueKind == JsonValueKind.Array)
                return string.Join(",", data.EnumerateArray().Select(v => v.GetInt32()));
            return kind == "bytes" && data.ValueKind == JsonValueKind.String
                ? Convert.FromBase64String(data.GetString()!) : ReadSetting(key, data);
        }
        return value.ValueKind switch
        {
            JsonValueKind.Number => value.GetInt32(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Array => value.EnumerateArray().Select(v => v.GetByte()).ToArray(),
            JsonValueKind.String => JsonSettingsStore.ReadText(key, value.GetString() ?? ""),
            _ => throw new InvalidDataException("unsupported setting value")
        };
    }

    public RunFiles FilesForRun { get; }
    public IFileSystem Files => FilesForRun;
    public IDialogService Dialogs => this;
    public IShellService Shell => this;
    public IClock Clock => this;
    public ISettingsStore Settings { get; } = new MemorySettingsStore();
    public string GameRoot { get; }
    public string ExeName => Files.ExpandPath(Path.Combine(GameRoot, "AoW.exe"));
    public DateTime Now => new(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public uint TickCount => 0;
    public double ElapsedMilliseconds => 0;

    public void WaitMilliseconds(int milliseconds) => ArgumentOutOfRangeException.ThrowIfNegative(milliseconds);

    public int DialogCount { get; private set; }

    // A headless run records the address and opens nothing.
    public void OpenUrl(string url)
    {
        File.AppendAllText(Path.Combine(Path.GetDirectoryName(dialogFile)!, "urls.jsonl"), JsonSerializer.Serialize(new { url }) + "\n");
        Console.WriteLine("open url: " + url);
    }

    public int Show(DialogRequest request)
    {
        DialogCount++;
        File.AppendAllText(dialogFile, JsonSerializer.Serialize(new { index = DialogCount, request.Text, request.Caption, request.Flags, result = request.DefaultResult }) + "\n");
        Console.WriteLine($"dialog {DialogCount}: {request.Text} -> {request.DefaultResult}");
        return request.DefaultResult;
    }
}

internal sealed class RunFiles : IFileSystem
{
    private readonly LayeredFileSystem files;
    private readonly string? input;

    public RunFiles(string gameRoot, string runDirectory, string? inputFile = null)
    {
        files = new(gameRoot, runDirectory);
        input = inputFile is null ? null : Path.GetFullPath(inputFile);
    }

    private bool IsInput(string path) => input is not null && Path.IsPathFullyQualified(path) &&
        string.Equals(Path.GetFullPath(path), input, StringComparison.OrdinalIgnoreCase);

    public string? AutoSaveTarget { get; set; }
    public string CurrentDirectory => files.CurrentDirectory;
    public string VirtualDrive => files.VirtualDrive;

    public string ExtractRelativePath(string baseFile, string destination) => files.ExtractRelativePath(baseFile, destination);
    public bool SetCurrentDirectory(string path) => files.SetCurrentDirectory(path);
    public string ExpandPath(string path) => files.ExpandPath(path);

    // The autosave and its replacement temporary go to the day file of the run.
    private string Target(string path)
    {
        if (AutoSaveTarget is null) return path;
        string name = RtlUnit.ExtractFileName(path).ToString();
        // The autosave goes to the day file under either extension, the one this host writes and
        // the original one (own-formats.md 4.2).
        var save = Aow.Formats.DocumentKind.Save;
        foreach (var extension in new[] { save.OwnExtension, save.OriginalExtension })
        {
            string autosave = "autosave" + extension;
            if (name.Equals(autosave, StringComparison.OrdinalIgnoreCase)) return AutoSaveTarget;
            if (name.Equals(Game.Saves.FileBoundary.TemporaryFor(autosave), StringComparison.OrdinalIgnoreCase)) return Game.Saves.FileBoundary.TemporaryFor(AutoSaveTarget);
        }
        return path;
    }

    public Stream Open(string path, int mode)
    {
        bool writing = mode == RtlUnit.fmCreate || (mode & 3) != 0;
        // The command-line save is an explicit read-only input outside the virtual drive.
        if (!writing && IsInput(path)) return File.Open(input!, FileMode.Open, FileAccess.Read, FileShare.Read);
        return files.Open(writing ? Target(path) : path, mode);
    }

    public bool ReplaceFile(string source, string target) => files.ReplaceFile(Target(source), Target(target));
    public string Layer(string path) => IsInput(path) ? "input" : files.Layer(Target(path));
    public bool FileExists(string path) => IsInput(path) ? File.Exists(input) : files.FileExists(Target(path));
    public bool DirectoryExists(string path) => files.DirectoryExists(path);
    public bool DeleteFile(string path) => files.DeleteFile(Target(path));
    public bool RenameFile(string source, string target) => files.RenameFile(source, target);
    public bool CreateDirectory(string path) => files.CreateDirectory(path);
    public bool RemoveDirectory(string path) => files.RemoveDirectory(path);
    public int GetAttributes(string path) => files.GetAttributes(path);
    public int SetAttributes(string path, int attributes) => files.SetAttributes(path, attributes);
    public DateTime? LastWriteTime(string path) => files.LastWriteTime(path);
    public IEnumerable<RtlFileEntry> Enumerate(string directory) => files.Enumerate(directory);
}
