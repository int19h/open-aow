using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Rtl.Host;
using SDL;
using static SDL.SDL_EventType;
using static SDL.SDL3;

namespace Aow.Platform;

public sealed unsafe class SdlDisplayHost : IDisplayHost, IDisposable
{
    private static int owned;
    private readonly int thread = Environment.CurrentManagedThreadId;
    private SDL_Window* window;
    private SDL_Renderer* renderer;
    private SDL_Texture* texture;
    private int width, height, windowedScale;
    private readonly InputMapper input = new();
    private byte[] upload = [];
    private bool disposed;

    public bool IsClosed { get; private set; }

    public SdlDisplayHost(string title, int width = 640, int height = 480, int scale = 1, bool hidden = false)
    {
        if (width <= 0 || height <= 0 || scale < 1) throw new ArgumentOutOfRangeException(nameof(width));
        if (Interlocked.CompareExchange(ref owned, 1, 0) != 0) throw new InvalidOperationException("One SDL window is supported.");
        try
        {
            // Real pixels on a scaled Windows desktop, instead of a bitmap the system stretches.
            SDL_SetHint("SDL_WINDOWS_DPI_AWARENESS", "permonitorv2");
            Check(SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO));
            this.width = width;
            this.height = height;
            windowedScale = scale;
            window = SDL_CreateWindow(title, checked(width * scale), checked(height * scale), SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL_WindowFlags.SDL_WINDOW_HIGH_PIXEL_DENSITY | (hidden ? SDL_WindowFlags.SDL_WINDOW_HIDDEN : 0));
            if (window is null) throw Error();
            renderer = SDL_CreateRenderer(window, (byte*)null);
            if (renderer is null) throw Error();
            Check(SDL_SetWindowMinimumSize(window, width, height));
            Check(SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255));
            CreateTexture();
            if (HasFocus) Check(SDL_StartTextInput(window));
        }
        catch { Dispose(); throw; }
    }

    private static InvalidOperationException Error() => new("SDL: " + SDL_GetError());

    private static void Check(SDLBool result) { if (!result) throw Error(); }

    private void OnThread()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        if (thread != Environment.CurrentManagedThreadId) throw new InvalidOperationException("SDL calls belong to the window thread.");
    }

    private (int Width, int Height) WindowSize()
    {
        int w, h;
        Check(SDL_GetWindowSize(window, &w, &h));
        return (w, h);
    }

    private DisplayViewport Viewport
    {
        get
        {
            int w, h;
            Check(SDL_GetCurrentRenderOutputSize(renderer, &w, &h));
            return DisplayViewport.Fit(width, height, w, h);
        }
    }

    private (float X, float Y) ToOutput(float x, float y)
    {
        float rx, ry;
        Check(SDL_RenderCoordinatesFromWindow(renderer, x, y, &rx, &ry));
        return (rx, ry);
    }

    private (int X, int Y, bool Inside) Map(float x, float y)
    {
        var point = ToOutput(x, y);
        return Viewport.Map(point.X, point.Y);
    }

    public (int Width, int Height) ClientSize
    {
        get { OnThread(); return (width, height); }
    }

    public (int Width, int Height) DesktopSize
    {
        get
        {
            OnThread();
            var mode = SDL_GetDesktopDisplayMode(SDL_GetDisplayForWindow(window));
            if (mode is null) throw Error();
            return ((int)(mode->w * mode->pixel_density), (int)(mode->h * mode->pixel_density));
        }
    }

    public IReadOnlyList<(int Width, int Height)> FullscreenSizes
    {
        get { var desktop = DesktopSize; return Aow.Game.Draw.PixelFormats.IntegerScaleSizes(desktop.Width, desktop.Height); }
    }

    public void ResizeImage(int newWidth, int newHeight)
    {
        OnThread();
        if (newWidth <= 0 || newHeight <= 0) throw new ArgumentOutOfRangeException(nameof(newWidth));
        if (width == newWidth && height == newHeight) return;
        width = newWidth;
        height = newHeight;
        CreateTexture();
        Check(SDL_SetWindowMinimumSize(window, width, height));
        if (!Fullscreen) Check(SDL_SetWindowSize(window, width * windowedScale, height * windowedScale));
    }

    public bool HasFocus
    {
        get { OnThread(); return (SDL_GetWindowFlags(window) & SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS) != 0; }
    }

    public bool Fullscreen
    {
        get { OnThread(); return (SDL_GetWindowFlags(window) & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) != 0; }
        set
        {
            OnThread();
            if (value == Fullscreen) return;
            Check(SDL_SetWindowFullscreen(window, value));
            // Wayland applies the state at the next configure. Wait for it so the first frame is not
            // presented in the old state. X11 reports a failed sync here, so only Wayland waits.
            if (SDL_GetCurrentVideoDriver() == "wayland") Check(SDL_SyncWindow(window));
            if (!value) Check(SDL_SetWindowSize(window, width * windowedScale, height * windowedScale));
        }
    }

    public (int X, int Y, bool Inside) PointerPosition()
    {
        OnThread();
        if (!HasFocus || SDL_GetMouseFocus() != window) return (-1, -1, false);
        float x, y;
        SDL_GetMouseState(&x, &y);
        return Map(x, y);
    }

    /// <summary>Waits for the vertical blank in `Present`, so a frame never tears. The game's own
    /// limiter keeps its 33 ms target (graphics.md 7.2). A driver without vsync leaves it off.</summary>
    public bool VSync
    {
        get
        {
            OnThread();
            int interval;
            Check(SDL_GetRenderVSync(renderer, &interval));
            return interval != 0;
        }
        set
        {
            OnThread();
            if (!SDL_SetRenderVSync(renderer, value ? 1 : 0)) Console.Error.WriteLine("SDL: vsync unavailable: " + SDL_GetError());
        }
    }

    /// <summary>The VCL message box as a modal SDL message box (platform.md 9). The buttons follow
    /// `MB_*` in the low four bits of the flags and return their `ID*` value, the icon follows
    /// `MB_ICON*`. Fullscreen leaves for the box and returns after, as the original's dialogs did.</summary>
    public int ShowMessageBox(DialogRequest request)
    {
        OnThread();
        (int Id, string Label)[] buttons = (request.Flags & 15) switch
        {
            1 => [(1, "OK"), (2, "Cancel")],
            2 => [(3, "Abort"), (4, "Retry"), (5, "Ignore")],
            3 => [(6, "Yes"), (7, "No"), (2, "Cancel")],
            4 => [(6, "Yes"), (7, "No")],
            5 => [(4, "Retry"), (2, "Cancel")],
            _ => [(1, "OK")]
        };
        var kind = (request.Flags & 0xF0) switch
        {
            0x10 => SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
            0x30 => SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING,
            _ => SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION
        };
        bool fullscreen = Fullscreen;
        if (fullscreen) Fullscreen = false;
        var labels = buttons.Select(b => Marshal.StringToCoTaskMemUTF8(b.Label)).ToArray();
        nint title = Marshal.StringToCoTaskMemUTF8(request.Caption.Length > 0 ? request.Caption : "Age of Wonders");
        nint text = Marshal.StringToCoTaskMemUTF8(request.Text);
        try
        {
            var data = stackalloc SDL_MessageBoxButtonData[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
                data[i] = new SDL_MessageBoxButtonData
                {
                    flags = (buttons[i].Id == request.DefaultResult ? SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT : 0)
                        | (buttons[i].Id is 2 || buttons.Length == 1 ? SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT : 0),
                    buttonID = buttons[i].Id,
                    text = (byte*)labels[i]
                };
            var box = new SDL_MessageBoxData
            {
                flags = kind | SDL_MessageBoxFlags.SDL_MESSAGEBOX_BUTTONS_LEFT_TO_RIGHT,
                window = window,
                title = (byte*)title,
                message = (byte*)text,
                numbuttons = buttons.Length,
                buttons = data
            };
            int pressed;
            return SDL_ShowMessageBox(&box, &pressed) && pressed > 0 ? pressed : request.DefaultResult;
        }
        finally
        {
            foreach (var label in labels) Marshal.FreeCoTaskMem(label);
            Marshal.FreeCoTaskMem(title);
            Marshal.FreeCoTaskMem(text);
            if (fullscreen) Fullscreen = true;
        }
    }

    public void ShowSystemCursor(bool show)
    {
        OnThread();
        Check(show ? SDL_ShowCursor() : SDL_HideCursor());
    }

    private void CreateTexture()
    {
        if (texture is not null) SDL_DestroyTexture(texture);
        texture = SDL_CreateTexture(renderer, SDL_PixelFormat.SDL_PIXELFORMAT_XRGB1555, SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, width, height);
        if (texture is null) throw Error();
        Check(SDL_SetTextureScaleMode(texture, SDL_ScaleMode.SDL_SCALEMODE_NEAREST));
        upload = new byte[checked(width * height * 2)];
    }

    public void Present(in TSurfaceDesc backSurface)
    {
        OnThread();
        if (backSurface.Width <= 0 || backSurface.Height <= 0 || backSurface.BitCount != 16 || backSurface.PixelFormatID != Aow.Game.Draw.PixelFormats.pfRGB555 || Math.Abs((long)backSurface.Pitch) < (long)backSurface.Width * 2)
            throw new ArgumentException("The presenter requires 555 rows with a valid pitch.", nameof(backSurface));
        if (backSurface.Width != width || backSurface.Height != height)
        {
            ResizeImage(backSurface.Width, backSurface.Height);
        }
        int rowBytes = checked(width * 2);
        for (int y = 0; y < height; y++) backSurface.Data.AsSpan(checked(backSurface.DataIndex + y * backSurface.Pitch), rowBytes).CopyTo(upload.AsSpan(y * rowBytes));
        fixed (byte* pixels = upload) Check(SDL_UpdateTexture(texture, null, (nint)pixels, rowBytes));
        Check(SDL_RenderClear(renderer));
        var viewport = Viewport;
        var destination = new SDL_FRect { x = viewport.Left, y = viewport.Top, w = width * viewport.Scale, h = height * viewport.Scale };
        Check(SDL_RenderTexture(renderer, texture, null, &destination));
        Check(SDL_RenderPresent(renderer));
    }

    public void PumpEvents(IDisplayEvents sink)
    {
        OnThread();
        SDL_Event ev;
        while (SDL_PollEvent(&ev))
        {
            var type = (SDL_EventType)ev.type;
            if (type == SDL_EVENT_QUIT) { if (sink.CloseRequested()) IsClosed = true; continue; }
            if (type is >= SDL_EVENT_WINDOW_FIRST and <= SDL_EVENT_WINDOW_LAST && ev.window.windowID != SDL_GetWindowID(window)) continue;
            if (type is SDL_EVENT_KEY_DOWN or SDL_EVENT_KEY_UP && ev.key.windowID != SDL_GetWindowID(window)) continue;
            if (type == SDL_EVENT_TEXT_INPUT && ev.text.windowID != SDL_GetWindowID(window)) continue;
            if (type == SDL_EVENT_MOUSE_MOTION && ev.motion.windowID != SDL_GetWindowID(window)) continue;
            if (type is SDL_EVENT_MOUSE_BUTTON_DOWN or SDL_EVENT_MOUSE_BUTTON_UP && ev.button.windowID != SDL_GetWindowID(window)) continue;
            if (type == SDL_EVENT_MOUSE_WHEEL && ev.wheel.windowID != SDL_GetWindowID(window)) continue;
            switch (type)
            {
                case SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                    if (sink.CloseRequested()) IsClosed = true;
                    break;
                case SDL_EVENT_WINDOW_FOCUS_GAINED:
                    Check(SDL_StartTextInput(window));
                    sink.FocusChanged(true);
                    break;
                case SDL_EVENT_WINDOW_FOCUS_LOST:
                    Check(SDL_StopTextInput(window));
                    input.Leave(sink);
                    input.Reset();
                    ShowSystemCursor(true);
                    sink.FocusChanged(false);
                    break;
                case SDL_EVENT_WINDOW_MOUSE_LEAVE:
                    input.Leave(sink);
                    ShowSystemCursor(true);
                    break;
                case SDL_EVENT_WINDOW_RESIZED:
                    if (!Fullscreen)
                    {
                        int scale = Math.Max(1, (int)Math.Round(Math.Min((double)ev.window.data1 / width, (double)ev.window.data2 / height), MidpointRounding.AwayFromZero));
                        windowedScale = scale;
                        if (ev.window.data1 != width * scale || ev.window.data2 != height * scale)
                            Check(SDL_SetWindowSize(window, width * scale, height * scale));
                    }
                    var size = ClientSize;
                    sink.Resize(size.Width, size.Height);
                    break;
                case SDL_EVENT_MOUSE_MOTION:
                    input.SetModifiers((int)SDL_GetModState());
                    var motion = ToOutput(ev.motion.x, ev.motion.y);
                    input.Motion(motion.X, motion.Y, (uint)ev.motion.state, Viewport, sink);
                    ShowSystemCursor(!Viewport.Map(motion.X, motion.Y).Inside);
                    break;
                case SDL_EVENT_MOUSE_BUTTON_DOWN:
                case SDL_EVENT_MOUSE_BUTTON_UP:
                    input.SetModifiers((int)SDL_GetModState());
                    var press = ToOutput(ev.button.x, ev.button.y);
                    input.Button(ev.button.button, type == SDL_EVENT_MOUSE_BUTTON_DOWN, ev.button.timestamp / 1_000_000, press.X, press.Y, Viewport, sink);
                    break;
                case SDL_EVENT_MOUSE_WHEEL:
                    input.SetModifiers((int)SDL_GetModState());
                    var rolled = ToOutput(ev.wheel.mouse_x, ev.wheel.mouse_y);
                    input.Wheel(ev.wheel.y, (int)ev.wheel.direction, rolled.X, rolled.Y, Viewport, sink);
                    break;
                case SDL_EVENT_KEY_DOWN:
                case SDL_EVENT_KEY_UP:
                    input.Key((int)ev.key.key, (int)ev.key.scancode, (int)ev.key.mod, type == SDL_EVENT_KEY_DOWN, sink);
                    break;
                case SDL_EVENT_TEXT_INPUT:
                    InputMapper.Text(Marshal.PtrToStringUTF8((nint)ev.text.text) ?? "", sink);
                    break;
                case SDL_EVENT_RENDER_TARGETS_RESET:
                case SDL_EVENT_RENDER_DEVICE_RESET:
                    CreateTexture();
                    break;
            }
        }
    }

    public void Dispose()
    {
        if (disposed) return;
        if (thread != Environment.CurrentManagedThreadId) throw new InvalidOperationException("Dispose belongs to the window thread.");
        if (texture is not null) SDL_DestroyTexture(texture);
        if (renderer is not null) SDL_DestroyRenderer(renderer);
        if (window is not null) SDL_DestroyWindow(window);
        SDL_Quit();
        window = null;
        renderer = null;
        texture = null;
        disposed = true;
        Interlocked.Exchange(ref owned, 0);
    }
}
