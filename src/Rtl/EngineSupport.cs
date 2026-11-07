using System.IO.Compression;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
    public string ExeName { get; set; } = Environment.ProcessPath ?? "";
}

public static partial class RtlUnit
{
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

    public static string ReadShortString(ReadOnlySpan<byte> image) => Cp1252.Decode(image.Slice(1, image[0]).ToArray());

    public static void WriteShortString(Span<byte> image, string value)
    {
        int length = Math.Min(value.Length, image.Length - 1);
        image[0] = (byte)length;
        Cp1252.Encode(value).AsSpan().Slice(0, length).CopyTo(image.Slice(1));
    }

    public static TApplication Application { get; } = new();
    public static Action<string> MessageHandler { get; set; } = message => throw new EInvalidOperation(message);

    public static void ShowMessage(string message) => MessageHandler(message);

    public static void RegisterClass(Type type) { }

    public static void RegisterComponents(string category, Type[] types) { }

    public static uint RegisterClipboardFormat(string name) => 1;
    public static byte[] ValueBytes<T>(T value, int count) where T : unmanaged => Bytes.Of(ref value, count).ToArray();
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

    public static int ReadString(TStream stream, ref string value, int start, int count, bool exact)
    {
        byte[] bytes = Cp1252.Encode(value).ToArray();
        var buffer = bytes.AsSpan(start, count);
        if (exact && count == 0) return 0;
        int read = stream.Read(buffer);
        value = Cp1252.Decode(bytes);
        if (exact && read != count) throw new EReadError("Stream read error");
        return read;
    }

    // Delphi code stored small integers in TObject slots (TStrings.Objects, a TNotifyEvent sender)
    // with TObject(I) and read them back with Integer(Obj). IntToObject gives one cached box per
    // value, so IndexOfObject finds it again, and ObjectToInt reads the box back. The values are
    // enum values, indices and small ids, so the cache stays bounded and holds no game object.
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

    // The ids of objects that went through Integer(obj). The table holds each object weakly, so an
    // object its owner drops stays collectible (review-9x.md finding 22). Entries of collected
    // objects go on lookup and when the table reaches the prune threshold.
    private static readonly Dictionary<int, WeakReference<TObject>> referenceIds = [];
    private static int pruneThreshold = 1024;

    /// <summary>
    /// Integer(Obj): the value of an integer box, 0 for nil, and the InstanceId of any other object,
    /// recorded weakly for <see cref="ReferenceFromId{T}"/>. No product code under src turns an
    /// object into a number. The routine serves the transpiler's reference output and the probes.
    /// </summary>
    public static int ObjectToInt(TObject? value)
    {
        switch (value)
        {
            case null: return 0;
            case IntegerObject box: return box.Value;
        }
        int id = value.InstanceId;
        lock (referenceIds)
        {
            if (referenceIds.TryGetValue(id, out var entry)) entry.SetTarget(value);
            else
            {
                if (referenceIds.Count >= pruneThreshold) PruneReferenceIds();
                referenceIds.Add(id, new WeakReference<TObject>(value));
            }
        }
        return id;
    }

    /// <summary>
    /// TFoo(Integer): the object whose InstanceId is <paramref name="id"/>, or null for 0, an unknown
    /// id or a collected object. No product code under src calls it. The routine serves the
    /// transpiler's reference output and the probes.
    /// </summary>
    public static T? ReferenceFromId<T>(int id) where T : TObject
    {
        if (id == 0) return null;
        lock (referenceIds)
        {
            if (!referenceIds.TryGetValue(id, out var entry)) return null;
            if (entry.TryGetTarget(out var value)) return (T)value;
            referenceIds.Remove(id);
            return null;
        }
    }

    private static void PruneReferenceIds()
    {
        foreach (var (id, entry) in referenceIds)
            if (!entry.TryGetTarget(out _)) referenceIds.Remove(id);
        pruneThreshold = Math.Max(1024, referenceIds.Count * 2);
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
