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
    internal sealed record Entry(string Call, string Argument, bool Engine, bool Set, int Engines, string[] DestroyFrames);

    internal sealed class RecordingHost : IRtlHost, IFileSystem, ISettingsStore, IDialogService, IDisposable
    {
        private readonly string _fault;
        private readonly string _root = Path.Combine(Path.GetTempPath(), "open-aow-engine-" + Guid.NewGuid().ToString("N"));
        private readonly PhysicalFileSystem _files = new();
        private readonly Dictionary<FieldInfo, object?> _globals = [];
        private readonly IFileSystem _previousFiles = Runtime.Files;
        private readonly ISettingsStore? _previousSettings = Runtime.Settings;
        private readonly IDialogService? _previousDialogs = Runtime.Dialogs;
        private readonly IShellService? _previousShell = Runtime.Shell;
        private readonly IClock _previousClock = Runtime.Clock;
        private readonly string _previousExecutable = Runtime.Application.ExeName;
        private readonly Action<string> _previousMessage = Runtime.MessageHandler;
        private readonly string _previousLocalization = LocalizationRuntime.StartupDirectory;

        internal List<Entry> Events { get; } = [];

        internal RecordingHost(string fault)
        {
            this._fault = fault;
            foreach (Type type in new[]
            {
                typeof(GameTables), typeof(AbilityRules), typeof(GameText),
                typeof(MovementTables), typeof(CombatAbilityTables), typeof(TroopConstants),
                typeof(AlignmentTables),                 typeof(RankTables), typeof(RosterTables),
                typeof(EngineHost), typeof(EngineServices), typeof(GameEngine),
                typeof(HexGeometry), typeof(HexText),
                typeof(ImageLibraries), typeof(AudioHost),
            })
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!field.IsLiteral && !field.IsInitOnly) _globals.Add(field, field.GetValue(null));
                }
            Directory.CreateDirectory(_root);
            foreach (string directory in new[] { "IMAGES", "SFX", "Dict" }) Directory.CreateDirectory(Path.Combine(_root, directory));
            if (fault == "dictionary") File.WriteAllBytes(Path.Combine(_root, "Dict", "ResStr.mld"), [0]);
            _files.SetCurrentDirectory(_root);
            EngineHost.Engines = new List<GameEngine>();
            EngineHost.Plugins = new List<Engine.Plugin>();
            EngineHost.VerifyEngineObjectEnabled = true;
            GameTables.GameEngine = null!;
            GamePack.Current = null!;
            StrategicMap.Current = null!;
            ImageLibraries.BaseImageLibraryPath = AudioHost.BaseSoundEnginePath = "";
            LocalizationRuntime.StartupDirectory = "";
        }

        internal string Relative(string value) => value.Replace(_root, "$root", StringComparison.OrdinalIgnoreCase).Replace('\\', '/');
        internal Entry Event(string call, string argument, string[]? destroy = null) => new(call, Relative(argument), GameTables.GameEngine is not null, GamePack.Current is not null, EngineHost.Engines.Count, destroy ?? []);
        private void Record(string call, string argument) => Events.Add(Event(call, argument));

        internal JsonObject State() => new()
        {
            ["AoWEngine"] = GameTables.GameEngine is not null,
            ["AoWHSSet"] = GamePack.Current is not null,
            ["BaseImageLibraryPath"] = Relative(ImageLibraries.BaseImageLibraryPath),
            ["BaseSoundEnginePath"] = Relative(AudioHost.BaseSoundEnginePath),
            ["LocalizationStartupDirectory"] = Relative(LocalizationRuntime.StartupDirectory),
            ["Engines"] = EngineHost.Engines.Count
        };

        public IFileSystem Files => this;
        public ISettingsStore Settings => this;
        public IDialogService Dialogs => this;
        // Engine tests record startup calls and never open a browser.
        public IShellService Shell { get; } = new RecordingShell();
        public IClock Clock => _previousClock;
        public string ExecutableName => Path.Combine(_root, "AoW.exe");
        public string GameRoot => _root;
        public string CurrentDirectory { get { Record("CurrentDirectory", ""); return _files.CurrentDirectory; } }

        public string ExpandPath(string path) { Record("ExpandPath", path); return Checked(path); }

        private string Checked(string path)
        {
            string full = _files.ExpandPath(path);
            Assert.True(full.Equals(_root, StringComparison.OrdinalIgnoreCase) || full.StartsWith(_root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Startup escaped temporary root: " + full);
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
            if (_fault == "images" && Checked(directory).TrimEnd('\\', '/').Equals(Path.Combine(_root, "IMAGES"), StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("injected IMAGES search failure");
            return _files.Enumerate(Checked(directory)).ToArray();
        }

        public object? Read(string key)
        {
            Record("Settings.Read", key);
            if (_fault == "debug" && key.EndsWith("\\Debug Mode", StringComparison.Ordinal)) throw new InvalidDataException("injected debug-mode settings failure");
            return null;
        }

        public void Write(string key, object value) => Record("Settings.Write", key + "=" + value);

        public int Show(DialogRequest request) { Record("Dialog", request.Text); return request.DefaultResult; }

        internal void RestoreServices()
        {
            Runtime.Files = _previousFiles; Runtime.Settings = _previousSettings; Runtime.Dialogs = _previousDialogs;
            Runtime.Shell = _previousShell;
            Runtime.Clock = _previousClock; Runtime.Application.ExeName = _previousExecutable; Runtime.MessageHandler = _previousMessage;
        }

        public void Dispose()
        {
            GameEngine partial = GameTables.GameEngine;
            if (partial is not null && partial.GameSet is null)
            {
                partial.Dictionary?.Dispose();
                partial.EmailGames?.Dispose();
            }
            EngineHost.Engines?.Clear(); EngineHost.Plugins?.Clear();
            foreach (var pair in _globals) pair.Key.SetValue(null, pair.Value);
            LocalizationRuntime.StartupDirectory = _previousLocalization;
            RestoreServices();
            Directory.Delete(_root, true);
        }
    }
}
