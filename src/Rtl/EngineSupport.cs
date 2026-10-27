using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.IO.Compression;
using System.Linq.Expressions;

namespace Aow.Rtl;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct TLargeInteger
{
    [FieldOffset(0)] public uint LowPart;
    [FieldOffset(4)] public int HighPart;
    [FieldOffset(0)] public long QuadPart;
}

[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct TRect
{
    [FieldOffset(0)] public int Left;
    [FieldOffset(4)] public int Top;
    [FieldOffset(8)] public int Right;
    [FieldOffset(12)] public int Bottom;
}

public class TCanvas : TObject { }
public class TLibraryImage : TObject { }
public class TPopupMenu : TObject { }
public enum TBltMode : byte { bmNone, bmStretch, bmStretchToFit, bmCenter }
public delegate void TStreamProc(TStream stream);
public delegate void RwData(Span<byte> buffer, int size, int id);
public readonly record struct THeapStatus(int TotalAllocated);

public sealed class TApplication
{
    public AnsiString ExeName { get; set; } = Environment.ProcessPath ?? "";
}

public static partial class Rtl
{
    public static object AllocMem(uint count) => Ptr<byte>.Alloc(checked((int)count)).Box();
    public static T? RebindDelegate<T>(Delegate? value) where T : Delegate
    {
        if (value is null) return null;
        if (value is T same) return same;
        var subscribers = value.GetInvocationList();
        if (subscribers.Length > 1)
            return (T)Delegate.Combine(subscribers.Select(RebindDelegate<T>).Cast<Delegate>().ToArray())!;
        if (Delegate.CreateDelegate(typeof(T), value.Target, value.Method, false) is T compatible)
            return compatible;
        var target = typeof(T).GetMethod("Invoke")!;
        var source = value.GetType().GetMethod("Invoke")!;
        var parameters = target.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
        var inputs = source.GetParameters();
        if (inputs.Length != parameters.Length) throw new EInvalidCast("Method pointer argument count differs");
        // Delphi method-pointer casts retain the call and its object argument. Adapt the
        // managed reference type at invocation, including every multicast subscriber.
        var arguments = parameters.Select((p, i) => (Expression)Expression.Convert(p, inputs[i].ParameterType));
        return Expression.Lambda<T>(Expression.Invoke(Expression.Constant(value), arguments), parameters).Compile();
    }
    public static AnsiString ReadShortString(ReadOnlySpan<byte> image) => AnsiString.FromBytes(image.Slice(1, image[0]).ToArray());
    public static void WriteShortString(Span<byte> image, AnsiString value)
    {
        int length = Math.Min(value.Length, image.Length - 1);
        image[0] = (byte)length;
        value.Span.Slice(0, length).CopyTo(image.Slice(1));
    }
    public static TApplication Application { get; } = new();
    public static Action<AnsiString> MessageHandler { get; set; } = message => throw new EInvalidOperation(message);
    public static void ShowMessage(AnsiString message) => MessageHandler(message);
    public static void RegisterClass(Type type) { }
    public static void RegisterComponents(AnsiString category, Type[] types) { }
    public static uint RegisterClipboardFormat(AnsiString name) => 1;
    public static byte[] ValueBytes<T>(T value, int count) where T : unmanaged => Bytes.Of(ref value, count).ToArray();
    public static ReadOnlySpan<byte> StringBytes(AnsiString value) => value.Span;
    public static THeapStatus GetHeapStatus() => new(unchecked((int)GC.GetTotalMemory(false)));
    public static void FillBytes(Span<byte> bytes, int value) => bytes.Fill(unchecked((byte)value));
    public static bool ArrayEquals<TArray, TElement>(TArray left, TArray right) where TArray : unmanaged where TElement : unmanaged => Bytes.Of(ref left).SequenceEqual(Bytes.Of(ref right));
    public static MethodInfo? MethodCode(Delegate? value) => value?.Method;
    public static object? MethodTarget(Delegate? value) => value?.Target;
    public static void RwSet<E>(ref TSet<E> value, int size, int id, RwData transfer, int low = 0) where E : unmanaged
    {
        ulong image = value.Image;
        transfer(Bytes.Of(ref image, size), size, id);
        value = TSet<E>.FromMask(image << (low / 8 * 8));
    }
    public static bool PointerSequenceEqual<T>(Ptr<T> left, Ptr<T> right, int count)
    {
        for (int i = 0; i < count; i++) if (!EqualityComparer<T>.Default.Equals(left[i], right[i])) return false;
        return true;
    }
    public static int ReadString(TStream stream, ref AnsiString value, int start, int count, bool exact)
    {
        byte[] bytes = value.Span.ToArray();
        var buffer = bytes.AsSpan(start, count);
        if (exact && count == 0) return 0;
        int read = stream.Read(buffer);
        value = AnsiString.FromBytes(bytes);
        if (exact && read != count) throw new EReadError("Stream read error");
        return read;
    }
    private sealed class IntegerObject(int value) : TObject { public int Value { get; } = value; }
    private static readonly Dictionary<int, TObject> integerObjects = [];
    public static TObject? IntToObject(int value)
    {
        if (value == 0) return null;
        lock (integerObjects)
        {
            if (!integerObjects.TryGetValue(value, out var box)) integerObjects.Add(value, box = new IntegerObject(value));
            return box;
        }
    }
    private static readonly Dictionary<int, TObject> referenceIds = [];
    public static int ObjectToInt(TObject? value)
    {
        if (value is IntegerObject box) return box.Value;
        if (value is null) return 0;
        int id = value.InstanceId;
        lock (referenceIds) referenceIds[id] = value;
        return id;
    }
    public static T? ReferenceFromId<T>(int id) where T : TObject
    {
        if (id == 0) return null;
        lock (referenceIds)
            if (referenceIds.TryGetValue(id, out var value)) return (T)value;
        throw new EAccessViolation("Object reference id is no longer available");
    }
    public static void ForgetReference(TObject value)
    {
        lock (referenceIds) referenceIds.Remove(value.InstanceId);
    }
}

public enum TCompressionLevel : byte { clNone, clFastest, clDefault, clMax }

// zlib.pas uses sequential zlib streams. The BCL backend owns no source stream.
internal sealed class StreamAdapter(TStream source) : Stream
{
    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => true;
    public override long Length => source.Size;
    public override long Position { get => source.Position; set => source.Position = checked((int)value); }
    public override void Flush() { }
    public override int Read(byte[] buffer, int offset, int count) => source.Read(buffer.AsSpan(offset, count));
    public override int Read(Span<byte> buffer) => source.Read(buffer);
    public override void Write(byte[] buffer, int offset, int count) => source.WriteBuffer(buffer.AsSpan(offset, count));
    public override void Write(ReadOnlySpan<byte> buffer) => source.WriteBuffer(buffer);
    public override long Seek(long offset, SeekOrigin origin) => source.Seek(checked((int)offset), (int)origin);
    public override void SetLength(long value) => source.SetSize(checked((int)value));
}

public class TCompressionStream : TStream
{
    private readonly ZLibStream stream;
    private int position;
    private bool destroyed;
    public TCompressionStream(TCompressionLevel level, TStream destination) => stream = new(new StreamAdapter(destination), level switch
    { TCompressionLevel.clNone => CompressionLevel.NoCompression, TCompressionLevel.clFastest => CompressionLevel.Fastest, TCompressionLevel.clMax => CompressionLevel.SmallestSize, _ => CompressionLevel.Optimal }, true);
    public override int Read(Span<byte> buffer) => throw new EStreamError("Invalid stream operation");
    public override int Write(ReadOnlySpan<byte> buffer) { stream.Write(buffer); position += buffer.Length; return buffer.Length; }
    public override int Seek(int offset, int origin) => origin == 1 && offset == 0 ? position : throw new EStreamError("Invalid stream operation");
    public override void Destroy() { if (!destroyed) { destroyed = true; stream.Dispose(); } base.Destroy(); }
}

public class TDecompressionStream : TStream
{
    private readonly ZLibStream stream;
    private int position;
    public TDecompressionStream(TStream source) => stream = new(new StreamAdapter(source), CompressionMode.Decompress, true);
    public override int Read(Span<byte> buffer)
    {
        int count = 0;
        while (count < buffer.Length)
        {
            int read = stream.Read(buffer[count..]);
            if (read == 0) break;
            count += read;
        }
        position += count;
        return count;
    }
    public override int Write(ReadOnlySpan<byte> buffer) => throw new EStreamError("Invalid stream operation");
    public override int Seek(int offset, int origin) => origin == 1 && offset == 0 ? position : throw new EStreamError("Invalid stream operation");
    public override void Destroy() { stream.Dispose(); base.Destroy(); }
}
