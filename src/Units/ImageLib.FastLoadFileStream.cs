using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public partial class FastLoadFileStream
{
    private Stream source;

    public FastLoadFileStream(string name)
    {
        CaptureAddress.Assign(this);
        source = RtlUnit.OpenFile(name, RtlUnit.fmOpenRead | RtlUnit.fmShareDenyWrite);
    }

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override long Length => source!.Length;

    public override long Position
    {
        get => source!.Position;
        set => source!.Position = value;
    }

    public override void Flush()
    {
    }

    public override int Read(Span<byte> buffer) => source!.Read(buffer);

    public override int Read(byte[] buffer, int offset, int count) => source!.Read(buffer, offset, count);

    public override void Write(ReadOnlySpan<byte> buffer) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => source!.Seek(offset, origin);

    public override void SetLength(long value) => source!.SetLength(value);

    // The file closes first. Stream.Dispose(bool) then marks this instance disposed.
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            source?.Dispose();
            source = null;
        }

        base.Dispose(disposing);
    }
}
