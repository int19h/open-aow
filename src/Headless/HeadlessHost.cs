using System.Text.Json;
using Aow.Rtl;

namespace Aow.Headless;

internal sealed class HeadlessHost : IRtlHost, IDialogService, IClock
{
    private readonly string dialogFile;
    public HeadlessHost(string root, string output, string? settings)
    {
        GameRoot = Path.GetFullPath(root);
        FilesForRun = new RunFiles(GameRoot, output);
        dialogFile = Path.Combine(output, "dialogs.jsonl");
        File.WriteAllText(dialogFile, "");
        if (settings is not null)
            foreach (var property in JsonDocument.Parse(File.ReadAllText(settings)).RootElement.EnumerateObject())
                Settings.Write(property.Name.StartsWith('\\') ? property.Name : "\\Software\\Triumph Studios\\Age of Wonders\\" + property.Name.Replace('/', '\\'), ReadSetting(property.Value));
    }
    private static object ReadSetting(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Object)
        {
            string kind = value.GetProperty("kind").GetString() ?? "";
            var data = value.GetProperty("value");
            if (kind == "rectangle" && data.ValueKind == JsonValueKind.Array)
                return (AnsiString)string.Join(",", data.EnumerateArray().Select(v => v.GetInt32()));
            return kind == "bytes" && data.ValueKind == JsonValueKind.String
                ? Convert.FromBase64String(data.GetString()!) : ReadSetting(data);
        }
        return value.ValueKind switch
        {
            JsonValueKind.Number => value.GetInt32(),
            JsonValueKind.True => true, JsonValueKind.False => false,
            JsonValueKind.Array => value.EnumerateArray().Select(v => v.GetByte()).ToArray(),
            JsonValueKind.String => (AnsiString)(value.GetString() ?? ""),
            _ => throw new InvalidDataException("unsupported setting value")
        };
    }
    public RunFiles FilesForRun { get; }
    public IFileSystem Files => FilesForRun;
    public IDialogService Dialogs => this;
    public IClock Clock => this;
    public ISettingsStore Settings { get; } = new MemorySettingsStore();
    public string GameRoot { get; }
    public string ExeName => Path.Combine(GameRoot, "AoW.exe");
    public DateTime Now => new(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public uint TickCount => 0;
    public int DialogCount { get; private set; }
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
    private readonly SystemIoFileSystem files = new();
    private readonly string root, output;
    private string currentDirectory;
    public RunFiles(string gameRoot, string runDirectory)
    {
        root = gameRoot;
        output = Path.GetFullPath(runDirectory);
        currentDirectory = root;
    }
    public string? AutoSaveTarget { get; set; }
    public string CurrentDirectory => currentDirectory;
    public bool SetCurrentDirectory(string path)
    {
        if (!DirectoryExists(path)) return false;
        currentDirectory = ExpandPath(path);
        return true;
    }
    public string ExpandPath(string path) => Path.GetFullPath(path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar), currentDirectory);
    private string? OverlayPath(string full)
    {
        string overlay = Path.GetFullPath(Path.Combine(output, Path.GetRelativePath(root, full)));
        return IsOutput(overlay) ? overlay : null;
    }
    private string ReadPath(string path)
    {
        string full = ExpandPath(path);
        string? overlay = OverlayPath(full);
        return overlay is not null && File.Exists(overlay) ? overlay : full;
    }
    private bool IsOutput(string path) => path.StartsWith(output + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
    private string WritePath(string path)
    {
        string full = ExpandPath(path);
        if (AutoSaveTarget is not null && Path.GetFileName(full).Equals("autosave.asg", StringComparison.OrdinalIgnoreCase)) return AutoSaveTarget;
        if (!IsOutput(full)) full = Path.GetFullPath(Path.Combine(output, Path.GetRelativePath(root, full)));
        if (!IsOutput(full)) throw new UnauthorizedAccessException("write outside run directory: " + path);
        return full;
    }
    public Stream Open(string path, int mode)
    {
        if (mode != Aow.Rtl.Rtl.fmCreate && (mode & 3) == 0) return files.Open(ReadPath(path), mode);
        string target = WritePath(path);
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        return files.Open(target, mode);
    }
    public bool FileExists(string path) => files.FileExists(ReadPath(path));
    public bool DirectoryExists(string path)
    {
        string full = ExpandPath(path);
        return files.DirectoryExists(full) || OverlayPath(full) is { } overlay && files.DirectoryExists(overlay);
    }
    public bool DeleteFile(string path) => files.DeleteFile(WritePath(path));
    public bool RenameFile(string source, string target) => files.RenameFile(WritePath(source), WritePath(target));
    public bool CreateDirectory(string path) { Directory.CreateDirectory(WritePath(path)); return true; }
    public bool RemoveDirectory(string path) => files.RemoveDirectory(WritePath(path));
    public int GetAttributes(string path) => files.GetAttributes(ReadPath(path));
    public int SetAttributes(string path, int attributes) => files.SetAttributes(WritePath(path), attributes);
    public DateTime? LastWriteTime(string path) => files.LastWriteTime(ReadPath(path));
    public IEnumerable<RtlFileEntry> Enumerate(string directory)
    {
        string full = ExpandPath(directory);
        var merged = new Dictionary<string, RtlFileEntry>(StringComparer.OrdinalIgnoreCase);
        foreach (string? layer in new[] { full, OverlayPath(full) })
            if (layer is not null && files.DirectoryExists(layer))
                foreach (var entry in files.Enumerate(layer)) merged[entry.Name] = entry;
        return merged.Values.OrderBy(e => e.Name.ToUpperInvariant(), StringComparer.Ordinal).ThenBy(e => e.Name, StringComparer.Ordinal);
    }
}
