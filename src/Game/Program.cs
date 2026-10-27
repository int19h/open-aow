using Aow.Platform;

namespace Aow.Game;

internal static class Program
{
    public static int Main(string[] args)
    {
        Aow.Rtl.ProcessSafety.Initialize();
        try
        {
            string root = Value(args, "--game-root") ?? @"D:\Games\GOG\Age of Wonders";
            if (args.Contains("--verify")) return Verification.Run(root, Value(args, "--source-root"));
            if (args.Contains("--verify-frame-inputs")) return FrameFixtureChecks.Run();
            if (args.Contains("--verify-pacing")) return PacingVerification.Run(Value(args, "--out") ?? throw new ArgumentException("--verify-pacing requires --out PATH"));
            if (args.Contains("--verify-sdl")) return SdlVerification.Run();
            if (args.Contains("--smoke-test"))
                return FrameFixture.Run(Value(args, "--fixtures") ?? @"D:\git\open-aow-private\tests\frames\fixtures.toml", "title-60", root,
                    Value(args, "--out") ?? Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "obj", "smoke", Guid.NewGuid().ToString("N")),
                    int.Parse(Value(args, "--frames") ?? "3"));
            if (Value(args, "--fixture") is { } fixture)
                return FrameFixture.Run(Value(args, "--fixtures") ?? @"D:\git\open-aow-private\tests\frames\fixtures.toml", fixture, root,
                    Value(args, "--out") ?? throw new ArgumentException("--fixture requires --out PATH"));
            if (args.Contains("--help"))
            {
                Console.WriteLine("OpenAoW [--game-root PATH] [--user-data PATH | --single-directory PATH] [--scale N] [--fullscreen] [--mute] [--smoke-test] [--frames N]");
                Console.WriteLine("OpenAoW --verify [--game-root PATH] [--source-root RECOVERED_DELPHI_DIRECTORY]");
                Console.WriteLine("OpenAoW --verify-sdl");
                Console.WriteLine("OpenAoW --fixture NAME --fixtures PATH --out PATH [--game-root PATH]");
                Console.WriteLine("OpenAoW --verify-frame-inputs");
                return 0;
            }
            string? single = Value(args, "--single-directory");
            if (single is not null && Value(args, "--user-data") is not null) throw new ArgumentException("Choose one user data option.");
            if (single is not null) root = single;
            int scale = int.Parse(Value(args, "--scale") ?? "1");
            int frames = int.Parse(Value(args, "--frames") ?? "0");
            if (frames < 0) throw new ArgumentOutOfRangeException(nameof(frames));
            using var host = new SdlHost(root, single ?? Value(args, "--user-data"), scale, args.Contains("--fullscreen") ? true : null);
            Aow.Rtl.Rtl.InstallHost(host);
            using var form = TMainForm.Create(host.Display, args.FirstOrDefault(a => Path.GetExtension(a).ToLowerInvariant() is ".asg" or ".acg") ?? "", args.Contains("--mute"), audioOutput: true);
            form.Run(frames);
            Console.WriteLine($"Title scene: presented {form.Display.PresentedFrames} frames, mute={args.Contains("--mute")}");
            return 0;
        }
        catch (System.Exception error) { Console.Error.WriteLine(error.ToString()); return 1; }
    }
    private static string? Value(string[] args, string name)
    {
        int index = Array.IndexOf(args, name);
        return index < 0 ? null : index + 1 < args.Length ? args[index + 1] : throw new ArgumentException("Missing value for " + name);
    }
}
