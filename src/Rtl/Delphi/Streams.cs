using System.Runtime.InteropServices;

namespace Aow.Rtl;

public static partial class RtlUnit
{
    public const int soFromBeginning = 0, soFromCurrent = 1, soFromEnd = 2;
    public const int fmCreate = 65535, fmOpenRead = 0, fmOpenWrite = 1, fmOpenReadWrite = 2;
    public const int fmShareCompat = 0, fmShareExclusive = 16, fmShareDenyWrite = 32, fmShareDenyRead = 48, fmShareDenyNone = 64;
}

public abstract class TStream : TObject, IDisposable
{
    public abstract int Read(Span<byte> buffer);
    public abstract int Write(ReadOnlySpan<byte> buffer);
    public abstract int Seek(int offset, int origin);

    // CLASSES.PAS TStream.GetPosition and SetPosition.
    public int Position { get => Seek(0, 1); set => Seek(value, 0); }
    // CLASSES.PAS TStream.GetSize and SetSize.
    public virtual int Size { get { int p = Position; int size = Seek(0, 2); Position = p; return size; } set => SetSize(value); }

    public virtual void SetSize(int size) { }

    public int Read(Span<byte> buffer, int count) => count < 0 ? 0 : Read(buffer[..count]);
    public int Write(ReadOnlySpan<byte> buffer, int count) => count < 0 ? 0 : Write(buffer[..count]);
    public int Read(Ptr<byte> buffer, int count) => count <= 0 ? 0 : Read(buffer.Array!.AsSpan(buffer.Index, count));
    public int Write(Ptr<byte> buffer, int count) => count < 0 ? 0 : Write(buffer.IsNull ? ReadOnlySpan<byte>.Empty : buffer.Array.AsSpan(buffer.Index, count));
    public int Read<T>(ref T value) where T : unmanaged => Read(Bytes.Of(ref value));
    public int Read<T>(ref T value, int count) where T : unmanaged => Read(Bytes.Of(ref value, count));
    public int Write<T>(in T value) where T : unmanaged => Write(MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1)));
    public int Write<T>(in T value, int count) where T : unmanaged => Write(MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1))[..count]);

    // CLASSES.PAS TStream.ReadBuffer and WriteBuffer each perform one transfer.
    public void ReadBuffer(Span<byte> buffer) { if (buffer.Length != 0 && Read(buffer) != buffer.Length) throw new EndOfStreamException("Stream read error"); }

    public void WriteBuffer(ReadOnlySpan<byte> buffer) { if (buffer.Length != 0 && Write(buffer) != buffer.Length) throw new IOException("Stream write error"); }

    public void ReadBuffer<T>(ref T value) where T : unmanaged => ReadBuffer(Bytes.Of(ref value));
    public void WriteBuffer<T>(in T value) where T : unmanaged => WriteBuffer(MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1)));
    public void ReadBuffer<T>(ref T value, int count) where T : unmanaged => ReadBuffer(Bytes.Of(ref value, count));
    public void WriteBuffer<T>(in T value, int count) where T : unmanaged => WriteBuffer(MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1))[..count]);

    public void ReadBuffer(Ptr<byte> buffer, int count) { if (count != 0 && Read(buffer, count) != count) throw new EndOfStreamException("Stream read error"); }

    public void WriteBuffer(Ptr<byte> buffer, int count) { if (count != 0 && Write(buffer, count) != count) throw new IOException("Stream write error"); }

    // CLASSES.PAS TStream.CopyFrom.
    public int CopyFrom(TStream source, int count)
    {
        if (count == 0) { source.Position = 0; count = source.Size; }
        if (count < 0) throw new IOException("Invalid copy count");
        int result = count;
        byte[] buffer = new byte[Math.Min(count, 61440)];
        while (count != 0)
        {
            int n = Math.Min(count, buffer.Length);
            source.ReadBuffer(buffer.AsSpan(0, n)); WriteBuffer(buffer.AsSpan(0, n)); count -= n;
        }
        return result;
    }

    public void Dispose() { Destroy(); GC.SuppressFinalize(this); }
}

public class TMemoryStream : TStream
{
    private byte[] memory = [];
    private int size, position;

    public byte[] Memory => memory;

    public int Capacity
    {
        get => memory.Length;
        // CLASSES.PAS TMemoryStream.SetCapacity and Realloc preserve allocated bytes.
        set
        {
            if (value < 0 || value > int.MaxValue - 8191) throw new IOException("Out of memory while expanding memory stream");
            int capacity = value > 0 ? (value + 8191) & ~8191 : 0;
            if (capacity != memory.Length) Array.Resize(ref memory, capacity);
        }
    }

