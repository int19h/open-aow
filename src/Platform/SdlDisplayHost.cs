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
    private static int s_owned;
    private readonly int _thread = Environment.CurrentManagedThreadId;
    private SDL_Window* _window;
    private SDL_Renderer* _renderer;
    private SDL_Texture* _texture;
    private int _width, _height, _windowedScale;
    private readonly InputMapper _input = new();
    private byte[] _upload = [];
    private bool _disposed;

    public bool IsClosed { get; private set; }

    public SdlDisplayHost(string title, int width = 640, int height = 480, int scale = 1, bool hidden = false)
    {
        if (width <= 0 || height <= 0 || scale < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (Interlocked.CompareExchange(ref s_owned, value: 1, comparand: 0) != 0)
        {
            throw new InvalidOperationException("One SDL window is supported.");
        }

        try
        {
            // Real pixels on a scaled Windows desktop, instead of a bitmap the system stretches.
            _ = SDL_SetHint("SDL_WINDOWS_DPI_AWARENESS", "permonitorv2");
            Check(SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO));
            _width = width;
            _height = height;
            _windowedScale = scale;
            _window = SDL_CreateWindow(title, w: checked(width * scale), h: checked(height * scale), flags: SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL_WindowFlags.SDL_WINDOW_HIGH_PIXEL_DENSITY | (hidden ? SDL_WindowFlags.SDL_WINDOW_HIDDEN : 0));
            if (_window is null)
            {
                throw Error();
            }

            _renderer = SDL_CreateRenderer(_window, name: (byte*)null);
            if (_renderer is null)
            {
                throw Error();
            }

            Check(SDL_SetWindowMinimumSize(_window, min_w: width, min_h: height));
            Check(SDL_SetRenderDrawColor(_renderer, r: 0, g: 0, b: 0, a: 255));
            CreateTexture();
            if (HasFocus)
            {
                Check(SDL_StartTextInput(_window));
            }
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    private static InvalidOperationException Error() => new($"SDL: {SDL_GetError()}");

    private static void Check(SDLBool result)
    {
        if (!result)
        {
            throw Error();
        }
    }

    private void OnThread()
    {
        ObjectDisposedException.ThrowIf(_disposed, instance: this);
        if (_thread != Environment.CurrentManagedThreadId)
        {
            throw new InvalidOperationException("SDL calls belong to the window thread.");
        }
    }

    private (int Width, int Height) WindowSize()
    {
        int w, h;
        Check(SDL_GetWindowSize(_window, w: &w, h: &h));
        return (w, h);
    }

    private DisplayViewport Viewport
    {
        get
        {
            int w, h;
            Check(SDL_GetCurrentRenderOutputSize(_renderer, w: &w, h: &h));
            return DisplayViewport.Fit(_width, _height, w, h);
        }
    }

    private (float X, float Y) ToOutput(float x, float y)
    {
        float rx, ry;
        Check(SDL_RenderCoordinatesFromWindow(_renderer, x, y, &rx, &ry));
        return (rx, ry);
    }

    private (int X, int Y, bool Inside) Map(float x, float y)
    {
        (float outputX, float outputY) = ToOutput(x, y);
        return Viewport.Map(outputX, outputY);
    }

    public (int Width, int Height) ClientSize
    {
        get
        {
            OnThread();
            return (_width, _height);
        }
    }

    public (int Width, int Height) DesktopSize
    {
        get
        {
            OnThread();
            SDL_DisplayMode* mode = SDL_GetDesktopDisplayMode(SDL_GetDisplayForWindow(_window));
            return mode is null
                ? throw Error()
                : ((int Width, int Height))((int)(mode->w * mode->pixel_density), (int)(mode->h * mode->pixel_density));
        }
    }

    public IReadOnlyList<(int Width, int Height)> FullscreenSizes
    {
        get
        {
            (int width, int height) = DesktopSize;
            return PixelFormats.IntegerScaleSizes(width, height);
        }
    }

    public void ResizeImage(int newWidth, int newHeight)
    {
        OnThread();
        if (newWidth <= 0 || newHeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newWidth));
        }

        if (_width == newWidth && _height == newHeight)
        {
            return;
        }

        _width = newWidth;
        _height = newHeight;
        CreateTexture();
        Check(SDL_SetWindowMinimumSize(_window, min_w: _width, min_h: _height));
        if (!Fullscreen)
        {
            Check(SDL_SetWindowSize(_window, w: _width * _windowedScale, h: _height * _windowedScale));
        }
    }

    public bool HasFocus
    {
        get
        {
            OnThread();
            return (SDL_GetWindowFlags(_window) & SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS) != 0;
        }
    }

    public bool Fullscreen
    {
        get
        {
            OnThread();
            return (SDL_GetWindowFlags(_window) & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) != 0;
        }
        set
        {
            OnThread();
            if (value == Fullscreen)
            {
                return;
            }

            Check(SDL_SetWindowFullscreen(_window, fullscreen: value));
            // Wayland applies the state at the next configure. Wait for it so the first frame is not
            // presented in the old state. X11 reports a failed sync here, so only Wayland waits.
            if (SDL_GetCurrentVideoDriver() == "wayland")
            {
                Check(SDL_SyncWindow(_window));
            }

            if (!value)
            {
                Check(SDL_SetWindowSize(_window, w: _width * _windowedScale, h: _height * _windowedScale));
            }
        }
    }

    public (int X, int Y, bool Inside) PointerPosition()
    {
        OnThread();
        if (!HasFocus || SDL_GetMouseFocus() != _window)
        {
            return (-1, -1, false);
        }

        float x, y;
        _ = SDL_GetMouseState(&x, &y);
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
            Check(SDL_GetRenderVSync(_renderer, vsync: &interval));
            return interval != 0;
        }
        set
        {
            OnThread();
            if (!SDL_SetRenderVSync(_renderer, vsync: value ? 1 : 0))
            {
                Console.Error.WriteLine($"SDL: vsync unavailable: {SDL_GetError()}");
            }
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
        SDL_MessageBoxFlags kind = (request.Flags & 0xF0) switch
        {
            0x10 => SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
            0x30 => SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING,
            _ => SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION
        };
        bool fullscreen = Fullscreen;
        if (fullscreen)
        {
            Fullscreen = false;
        }

        nint[] labels = [.. buttons.Select(b => Marshal.StringToCoTaskMemUTF8(b.Label))];
        nint title = Marshal.StringToCoTaskMemUTF8(request.Caption.Length > 0 ? request.Caption : "Age of Wonders");
        nint text = Marshal.StringToCoTaskMemUTF8(request.Text);
        try
        {
            SDL_MessageBoxButtonData* data = stackalloc SDL_MessageBoxButtonData[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                data[i] = new SDL_MessageBoxButtonData
                {
                    flags = (buttons[i].Id == request.DefaultResult ? SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT : 0)
                        | (buttons[i].Id is 2 || buttons.Length == 1 ? SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT : 0),
                    buttonID = buttons[i].Id,
                    text = (byte*)labels[i]
                };
            }

            SDL_MessageBoxData box = new()
            {
                flags = kind | SDL_MessageBoxFlags.SDL_MESSAGEBOX_BUTTONS_LEFT_TO_RIGHT,
                window = _window,
                title = (byte*)title,
                message = (byte*)text,
                numbuttons = buttons.Length,
                buttons = data
            };
            int pressed;
            return SDL_ShowMessageBox(&box, buttonid: &pressed) && pressed > 0 ? pressed : request.DefaultResult;
        }
        finally
        {
            foreach (nint label in labels)
            {
                Marshal.FreeCoTaskMem(label);
            }

            Marshal.FreeCoTaskMem(title);
            Marshal.FreeCoTaskMem(text);
            if (fullscreen)
            {
                Fullscreen = true;
            }
        }
    }

    public void ShowSystemCursor(bool show)
    {
        OnThread();
        Check(show ? SDL_ShowCursor() : SDL_HideCursor());
    }

    private void CreateTexture()
    {
        if (_texture is not null)
        {
            SDL_DestroyTexture(_texture);
        }

        _texture = SDL_CreateTexture(_renderer, format: SDL_PixelFormat.SDL_PIXELFORMAT_XRGB1555, access: SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, w: _width, h: _height);
        if (_texture is null)
        {
            throw Error();
        }

        Check(SDL_SetTextureScaleMode(_texture, scaleMode: SDL_ScaleMode.SDL_SCALEMODE_NEAREST));
        _upload = new byte[checked(_width * _height * 2)];
    }

    public void Present(in LockedSurface backSurface)
    {
        OnThread();
        if (backSurface.Width <= 0 || backSurface.Height <= 0 || backSurface.BitCount != 16 || backSurface.PixelFormatId != PixelFormat.Rgb555 || Math.Abs((long)backSurface.Pitch) < (long)backSurface.Width * 2)
        {
            throw new ArgumentException("The presenter requires 555 rows with a valid pitch.", paramName: nameof(backSurface));
        }

        if (backSurface.Width != _width || backSurface.Height != _height)
        {
            ResizeImage(backSurface.Width, backSurface.Height);
        }
        int rowBytes = checked(_width * 2);
        for (int y = 0; y < _height; y++)
        {
            backSurface.Data.AsSpan(checked(backSurface.DataIndex + (y * backSurface.Pitch)), length: rowBytes).CopyTo(_upload.AsSpan(y * rowBytes));
        }

        fixed (byte* pixels = _upload)
        {
            Check(SDL_UpdateTexture(_texture, rect: null, pixels: (nint)pixels, pitch: rowBytes));
        }

        Check(SDL_RenderClear(_renderer));
        DisplayViewport viewport = Viewport;
        SDL_FRect destination = new() { x = viewport.Left, y = viewport.Top, w = _width * viewport.Scale, h = _height * viewport.Scale };
        Check(SDL_RenderTexture(_renderer, _texture, srcrect: null, dstrect: &destination));
        Check(SDL_RenderPresent(_renderer));
    }

    public void PumpEvents(IDisplayEvents sink)
    {
        OnThread();
        SDL_Event ev;
        while (SDL_PollEvent(&ev))
        {
            SDL_EventType type = (SDL_EventType)ev.type;
            if (type == SDL_EVENT_QUIT)
            {
                if (sink.CloseRequested())
                {
                    IsClosed = true;
                }

                continue;
            }
            if (type is >= SDL_EVENT_WINDOW_FIRST and <= SDL_EVENT_WINDOW_LAST && ev.window.windowID != SDL_GetWindowID(_window))
            {
                continue;
            }

            if (type is SDL_EVENT_KEY_DOWN or SDL_EVENT_KEY_UP && ev.key.windowID != SDL_GetWindowID(_window))
            {
                continue;
            }

            if (type == SDL_EVENT_TEXT_INPUT && ev.text.windowID != SDL_GetWindowID(_window))
            {
                continue;
            }

            if (type == SDL_EVENT_MOUSE_MOTION && ev.motion.windowID != SDL_GetWindowID(_window))
            {
                continue;
            }

            if (type is SDL_EVENT_MOUSE_BUTTON_DOWN or SDL_EVENT_MOUSE_BUTTON_UP && ev.button.windowID != SDL_GetWindowID(_window))
            {
                continue;
            }

            if (type == SDL_EVENT_MOUSE_WHEEL && ev.wheel.windowID != SDL_GetWindowID(_window))
            {
                continue;
            }

            switch (type)
            {
                case SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                    if (sink.CloseRequested())
                    {
                        IsClosed = true;
                    }

                    break;
                case SDL_EVENT_WINDOW_FOCUS_GAINED:
                    Check(SDL_StartTextInput(_window));
                    sink.FocusChanged(true);
                    break;
                case SDL_EVENT_WINDOW_FOCUS_LOST:
                    Check(SDL_StopTextInput(_window));
                    _input.Leave(sink);
                    _input.Reset();
                    ShowSystemCursor(true);
                    sink.FocusChanged(false);
                    break;
                case SDL_EVENT_WINDOW_MOUSE_LEAVE:
                    _input.Leave(sink);
                    ShowSystemCursor(true);
                    break;
                case SDL_EVENT_WINDOW_RESIZED:
                    if (!Fullscreen)
                    {
                        int scale = Math.Max(1, (int)Math.Round(Math.Min((double)ev.window.data1 / _width, (double)ev.window.data2 / _height), mode: MidpointRounding.AwayFromZero));
                        _windowedScale = scale;
                        if (ev.window.data1 != _width * scale || ev.window.data2 != _height * scale)
                        {
                            Check(SDL_SetWindowSize(_window, w: _width * scale, h: _height * scale));
                        }
                    }
                    (int width, int height) = ClientSize;
                    sink.Resize(width, height);
                    break;
                case SDL_EVENT_MOUSE_MOTION:
                    _input.SetModifiers((int)SDL_GetModState());
                    (float x, float y) = ToOutput(ev.motion.x, ev.motion.y);
                    _input.Motion(x, y, state: (uint)ev.motion.state, Viewport, sink);
                    ShowSystemCursor(!Viewport.Map(x, y).Inside);
                    break;
                case SDL_EVENT_MOUSE_BUTTON_DOWN:
                case SDL_EVENT_MOUSE_BUTTON_UP:
                    _input.SetModifiers((int)SDL_GetModState());
                    (float pressX, float pressY) = ToOutput(ev.button.x, ev.button.y);
                    _input.Button(ev.button.button, down: type == SDL_EVENT_MOUSE_BUTTON_DOWN, milliseconds: ev.button.timestamp / 1_000_000, pressX, pressY, Viewport, sink);
                    break;
                case SDL_EVENT_MOUSE_WHEEL:
                    _input.SetModifiers((int)SDL_GetModState());
                    (float wheelX, float wheelY) = ToOutput(ev.wheel.mouse_x, ev.wheel.mouse_y);
                    _input.Wheel(ev.wheel.y, direction: (int)ev.wheel.direction, wheelX, wheelY, Viewport, sink);
                    break;
                case SDL_EVENT_KEY_DOWN:
                case SDL_EVENT_KEY_UP:
                    _input.Key((int)ev.key.key, scancode: (int)ev.key.scancode, keyModifiers: (int)ev.key.mod, down: type == SDL_EVENT_KEY_DOWN, sink);
                    break;
                case SDL_EVENT_TEXT_INPUT:
                    InputMapper.Text(Marshal.PtrToStringUTF8((nint)ev.text.text) ?? "", sink);
                    break;
                case SDL_EVENT_RENDER_TARGETS_RESET:
                case SDL_EVENT_RENDER_DEVICE_RESET:
                    CreateTexture();
                    break;
                case SDL_EVENT_FIRST:
                    break;
                case SDL_EVENT_QUIT:
                    break;
                case SDL_EVENT_TERMINATING:
                    break;
                case SDL_EVENT_LOW_MEMORY:
                    break;
                case SDL_EVENT_WILL_ENTER_BACKGROUND:
                    break;
                case SDL_EVENT_DID_ENTER_BACKGROUND:
                    break;
                case SDL_EVENT_WILL_ENTER_FOREGROUND:
                    break;
                case SDL_EVENT_DID_ENTER_FOREGROUND:
                    break;
                case SDL_EVENT_LOCALE_CHANGED:
                    break;
                case SDL_EVENT_SYSTEM_THEME_CHANGED:
                    break;
                case SDL_EVENT_DISPLAY_ORIENTATION:
                    break;
                case SDL_EVENT_DISPLAY_ADDED:
                    break;
                case SDL_EVENT_DISPLAY_REMOVED:
                    break;
                case SDL_EVENT_DISPLAY_MOVED:
                    break;
                case SDL_EVENT_DISPLAY_DESKTOP_MODE_CHANGED:
                    break;
                case SDL_EVENT_DISPLAY_CURRENT_MODE_CHANGED:
                    break;
                case SDL_EVENT_DISPLAY_CONTENT_SCALE_CHANGED:
                    break;
                case SDL_EVENT_DISPLAY_USABLE_BOUNDS_CHANGED:
                    break;
                case SDL_EVENT_WINDOW_SHOWN:
                    break;
                case SDL_EVENT_WINDOW_HIDDEN:
                    break;
                case SDL_EVENT_WINDOW_EXPOSED:
                    break;
                case SDL_EVENT_WINDOW_MOVED:
                    break;
                case SDL_EVENT_WINDOW_PIXEL_SIZE_CHANGED:
                    break;
                case SDL_EVENT_WINDOW_METAL_VIEW_RESIZED:
                    break;
                case SDL_EVENT_WINDOW_MINIMIZED:
                    break;
                case SDL_EVENT_WINDOW_MAXIMIZED:
                    break;
                case SDL_EVENT_WINDOW_RESTORED:
                    break;
                case SDL_EVENT_WINDOW_MOUSE_ENTER:
                    break;
                case SDL_EVENT_WINDOW_HIT_TEST:
                    break;
                case SDL_EVENT_WINDOW_ICCPROF_CHANGED:
                    break;
                case SDL_EVENT_WINDOW_DISPLAY_CHANGED:
                    break;
                case SDL_EVENT_WINDOW_DISPLAY_SCALE_CHANGED:
                    break;
                case SDL_EVENT_WINDOW_SAFE_AREA_CHANGED:
                    break;
                case SDL_EVENT_WINDOW_OCCLUDED:
                    break;
                case SDL_EVENT_WINDOW_ENTER_FULLSCREEN:
                    break;
                case SDL_EVENT_WINDOW_LEAVE_FULLSCREEN:
                    break;
                case SDL_EVENT_WINDOW_DESTROYED:
                    break;
                case SDL_EVENT_WINDOW_HDR_STATE_CHANGED:
                    break;
                case SDL_EVENT_WINDOW_SETTINGS_CHANGED:
                    break;
                case SDL_EVENT_TEXT_EDITING:
                    break;
                case SDL_EVENT_KEYMAP_CHANGED:
                    break;
                case SDL_EVENT_KEYBOARD_ADDED:
                    break;
                case SDL_EVENT_KEYBOARD_REMOVED:
                    break;
                case SDL_EVENT_TEXT_EDITING_CANDIDATES:
                    break;
                case SDL_EVENT_SCREEN_KEYBOARD_SHOWN:
                    break;
                case SDL_EVENT_SCREEN_KEYBOARD_HIDDEN:
                    break;
                case SDL_EVENT_MOUSE_ADDED:
                    break;
                case SDL_EVENT_MOUSE_REMOVED:
                    break;
                case SDL_EVENT_JOYSTICK_AXIS_MOTION:
                    break;
                case SDL_EVENT_JOYSTICK_BALL_MOTION:
                    break;
                case SDL_EVENT_JOYSTICK_HAT_MOTION:
                    break;
                case SDL_EVENT_JOYSTICK_BUTTON_DOWN:
                    break;
                case SDL_EVENT_JOYSTICK_BUTTON_UP:
                    break;
                case SDL_EVENT_JOYSTICK_ADDED:
                    break;
                case SDL_EVENT_JOYSTICK_REMOVED:
                    break;
                case SDL_EVENT_JOYSTICK_BATTERY_UPDATED:
                    break;
                case SDL_EVENT_JOYSTICK_UPDATE_COMPLETE:
                    break;
                case SDL_EVENT_GAMEPAD_AXIS_MOTION:
                    break;
                case SDL_EVENT_GAMEPAD_BUTTON_DOWN:
                    break;
                case SDL_EVENT_GAMEPAD_BUTTON_UP:
                    break;
                case SDL_EVENT_GAMEPAD_ADDED:
                    break;
                case SDL_EVENT_GAMEPAD_REMOVED:
                    break;
                case SDL_EVENT_GAMEPAD_REMAPPED:
                    break;
                case SDL_EVENT_GAMEPAD_TOUCHPAD_DOWN:
                    break;
                case SDL_EVENT_GAMEPAD_TOUCHPAD_MOTION:
                    break;
                case SDL_EVENT_GAMEPAD_TOUCHPAD_UP:
                    break;
                case SDL_EVENT_GAMEPAD_SENSOR_UPDATE:
                    break;
                case SDL_EVENT_GAMEPAD_UPDATE_COMPLETE:
                    break;
                case SDL_EVENT_GAMEPAD_STEAM_HANDLE_UPDATED:
                    break;
                case SDL_EVENT_GAMEPAD_CAPSENSE_TOUCH:
                    break;
                case SDL_EVENT_GAMEPAD_CAPSENSE_RELEASE:
                    break;
                case SDL_EVENT_FINGER_DOWN:
                    break;
                case SDL_EVENT_FINGER_UP:
                    break;
                case SDL_EVENT_FINGER_MOTION:
                    break;
                case SDL_EVENT_FINGER_CANCELED:
                    break;
                case SDL_EVENT_PINCH_BEGIN:
                    break;
                case SDL_EVENT_PINCH_UPDATE:
                    break;
                case SDL_EVENT_PINCH_END:
                    break;
                case SDL_EVENT_CLIPBOARD_UPDATE:
                    break;
                case SDL_EVENT_DROP_FILE:
                    break;
                case SDL_EVENT_DROP_TEXT:
                    break;
                case SDL_EVENT_DROP_BEGIN:
                    break;
                case SDL_EVENT_DROP_COMPLETE:
                    break;
                case SDL_EVENT_DROP_POSITION:
                    break;
                case SDL_EVENT_AUDIO_DEVICE_ADDED:
                    break;
                case SDL_EVENT_AUDIO_DEVICE_REMOVED:
                    break;
                case SDL_EVENT_AUDIO_DEVICE_FORMAT_CHANGED:
                    break;
                case SDL_EVENT_SENSOR_UPDATE:
                    break;
                case SDL_EVENT_PEN_PROXIMITY_IN:
                    break;
                case SDL_EVENT_PEN_PROXIMITY_OUT:
                    break;
                case SDL_EVENT_PEN_DOWN:
                    break;
                case SDL_EVENT_PEN_UP:
                    break;
                case SDL_EVENT_PEN_BUTTON_DOWN:
                    break;
                case SDL_EVENT_PEN_BUTTON_UP:
                    break;
                case SDL_EVENT_PEN_MOTION:
                    break;
                case SDL_EVENT_PEN_AXIS:
                    break;
                case SDL_EVENT_CAMERA_DEVICE_ADDED:
                    break;
                case SDL_EVENT_CAMERA_DEVICE_REMOVED:
                    break;
                case SDL_EVENT_CAMERA_DEVICE_APPROVED:
                    break;
                case SDL_EVENT_CAMERA_DEVICE_DENIED:
                    break;
                case SDL_EVENT_NOTIFICATION_ACTION_INVOKED:
                    break;
                case SDL_EVENT_RENDER_DEVICE_LOST:
                    break;
                case SDL_EVENT_PRIVATE0:
                    break;
                case SDL_EVENT_PRIVATE1:
                    break;
                case SDL_EVENT_PRIVATE2:
                    break;
                case SDL_EVENT_PRIVATE3:
                    break;
                case SDL_EVENT_POLL_SENTINEL:
                    break;
                case SDL_EVENT_USER:
                    break;
                case SDL_EVENT_LAST:
                    break;
                case SDL_EVENT_ENUM_PADDING:
                    break;
                default:
                    break;
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_thread != Environment.CurrentManagedThreadId)
        {
            throw new InvalidOperationException("Dispose belongs to the window thread.");
        }

        if (_texture is not null)
        {
            SDL_DestroyTexture(_texture);
        }

        if (_renderer is not null)
        {
            SDL_DestroyRenderer(_renderer);
        }

        if (_window is not null)
        {
            SDL_DestroyWindow(_window);
        }

        SDL_Quit();
        _window = null;
        _renderer = null;
        _texture = null;
        _disposed = true;
        _ = Interlocked.Exchange(ref s_owned, value: 0);
    }
}
