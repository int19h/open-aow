using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Platform;
using Aow.Rtl;
using SDL;
using static SDL.SDL3;

namespace Aow;

internal static unsafe class SdlVerification
{
    public static int Run()
    {
        Environment.SetEnvironmentVariable("SDL_VIDEODRIVER", "dummy");
        for (int iteration = 0; iteration < 2; iteration++)
        {
            using var display = new SdlDisplayHost("OpenAoW verification", hidden: true);
            display.VSync = true;
            display.VSync = false;
            if (display.VSync) throw new InvalidOperationException("Vsync stayed on.");
            var sink = new Sink();
            display.PumpEvents(sink);
            sink.Reset();
            int count;
            var windows = SDL_GetWindows(&count);
            if (count != 1) throw new InvalidOperationException("Expected one SDL window.");
            var id = SDL_GetWindowID(windows[0]);
            SDL_free(windows);
            var surface = new SurfaceDesc { Width = 32, Height = 24, Pitch = 68, BitCount = 16, PixelFormatID = Aow.Game.Draw.PixelFormats.pfRGB555, Data = new byte[68 * 24] };
            display.Present(surface);
            surface.Pitch = -68;
            surface.Data = new byte[68 * 24];
            surface.DataIndex = 68 * 23;
            display.Present(surface);
            surface.Width = 640; surface.Height = 480; surface.Pitch = 1280; surface.Data = new byte[1280 * 480]; surface.DataIndex = 0;
            display.Present(surface);
            // Drain resize events from texture size changes before testing the queue.
            display.PumpEvents(sink);
            sink.Reset();
            Push(new SDL_Event { key = new SDL_KeyboardEvent { type = SDL_EventType.SDL_EVENT_KEY_DOWN, windowID = id, key = (SDL_Keycode)'a', scancode = (SDL_Scancode)4, mod = (SDL_Keymod)3 } });
            Push(new SDL_Event { key = new SDL_KeyboardEvent { type = SDL_EventType.SDL_EVENT_KEY_DOWN, windowID = id, key = (SDL_Keycode)'a', scancode = (SDL_Scancode)4, repeat = true } });
            Push(new SDL_Event { key = new SDL_KeyboardEvent { type = SDL_EventType.SDL_EVENT_KEY_UP, windowID = id, key = (SDL_Keycode)'a', scancode = (SDL_Scancode)4 } });
            Push(new SDL_Event { window = new SDL_WindowEvent { type = SDL_EventType.SDL_EVENT_WINDOW_FOCUS_LOST, windowID = id } });
            Push(new SDL_Event { window = new SDL_WindowEvent { type = SDL_EventType.SDL_EVENT_WINDOW_FOCUS_GAINED, windowID = id } });
            display.PumpEvents(sink);
            if (sink.Keys != 2 || sink.KeyUps != 1 || sink.FocusChanges != 2) throw new InvalidOperationException("SDL event translation failed.");
            Verification.QueueClose();
            display.PumpEvents(sink);
            if (display.IsClosed || sink.CloseQueries != 1) throw new InvalidOperationException("Close refusal failed.");
            sink.AllowClose = true;
            Push(new SDL_Event { window = new SDL_WindowEvent { type = SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED, windowID = id } });
            display.PumpEvents(sink);
            if (!display.IsClosed || sink.CloseQueries != 2) throw new InvalidOperationException("Close acceptance failed.");
            bool rejected = Task.Run(() =>
            {
                try { _ = display.ClientSize; return false; }
                catch (InvalidOperationException) { return true; }
            }).GetAwaiter().GetResult();
            if (!rejected) throw new InvalidOperationException("The display accepted a call from another thread.");
        }
        Console.WriteLine("SDL PASS: two window lifecycles, vsync switch, positive and negative pitch, texture resize, key repeat, focus, close refusal and acceptance, thread ownership");
        return 0;
    }

    private static void Push(SDL_Event ev)
    {
        if (!SDL_PushEvent(&ev)) throw new InvalidOperationException(SDL_GetError());
    }

    private sealed class Sink : IDisplayEvents
    {
        public int Keys, KeyUps, FocusChanges, CloseQueries;
        public bool AllowClose;

        public void Reset() => Keys = KeyUps = FocusChanges = CloseQueries = 0;

        public void KeyDown(ushort key, Set<ShiftState> shift)
        {
            if (key != 'A') throw new InvalidOperationException("Expected virtual key A.");
            Keys++;
        }

        public void KeyUp(ushort key, Set<ShiftState> shift) => KeyUps++;
        public void FocusChanged(bool active) => FocusChanges++;

        public bool CloseRequested() { CloseQueries++; return AllowClose; }
    }
}
