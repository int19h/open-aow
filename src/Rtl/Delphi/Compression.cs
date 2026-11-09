using System.IO.Compression;

namespace Aow.Rtl;

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

    internal static CompressionLevel CompressionLevelOf(byte level) => level switch
    {
        0 => CompressionLevel.NoCompression,
        1 => CompressionLevel.Fastest,
        3 => CompressionLevel.SmallestSize,
        _ => CompressionLevel.Optimal,
    };

    public TCompressionStream(CompressionLevel level, TStream destination) => stream = new(new StreamAdapter(destination), level, true);

    public override int Read(Span<byte> buffer) => throw new IOException("Invalid stream operation");

    public override int Write(ReadOnlySpan<byte> buffer) { stream.Write(buffer); position += buffer.Length; return buffer.Length; }

    public override int Seek(int offset, int origin) => origin == 1 && offset == 0 ? position : throw new IOException("Invalid stream operation");

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

    public override int Write(ReadOnlySpan<byte> buffer) => throw new IOException("Invalid stream operation");
    public override int Seek(int offset, int origin) => origin == 1 && offset == 0 ? position : throw new IOException("Invalid stream operation");

    public override void Destroy() { stream.Dispose(); base.Destroy(); }
}
