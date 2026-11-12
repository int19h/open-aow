using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public partial class FastLoadFileStream
{
    private Stream _source;

    public FastLoadFileStream(string name)
    {
        _source = RtlUnit.OpenFile(name, RtlUnit.fmOpenRead | RtlUnit.fmShareDenyWrite);
    }

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override long Length => _source!.Length;

    public override long Position
    {
        get => _source!.Position;
        set => _source!.Position = value;
    }

    public override void Flush()
    {
    }

    public override int Read(Span<byte> buffer) => _source!.Read(buffer);

    public override int Read(byte[] buffer, int offset, int count) => _source!.Read(buffer, offset, count);

    public override void Write(ReadOnlySpan<byte> buffer) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => _source!.Seek(offset, origin);

    public override void SetLength(long value) => _source!.SetLength(value);

    // The file closes first. Stream.Dispose(bool) then marks this instance disposed.
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _source?.Dispose();
            _source = null;
        }

        base.Dispose(disposing);
    }
}
