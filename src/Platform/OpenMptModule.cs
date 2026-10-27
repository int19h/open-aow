using System.Reflection;
using System.Runtime.InteropServices;

namespace Aow.Platform;

public sealed unsafe class OpenMptModule : IDisposable
{
    private const string Library = "aow-openmpt";
    private nint module;
    private static readonly LogCallback log = (_, _) => { };
    private static readonly ErrorCallback errorHandler = (_, _) => 2;
    public static string? LastError { get; private set; }

    static OpenMptModule() => NativeLibrary.SetDllImportResolver(typeof(OpenMptModule).Assembly, Resolve);

    private static nint Resolve(string name, Assembly assembly, DllImportSearchPath? path)
    {
        if (name != Library) return 0;
        string file = OperatingSystem.IsWindows() ? "openmpt.dll" : OperatingSystem.IsLinux() ? "libopenmpt.so" : "libopenmpt.dylib";
        if (NativeLibrary.TryLoad(Path.Combine(AppContext.BaseDirectory, file), out nint handle) ||
            NativeLibrary.TryLoad(file, assembly, path, out handle)) return handle;
        throw new DllNotFoundException($"Music is silent: {file} is missing. Install the native libopenmpt libraries next to the game.");
    }

    private OpenMptModule(nint module) => this.module = module;
    public static OpenMptModule? Load(ReadOnlySpan<byte> bytes, bool loop = false)
    {
        try
        {
            int code = 0;
            nint message = 0;
            nint result;
            fixed (byte* data = bytes)
                result = Create(data, (nuint)bytes.Length, log, 0, errorHandler, 0, &code, &message, 0);
            string? detail = message == 0 ? null : Marshal.PtrToStringUTF8(message);
            if (message != 0) FreeString(message);
            if (result == 0) throw new InvalidDataException($"libopenmpt failed to load the song ({code}): {detail}");
            var song = new OpenMptModule(result);
            song.SetLoop(loop);
            LastError = null;
            return song;
        }
        catch (System.Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or BadImageFormatException or InvalidDataException)
        {
            LastError = ex.Message;
            Console.Error.WriteLine(LastError);
            return null;
        }
    }
    public int Order { get { Check(); return GetOrder(module); } set { Check(); SetOrder(module, value, 0); } }
    public void SetLoop(bool loop) { Check(); SetRepeat(module, loop ? -1 : 0); }
    public int Render(int rate, Span<float> stereo)
    {
        Check();
        if (rate <= 0 || stereo.Length % 2 != 0) throw new ArgumentException("Expected stereo frames at a positive rate.");
        int count;
        fixed (float* data = stereo) count = checked((int)Read(module, rate, (nuint)(stereo.Length / 2), data));
        if (GetError(module) != 0) throw new InvalidDataException("libopenmpt failed while rendering the song.");
        return count;
    }
    private void Check() => ObjectDisposedException.ThrowIf(module == 0, this);
    public void Dispose() { if (module != 0) { Destroy(module); module = 0; } }

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
