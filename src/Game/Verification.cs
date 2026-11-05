using System.Text.RegularExpressions;
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.AoWReg;
using SDL;

namespace Aow.Game;

/// <summary>Package-free unit and install conformance tests for brief 5.1.</summary>
internal static class Verification
{
    private static int assertions;
    private static string scratch = "";
    private static void Check(bool condition, string label)
    {
        assertions++;
        if (!condition) throw new InvalidOperationException("FAIL: " + label);
    }
    private static void Equal<T>(T expected, T actual, string label) => Check(EqualityComparer<T>.Default.Equals(expected, actual), $"{label}: expected {expected}, got {actual}");
    private static void Throws<T>(Action action, string label) where T : System.Exception
    {
        try { action(); }
        catch (T) { assertions++; return; }
        throw new InvalidOperationException("FAIL: " + label);
    }
    public static int Run(string gameRoot, string? sourceRoot)
    {
        scratch = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "obj", "verification", Guid.NewGuid().ToString("N")));
        Directory.CreateDirectory(scratch);
        InputTests();
        SettingsTests();
        FileTests();
        ClockTests();
        FrameVerification.Run(Path.Combine(scratch, "frames"));
        InstallTests(gameRoot, sourceRoot);
        Console.WriteLine($"PASS: {assertions} assertions");
        return 0;
    }
    public static unsafe void QueueClose()
    {
        var ev = new SDL_Event { type = (uint)SDL_EventType.SDL_EVENT_QUIT };
        if (!SDL3.SDL_PushEvent(&ev)) throw new InvalidOperationException(SDL3.SDL_GetError());
    }
    private static void InputTests()
    {
        for (int c = 'a'; c <= 'z'; c++) Equal((ushort)(c - 32), InputMapper.VirtualKey(c, 4 + c - 'a'), "letter virtual key");
        for (int c = 'A'; c <= 'Z'; c++) Equal((ushort)c, InputMapper.VirtualKey(c, 0), "uppercase virtual key");
        for (int c = '0'; c <= '9'; c++) Equal((ushort)c, InputMapper.VirtualKey(c, 0), "digit virtual key");
        (int Scan, ushort Key)[] keys = [(40,13),(41,27),(42,8),(43,9),(44,32),(73,45),(74,36),(75,33),(76,46),(77,35),(78,34),(79,39),(80,37),(81,40),(82,38),
            (45,189),(46,187),(47,219),(48,221),(49,220),(51,186),(52,222),(53,192),(54,188),(55,190),(56,191),
            (57,20),(70,44),(71,145),(72,19),(83,144),(84,111),(85,106),(86,109),(87,107),(88,13),(98,96),(99,110),(100,226),(101,93),
            (224,17),(225,16),(226,18),(227,91),(228,17),(229,16),(230,18),(231,92),
            (155,3),(156,12),(158,13),(163,247),(164,248),(258,95),(267,176),(268,177),(269,178),(271,179),(272,181),
            (280,170),(281,172),(282,166),(283,167),(284,169),(285,168),(286,171)];
        foreach (var (scan, key) in keys) Equal(key, InputMapper.VirtualKey(0, scan), "scancode " + scan);
        for (int n = 0; n < 12; n++) Equal((ushort)(112+n), InputMapper.VirtualKey(0,58+n), "F1-F12");
        for (int n = 0; n < 12; n++) Equal((ushort)(124+n), InputMapper.VirtualKey(0,104+n), "F13-F24");
        for (int n = 0; n < 9; n++) Equal((ushort)(97+n), InputMapper.VirtualKey(0,89+n), "numpad");
        Equal((ushort)0, InputMapper.VirtualKey(0,0), "unknown key");
        var sink = new Events();
        var mapper = new InputMapper();
        foreach (int scan in new[] { 40,42,41,43 }) mapper.Key(0,scan,3|192|768,true,sink);
        Check(sink.Characters.SequenceEqual(new byte[] {13,8,27,9}), "synthesized WM_CHAR controls");
        Check(sink.Shift.Contains(TShiftState.ssShift) && sink.Shift.Contains(TShiftState.ssCtrl) && sink.Shift.Contains(TShiftState.ssAlt), "key modifiers");
        mapper.Key('a',4,0,true,sink);
        mapper.Key('a',4,0,true,sink);
        mapper.Key('a',4,0,false,sink);
        Equal(6, sink.Keys.Count, "repeat delivery");
        Equal((ushort)'A', sink.ReleasedKey, "key release");
        sink.Characters.Clear();
        InputMapper.Text("Aé€Ÿ😀中\r\n",sink);
        Check(sink.Characters.SequenceEqual(new byte[] {65,233,128,159}), "CP1252 text and dropped characters");
        var viewport = DisplayViewport.Fit(640,480,1400,1000);
        Equal((0,0,true),viewport.Map(60,20),"letterbox origin");
        Equal((639,479,true),viewport.Map(1339,979),"last scaled pixel");
        Equal((639,479,true),viewport.Map(1340,980),"bottom right margin maps to the last pixel");
        Equal((0,0,true),viewport.Map(0,0),"top left margin maps to the first pixel");
        Equal((639,200,true),viewport.Map(1399,420),"right margin maps to the right edge");
        Equal((100,0,true),viewport.Map(260,5),"top margin maps to the top edge");
        Equal((-1,-1,false),viewport.Map(1400,500),"right of the output is outside");
        Equal((-1,-1,false),viewport.Map(-1,500),"left of the output is outside");
        Equal((-1,-1,false),viewport.Map(500,1000),"below the output is outside");
        var sizes = Aow.Units.GFXE.GFXE.IntegerScaleSizes(3840,2160);
        Check(sizes.SequenceEqual(new[] {(960,540),(1280,720),(1920,1080),(3840,2160)}), "integer scale sizes of 3840x2160");
        Check(Aow.Units.GFXE.GFXE.IntegerScaleSizes(1920,1200).SequenceEqual(new[] {(960,600),(1920,1200)}), "integer scale sizes of 1920x1200");
        Check(Aow.Units.GFXE.GFXE.IntegerScaleSizes(2560,1440).SequenceEqual(new[] {(853,480),(1280,720),(2560,1440)}), "uneven division rounds down");
        Check(Aow.Units.GFXE.GFXE.IntegerScaleSizes(640,480).SequenceEqual(new[] {(640,480)}), "smallest desktop");
        var fullscreen = DisplayViewport.Fit(1280,720,2560,1440);
        Equal((2,0,0),(fullscreen.Scale,fullscreen.Left,fullscreen.Top),"a listed size fills the desktop at its scale");
        mapper.Motion(60,20,1,viewport,sink);
        mapper.Motion(-5,-5,0,viewport,sink);
        mapper.Motion(-5,-5,0,viewport,sink);
        Equal(2,sink.Moves.Count,"one leave notification");
        Equal((-1,-1),sink.Moves[^1],"leave coordinates");
        var one = DisplayViewport.Fit(640,480,640,480);
        mapper.Reset(); sink.Trace.Clear();
        mapper.SetModifiers(3|192|768);
        mapper.Button(1,true,100,10,10,one,sink);
        mapper.Button(1,false,150,10,10,one,sink);
        mapper.Button(1,true,600,14,14,one,sink);
        Check(sink.Trace.SequenceEqual(new[] {"down","up","double","down"}), "VCL double before second down at threshold");
        Check(sink.Shift.Contains(TShiftState.ssDouble) && sink.Shift.Contains(TShiftState.ssLeft), "double and button shift flags");
        mapper.Button(1,false,601,14,14,one,sink);
        mapper.Button(1,true,602,14,14,one,sink);
        Equal(1,sink.Doubles,"triple click starts a new pair");
        mapper.Reset();
        mapper.Button(1,true,100,10,10,one,sink);
        mapper.Button(1,true,601,10,10,one,sink);
        mapper.Button(1,true,602,15,10,one,sink);
        mapper.Button(3,true,603,15,10,one,sink);
        mapper.Button(2,true,604,15,10,one,sink);
        Equal(1,sink.Doubles,"time distance and different button rejection");
        int count = sink.Trace.Count;
        mapper.Button(2,false,605,15,10,one,sink);
        Equal(count,sink.Trace.Count,"middle ignored");
        mapper.Reset();
        mapper.Button(3,true,10,10,10,one,sink);
        mapper.Button(3,false,20,10,10,one,sink);
        mapper.Button(3,true,30,10,10,one,sink);
        Equal(2,sink.Doubles,"right double click");
        Check(sink.Shift.Contains(TShiftState.ssRight),"right shift state");
        Console.WriteLine("P2 PASS: virtual keys, repeats, controls, CP1252, modifiers, buttons, double clicks and letterbox exits");
    }
    private static void SettingsTests()
    {
        string directory = Path.Combine(scratch,"settings");
        var oldSettings = Aow.Rtl.Rtl.Settings;
        try
        {
            Aow.Rtl.Rtl.Settings = new JsonSettingsStore(directory);
            var registry = new TAoWRegistry();
            Equal(0xffUL,registry.GetClassicWindowPopupEvents().Mask,"classic popup default");
            Equal(0xefUL,registry.GetSimultaneousWindowPopupEvents().Mask,"simultaneous popup default");
            registry.SetClassicWindowPopupEvents(TSet<TWindowPopupEvent>.FromMask(0x81));
            registry.SetSimultaneousWindowPopupEvents(TSet<TWindowPopupEvent>.FromMask(0x42));
            Aow.Rtl.Rtl.Settings = new JsonSettingsStore(directory);
            registry = new TAoWRegistry();
            Equal(0x81UL,registry.GetClassicWindowPopupEvents().Mask,"classic popup persisted byte");
            Equal(0x42UL,registry.GetSimultaneousWindowPopupEvents().Mask,"simultaneous popup persisted byte");
            // Use a fresh store for the complete missing-value battery.
            directory = Path.Combine(scratch,"settings-defaults");
            Aow.Rtl.Rtl.Settings = new JsonSettingsStore(directory);
            registry = new TAoWRegistry();
            Equal(640,registry.GetDisplayWidth(),"missing width default");
            Equal(480,registry.GetDisplayHeight(),"missing height default");
            Equal(50,registry.GetMusicVolume(),"missing volume default");
            Equal(false,registry.GetSkipIntro(),"missing bool default");
            var schemas = new (string Section,string Kind,string[] Names)[]
            {
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
            };
            int count = 0;
            foreach (var (section,kind,names) in schemas)
                foreach (string name in names)
                {
                    switch(kind)
                    {
                        case "integer": int number = 37; registry.rwInteger(section,name,ref number,TReadWriteMode.rwRead); Equal(37,number,"missing integer keeps getter default"); break;
                        case "boolean": bool flag = true; registry.rwBool(section,name,ref flag,TReadWriteMode.rwRead); Equal(true,flag,"missing bool keeps getter default"); break;
                        case "string": string text = "default"; registry.rwString(section,name,ref text,TReadWriteMode.rwRead); Equal("default",text,"missing string keeps getter default"); break;
                        case "rectangle": TRect rect = new(){Left=7,Top=8,Right=9,Bottom=10}; registry.rwRect(section,name,ref rect,TReadWriteMode.rwRead); Equal(7,rect.Left,"missing rectangle keeps getter default"); break;
                        case "bytes": byte[] bytes = [0x5A]; registry.rwBinaryData(section,name,bytes,1,TReadWriteMode.rwRead); Equal((byte)0x5A,bytes[0],"missing bytes keep getter default"); break;
                    }
                }
            foreach (var (section,kind,names) in schemas)
                foreach (string name in names)
                {
                    registry.OpenKey("\\Software\\Triumph Studios\\Age of Wonders\\" + section.Replace('/','\\'),true);
                    switch(kind)
                    {
                        case "integer": registry.WriteInteger(name,-123); break;
                        case "boolean": registry.WriteBool(name,true); break;
                        case "string": registry.WriteString(name,"é test €"); break;
                        case "rectangle": registry.WriteString(name,"-10,20,640,480"); break;
                        case "bytes": registry.WriteBinaryData(name,new byte[] {0xA5},1); break;
                    }
                    registry.CloseKey();
                    count++;
                }
            Aow.Rtl.Rtl.Settings = new JsonSettingsStore(directory);
            registry = new TAoWRegistry();
            foreach (var (section,kind,names) in schemas)
                foreach (string name in names)
                {
                    registry.OpenKey("\\software\\triumph studios\\age of wonders\\" + section.Replace('/','\\').ToLowerInvariant(),false);
                    Check(registry.ValueExists(name.ToUpperInvariant()),"case insensitive registry value");
                    switch(kind)
                    {
                        case "integer": Equal(-123,registry.ReadInteger(name),name); break;
                        case "boolean": Equal(true,registry.ReadBool(name),name); break;
                        case "string": Equal("é test €",registry.ReadString(name),name); break;
                        case "rectangle": Equal("-10,20,640,480",registry.ReadString(name),name); break;
                        case "bytes": byte[] bytes = [0]; Equal(1,registry.ReadBinaryData(name,bytes,1),name); Equal((byte)0xA5,bytes[0],name); break;
                    }
                    registry.CloseKey();
                }
            Equal(-10,registry.GetMainWindowPosition().Left,"transpiled rectangle getter");
            Equal(-123,registry.GetDisplayWidth(),"transpiled integer getter");
            Equal(true,registry.GetMusicEnabled(),"transpiled boolean getter");
            Equal("é test €",registry.GetLocalEmailAddress(),"transpiled string getter");
            Equal((byte)0xA5,(byte)registry.GetMovementSpeed(),"transpiled binary getter");
            var store = (JsonSettingsStore)Aow.Rtl.Rtl.Settings;
            store.Write("Software/Classes/AgeOfWondersMap","ignored");
            Check(store.Read("Software/Classes/AgeOfWondersMap") is null,"association ignored");
            byte[] original = [1,2]; store.Write("General/copy",original); original[0] = 9;
            byte[] read = (byte[])store.Read("General/copy")!; read[0] = 8;
            Equal((byte)1,((byte[])store.Read("General/copy")!)[0],"binary isolation");
            string json = File.ReadAllText(Path.Combine(directory,"settings.json"));
            Check(json.Contains("rectangle") && json.Contains("boolean") && json.Contains("bytes"),"typed JSON kinds");
            Console.WriteLine($"P3 PASS: {count} settings round-trip through TRegistry, typed JSON, defaults and representative transpiled getters");
        }
        finally { Aow.Rtl.Rtl.Settings = oldSettings; }
    }
    private static void Write(IFileSystem files,string name,string value,int mode = Aow.Rtl.Rtl.fmCreate)
    {
        using var stream = files.Open(name,mode);
        using var writer = new StreamWriter(stream);
        writer.Write(value);
    }
    private static string Read(IFileSystem files,string name)
    {
        using var stream = files.Open(name,Aow.Rtl.Rtl.fmOpenRead);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
    private static void FileTests()
    {
        string root = Path.Combine(scratch,"install"), user = Path.Combine(scratch,"user");
        Directory.CreateDirectory(Path.Combine(root,"Images","Scenes"));
        Directory.CreateDirectory(Path.Combine(root,"Scenario","Custom"));
        Directory.CreateDirectory(Path.Combine(root,"Scenario","Campaign"));
        Directory.CreateDirectory(Path.Combine(root,"Int","Scenes"));
        Directory.CreateDirectory(Path.Combine(root,"Songs"));
        File.WriteAllText(Path.Combine(root,"Images","Scenes","Intro.avi"),"root");
        File.WriteAllText(Path.Combine(root,"Images","B.ilb"),"B");
        File.WriteAllText(Path.Combine(root,"Images","a.ilb"),"a");
        File.WriteAllText(Path.Combine(root,"Images","Copy.dat"),"12345");
        var files = new LayeredFileSystem(root,user);
        Equal("root",Read(files,@"IMAGES\scenes/INTRO.AVI"),"mixed case separators");
        Write(files,@"images\SCENES\intro.avi","overlay");
        Equal("overlay",Read(files,@"Images/Scenes/Intro.avi"),"overlay wins");
        Equal("root",File.ReadAllText(Path.Combine(root,"Images","Scenes","Intro.avi")),"root unchanged");
        Equal(1,files.Enumerate(@"Images\Scenes").Count(),"shadow merged once");
        Write(files,@"IMAGES\copy.dat","x",Aow.Rtl.Rtl.fmOpenReadWrite);
        Equal("x2345",Read(files,"images/copy.dat"),"copy on write keeps tail");
        Equal("12345",File.ReadAllText(Path.Combine(root,"Images","Copy.dat")),"copy on write protects install");
        Check(files.DeleteFile(@"Images\Scenes\Intro.avi"),"delete overlay");
        Equal("root",Read(files,@"Images\Scenes\Intro.avi"),"delete reveals root");
        Check(!files.DeleteFile(@"Images\Scenes\Intro.avi"),"root delete refused");
        Write(files,"Images/Z.ilb","Z");
        Check(files.Enumerate("Images").Select(e => e.Name.ToUpperInvariant()).SequenceEqual(new[]{"A.ILB","B.ILB","COPY.DAT","SCENES","Z.ILB"}),"merged NTFS order");
        Write(files,@"Save\a.asg","save");
        Check(files.RenameFile(@"save\A.ASG",@"Save\nested\b.asg"),"rename in user layer");
        Check(!files.RenameFile("Images/B.ilb","Images/c.ilb"),"root rename refused");
        Check(files.FileExists(@"SAVE/nested/B.ASG"),"rename lookup invalidation");
        string processDirectory = Directory.GetCurrentDirectory();
        foreach (string directory in new[]{@"Scenario\Custom","..","Campaign","..","..",@"Int\Scenes","..","..","Songs",".."})
            Check(files.SetCurrentDirectory(directory),"browse " + directory);
        Equal(processDirectory,Directory.GetCurrentDirectory(),"process directory unchanged");
        Equal(root,files.CurrentDirectory,"browse returns to virtual root");
        Check(files.SetCurrentDirectory(files.VirtualDrive + @"\"),"virtual drive");
        Equal(files.VirtualDrive,Aow.Rtl.Rtl.ExtractFileDrive(files.CurrentDirectory).ToString(),"Delphi drive extraction");
        Check(files.SetCurrentDirectory(@"..\.."),"parent at drive root");
        Equal(root,files.CurrentDirectory,"drive root clamp");
        Equal(@"..\Campaign\x.hsm",files.ExtractRelativePath(Path.Combine(root,"Scenario","Custom","base.hsm"),Path.Combine(root,"Scenario","Campaign","x.hsm")),"relative path across siblings");
        Check(!files.SetCurrentDirectory("missing"),"missing directory rejected");
        Check(!files.RemoveDirectory(files.VirtualDrive + @"\"),"virtual drive deletion refused");
        Check(files.CreateDirectory("EmailIn"),"create user directory");
        Check(files.RemoveDirectory("EMAILIN"),"remove user directory");
        Throws<UnauthorizedAccessException>(() => files.Open("Z:\\outside.dat",Aow.Rtl.Rtl.fmCreate),"unknown drive refused");
        Write(files,"../../escape.txt","contained");
        Check(File.Exists(Path.Combine(user,"escape.txt")),"parent traversal contained");
        var old = Aow.Rtl.Rtl.Files;
        Aow.Rtl.Rtl.Files = files;
        try
        {
            Equal(files.VirtualDrive,Aow.Rtl.Rtl.Files.VirtualDrive,"shared virtual drive");
            Equal(@"Scenario\Campaign\x.hsm",Aow.Rtl.Rtl.ExtractRelativePath(
                root + "\\",@"Scenario\Campaign\x.hsm"),"startup directory relative file");
            Equal(@"..\Campaign\",Aow.Rtl.Rtl.ExtractRelativePath(
                @"Scenario\Custom\",@"scenario\Campaign\"),"relative directory keeps separator and ignores case");
            Equal("",Aow.Rtl.Rtl.ExtractRelativePath(
                @"Scenario\Custom\",@"scenario\CUSTOM\"),"same directory is empty");
            Equal(@"..\Campaign\x.hsm",Aow.Rtl.Rtl.ExtractRelativePath(
                @"Scenario\Custom\base.hsm",@"Scenario\Campaign\x.hsm"),"shim relative path");
            var search = new TSearchRec();
            Equal(0,Aow.Rtl.Rtl.FindFirst(@"Images\*.ilb",0,ref search),"shim FindFirst");
            Equal("a.ilb",search.Name,"FindFirst order");
            Aow.Rtl.Rtl.FindClose(ref search);
            int missingDirectoryExceptions = 0;
            void MissingDirectoryException(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs error)
            {
                if (error.Exception is DirectoryNotFoundException) missingDirectoryExceptions++;
            }
            AppDomain.CurrentDomain.FirstChanceException += MissingDirectoryException;
            try
            {
                Equal(0,files.Enumerate("PLUGINS").Count(),"missing directory enumerates no entries");
                Equal(3,Aow.Rtl.Rtl.FindFirst(@"PLUGINS\*.dll",0,ref search),"missing plugin directory returns path not found");
                Equal(6,Aow.Rtl.Rtl.FindNext(ref search),"missing directory leaves no search handle");
                Equal(3,Aow.Rtl.Rtl.FindFirst(@"PLUGINS\*",Aow.Rtl.Rtl.faDirectory,ref search),"missing directory has no synthetic dot entries");
                Equal(0,missingDirectoryExceptions,"missing directory emits no first chance exception");
            }
            finally { AppDomain.CurrentDomain.FirstChanceException -= MissingDirectoryException; }
            Check(Aow.Rtl.Rtl.SetCurrentDir(@"Scenario\Custom"),"shim current directory");
            Equal(Path.Combine(root,"Scenario","Campaign","x.hsm"),Aow.Rtl.Rtl.ExpandFileName(@"..\Campaign\x.hsm").ToString(),"shim expansion");
        }
        finally { Aow.Rtl.Rtl.Files = old; }
        var single = new LayeredFileSystem(root,root);
        Write(single,"temp.txt","same");
        Check(single.DeleteFile("TEMP.TXT"),"single directory delete");
        Check(!single.Enumerate("").Any(e => e.Name == "temp.txt"),"single directory listing refreshed");
        Console.WriteLine("Files PASS: overlay, copy on write, mutation isolation, sorted merge, browse sequence, drive root and shim integration");
    }
    private static void ClockTests()
    {
        var clock = new FakeClock();
        var pacer = new FramePacer(clock);
        clock.Elapsed = 10; pacer.Wait(); Equal(33d,clock.Elapsed,"30 fps deadline");
        clock.Elapsed = 100; pacer.Wait(); Equal(100d,clock.Elapsed,"slow frame no extra wait");
        clock.Elapsed = 105; pacer.Wait(); Equal(133d,clock.Elapsed,"no catch up burst");
        var real = new SdlClock();
        double start = real.ElapsedMilliseconds;
        real.WaitMilliseconds(3);
        Check(real.ElapsedMilliseconds - start >= 3,"precise monotonic wait");
        real.WaitMilliseconds(0); real.WaitMilliseconds(1);
        Throws<ArgumentOutOfRangeException>(() => real.WaitMilliseconds(-1),"negative wait");
        Console.WriteLine("Clock PASS: monotonic waits, yield and pacing without catch up");
    }
    private static string FlipCase(string text) => new(text.Select(c => char.IsUpper(c) ? char.ToLowerInvariant(c) : char.ToUpperInvariant(c)).ToArray());
    private static void InstallTests(string gameRoot,string? sourceRoot)
    {
        var files = new LayeredFileSystem(gameRoot,Path.Combine(scratch,"gog-user"));
        string[] all = Directory.GetFileSystemEntries(gameRoot,"*",SearchOption.AllDirectories);
        foreach (string path in all)
        {
            string relative = Path.GetRelativePath(gameRoot,path);
            string alternate = FlipCase(relative).Replace(Path.DirectorySeparatorChar,'/');
            Check(Directory.Exists(path) ? files.DirectoryExists(alternate) : files.FileExists(alternate),"install lookup " + relative);
            if (!Directory.Exists(path))
            {
                using var stream = files.Open(alternate,Aow.Rtl.Rtl.fmOpenRead);
                Equal(new FileInfo(path).Length,stream.Length,"install open " + relative);
            }
        }
        int literals = 0;
        if (sourceRoot is not null)
        {
            var candidates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var sourceFiles = new[]{"AoW","AoWEPACK","AoWTCPCK","AOWTools","HSEPack","aowInt"}
                .SelectMany(package => Directory.EnumerateFiles(Path.Combine(sourceRoot,package),"*",SearchOption.AllDirectories))
                .Where(path => Path.GetExtension(path).ToLowerInvariant() is ".pas" or ".inc")
                .Append(Path.Combine(sourceRoot,"Engine.pas"));
            foreach (string path in sourceFiles)
                foreach (Match match in Regex.Matches(Cp1252.Decode(File.ReadAllBytes(path)),"'([^'\\r\\n]*\\\\[^'\\r\\n]*)'"))
                    candidates.Add(match.Groups[1].Value);
            foreach (string literal in candidates.Order(StringComparer.Ordinal))
            {
                string part = literal.Trim('\\','/').Replace('\\',Path.DirectorySeparatorChar);
                if (part.Length == 0 || part.Contains(':') || part.IndexOfAny(['*','?']) >= 0) continue;
                var matches = all.Where(p => Path.GetRelativePath(gameRoot,p).Equals(part,StringComparison.OrdinalIgnoreCase) || p.EndsWith(Path.DirectorySeparatorChar + part,StringComparison.OrdinalIgnoreCase)).ToArray();
                if (matches.Length == 0)
                {
                    Console.WriteLine("P4 absent or contextual literal: " + literal);
                    continue;
                }
                foreach (string match in matches)
                {
                    string relative = Path.GetRelativePath(gameRoot,match);
                    Check(Directory.Exists(match) ? files.DirectoryExists(FlipCase(relative)) : files.FileExists(FlipCase(relative)),"literal " + literal);
                }
                literals++;
            }
            Console.WriteLine($"P4 literal scan: {candidates.Count} distinct backslash literals, {literals} present resource suffixes checked");
        }
        Console.WriteLine($"P4 PASS: {all.Length} GOG entries resolve with swapped case and separators, all files open, browse sequence passed");
    }
    private sealed class FakeClock : IClock
    {
        public double Elapsed;
        public double ElapsedMilliseconds => Elapsed;
        public DateTime Now => new(2000,1,1);
        public uint TickCount => (uint)Elapsed;
        public void WaitMilliseconds(int milliseconds) => Elapsed += milliseconds;
    }
    private sealed class Events : IDisplayEvents
    {
        public List<ushort> Keys { get; } = [];
        public List<byte> Characters { get; } = [];
        public List<(int,int)> Moves { get; } = [];
        public List<string> Trace { get; } = [];
        public TSet<TShiftState> Shift;
        public ushort ReleasedKey;
        public int Doubles;
        public void KeyDown(ushort key,TSet<TShiftState> shift) { Keys.Add(key); Shift = shift; }
        public void KeyUp(ushort key,TSet<TShiftState> shift) => ReleasedKey = key;
        public void KeyPress(byte value) => Characters.Add(value);
        public void MouseMove(TSet<TShiftState> shift,int x,int y) { Moves.Add((x,y)); Shift = shift; }
        public void MouseDown(TMouseButton button,TSet<TShiftState> shift,int x,int y) { Trace.Add("down"); Shift = shift; }
        public void MouseUp(TMouseButton button,TSet<TShiftState> shift,int x,int y) { Trace.Add("up"); Shift = shift; }
        public void DoubleClick() { Trace.Add("double"); Doubles++; }
    }
}
