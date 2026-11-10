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
                typeof(Aow.Game.Magic.Abilities.GameTables), typeof(Aow.Game.Magic.Abilities.AbilityRules), typeof(Aow.Game.Magic.Abilities.GameText),
                typeof(Aow.Game.Magic.Abilities.MovementTables), typeof(Aow.Game.Magic.Abilities.CombatAbilityTables), typeof(Aow.Game.Magic.Abilities.AbstractSets),
                typeof(Aow.Game.Magic.Abilities.AlignmentTables), typeof(Aow.Game.Magic.Abilities.EmptySelections),
                typeof(Aow.Game.Magic.Abilities.RankTables), typeof(Aow.Game.Magic.Abilities.RosterTables),
                typeof(Aow.Game.Engine.Nodes.EngineHost), typeof(Aow.Game.Engine.Nodes.EngineServices), typeof(Aow.Game.Engine.AoWEngine),
                typeof(Aow.Game.World.Map.HexGeometry), typeof(Aow.Game.World.Map.HexText), typeof(Aow.Game.World.Map.HSEngine),
                typeof(Aow.Game.Draw.Images.ImageLibraries), typeof(Aow.Game.Audio.AudioHost),
            })
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (field.Name == "captureOrdinal") continue;
                    if (!field.IsLiteral && !field.IsInitOnly) globals.Add(field, field.GetValue(null));
                }
            Directory.CreateDirectory(root);
            foreach (string directory in new[] { "IMAGES", "SFX", "PLUGINS", "Dict" }) Directory.CreateDirectory(Path.Combine(root, directory));
            if (fault == "dictionary") File.WriteAllBytes(Path.Combine(root, "Dict", "ResStr.mld"), [0]);
            if (fault == "plugin") File.WriteAllBytes(Path.Combine(root, "PLUGINS", "X.DPL"), []);
            files.SetCurrentDirectory(root);
            Aow.Game.Engine.Nodes.EngineHost.Engines = new DelphiList();
            Aow.Game.Engine.Nodes.EngineHost.PlugIns = new DelphiList();
            Aow.Game.Engine.Nodes.EngineHost.Packages = new StringList();
            Aow.Game.Engine.Nodes.EngineHost.VerifyEObjectEnabled = true;
            Aow.Game.Magic.Abilities.GameTables.AoWEngine = null!;
            Aow.Game.Engine.AoWHSSet.Current = null!;
            Aow.Game.World.Map.AoWHSMap.Current = null!;
            Aow.Game.World.Map.HSEngine.HSEngineDID = Aow.Game.Magic.Abilities.CombatAbilityTables.CombatDID = Aow.Game.Magic.Abilities.AbilityRules.AIDID = Aow.Game.Magic.Abilities.AbilityRules.TMPDID = Aow.Game.Engine.AoWEngine.MAGICDID = 0;
            Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath = Aow.Game.Audio.AudioHost.BaseSoundEnginePath = "";
            LocalizationRuntime.StartupDirectory = "";
        }

        internal string Relative(string value) => value.Replace(root, "$root", StringComparison.OrdinalIgnoreCase).Replace('\\', '/');
        internal Entry Event(string call, string argument, string[]? destroy = null) => new(call, Relative(argument), Aow.Game.Magic.Abilities.GameTables.AoWEngine is not null, Aow.Game.Engine.AoWHSSet.Current is not null, Aow.Game.Engine.Nodes.EngineHost.Engines.Count, destroy ?? []);
        private void Record(string call, string argument) => Events.Add(Event(call, argument));

        internal JsonObject State() => new()
        {
            ["AoWEngine"] = Aow.Game.Magic.Abilities.GameTables.AoWEngine is not null,
            ["AoWHSSet"] = Aow.Game.Engine.AoWHSSet.Current is not null,
            ["HSEngineDID"] = Aow.Game.World.Map.HSEngine.HSEngineDID,
            ["CombatDID"] = Aow.Game.Magic.Abilities.CombatAbilityTables.CombatDID,
            ["AIDID"] = Aow.Game.Magic.Abilities.AbilityRules.AIDID,
            ["TMPDID"] = Aow.Game.Magic.Abilities.AbilityRules.TMPDID,
            ["MAGICDID"] = Aow.Game.Engine.AoWEngine.MAGICDID,
            ["BaseImageLibraryPath"] = Relative(Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath),
            ["BaseSoundEnginePath"] = Relative(Aow.Game.Audio.AudioHost.BaseSoundEnginePath),
            ["LocalizationStartupDirectory"] = Relative(LocalizationRuntime.StartupDirectory),
            ["Engines"] = Aow.Game.Engine.Nodes.EngineHost.Engines.Count
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
            var partial = Aow.Game.Magic.Abilities.GameTables.AoWEngine;
            if (partial is not null && partial.FGameSet is null)
            {
                partial.FDictionary?.Dispose();
                partial.FEmailGames?.Dispose();
            }
            Aow.Game.Engine.Nodes.EngineHost.Engines?.Dispose(); Aow.Game.Engine.Nodes.EngineHost.PlugIns?.Dispose(); Aow.Game.Engine.Nodes.EngineHost.Packages?.Dispose();
            foreach (var pair in globals) pair.Key.SetValue(null, pair.Value);
            LocalizationRuntime.StartupDirectory = previousLocalization;
            RestoreServices();
            Directory.Delete(root, true);
        }
    }
}
