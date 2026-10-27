using Aow.Platform;
using Aow.Units.GFXE;

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
            if (args.Contains("--verify-sdl")) return SdlVerification.Run();
            if (args.Contains("--help"))
            {
                Console.WriteLine("OpenAoW [--game-root PATH] [--user-data PATH | --single-directory PATH] [--scale N] [--fullscreen] [--smoke-test] [--frames N]");
                Console.WriteLine("OpenAoW --verify [--game-root PATH] [--source-root RECOVERED_DELPHI_DIRECTORY]");
                Console.WriteLine("OpenAoW --verify-sdl");
                return 0;
            }
            string? single = Value(args, "--single-directory");
            if (single is not null && Value(args, "--user-data") is not null) throw new ArgumentException("Choose one user data option.");
            if (single is not null) root = single;
            int scale = int.Parse(Value(args, "--scale") ?? "1");
            bool smoke = args.Contains("--smoke-test");
            int frames = int.Parse(Value(args, "--frames") ?? (smoke ? "3" : "0"));
            if (frames < 0) throw new ArgumentOutOfRangeException(nameof(frames));
            using var host = new SdlHost(root, single ?? Value(args, "--user-data"), scale, args.Contains("--fullscreen"), hidden: smoke);
            Aow.Rtl.Rtl.InstallHost(host);
            var sink = new WindowEvents();
            byte[] pixels = new byte[640 * 480 * 2];
            var surface = new TSurfaceDesc { Width = 640, Height = 480, BitCount = 16, PixelFormatID = GFXE.pfRGB555, Pitch = 640 * 2, Data = pixels };
            var pacer = new FramePacer(host.Clock);
            int presented = 0;
            for (int frame = 0; !host.Display.IsClosed; frame++)
            {
                if (smoke && frame == frames) Verification.QueueClose();
                host.Display.PumpEvents(sink);
                if (host.Display.IsClosed) break;
                if (!smoke && frames > 0 && frame >= frames) break;
                DrawPattern(pixels, frame);
                host.Display.Present(surface);
                presented++;
                pacer.Wait();
            }
            Console.WriteLine(smoke ? $"SDL smoke: presented {presented} frames and accepted queued close" : "SDL window closed cleanly");
            return 0;
        }
        catch (System.Exception error) { Console.Error.WriteLine(error.ToString()); return 1; }
    }
    private static string? Value(string[] args, string name)
    {
        int index = Array.IndexOf(args, name);
        return index < 0 ? null : index + 1 < args.Length ? args[index + 1] : throw new ArgumentException("Missing value for " + name);
    }
    private static void DrawPattern(byte[] pixels, int frame)
    {
        for (int y = 0; y < 480; y++)
            for (int x = 0; x < 640; x++)
            {
                ushort color = (ushort)(((x * 31 / 639) << 10) | ((y * 31 / 479) << 5) | (((x / 32 + y / 32 + frame / 15) & 1) * 31));
                int offset = (y * 640 + x) * 2;
                pixels[offset] = (byte)color;
                pixels[offset + 1] = (byte)(color >> 8);
            }
    }
    private sealed class WindowEvents : IDisplayEvents { }
}
