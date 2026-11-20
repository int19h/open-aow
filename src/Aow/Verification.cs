using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using Aow.Game;
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Platform;
using Aow.Rtl;
using Aow.Rtl.Host;
using SDL;

namespace Aow;

/// <summary>Package-free unit and install conformance tests for brief 5.1.</summary>
internal static class Verification
{
    private static int s_assertions;
    private static string s_scratch = "";

    private static void Check(bool condition, string label)
    {
        s_assertions++;
        if (!condition)
        {
            throw new InvalidOperationException($"FAIL: {label}");
        }
    }

    private static void Equal<T>(T expected, T actual, string label) => Check(EqualityComparer<T>.Default.Equals(expected, actual), label: $"{label}: expected {expected}, got {actual}");

    private static void Throws<T>(Action action, string label) where T : Exception
    {
        try
        {
            action();
        }
        catch (T)
        {
            s_assertions++;
            return;
        }
        throw new InvalidOperationException($"FAIL: {label}");
    }

    public static int Run(string gameRoot, string? sourceRoot)
    {
        s_scratch = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "obj", "verification", Guid.NewGuid().ToString("N")));
        _ = Directory.CreateDirectory(s_scratch);
        InputTests();
        SettingsTests();
        FileTests();
        ClockTests();
        FrameVerification.Run(Path.Combine(s_scratch, "frames"));
        InstallTests(gameRoot, sourceRoot);
        Console.WriteLine($"PASS: {s_assertions} assertions");
        return 0;
    }

    public static unsafe void QueueClose()
    {
        SDL_Event ev = new() { type = (uint)SDL_EventType.SDL_EVENT_QUIT };
        if (!SDL3.SDL_PushEvent(&ev))
        {
            throw new InvalidOperationException(SDL3.SDL_GetError());
        }
    }

    private static void InputTests()
    {
        for (int c = 'a'; c <= 'z'; c++)
        {
            Equal((ushort)(c - 32), InputMapper.VirtualKey(c, scancode: 4 + c - 'a'), label: "letter virtual key");
        }

        for (int c = 'A'; c <= 'Z'; c++)
        {
            Equal((ushort)c, InputMapper.VirtualKey(c, scancode: 0), label: "uppercase virtual key");
        }

        for (int c = '0'; c <= '9'; c++)
        {
            Equal((ushort)c, InputMapper.VirtualKey(c, scancode: 0), label: "digit virtual key");
        }

        (int Scan, ushort Key)[] keys = [(40,13),(41,27),(42,8),(43,9),(44,32),(73,45),(74,36),(75,33),(76,46),(77,35),(78,34),(79,39),(80,37),(81,40),(82,38),
            (45,189),(46,187),(47,219),(48,221),(49,220),(51,186),(52,222),(53,192),(54,188),(55,190),(56,191),
            (57,20),(70,44),(71,145),(72,19),(83,144),(84,111),(85,106),(86,109),(87,107),(88,13),(98,96),(99,110),(100,226),(101,93),
            (224,17),(225,16),(226,18),(227,91),(228,17),(229,16),(230,18),(231,92),
            (155,3),(156,12),(158,13),(163,247),(164,248),(258,95),(267,176),(268,177),(269,178),(271,179),(272,181),
            (280,170),(281,172),(282,166),(283,167),(284,169),(285,168),(286,171)];
        foreach ((int scan, ushort key) in keys)
        {
            Equal(key, InputMapper.VirtualKey(0, scancode: scan), label: $"scancode {scan}");
        }

        for (int n = 0; n < 12; n++)
        {
            Equal((ushort)(112 + n), InputMapper.VirtualKey(0, scancode: 58 + n), label: "F1-F12");
        }

        for (int n = 0; n < 12; n++)
        {
            Equal((ushort)(124 + n), InputMapper.VirtualKey(0, scancode: 104 + n), label: "F13-F24");
        }

        for (int n = 0; n < 9; n++)
        {
            Equal((ushort)(97 + n), InputMapper.VirtualKey(0, scancode: 89 + n), label: "numpad");
        }

        Equal((ushort)0, InputMapper.VirtualKey(0, scancode: 0), label: "unknown key");
        Events sink = new();
        InputMapper mapper = new();
        foreach (int scan in new[] { 40, 42, 41, 43 })
        {
            mapper.Key(0, scancode: scan, keyModifiers: 3 | 192 | 768, down: true, sink);
        }

        Check(sink.Characters.SequenceEqual(new byte[] { 13, 8, 27, 9 }), label: "synthesized WM_CHAR controls");
        Check(sink.Shift.Contains(ShiftState.Shift) && sink.Shift.Contains(ShiftState.Ctrl) && sink.Shift.Contains(ShiftState.Alt), label: "key modifiers");
        mapper.Key('a', scancode: 4, keyModifiers: 0, down: true, sink);
        mapper.Key('a', scancode: 4, keyModifiers: 0, down: true, sink);
        mapper.Key('a', scancode: 4, keyModifiers: 0, down: false, sink);
        Equal(6, sink.Keys.Count, label: "repeat delivery");
        Equal('A', sink.ReleasedKey, label: "key release");
        sink.Characters.Clear();
        InputMapper.Text("Aé€Ÿ😀中\r\n", sink);
        Check(sink.Characters.SequenceEqual(new byte[] { 65, 233, 128, 159 }), label: "CP1252 text and dropped characters");
        DisplayViewport viewport = DisplayViewport.Fit(640, 480, 1400, 1000);
        Equal((0, 0, true), viewport.Map(60, 20), label: "letterbox origin");
        Equal((639, 479, true), viewport.Map(1339, 979), label: "last scaled pixel");
        Equal((639, 479, true), viewport.Map(1340, 980), label: "bottom right margin maps to the last pixel");
        Equal((0, 0, true), viewport.Map(0, 0), label: "top left margin maps to the first pixel");
        Equal((639, 200, true), viewport.Map(1399, 420), label: "right margin maps to the right edge");
        Equal((100, 0, true), viewport.Map(260, 5), label: "top margin maps to the top edge");
        Equal((-1, -1, false), viewport.Map(1400, 500), label: "right of the output is outside");
        Equal((-1, -1, false), viewport.Map(-1, 500), label: "left of the output is outside");
        Equal((-1, -1, false), viewport.Map(500, 1000), label: "below the output is outside");
        IReadOnlyList<(int Width, int Height)> sizes = PixelFormats.IntegerScaleSizes(3840, 2160);
        Check(sizes.SequenceEqual(new[] { (960, 540), (1280, 720), (1920, 1080), (3840, 2160) }), label: "integer scale sizes of 3840x2160");
        Check(PixelFormats.IntegerScaleSizes(1920, 1200).SequenceEqual(new[] { (960, 600), (1920, 1200) }), label: "integer scale sizes of 1920x1200");
        Check(PixelFormats.IntegerScaleSizes(2560, 1440).SequenceEqual(new[] { (853, 480), (1280, 720), (2560, 1440) }), label: "uneven division rounds down");
        Check(PixelFormats.IntegerScaleSizes(640, 480).SequenceEqual(new[] { (640, 480) }), label: "smallest desktop");
        DisplayViewport fullscreen = DisplayViewport.Fit(1280, 720, 2560, 1440);
        Equal((2, 0, 0), (fullscreen.Scale, fullscreen.Left, fullscreen.Top), label: "a listed size fills the desktop at its scale");
        mapper.Motion(60, 20, state: 1, viewport, sink);
        mapper.Motion(-5, -5, state: 0, viewport, sink);
        mapper.Motion(-5, -5, state: 0, viewport, sink);
        Equal(2, sink.Moves.Count, label: "one leave notification");
        Equal((-1, -1), sink.Moves[^1], label: "leave coordinates");
        DisplayViewport one = DisplayViewport.Fit(640, 480, 640, 480);
        mapper.Reset();
        sink.ConstructionTrace.Clear();
        mapper.SetModifiers(3 | 192 | 768);
        mapper.Button(1, down: true, milliseconds: 100, 10, 10, viewport: one, sink);
        mapper.Button(1, down: false, milliseconds: 150, 10, 10, viewport: one, sink);
        mapper.Button(1, down: true, milliseconds: 600, 14, 14, viewport: one, sink);
        Check(sink.ConstructionTrace.SequenceEqual(new[] { "down", "up", "double", "down" }), label: "VCL double before second down at threshold");
        Check(sink.Shift.Contains(ShiftState.Double) && sink.Shift.Contains(ShiftState.Left), label: "double and button shift flags");
        mapper.Button(1, down: false, milliseconds: 601, 14, 14, viewport: one, sink);
        mapper.Button(1, down: true, milliseconds: 602, 14, 14, viewport: one, sink);
        Equal(1, sink.Doubles, label: "triple click starts a new pair");
        mapper.Reset();
        mapper.Button(1, down: true, milliseconds: 100, 10, 10, viewport: one, sink);
        mapper.Button(1, down: true, milliseconds: 601, 10, 10, viewport: one, sink);
        mapper.Button(1, down: true, milliseconds: 602, 15, 10, viewport: one, sink);
        mapper.Button(3, down: true, milliseconds: 603, 15, 10, viewport: one, sink);
        mapper.Button(2, down: true, milliseconds: 604, 15, 10, viewport: one, sink);
        Equal(1, sink.Doubles, label: "time distance and different button rejection");
        int count = sink.ConstructionTrace.Count;
        mapper.Button(2, down: false, milliseconds: 605, 15, 10, viewport: one, sink);
        Equal(count, sink.ConstructionTrace.Count, label: "middle ignored");
        mapper.Reset();
        mapper.Button(3, down: true, milliseconds: 10, 10, 10, viewport: one, sink);
        mapper.Button(3, down: false, milliseconds: 20, 10, 10, viewport: one, sink);
        mapper.Button(3, down: true, milliseconds: 30, 10, 10, viewport: one, sink);
        Equal(2, sink.Doubles, label: "right double click");
        Check(sink.Shift.Contains(ShiftState.Right), label: "right shift state");
        Console.WriteLine("P2 PASS: virtual keys, repeats, controls, CP1252, modifiers, buttons, double clicks and letterbox exits");
    }

    private static void SettingsTests()
    {
        string directory = Path.Combine(s_scratch, "settings");
        ISettingsStore? oldSettings = RtlHost.Settings;
        try
        {
            RtlHost.Settings = new JsonSettingsStore(directory);
            GameSettings registry = new();
            Equal(0xffUL, registry.GetClassicWindowPopupEvents().Mask, label: "classic popup default");
            Equal(0xefUL, registry.GetSimultaneousWindowPopupEvents().Mask, label: "simultaneous popup default");
            registry.SetClassicWindowPopupEvents(Set<WindowPopupEvent>.FromMask(0x81));
            registry.SetSimultaneousWindowPopupEvents(Set<WindowPopupEvent>.FromMask(0x42));
            RtlHost.Settings = new JsonSettingsStore(directory);
            registry = new GameSettings();
            Equal(0x81UL, registry.GetClassicWindowPopupEvents().Mask, label: "classic popup persisted byte");
            Equal(0x42UL, registry.GetSimultaneousWindowPopupEvents().Mask, label: "simultaneous popup persisted byte");
            // Use a fresh store for the complete missing-value battery.
            directory = Path.Combine(s_scratch, "settings-defaults");
            RtlHost.Settings = new JsonSettingsStore(directory);
            registry = new GameSettings();
            Equal(640, registry.GetDisplayWidth(), label: "missing width default");
            Equal(480, registry.GetDisplayHeight(), label: "missing height default");
            Equal(50, registry.GetMusicVolume(), label: "missing volume default");
            Equal(false, actual: registry.GetSkipIntro(), label: "missing bool default");
            (string Section, string Kind, string[] Names)[] schemas =
            [
                ("General","boolean",["Debug Mode","Skip Intro","Auto Save","Auto Debug Save","Fast Movement Enabled","Log Token Events"]),
                ("General","integer",["Language"]),
                ("General","string",["Root Directory","Startup Directory","Serial number","D"]),
                ("General","bytes",["Classic Window Popup Events","Simultaneous Window Popup Events","Combat Resolve Mode","Movement Speed"]),
                ("Video","integer",["Display Mode","Display Width","Display Height"]),
                ("Video","boolean",["Allow Resolution Switches","Shadows Enabled","Surface In VMem"]),
                ("Sound","integer",["Sound Quality","Music Volume","SFX Volume"]),
                ("Sound","boolean",["Music Enabled","Sound Effects Enabled","Use DirectSound"]),
                ("Workspace","integer",["Detail Level","Main Window State"]),
                ("Workspace","rectangle",["Main Window Position","Map Window Position"]),
                ("Email","string",["SMTP Server","POP3 Server","POP3 UserName","POP3 Password","Local Email Address","Attachment Directory"]),
                ("Email","integer",["Check Email Frequency"]),
                ("General/rectangles","rectangle",["1"]),("General/booleans","boolean",["1"]),("General/strings","string",["1"]),("General/integers","integer",["1"])
            ];
            int count = 0;
            foreach ((string? section, string? kind, string[]? names) in schemas)
            {
                foreach (string name in names)
                {
                    switch (kind)
                    {
                        case "integer": int number = 37; registry.Int32(section, name, ref number, mode: GameSettings.ReadWriteMode.Read); Equal(37, number, label: "missing integer keeps getter default"); break;
                        case "boolean": bool flag = true; registry.Boolean(section, name, value: ref flag, mode: GameSettings.ReadWriteMode.Read); Equal(true, actual: flag, label: "missing bool keeps getter default"); break;
                        case "string": string text = "default"; registry.String(section, name, ref text, mode: GameSettings.ReadWriteMode.Read); Equal("default", text, label: "missing string keeps getter default"); break;
                        case "rectangle": Rect rectangle = new() { Left = 7, Top = 8, Right = 9, Bottom = 10 }; registry.Rectangle(section, name, ref rectangle, mode: GameSettings.ReadWriteMode.Read); Equal(7, rectangle.Left, label: "missing rectangle keeps getter default"); break;
                        case "bytes": byte[] bytes = [0x5A]; registry.Bytes(section, name, bytes, size: 1, mode: GameSettings.ReadWriteMode.Read); Equal((byte)0x5A, bytes[0], label: "missing bytes keep getter default"); break;
                        default:
                            break;
                    }
                }
            }

            foreach ((string? section, string? kind, string[]? names) in schemas)
            {
                foreach (string name in names)
                {
                    _ = registry.OpenKey($"\\Software\\Triumph Studios\\Age of Wonders\\{section.Replace('/', '\\')}", create: true);
                    switch (kind)
                    {
                        case "integer": registry.WriteInteger(name, -123); break;
                        case "boolean": registry.WriteBool(name, value: true); break;
                        case "string": registry.WriteString(name, "é test €"); break;
                        case "rectangle": registry.WriteString(name, "-10,20,640,480"); break;
                        case "bytes": registry.WriteBinaryData(name, buffer: [0xA5], size: 1); break;
                        default:
                            break;
                    }
                    registry.CloseKey();
                    count++;
                }
            }

            RtlHost.Settings = new JsonSettingsStore(directory);
            registry = new GameSettings();
            foreach ((string? section, string? kind, string[]? names) in schemas)
            {
                foreach (string name in names)
                {
                    _ = registry.OpenKey($"\\software\\triumph studios\\age of wonders\\{section.Replace('/', '\\').ToLowerInvariant()}", create: false);
                    Check(registry.ValueExists(name.ToUpperInvariant()), label: "case insensitive registry value");
                    switch (kind)
                    {
                        case "integer": Equal(-123, registry.ReadInteger(name), label: name); break;
                        case "boolean": Equal(true, actual: registry.ReadBool(name), label: name); break;
                        case "string": Equal("é test €", registry.ReadString(name), label: name); break;
                        case "rectangle": Equal("-10,20,640,480", registry.ReadString(name), label: name); break;
                        case "bytes": byte[] bytes = [0]; Equal(1, registry.ReadBinaryData(name, buffer: bytes, size: 1), label: name); Equal((byte)0xA5, bytes[0], label: name); break;
                        default:
                            break;
                    }
                    registry.CloseKey();
                }
            }

            Equal(-10, registry.GetMainWindowPosition().Left, label: "transpiled rectangle getter");
            Equal(-123, registry.GetDisplayWidth(), label: "transpiled integer getter");
            Equal(true, actual: registry.GetMusicEnabled(), label: "transpiled boolean getter");
            Equal("é test €", registry.GetLocalEmailAddress(), label: "transpiled string getter");
            Equal((byte)0xA5, (byte)registry.GetMovementSpeed(), label: "transpiled binary getter");
            JsonSettingsStore store = (JsonSettingsStore)RtlHost.Settings;
            store.Write("Software/Classes/AgeOfWondersMap", "ignored");
            Check(store.Read("Software/Classes/AgeOfWondersMap") is null, label: "association ignored");
            byte[] original = [1, 2];
            store.Write("General/copy", original);
            original[0] = 9;
            byte[] read = (byte[])store.Read("General/copy")!;
            read[0] = 8;
            Equal((byte)1, ((byte[])store.Read("General/copy")!)[0], label: "binary isolation");
            string json = File.ReadAllText(Path.Combine(directory, "settings.json"));
            Check(json.Contains("rectangle") && json.Contains("boolean") && json.Contains("bytes"), label: "typed JSON kinds");
            Console.WriteLine($"P3 PASS: {count} settings round-trip through TRegistry, typed JSON, defaults and representative transpiled getters");
        }
        finally
        {
            RtlHost.Settings = oldSettings;
        }
    }

    private static void Write(IFileSystem files, string name, string value, FileOpenMode mode = FileOpenMode.Create)
    {
        using Stream stream = files.Open(name, mode: mode);
        using StreamWriter writer = new(stream);
        writer.Write(value);
    }

    private static string Read(IFileSystem files, string name)
    {
        using Stream stream = files.Open(name, mode: FileOpenMode.Read);
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }

    private static void FileTests()
    {
        string root = Path.Combine(s_scratch, "install"), user = Path.Combine(s_scratch, "user");
        _ = Directory.CreateDirectory(Path.Combine(root, "Images", "Scenes"));
        _ = Directory.CreateDirectory(Path.Combine(root, "Scenario", "Custom"));
        _ = Directory.CreateDirectory(Path.Combine(root, "Scenario", "Campaign"));
        _ = Directory.CreateDirectory(Path.Combine(root, "Int", "Scenes"));
        _ = Directory.CreateDirectory(Path.Combine(root, "Songs"));
        File.WriteAllText(Path.Combine(root, "Images", "Scenes", "Intro.avi"), contents: "root");
        File.WriteAllText(Path.Combine(root, "Images", "B.ilb"), contents: "B");
        File.WriteAllText(Path.Combine(root, "Images", "a.ilb"), contents: "a");
        File.WriteAllText(Path.Combine(root, "Images", "Copy.dat"), contents: "12345");
        LayeredFileSystem files = new(root, userDirectory: user);
        Equal("root", Read(files, name: @"IMAGES\scenes/INTRO.AVI"), label: "mixed case separators");
        Write(files, @"images\SCENES\intro.avi", "overlay");
        Equal("overlay", Read(files, name: @"Images/Scenes/Intro.avi"), label: "overlay wins");
        Equal("root", File.ReadAllText(Path.Combine(root, "Images", "Scenes", "Intro.avi")), label: "root unchanged");
        Equal(1, files.Enumerate(@"Images\Scenes").Count(), label: "shadow merged once");
        Write(files, @"IMAGES\copy.dat", "x", mode: FileOpenMode.ReadWrite);
        Equal("x2345", Read(files, name: "images/copy.dat"), label: "copy on write keeps tail");
        Equal("12345", File.ReadAllText(Path.Combine(root, "Images", "Copy.dat")), label: "copy on write protects install");
        Check(files.DeleteFile(@"Images\Scenes\Intro.avi"), label: "delete overlay");
        Equal("root", Read(files, name: @"Images\Scenes\Intro.avi"), label: "delete reveals root");
        Check(!files.DeleteFile(@"Images\Scenes\Intro.avi"), label: "root delete refused");
        Write(files, "Images/Z.ilb", "Z");
        Check(files.Enumerate("Images").Select(e => e.Name.ToUpperInvariant()).SequenceEqual(new[] { "A.ILB", "B.ILB", "COPY.DAT", "SCENES", "Z.ILB" }), label: "merged NTFS order");
        Write(files, @"Save\a.asg", "save");
        Check(files.RenameFile(@"save\A.ASG", target: @"Save\nested\b.asg"), label: "rename in user layer");
        Check(!files.RenameFile("Images/B.ilb", target: "Images/c.ilb"), label: "root rename refused");
        Check(files.FileExists(@"SAVE/nested/B.ASG"), label: "rename lookup invalidation");
        string processDirectory = Directory.GetCurrentDirectory();
        foreach (string directory in new[] { @"Scenario\Custom", "..", "Campaign", "..", "..", @"Int\Scenes", "..", "..", "Songs", ".." })
        {
            Check(files.SetCurrentDirectory(directory), label: $"browse {directory}");
        }

        Equal(processDirectory, Directory.GetCurrentDirectory(), label: "process directory unchanged");
        Equal(root, files.CurrentDirectory, label: "browse returns to virtual root");
        Check(files.SetCurrentDirectory($@"{files.VirtualDrive}\"), label: "virtual drive");
        Equal(files.VirtualDrive, DelphiPaths.ExtractFileDrive(files.CurrentDirectory).ToString(), label: "Delphi drive extraction");
        Check(files.SetCurrentDirectory(@"..\.."), label: "parent at drive root");
        Equal(root, files.CurrentDirectory, label: "drive root clamp");
        Equal(@"..\Campaign\x.hsm", files.ExtractRelativePath(Path.Combine(root, "Scenario", "Custom", "base.hsm"), destination: Path.Combine(root, "Scenario", "Campaign", "x.hsm")), label: "relative path across siblings");
        Check(!files.SetCurrentDirectory("missing"), label: "missing directory rejected");
        Check(!files.RemoveDirectory($@"{files.VirtualDrive}\"), label: "virtual drive deletion refused");
        Check(files.CreateDirectory("EmailIn"), label: "create user directory");
        Check(files.RemoveDirectory("EMAILIN"), label: "remove user directory");
        Throws<UnauthorizedAccessException>(() => files.Open("Z:\\outside.dat", mode: FileOpenMode.Create), label: "unknown drive refused");
        Write(files, "../../escape.txt", "contained");
        Check(File.Exists(Path.Combine(user, "escape.txt")), label: "parent traversal contained");
        IFileSystem old = RtlHost.Files;
        RtlHost.Files = files;
        try
        {
            Equal(files.VirtualDrive, RtlHost.Files.VirtualDrive, label: "shared virtual drive");
            Equal(@"Scenario\Campaign\x.hsm", RtlHost.Files.ExtractRelativePath($"{root}\\", destination: @"Scenario\Campaign\x.hsm"), label: "startup directory relative file");
            Equal(@"..\Campaign\", RtlHost.Files.ExtractRelativePath(@"Scenario\Custom\", destination: @"scenario\Campaign\"), label: "relative directory keeps separator and ignores case");
            Equal("", RtlHost.Files.ExtractRelativePath(@"Scenario\Custom\", destination: @"scenario\CUSTOM\"), label: "same directory is empty");
            Equal(@"..\Campaign\x.hsm", RtlHost.Files.ExtractRelativePath(@"Scenario\Custom\base.hsm", destination: @"Scenario\Campaign\x.hsm"), label: "shim relative path");
            Equal(Path.Combine(root, "Images", "x.ilb"), RtlHost.Files.ExtractRelativePath("", destination: Path.Combine(root, "Images", "x.ilb")), label: "empty base keeps the destination");
            IReadOnlyList<RtlFileEntry> libraries = RtlHost.Files.FindEntries(@"Images\*.ilb", attributes: 0);
            Check(libraries.Count != 0, label: "file search finds the libraries");
            Equal("a.ilb", libraries[0].Name, label: "file search order");
            int missingDirectoryExceptions = 0;
            void MissingDirectoryException(object? sender, FirstChanceExceptionEventArgs error)
            {
                if (error.Exception is DirectoryNotFoundException)
                {
                    missingDirectoryExceptions++;
                }
            }
            AppDomain.CurrentDomain.FirstChanceException += MissingDirectoryException;
            try
            {
                Equal(0, files.Enumerate("PLUGINS").Count(), label: "missing directory enumerates no entries");
                Equal(0, RtlHost.Files.FindEntries(@"PLUGINS\*.dll", attributes: 0).Count, label: "missing plugin directory finds nothing");
                Equal(0, RtlHost.Files.FindEntries(@"PLUGINS\*", attributes: DelphiFiles.faDirectory).Count, label: "missing directory has no synthetic dot entries");
                Equal(0, missingDirectoryExceptions, label: "missing directory emits no first chance exception");
            }
            finally
            {
                AppDomain.CurrentDomain.FirstChanceException -= MissingDirectoryException;
            }
            Check(RtlHost.Files.SetCurrentDirectory(@"Scenario\Custom"), label: "shim current directory");
            Equal(Path.Combine(root, "Scenario", "Campaign", "x.hsm"), DelphiPaths.ExpandFileName(@"..\Campaign\x.hsm").ToString(), label: "shim expansion");
        }
        finally
        {
            RtlHost.Files = old;
        }
        LayeredFileSystem single = new(root, userDirectory: root);
        Write(single, "temp.txt", "same");
        Check(single.DeleteFile("TEMP.TXT"), label: "single directory delete");
        Check(!single.Enumerate("").Any(e => e.Name == "temp.txt"), label: "single directory listing refreshed");
        Console.WriteLine("Files PASS: overlay, copy on write, mutation isolation, sorted merge, browse sequence, drive root and shim integration");
    }

    private static void ClockTests()
    {
        FakeClock clock = new();
        FramePacer pacer = new(clock);
        clock.Elapsed = 10;
        pacer.Wait();
        Equal(33d, clock.Elapsed, label: "30 fps deadline");
        clock.Elapsed = 100;
        pacer.Wait();
        Equal(100d, clock.Elapsed, label: "slow frame no extra wait");
        clock.Elapsed = 105;
        pacer.Wait();
        Equal(133d, clock.Elapsed, label: "no catch up burst");
        SdlClock real = new();
        double start = real.ElapsedMilliseconds;
        real.WaitMilliseconds(3);
        Check(real.ElapsedMilliseconds - start >= 3, label: "precise monotonic wait");
        real.WaitMilliseconds(0);
        real.WaitMilliseconds(1);
        Throws<ArgumentOutOfRangeException>(() => real.WaitMilliseconds(-1), label: "negative wait");
        Console.WriteLine("Clock PASS: monotonic waits, yield and pacing without catch up");
    }

    private static string FlipCase(string text) => new([.. text.Select(c => char.IsUpper(c) ? char.ToLowerInvariant(c) : char.ToUpperInvariant(c))]);

    private static void InstallTests(string gameRoot, string? sourceRoot)
    {
        LayeredFileSystem files = new(gameRoot, userDirectory: Path.Combine(s_scratch, "gog-user"));
        string[] all = Directory.GetFileSystemEntries(gameRoot, searchPattern: "*", searchOption: SearchOption.AllDirectories);
        foreach (string path in all)
        {
            string relative = Path.GetRelativePath(gameRoot, path);
            string alternate = FlipCase(relative).Replace(Path.DirectorySeparatorChar, '/');
            Check(Directory.Exists(path) ? files.DirectoryExists(alternate) : files.FileExists(alternate), label: $"install lookup {relative}");
            if (!Directory.Exists(path))
            {
                using Stream stream = files.Open(alternate, mode: FileOpenMode.Read);
                Equal(new FileInfo(path).Length, stream.Length, label: $"install open {relative}");
            }
        }
        if (sourceRoot is not null)
        {
            HashSet<string> candidates = [with(StringComparer.OrdinalIgnoreCase)];
            IEnumerable<string> sourceFiles = new[] { "AoW", "AoWEPACK", "AoWTCPCK", "AOWTools", "HSEPack", "aowInt" }
                .SelectMany(package => Directory.EnumerateFiles(Path.Combine(sourceRoot, package), searchPattern: "*", searchOption: SearchOption.AllDirectories))
                .Where(path => Path.GetExtension(path).ToLowerInvariant() is ".pas" or ".inc")
                .Append(Path.Combine(sourceRoot, "Engine.pas"));
            foreach (string path in sourceFiles)
            {
                foreach (Match match in Regex.Matches(Cp1252.Decode(File.ReadAllBytes(path)), pattern: "'([^'\\r\\n]*\\\\[^'\\r\\n]*)'"))
                {
                    _ = candidates.Add(match.Groups[1].Value);
                }
            }

            int literals = 0;
            foreach (string literal in candidates.Order(StringComparer.Ordinal))
            {
                string part = literal.Trim('\\', '/').Replace('\\', Path.DirectorySeparatorChar);
                if (part.Length == 0 || part.Contains(':') || part.IndexOfAny(['*', '?']) >= 0)
                {
                    continue;
                }

                string[] matches = [.. all.Where(p => Path.GetRelativePath(gameRoot, path: p).Equals(part, comparisonType: StringComparison.OrdinalIgnoreCase) || p.EndsWith(Path.DirectorySeparatorChar + part, comparisonType: StringComparison.OrdinalIgnoreCase))];
                if (matches.Length == 0)
                {
                    Console.WriteLine($"P4 absent or contextual literal: {literal}");
                    continue;
                }
                foreach (string match in matches)
                {
                    string relative = Path.GetRelativePath(gameRoot, path: match);
                    Check(Directory.Exists(match) ? files.DirectoryExists(FlipCase(relative)) : files.FileExists(FlipCase(relative)), label: $"literal {literal}");
                }
                literals++;
            }
            Console.WriteLine($"P4 literal scan: {candidates.Count} distinct backslash literals, {literals} present resource suffixes checked");
        }
        Console.WriteLine($"P4 PASS: {all.Length} GOG entries resolve with swapped case and separators, all files open, browse sequence passed");
    }

    private sealed class FakeClock : IClock
    {
        public double Elapsed { get; set; }

        public double ElapsedMilliseconds => Elapsed;
        public DateTime Now => new(2000, 1, 1);
        public uint TickCount => (uint)Elapsed;

        public void WaitMilliseconds(int milliseconds) => Elapsed += milliseconds;
    }

    private sealed class Events : IDisplayEvents
    {
        public List<ushort> Keys { get; } = [];
        public List<byte> Characters { get; } = [];
        public List<(int, int)> Moves { get; } = [];
        public List<string> ConstructionTrace { get; } = [];

        public Set<ShiftState> Shift { get; private set; }
        public ushort ReleasedKey { get; private set; }
        public int Doubles { get; private set; }

        public void KeyDown(ushort key, Set<ShiftState> shift)
        {
            Keys.Add(key);
            Shift = shift;
        }

        public void KeyUp(ushort key, Set<ShiftState> shift) => ReleasedKey = key;
        public void KeyPress(byte value) => Characters.Add(value);

        public void MouseMove(Set<ShiftState> shift, int x, int y)
        {
            Moves.Add((x, y));
            Shift = shift;
        }

        public void MouseDown(MouseButton button, Set<ShiftState> shift, int x, int y)
        {
            ConstructionTrace.Add("down");
            Shift = shift;
        }

        public void MouseUp(MouseButton button, Set<ShiftState> shift, int x, int y)
        {
            ConstructionTrace.Add("up");
            Shift = shift;
        }

        public void DoubleClick()
        {
            ConstructionTrace.Add("double");
            Doubles++;
        }
    }
}
