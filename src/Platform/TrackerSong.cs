using System.Reflection;
using System.Runtime.InteropServices;

namespace Aow.Platform;

public sealed unsafe class TrackerSong : IDisposable
{
    private const string Library = "aow-openmpt";
    private nint _module;
    private static readonly LogCallback s_log = (_, _) => { };
    private static readonly ErrorCallback s_errorHandler = (_, _) => 2;

    public static string? LastError { get; private set; }

    private static string? s_reported;

    static TrackerSong() => NativeLibrary.SetDllImportResolver(typeof(TrackerSong).Assembly, Resolve);

    private static nint Resolve(string name, Assembly assembly, DllImportSearchPath? path)
    {
        if (name != Library) return 0;
        // A distribution's runtime package installs only the versioned name, the development package adds the plain one.
        string[] files = OperatingSystem.IsWindows() ? ["openmpt.dll"] : OperatingSystem.IsLinux() ? ["libopenmpt.so.0", "libopenmpt.so"] : ["libopenmpt.0.dylib", "libopenmpt.dylib"];
        foreach (string file in files)
            if (NativeLibrary.TryLoad(Path.Combine(AppContext.BaseDirectory, file), out nint handle) ||
                NativeLibrary.TryLoad(file, assembly, path, out handle)) return handle;
        throw new DllNotFoundException($"Music is silent: {files[0]} is missing. Install the native libopenmpt libraries next to the game or system-wide.");
    }

    private TrackerSong(nint module) => this._module = module;

    public static TrackerSong? Load(ReadOnlySpan<byte> bytes, bool loop = false)
    {
        try
        {
            int code = 0;
            nint message = 0;
            nint result;
            fixed (byte* data = bytes)
                result = Create(data, (nuint)bytes.Length, s_log, 0, s_errorHandler, 0, &code, &message, 0);
            string? detail = message == 0 ? null : Marshal.PtrToStringUTF8(message);
            if (message != 0) FreeString(message);
            if (result == 0) throw new InvalidDataException($"libopenmpt failed to load the song ({code}): {detail}");
            var song = new TrackerSong(result);
            song.SetLoop(loop);
            LastError = null;
            return song;
        }
        catch (System.Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or BadImageFormatException or InvalidDataException)
        {
            LastError = ex.Message;
            if (LastError != s_reported) Console.Error.WriteLine(s_reported = LastError);
            return null;
        }
    }

    public int Order { get { Check(); return GetOrder(_module); } set { Check(); SetOrder(_module, value, 0); } }

    public void SetLoop(bool loop) { Check(); SetRepeat(_module, loop ? -1 : 0); }

    public int Render(int rate, Span<float> stereo)
    {
        Check();
        if (rate <= 0 || stereo.Length % 2 != 0) throw new ArgumentException("Expected stereo frames at a positive rate.");
        int count;
        fixed (float* data = stereo) count = checked((int)Read(_module, rate, (nuint)(stereo.Length / 2), data));
        if (GetError(_module) != 0) throw new InvalidDataException("libopenmpt failed while rendering the song.");
        return count;
    }

    private void Check() => ObjectDisposedException.ThrowIf(_module == 0, this);

    public void Dispose() { if (_module != 0) { Destroy(_module); _module = 0; } }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)] private delegate void LogCallback(nint message, nint user);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)] private delegate int ErrorCallback(int error, nint user);

    [DllImport(Library, EntryPoint = "openmpt_module_create_from_memory2", CallingConvention = CallingConvention.Cdecl)]
    private static extern nint Create(byte* data, nuint size, LogCallback log, nint logUser, ErrorCallback error, nint errorUser, int* code, nint* message, nint controls);
    [DllImport(Library, EntryPoint = "openmpt_free_string", CallingConvention = CallingConvention.Cdecl)] private static extern void FreeString(nint value);
    [DllImport(Library, EntryPoint = "openmpt_module_destroy", CallingConvention = CallingConvention.Cdecl)] private static extern void Destroy(nint mod);
    [DllImport(Library, EntryPoint = "openmpt_module_error_get_last", CallingConvention = CallingConvention.Cdecl)] private static extern int GetError(nint mod);
    [DllImport(Library, EntryPoint = "openmpt_module_set_repeat_count", CallingConvention = CallingConvention.Cdecl)] private static extern int SetRepeat(nint mod, int count);
    [DllImport(Library, EntryPoint = "openmpt_module_read_interleaved_float_stereo", CallingConvention = CallingConvention.Cdecl)] private static extern nuint Read(nint mod, int rate, nuint count, float* data);
    [DllImport(Library, EntryPoint = "openmpt_module_get_current_order", CallingConvention = CallingConvention.Cdecl)] private static extern int GetOrder(nint mod);
    [DllImport(Library, EntryPoint = "openmpt_module_set_position_order_row", CallingConvention = CallingConvention.Cdecl)] private static extern double SetOrder(nint mod, int order, int row);
}
