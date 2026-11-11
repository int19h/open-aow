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
        private readonly string fault;
        private readonly string root = Path.Combine(Path.GetTempPath(), "open-aow-engine-" + Guid.NewGuid().ToString("N"));
        private readonly SystemIoFileSystem files = new();
        private readonly Dictionary<FieldInfo, object?> globals = [];
        private readonly IFileSystem previousFiles = Runtime.Files;
        private readonly ISettingsStore? previousSettings = Runtime.Settings;
        private readonly IDialogService? previousDialogs = Runtime.Dialogs;
        private readonly IShellService? previousShell = Runtime.Shell;
        private readonly IClock previousClock = Runtime.Clock;
        private readonly string previousExe = Runtime.Application.ExeName;
        private readonly Action<string> previousMessage = Runtime.MessageHandler;
        private readonly string previousLocalization = LocalizationRuntime.StartupDirectory;
        internal readonly List<Entry> Events = [];

        internal RecordingHost(string fault)
        {
            this.fault = fault;
            foreach (Type type in new[]
            {
                typeof(GameTables), typeof(AbilityRules), typeof(GameText),
                typeof(MovementTables), typeof(CombatAbilityTables), typeof(AbstractSets),
                typeof(AlignmentTables),                 typeof(RankTables), typeof(RosterTables),
                typeof(EngineHost), typeof(EngineServices), typeof(GameEngine),
                typeof(HexGeometry), typeof(HexText),
                typeof(ImageLibraries), typeof(AudioHost),
            })
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!field.IsLiteral && !field.IsInitOnly) globals.Add(field, field.GetValue(null));
                }
            Directory.CreateDirectory(root);
            foreach (string directory in new[] { "IMAGES", "SFX", "PLUGINS", "Dict" }) Directory.CreateDirectory(Path.Combine(root, directory));
            if (fault == "dictionary") File.WriteAllBytes(Path.Combine(root, "Dict", "ResStr.mld"), [0]);
            if (fault == "plugin") File.WriteAllBytes(Path.Combine(root, "PLUGINS", "X.DPL"), []);
            files.SetCurrentDirectory(root);
            EngineHost.Engines = new List<GameEngine>();
            EngineHost.PlugIns = new List<Engine.PlugIn>();
            EngineHost.Packages = new StringList();
            EngineHost.VerifyEObjectEnabled = true;
            GameTables.AoWEngine = null!;
            AoWHSSet.Current = null!;
            AoWHSMap.Current = null!;
            GameEngine.HSEngineDID = CombatAbilityTables.CombatDID = AbilityRules.AIDID = AbilityRules.TMPDID = GameEngine.MAGICDID = 0;
            ImageLibraries.BaseImageLibraryPath = AudioHost.BaseSoundEnginePath = "";
            LocalizationRuntime.StartupDirectory = "";
        }

        internal string Relative(string value) => value.Replace(root, "$root", StringComparison.OrdinalIgnoreCase).Replace('\\', '/');
        internal Entry Event(string call, string argument, string[]? destroy = null) => new(call, Relative(argument), GameTables.AoWEngine is not null, AoWHSSet.Current is not null, EngineHost.Engines.Count, destroy ?? []);
        private void Record(string call, string argument) => Events.Add(Event(call, argument));

        internal JsonObject State() => new()
        {
            ["AoWEngine"] = GameTables.AoWEngine is not null,
            ["AoWHSSet"] = AoWHSSet.Current is not null,
            ["HSEngineDID"] = GameEngine.HSEngineDID,
            ["CombatDID"] = CombatAbilityTables.CombatDID,
            ["AIDID"] = AbilityRules.AIDID,
            ["TMPDID"] = AbilityRules.TMPDID,
            ["MAGICDID"] = GameEngine.MAGICDID,
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
        public IClock Clock => previousClock;
        public string ExeName => Path.Combine(root, "AoW.exe");
        public string GameRoot => root;
        public string CurrentDirectory { get { Record("CurrentDirectory", ""); return files.CurrentDirectory; } }

        public string ExpandPath(string path) { Record("ExpandPath", path); return Checked(path); }

        private string Checked(string path)
        {
            string full = files.ExpandPath(path);
            Assert.True(full.Equals(root, StringComparison.OrdinalIgnoreCase) || full.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Startup escaped temporary root: " + full);
            return full;
        }

        public bool SetCurrentDirectory(string path) { Record("SetCurrentDirectory", path); return files.SetCurrentDirectory(Checked(path)); }

        public Stream Open(string path, int mode) { Record("Open:" + mode, path); return files.Open(Checked(path), mode); }

        public bool FileExists(string path) { Record("FileExists", path); return files.FileExists(Checked(path)); }

        public bool DirectoryExists(string path) { Record("DirectoryExists", path); return files.DirectoryExists(Checked(path)); }

        public bool DeleteFile(string path) { Record("DeleteFile", path); return files.DeleteFile(Checked(path)); }

        public bool RenameFile(string source, string target) { Record("RenameFile", source + " -> " + target); return files.RenameFile(Checked(source), Checked(target)); }

        public bool CreateDirectory(string path) { Record("CreateDirectory", path); return files.CreateDirectory(Checked(path)); }

        public bool RemoveDirectory(string path) { Record("RemoveDirectory", path); return files.RemoveDirectory(Checked(path)); }

        public int GetAttributes(string path) { Record("GetAttributes", path); return files.GetAttributes(Checked(path)); }

        public int SetAttributes(string path, int attributes) { Record("SetAttributes:" + attributes, path); return files.SetAttributes(Checked(path), attributes); }

        public DateTime? LastWriteTime(string path) { Record("LastWriteTime", path); return files.LastWriteTime(Checked(path)); }

        public IEnumerable<RtlFileEntry> Enumerate(string directory)
        {
            Record("Enumerate", directory);
            if (fault == "images" && Checked(directory).TrimEnd('\\', '/').Equals(Path.Combine(root, "IMAGES"), StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("injected IMAGES search failure");
            return files.Enumerate(Checked(directory)).ToArray();
        }

        public object? Read(string key)
        {
            Record("Settings.Read", key);
            if (fault == "debug" && key.EndsWith("\\Debug Mode", StringComparison.Ordinal)) throw new InvalidDataException("injected debug-mode settings failure");
            return null;
        }

        public void Write(string key, object value) => Record("Settings.Write", key + "=" + value);

        public int Show(DialogRequest request) { Record("Dialog", request.Text); return request.DefaultResult; }

        internal void RestoreServices()
        {
            Runtime.Files = previousFiles; Runtime.Settings = previousSettings; Runtime.Dialogs = previousDialogs;
            Runtime.Shell = previousShell;
            Runtime.Clock = previousClock; Runtime.Application.ExeName = previousExe; Runtime.MessageHandler = previousMessage;
        }

        public void Dispose()
        {
            GameEngine partial = GameTables.AoWEngine;
            if (partial is not null && partial.FGameSet is null)
            {
                partial.FDictionary?.Dispose();
                partial.FEmailGames?.Dispose();
            }
            EngineHost.Engines?.Clear(); EngineHost.PlugIns?.Clear(); EngineHost.Packages?.Dispose();
            foreach (var pair in globals) pair.Key.SetValue(null, pair.Value);
            LocalizationRuntime.StartupDirectory = previousLocalization;
            RestoreServices();
            Directory.Delete(root, true);
        }
    }
}
