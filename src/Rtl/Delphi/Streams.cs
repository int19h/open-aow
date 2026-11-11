using System.Runtime.InteropServices;

namespace Aow.Rtl;

public static partial class RtlUnit
{
    public const int fmCreate = 65535, fmOpenRead = 0, fmOpenWrite = 1, fmOpenReadWrite = 2;
    public const int fmShareCompat = 0, fmShareExclusive = 16, fmShareDenyWrite = 32, fmShareDenyRead = 48, fmShareDenyNone = 64;

    // CLASSES.PAS TFileStream.Create. The host maps the mode. These messages stay the Delphi ones.
    public static Stream OpenFile(string name, int mode)
    {
        try
        {
            return Files.Open(name, mode);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            if (mode == fmCreate) throw new IOException($"Cannot create file {name}");
            if (e is FileNotFoundException or DirectoryNotFoundException) throw new FileNotFoundException($"Cannot open file {name}");
            throw new IOException($"Cannot open file {name}");
        }
    }
}

// CLASSES.PAS helpers that System.IO.Stream does not provide.
public static class StreamExtensions
{
    public static int Read(this Stream stream, Span<byte> buffer, int count) => count < 0 ? 0 : stream.Read(buffer[..count]);

    public static int Write(this Stream stream, ReadOnlySpan<byte> buffer, int count) => WriteCounted(stream, buffer, count);

    public static int Read(this Stream stream, Ptr<byte> buffer, int count) => count <= 0 ? 0 : stream.Read(buffer.Array!.AsSpan(buffer.Index, count));

    public static int Write(this Stream stream, Ptr<byte> buffer, int count) => WritePointer(stream, buffer, count);

    public static int Read<T>(this Stream stream, ref T value) where T : unmanaged => stream.Read(Bytes.Of(ref value));

    public static int Read<T>(this Stream stream, ref T value, int count) where T : unmanaged => stream.Read(Bytes.Of(ref value, count));

    public static int Write<T>(this Stream stream, in T value) where T : unmanaged => WriteSpan(stream, MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1)));

    public static int Write<T>(this Stream stream, in T value, int count) where T : unmanaged => WriteSpan(stream, MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1))[..count]);

    // A short transfer raises the Delphi read or write error. An empty buffer does not call Read or Write.
    public static void ReadBuffer(this Stream stream, Span<byte> buffer) => ReadBufferExact(stream, buffer);

    public static void WriteBuffer(this Stream stream, ReadOnlySpan<byte> buffer) => WriteBufferExact(stream, buffer);

    public static void ReadBuffer<T>(this Stream stream, ref T value) where T : unmanaged => ReadBufferExact(stream, Bytes.Of(ref value));

    public static void WriteBuffer<T>(this Stream stream, in T value) where T : unmanaged => WriteBufferExact(stream, MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1)));

    public static void ReadBuffer<T>(this Stream stream, ref T value, int count) where T : unmanaged => ReadBufferExact(stream, Bytes.Of(ref value, count));

    public static void WriteBuffer<T>(this Stream stream, in T value, int count) where T : unmanaged => WriteBufferExact(stream, MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1))[..count]);

    public static void ReadBuffer(this Stream stream, Ptr<byte> buffer, int count)
    {
        if (count == 0) return;
        if (count < 0) throw new EndOfStreamException("Stream read error");
        ReadBufferExact(stream, buffer.Array!.AsSpan(buffer.Index, count));
    }

    public static void WriteBuffer(this Stream stream, Ptr<byte> buffer, int count)
    {
        if (count == 0) return;
        if (count < 0 || WritePointer(stream, buffer, count) != count) throw new IOException("Stream write error");
    }

    // A zero count rewinds the source and copies its length, 61440 bytes at a time.
    public static int CopyFrom(this Stream stream, Stream source, int count)
    {
        if (count == 0)
        {
            source.Position = 0;
            count = (int)source.Length;
        }

        if (count < 0) throw new IOException("Invalid copy count");
        int result = count;
        byte[] buffer = new byte[Math.Min(count, 61440)];
        while (count != 0)
        {
            int n = Math.Min(count, buffer.Length);
            ReadBufferExact(source, buffer.AsSpan(0, n));
            WriteBufferExact(stream, buffer.AsSpan(0, n));
            count -= n;
        }

        return result;
    }

    public static void Clear(this MemoryStream stream)
    {
        stream.SetLength(0);
        stream.Capacity = 0;
    }

    // Rewinds the source and fills this buffer. This position stays where SetLength left it.
    public static void LoadFromStream(this MemoryStream stream, Stream source)
    {
        source.Position = 0;
        int count = (int)source.Length;
        stream.SetLength(count);
        if (count != 0) ReadBufferExact(source, stream.GetBuffer().AsSpan(0, count));
    }

    public static void LoadFromFile(this MemoryStream stream, string name)
    {
        using Stream file = RtlUnit.OpenFile(name, RtlUnit.fmOpenRead);
        stream.LoadFromStream(file);
    }

    public static void SaveToFile(this MemoryStream stream, string name)
    {
        using Stream file = RtlUnit.OpenFile(name, RtlUnit.fmCreate);
        stream.WriteTo(file);
    }

    private static int WriteCounted(Stream stream, ReadOnlySpan<byte> buffer, int count)
    {
        if (count < 0) return 0;
        return WriteSpan(stream, buffer[..count]);
    }

    private static int WriteSpan(Stream stream, ReadOnlySpan<byte> buffer)
    {
        stream.Write(buffer);
        return buffer.Length;
    }

    private static int WritePointer(Stream stream, Ptr<byte> buffer, int count)
    {
        if (count < 0) return 0;
        ReadOnlySpan<byte> slice = count == 0 || buffer.IsNull ? default : buffer.Array!.AsSpan(buffer.Index, count);
        return WriteSpan(stream, slice);
    }

    private static void ReadBufferExact(Stream stream, Span<byte> buffer)
    {
        if (buffer.Length == 0) return;
        try
        {
            stream.ReadExactly(buffer);
        }
        catch (EndOfStreamException exception) when (exception.Message != "Stream read error")
        {
            throw new EndOfStreamException("Stream read error");
        }
    }

    private static void WriteBufferExact(Stream stream, ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length == 0) return;
        try
        {
            stream.Write(buffer);
        }
        catch (Exception exception) when (exception is NotSupportedException or UnauthorizedAccessException)
        {
            throw new IOException("Stream write error");
        }
    }
}