    public override int Size { get => size; set => SetSize(value); }

    // CLASSES.PAS TMemoryStream.SetSize.
    public override void SetSize(int value) { Capacity = value; size = value; if (position > size) Seek(0, 2); }

    // CLASSES.PAS TCustomMemoryStream.Read.
    public override int Read(Span<byte> buffer)
    {
        if (position < 0 || position >= size) return 0;
        int n = Math.Min(size - position, buffer.Length);
        memory.AsSpan(position, n).CopyTo(buffer); position += n; return n;
    }

    // CLASSES.PAS TMemoryStream.Write, including a zero write past the end.
    public override int Write(ReadOnlySpan<byte> buffer)
    {
        int end = unchecked(position + buffer.Length);
        if (position < 0 || end <= 0) return 0;
        if (end > size) { if (end > Capacity) Capacity = end; size = end; }
        buffer.CopyTo(memory.AsSpan(position)); position = end; return buffer.Length;
    }

    // CLASSES.PAS TCustomMemoryStream.Seek.
    public override int Seek(int offset, int origin)
    {
        position = origin switch { 0 => offset, 1 => unchecked(position + offset), 2 => unchecked(size + offset), _ => position };
        return position;
    }

    // CLASSES.PAS TMemoryStream.Clear.
    public void Clear() { Capacity = 0; size = 0; position = 0; }

    // CLASSES.PAS TMemoryStream.LoadFromStream resets the source, preserving our position.
    public void LoadFromStream(TStream source) { source.Position = 0; SetSize(source.Size); source.ReadBuffer(memory.AsSpan(0, size)); }

    // CLASSES.PAS TMemoryStream.LoadFromFile.
    public void LoadFromFile(string name) { using var stream = new TFileStream(name, RtlUnit.fmOpenRead); LoadFromStream(stream); }

    // CLASSES.PAS TCustomMemoryStream.SaveToStream and SaveToFile.
    public void SaveToStream(TStream stream) => stream.WriteBuffer(memory.AsSpan(0, size));

    public void SaveToFile(string name) { using var stream = new TFileStream(name, RtlUnit.fmCreate); SaveToStream(stream); }

    // CLASSES.PAS TMemoryStream.Destroy.
    public override void Destroy() { Clear(); base.Destroy(); }
}

public class TFileStream : TStream
{
    private readonly Stream stream;

    // CLASSES.PAS TFileStream.Create.
    public TFileStream(string name, int mode)
    {
        try { stream = RtlUnit.Files.Open(name, mode); }
        catch (System.Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            if (mode == RtlUnit.fmCreate) throw new IOException($"Cannot create file {name}");
            if (e is FileNotFoundException or DirectoryNotFoundException)
                throw new FileNotFoundException($"Cannot open file {name}");
            throw new IOException($"Cannot open file {name}");
        }
    }

    // CLASSES.PAS THandleStream.Read and Write return zero on OS failure.
    public override int Read(Span<byte> buffer) { try { return stream.Read(buffer); } catch (System.Exception e) when (e is IOException or NotSupportedException) { return 0; } }

    public override int Write(ReadOnlySpan<byte> buffer)
    {
        try { if (!stream.CanWrite) return 0; stream.Write(buffer); return buffer.Length; }
        catch (System.Exception e) when (e is IOException or NotSupportedException) { return 0; }
    }

    // CLASSES.PAS THandleStream.Seek leaves the OS position intact on failure.
    public override int Seek(int offset, int origin)
    {
        if (origin is < 0 or > 2) return -1;
        try { return checked((int)stream.Seek(offset, (SeekOrigin)origin)); } catch (IOException) { return -1; }
    }

    // CLASSES.PAS THandleStream.SetSize seeks before truncating.
    public override void SetSize(int size)
    {
        Position = size;
        try { stream.SetLength(stream.Position); }
        catch (System.Exception e) when (e is IOException or NotSupportedException)
        {
            int code = e is NotSupportedException ? 5 : e.HResult & 65535;
            string text = $"Win32 Error.  Code: {code}.\n{new System.ComponentModel.Win32Exception(code).Message}";
            throw new System.ComponentModel.Win32Exception(code, text);
        }
    }

    // CLASSES.PAS TFileStream.Destroy.
    public override void Destroy() { stream.Dispose(); base.Destroy(); }
}
