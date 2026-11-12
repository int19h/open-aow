using Aow.Platform;

namespace Aow;

internal static class Program
{
    public static int Main(string[] arguments)
    {
        Aow.Rtl.Host.ProcessSafety.Initialize();
        if (Aow.Rtl.Host.GateSwitches.CultureError("OpenAoW") is { } culture)
        {
            Console.Error.WriteLine(culture);
            return 1;
        }
        Aow.Rtl.Host.GateSwitches.Enter("Game", arguments);
        StreamWriter? fileLog = null;
        try
        {
            if (Value(arguments, "--file-log") is { } fileLogPath)
            {
                // own-formats.md 4.2: every open and write at the file boundary, refusals included.
                var log = fileLog = new StreamWriter(Path.GetFullPath(fileLogPath), false, new System.Text.UTF8Encoding(false));
                Game.Saves.FileBoundary.Log = record => { log.Write(record.ToJson() + "\n"); log.Flush(); };
            }
            string root = Value(arguments, "--game-root") ?? @"D:\Games\GOG\Age of Wonders";
            if (arguments.Contains("--verify")) return Verification.Run(root, Value(arguments, "--source-root"));
            if (arguments.Contains("--verify-frame-inputs")) return FrameFixtureChecks.Run(root);
            if (arguments.Contains("--verify-pacing")) return PacingVerification.Run(Value(arguments, "--out") ?? throw new ArgumentException("--verify-pacing requires --out PATH"));
            if (arguments.Contains("--verify-sdl")) return SdlVerification.Run();
            if (arguments.Contains("--smoke-test"))
                return FrameFixture.Run(Value(arguments, "--fixtures") ?? @"D:\git\open-aow-private\tests\frames\fixtures.toml", "title-60", root,
                    Value(arguments, "--out") ?? Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "obj", "smoke", Guid.NewGuid().ToString("N")),
                    int.Parse(Value(arguments, "--frames") ?? "3"));
            if (Value(arguments, "--fixture") is { } fixture)
                return FrameFixture.Run(Value(arguments, "--fixtures") ?? @"D:\git\open-aow-private\tests\frames\fixtures.toml", fixture, root,
                    Value(arguments, "--out") ?? throw new ArgumentException("--fixture requires --out PATH"));
            if (arguments.Contains("--help"))
            {
                Console.WriteLine("OpenAoW [--game-root PATH] [--user-data PATH | --single-directory PATH] [--scale N] [--fullscreen] [--no-vsync] [--mute] [--smoke-test] [--frames N] [--file-log PATH]");
                Console.WriteLine("OpenAoW --verify [--game-root PATH] [--source-root RECOVERED_DELPHI_DIRECTORY]");
                Console.WriteLine("OpenAoW --verify-sdl");
                Console.WriteLine("OpenAoW --fixture NAME --fixtures PATH --out PATH [--game-root PATH]");
                Console.WriteLine("OpenAoW --verify-frame-inputs");
                return 0;
            }
            string? single = Value(arguments, "--single-directory");
            if (single is not null && Value(arguments, "--user-data") is not null) throw new ArgumentException("Choose one user data option.");
            if (single is not null) root = single;
            int scale = int.Parse(Value(arguments, "--scale") ?? "1");
            int frames = int.Parse(Value(arguments, "--frames") ?? "0");
            if (frames < 0) throw new ArgumentOutOfRangeException(nameof(frames));
            using var host = new SdlHost(root, single ?? Value(arguments, "--user-data"), scale, arguments.Contains("--fullscreen") ? true : null, vsync: arguments.Contains("--no-vsync") ? false : null);
            Rtl.RtlUnit.InstallHost(host);
            using MainForm form = MainForm.Create(host.Display, arguments.FirstOrDefault(MainForm.IsSaveName) ?? "", arguments.Contains("--mute"), audioOutput: true);
            form.Run(frames);
            Console.WriteLine($"Title scene: presented {form.Display.PresentedFrames} frames, mute={arguments.Contains("--mute")}");
            return 0;
        }
        catch (System.Exception error) { Console.Error.WriteLine(error.ToString()); return 1; }
        finally
        {
            Game.Saves.FileBoundary.Log = null;
            fileLog?.Dispose();
        }
    }

    private static string? Value(string[] arguments, string name)
    {
        int index = Array.IndexOf(arguments, name);
        return index < 0 ? null : index + 1 < arguments.Length ? arguments[index + 1] : throw new ArgumentException("Missing value for " + name);
    }
}
