using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aow.Game.Engine;
using Aow.Rtl.Localize;

namespace Aow.Units.Tests;

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
                typeof(Game.Magic.Abilities.GameTables), typeof(Game.Magic.Abilities.AbilityRules), typeof(Game.Magic.Abilities.GameText),
                typeof(Game.Magic.Abilities.MovementTables), typeof(Game.Magic.Abilities.CombatAbilityTables), typeof(Game.Magic.Abilities.AbstractSets),
                typeof(Game.Magic.Abilities.AlignmentTables), typeof(Game.Magic.Abilities.EmptySelections),
                typeof(Game.Magic.Abilities.RankTables), typeof(Game.Magic.Abilities.RosterTables),
                typeof(Game.Engine.Nodes.EngineHost), typeof(Game.Engine.Nodes.EngineServices), typeof(AoWEngine),
                typeof(Game.World.Map.HexGeometry), typeof(Game.World.Map.HexText), typeof(Game.World.Map.HSEngine),
                typeof(Game.Draw.Images.ImageLibraries), typeof(Game.Audio.AudioHost),
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
            Game.Engine.Nodes.EngineHost.Engines = new DelphiList();
            Game.Engine.Nodes.EngineHost.PlugIns = new DelphiList();
            Game.Engine.Nodes.EngineHost.Packages = new StringList();
            Game.Engine.Nodes.EngineHost.VerifyEObjectEnabled = true;
            Game.Magic.Abilities.GameTables.AoWEngine = null!;
            AoWHSSet.Current = null!;
            Game.World.Map.AoWHSMap.Current = null!;
            Game.World.Map.HSEngine.HSEngineDID = Game.Magic.Abilities.CombatAbilityTables.CombatDID = Game.Magic.Abilities.AbilityRules.AIDID = Game.Magic.Abilities.AbilityRules.TMPDID = AoWEngine.MAGICDID = 0;
            Game.Draw.Images.ImageLibraries.BaseImageLibraryPath = Game.Audio.AudioHost.BaseSoundEnginePath = "";
            LocalizationRuntime.StartupDirectory = "";
        }

        internal string Relative(string value) => value.Replace(root, "$root", StringComparison.OrdinalIgnoreCase).Replace('\\', '/');
        internal Entry Event(string call, string argument, string[]? destroy = null) => new(call, Relative(argument), Game.Magic.Abilities.GameTables.AoWEngine is not null, AoWHSSet.Current is not null, Game.Engine.Nodes.EngineHost.Engines.Count, destroy ?? []);
        private void Record(string call, string argument) => Events.Add(Event(call, argument));

        internal JsonObject State() => new()
        {
            ["AoWEngine"] = Game.Magic.Abilities.GameTables.AoWEngine is not null,
            ["AoWHSSet"] = AoWHSSet.Current is not null,
            ["HSEngineDID"] = Game.World.Map.HSEngine.HSEngineDID,
            ["CombatDID"] = Game.Magic.Abilities.CombatAbilityTables.CombatDID,
            ["AIDID"] = Game.Magic.Abilities.AbilityRules.AIDID,
            ["TMPDID"] = Game.Magic.Abilities.AbilityRules.TMPDID,
            ["MAGICDID"] = AoWEngine.MAGICDID,
            ["BaseImageLibraryPath"] = Relative(Game.Draw.Images.ImageLibraries.BaseImageLibraryPath),
            ["BaseSoundEnginePath"] = Relative(Game.Audio.AudioHost.BaseSoundEnginePath),
            ["LocalizationStartupDirectory"] = Relative(LocalizationRuntime.StartupDirectory),
            ["Engines"] = Game.Engine.Nodes.EngineHost.Engines.Count
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
            var partial = Game.Magic.Abilities.GameTables.AoWEngine;
            if (partial is not null && partial.FGameSet is null)
            {
                partial.FDictionary?.Dispose();
                partial.FEmailGames?.Dispose();
            }
            Game.Engine.Nodes.EngineHost.Engines?.Dispose(); Game.Engine.Nodes.EngineHost.PlugIns?.Dispose(); Game.Engine.Nodes.EngineHost.Packages?.Dispose();
            foreach (var pair in globals) pair.Key.SetValue(null, pair.Value);
            LocalizationRuntime.StartupDirectory = previousLocalization;
            RestoreServices();
            Directory.Delete(root, true);
        }
    }
}
