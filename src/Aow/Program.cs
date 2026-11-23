using Aow.Platform;
using Aow.Platform.Logging;
using Aow.Rtl;
using Aow.Rtl.Host;
using Microsoft.Extensions.Logging;

namespace Aow;

internal static class Program
{
    public static int Main(string[] arguments)
    {
        ProcessSafety.Initialize();
        using ILoggerFactory loggerFactory = HostLogging.Create(LogDirectory(arguments), honorEnvironmentOverrides: !IsToolRun(arguments));
        GlobalExceptionHandlers.Install(loggerFactory);
        RtlHost.LoggerFactory = loggerFactory;
        if (GateSwitches.CultureError("OpenAoW") is { } culture)
        {
            Console.Error.WriteLine(culture);
            return 1;
        }
        GateSwitches.Enter("Game", arguments);
        StreamWriter? fileLog = null;
        try
        {
            if (Value(arguments, name: "--file-log") is { } fileLogPath)
            {
                // own-formats.md 4.2: every open and write at the file boundary, refusals included.
                StreamWriter log = fileLog = new StreamWriter(Path.GetFullPath(fileLogPath), append: false, encoding: new System.Text.UTF8Encoding(false));
                Game.Saves.FileBoundary.Log = record =>
                {
                    log.Write($"{record.ToJson()}\n");
                    log.Flush();
                };
            }
            string root = Value(arguments, name: "--game-root") ?? @"D:\Games\GOG\Age of Wonders";
            if (arguments.Contains("--verify"))
            {
                return Verification.Run(root, sourceRoot: Value(arguments, name: "--source-root"));
            }

            if (arguments.Contains("--verify-frame-inputs"))
            {
                return FrameFixtureChecks.Run(root);
            }

            if (arguments.Contains("--verify-pacing"))
            {
                return PacingVerification.Run(Value(arguments, name: "--out") ?? throw new ArgumentException("--verify-pacing requires --out PATH"));
            }

            if (arguments.Contains("--verify-sdl"))
            {
                return SdlVerification.Run();
            }

            if (arguments.Contains("--smoke-test"))
            {
                return FrameFixture.Run(Value(arguments, name: "--fixtures") ?? @"D:\git\open-aow-private\tests\frames\fixtures.toml", name: "title-60", gameRoot: root,
                    output: Value(arguments, name: "--out") ?? Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "obj", "smoke", Guid.NewGuid().ToString("N")),
                    frameLimit: int.Parse(Value(arguments, name: "--frames") ?? "3"));
            }

            if (Value(arguments, name: "--fixture") is { } fixture)
            {
                return FrameFixture.Run(Value(arguments, name: "--fixtures") ?? @"D:\git\open-aow-private\tests\frames\fixtures.toml", name: fixture, gameRoot: root,
                    output: Value(arguments, name: "--out") ?? throw new ArgumentException("--fixture requires --out PATH"));
            }

            if (arguments.Contains("--help"))
            {
                Console.WriteLine("OpenAoW [--game-root PATH] [--user-data PATH | --single-directory PATH] [--scale N] [--fullscreen] [--no-vsync] [--mute] [--smoke-test] [--frames N] [--file-log PATH]");
                Console.WriteLine("OpenAoW --verify [--game-root PATH] [--source-root RECOVERED_DELPHI_DIRECTORY]");
                Console.WriteLine("OpenAoW --verify-sdl");
                Console.WriteLine("OpenAoW --fixture NAME --fixtures PATH --out PATH [--game-root PATH]");
                Console.WriteLine("OpenAoW --verify-frame-inputs");
                return 0;
            }
            string? single = Value(arguments, name: "--single-directory");
            if (single is not null && Value(arguments, name: "--user-data") is not null)
            {
                throw new ArgumentException("Choose one user data option.");
            }

            if (single is not null)
            {
                root = single;
            }

            int scale = int.Parse(Value(arguments, name: "--scale") ?? "1");
            int frames = int.Parse(Value(arguments, name: "--frames") ?? "0");
            if (frames < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(frames));
            }

            using SdlHost host = new(root, userDirectory: single ?? Value(arguments, name: "--user-data"), scale, fullscreen: arguments.Contains("--fullscreen") ? true : null, vsync: arguments.Contains("--no-vsync") ? false : null, loggerFactory: loggerFactory);
            RtlHost.InstallHost(host);
            using MainForm form = MainForm.Create(host.Display, argument: arguments.FirstOrDefault(MainForm.IsSaveName) ?? "", mute: arguments.Contains("--mute"), audioOutput: true);
            form.Run(frames);
            Console.WriteLine($"Title scene: presented {form.Display.PresentedFrames} frames, mute={arguments.Contains("--mute")}");
            return 0;
        }
        catch (Exception error)
        {
            HostLog.HostError(loggerFactory.CreateLogger("OpenAoW"), error: error, location: "Program.Main");
            return 1;
        }
        finally
        {
            Game.Saves.FileBoundary.Log = null;
            fileLog?.Dispose();
        }
    }

    private static string? Value(string[] arguments, string name)
    {
        int index = Array.IndexOf(arguments, value: name);
        return index < 0 ? null : index + 1 < arguments.Length ? arguments[index + 1] : throw new ArgumentException($"Missing value for {name}");
    }

    private static bool IsToolRun(string[] arguments) => arguments.Contains("--fixture") || arguments.Contains("--smoke-test") || arguments.Any(argument => argument.StartsWith("--verify", comparisonType: StringComparison.Ordinal));

    private static string LogDirectory(string[] arguments)
    {
        try
        {
            string? output = OptionalValue(arguments, name: "--out");
            if (arguments.Contains("--fixture") || arguments.Contains("--smoke-test") || IsToolRun(arguments))
            {
                return output is null ? Path.Combine(AppContext.BaseDirectory, "logs") : Path.GetFullPath(output);
            }

            string? directory = OptionalValue(arguments, name: "--single-directory") ?? OptionalValue(arguments, name: "--user-data");
            return directory is null ? SdlHost.DefaultUserDirectory() : Path.GetFullPath(directory);
        }
        catch
        {
            return SdlHost.DefaultUserDirectory();
        }
    }

    private static string? OptionalValue(string[] arguments, string name)
    {
        int index = Array.IndexOf(arguments, value: name);
        return index >= 0 && index + 1 < arguments.Length ? arguments[index + 1] : null;
    }
}
