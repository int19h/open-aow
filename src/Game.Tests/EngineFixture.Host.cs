using System.Reflection;

using System.Text.Json.Nodes;
using Aow.Engine.Nodes;

using Aow.Game.Audio;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Rtl.Localize;

namespace Aow.Game.Tests;

public sealed partial class EngineFixture
{
    internal sealed record Entry(string Call, string Argument, bool Set, string[] DestroyFrames);

    internal sealed class RecordingHost : IRtlHost, IFileSystem, ISettingsStore, IDialogService, IDisposable
    {
        private readonly string _fault;
        private readonly PhysicalFileSystem _files = new();
        private readonly Dictionary<FieldInfo, object?> _globals = [];
        private readonly IFileSystem _previousFiles = Runtime.Files;
        private readonly ISettingsStore? _previousSettings = Runtime.Settings;
        private readonly IDialogService? _previousDialogs = Runtime.Dialogs;
        private readonly IShellService? _previousShell = Runtime.Shell;
        private readonly string _previousExecutable = Runtime.Application.ExeName;
        private readonly Action<string> _previousMessage = Runtime.MessageHandler;
        private readonly string _previousLocalization = LocalizationRuntime.StartupDirectory;

        internal List<Entry> Events { get; } = [];

        internal RecordingHost(string fault)
        {
            _fault = fault;
            foreach (Type type in new[]
            {
                typeof(GameTables), typeof(AbilityRules), typeof(GameText),
                typeof(MovementTables), typeof(CombatAbilityTables), typeof(TroopConstants),
                typeof(AlignmentTables),                 typeof(RankTables), typeof(RosterTables),
                typeof(EngineServices), typeof(GameEngine),
                typeof(HexGeometry), typeof(HexText),
                typeof(ImageLibraries), typeof(AudioHost),
            })
            {
                foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!field.IsLiteral && !field.IsInitOnly)
                    {
                        _globals.Add(field, field.GetValue(null));
                    }
                }
            }

            _ = Directory.CreateDirectory(GameRoot);
            foreach (string directory in new[] { "IMAGES", "SFX", "Dict" })
            {
                _ = Directory.CreateDirectory(Path.Combine(GameRoot, directory));
            }

            if (fault == "dictionary")
            {
                File.WriteAllBytes(Path.Combine(GameRoot, "Dict", "ResStr.mld"), [0]);
            }

            _ = _files.SetCurrentDirectory(GameRoot);
            GamePack.Current = null;
            StrategicMap.Current = null;
            ImageLibraries.BaseImageLibraryPath = AudioHost.BaseSoundEnginePath = "";
            LocalizationRuntime.StartupDirectory = "";
        }

        internal string Relative(string value) => value.Replace(GameRoot, "$root", StringComparison.OrdinalIgnoreCase).Replace('\\', '/');
        internal Entry Event(string call, string argument, string[]? destroy = null) => new(call, Relative(argument), GamePack.Current is not null, destroy ?? []);
        private void Record(string call, string argument) => Events.Add(Event(call, argument));

        internal JsonObject State() => new()
        {
            ["AoWHSSet"] = GamePack.Current is not null,
            ["BaseImageLibraryPath"] = Relative(ImageLibraries.BaseImageLibraryPath),
            ["BaseSoundEnginePath"] = Relative(AudioHost.BaseSoundEnginePath),
            ["LocalizationStartupDirectory"] = Relative(LocalizationRuntime.StartupDirectory)
        };

        public IFileSystem Files => this;
        public ISettingsStore Settings => this;
        public IDialogService Dialogs => this;
        // Engine tests record startup calls and never open a browser.
        public IShellService Shell { get; } = new RecordingShell();
        public IClock Clock { get; } = Runtime.Clock;
        public string ExecutableName => Path.Combine(GameRoot, "AoW.exe");
        public string GameRoot { get; } = Path.Combine(Path.GetTempPath(), "open-aow-engine-" + Guid.NewGuid().ToString("N"));
        public string CurrentDirectory { get { Record("CurrentDirectory", ""); return _files.CurrentDirectory; } }

        public string ExpandPath(string path) { Record("ExpandPath", path); return Checked(path); }

        private string Checked(string path)
        {
            string full = _files.ExpandPath(path);
            Assert.True(full.Equals(GameRoot, StringComparison.OrdinalIgnoreCase) || full.StartsWith(GameRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Startup escaped temporary root: " + full);
            return full;
        }

        public bool SetCurrentDirectory(string path) { Record("SetCurrentDirectory", path); return _files.SetCurrentDirectory(Checked(path)); }

        public Stream Open(string path, int mode) { Record("Open:" + mode, path); return _files.Open(Checked(path), mode); }

        public bool FileExists(string path) { Record("FileExists", path); return _files.FileExists(Checked(path)); }

        public bool DirectoryExists(string path) { Record("DirectoryExists", path); return _files.DirectoryExists(Checked(path)); }

        public bool DeleteFile(string path) { Record("DeleteFile", path); return _files.DeleteFile(Checked(path)); }

        public bool RenameFile(string source, string target) { Record("RenameFile", source + " -> " + target); return _files.RenameFile(Checked(source), Checked(target)); }

        public bool CreateDirectory(string path) { Record("CreateDirectory", path); return _files.CreateDirectory(Checked(path)); }

        public bool RemoveDirectory(string path) { Record("RemoveDirectory", path); return _files.RemoveDirectory(Checked(path)); }

        public int GetAttributes(string path) { Record("GetAttributes", path); return _files.GetAttributes(Checked(path)); }

        public int SetAttributes(string path, int attributes) { Record("SetAttributes:" + attributes, path); return _files.SetAttributes(Checked(path), attributes); }

        public DateTime? LastWriteTime(string path) { Record("LastWriteTime", path); return _files.LastWriteTime(Checked(path)); }

        public IEnumerable<RtlFileEntry> Enumerate(string directory)
        {
            Record("Enumerate", directory);
            return _fault == "images" && Checked(directory).TrimEnd('\\', '/').Equals(Path.Combine(GameRoot, "IMAGES"), StringComparison.OrdinalIgnoreCase)
                ? throw new InvalidDataException("injected IMAGES search failure")
                : [.. _files.Enumerate(Checked(directory))];
        }

        public object? Read(string key)
        {
            Record("Settings.Read", key);
            return _fault == "debug" && key.EndsWith("\\Debug Mode", StringComparison.Ordinal)
                ? throw new InvalidDataException("injected debug-mode settings failure")
                : null;
        }

        public void Write(string key, object value) => Record("Settings.Write", key + "=" + value);

        public int Show(DialogRequest request) { Record("Dialog", request.Text); return request.DefaultResult; }

        internal void RestoreServices()
        {
            Runtime.Files = _previousFiles; Runtime.Settings = _previousSettings; Runtime.Dialogs = _previousDialogs;
            Runtime.Shell = _previousShell;
            Runtime.Clock = Clock; Runtime.Application.ExeName = _previousExecutable; Runtime.MessageHandler = _previousMessage;
        }

        public void Dispose()
        {
            foreach (KeyValuePair<FieldInfo, object?> pair in _globals)
            {
                pair.Key.SetValue(null, pair.Value);
            }

            LocalizationRuntime.StartupDirectory = _previousLocalization;
            RestoreServices();
            Directory.Delete(GameRoot, true);
        }
    }
}
