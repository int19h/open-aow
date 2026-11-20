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
        private readonly IFileSystem _previousFiles = RtlHost.Files;
        private readonly ISettingsStore? _previousSettings = RtlHost.Settings;
        private readonly IDialogService? _previousDialogs = RtlHost.Dialogs;
        private readonly IShellService? _previousShell = RtlHost.Shell;
        private readonly string _previousExecutable = LegacyApplication.Application.ExeName;
        private readonly Action<string> _previousMessage = LegacyApplication.MessageHandler;
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
                File.WriteAllBytes(Path.Combine(GameRoot, "Dict", "ResStr.mld"), bytes: [0]);
            }

            _ = _files.SetCurrentDirectory(GameRoot);
            GamePack.Current = null;
            StrategicMap.Current = null;
            ImageLibraries.BaseImageLibraryPath = AudioHost.BaseSoundEnginePath = "";
            LocalizationRuntime.StartupDirectory = "";
        }

        internal string Relative(string value) => value.Replace(GameRoot, "$root", comparisonType: StringComparison.OrdinalIgnoreCase).Replace('\\', '/');
        internal Entry Event(string call, string argument, string[]? destroy = null) => new(call, Argument: Relative(argument), Set: GamePack.Current is not null, DestroyFrames: destroy ?? []);
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
        public IClock Clock { get; } = RtlHost.Clock;
        public string ExecutableName => Path.Combine(GameRoot, "AoW.exe");
        public string GameRoot { get; } = Path.Combine(Path.GetTempPath(), $"open-aow-engine-{Guid.NewGuid():N}");

        public string CurrentDirectory
        {
            get
            {
                Record("CurrentDirectory", argument: "");
                return _files.CurrentDirectory;
            }
        }

        public string ExpandPath(string path)
        {
            Record("ExpandPath", argument: path);
            return Checked(path);
        }

        private string Checked(string path)
        {
            string full = _files.ExpandPath(path);
            Assert.True(full.Equals(GameRoot, comparisonType: StringComparison.OrdinalIgnoreCase) || full.StartsWith(GameRoot + Path.DirectorySeparatorChar, comparisonType: StringComparison.OrdinalIgnoreCase), userMessage: $"Startup escaped temporary root: {full}");
            return full;
        }

        public bool SetCurrentDirectory(string path)
        {
            Record("SetCurrentDirectory", argument: path);
            return _files.SetCurrentDirectory(Checked(path));
        }

        public Stream Open(string path, int mode)
        {
            Record($"Open:{mode}", argument: path);
            return _files.Open(Checked(path), mode);
        }

        public bool FileExists(string path)
        {
            Record("FileExists", argument: path);
            return _files.FileExists(Checked(path));
        }

        public bool DirectoryExists(string path)
        {
            Record("DirectoryExists", argument: path);
            return _files.DirectoryExists(Checked(path));
        }

        public bool DeleteFile(string path)
        {
            Record("DeleteFile", argument: path);
            return _files.DeleteFile(Checked(path));
        }

        public bool RenameFile(string source, string target)
        {
            Record("RenameFile", argument: $"{source} -> {target}");
            return _files.RenameFile(Checked(source), target: Checked(target));
        }

        public bool CreateDirectory(string path)
        {
            Record("CreateDirectory", argument: path);
            return _files.CreateDirectory(Checked(path));
        }

        public bool RemoveDirectory(string path)
        {
            Record("RemoveDirectory", argument: path);
            return _files.RemoveDirectory(Checked(path));
        }

        public int GetAttributes(string path)
        {
            Record("GetAttributes", argument: path);
            return _files.GetAttributes(Checked(path));
        }

        public int SetAttributes(string path, int attributes)
        {
            Record($"SetAttributes:{attributes}", argument: path);
            return _files.SetAttributes(Checked(path), attributes);
        }

        public DateTime? LastWriteTime(string path)
        {
            Record("LastWriteTime", argument: path);
            return _files.LastWriteTime(Checked(path));
        }

        public IEnumerable<RtlFileEntry> Enumerate(string directory)
        {
            Record("Enumerate", argument: directory);
            return _fault == "images" && Checked(directory).TrimEnd('\\', '/').Equals(Path.Combine(GameRoot, "IMAGES"), comparisonType: StringComparison.OrdinalIgnoreCase)
                ? throw new InvalidDataException("injected IMAGES search failure")
                : [.. _files.Enumerate(Checked(directory))];
        }

        public object? Read(string key)
        {
            Record("Settings.Read", argument: key);
            return _fault == "debug" && key.EndsWith("\\Debug Mode", comparisonType: StringComparison.Ordinal)
                ? throw new InvalidDataException("injected debug-mode settings failure")
                : null;
        }

        public void Write(string key, object value) => Record("Settings.Write", argument: $"{key}={value}");

        public int Show(DialogRequest request)
        {
            Record("Dialog", argument: request.Text);
            return request.DefaultResult;
        }

        internal void RestoreServices()
        {
            RtlHost.Files = _previousFiles;
            RtlHost.Settings = _previousSettings;
            RtlHost.Dialogs = _previousDialogs;
            RtlHost.Shell = _previousShell;
            RtlHost.Clock = Clock;
            LegacyApplication.Application.ExeName = _previousExecutable;
            LegacyApplication.MessageHandler = _previousMessage;
        }

        public void Dispose()
        {
            foreach (KeyValuePair<FieldInfo, object?> pair in _globals)
            {
                pair.Key.SetValue(null, pair.Value);
            }

            LocalizationRuntime.StartupDirectory = _previousLocalization;
            RestoreServices();
            Directory.Delete(GameRoot, recursive: true);
        }
    }
}
