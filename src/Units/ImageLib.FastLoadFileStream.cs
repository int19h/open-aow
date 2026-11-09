using Aow.Rtl;

namespace Aow.Units.ImageLib;

public partial class TFastLoadFileStream
{
    private TFileStream source;

    public TFastLoadFileStream(string name) : base(0) => source = new TFileStream(name, RtlUnit.fmOpenRead | RtlUnit.fmShareDenyWrite);
    public override int Read(Span<byte> buffer) => source.Read(buffer);
    public override int Write(ReadOnlySpan<byte> buffer) => source.Write(buffer);
    public override int Seek(int offset, int origin) => source.Seek(offset, origin);
    public override void SetSize(int size) => source.SetSize(size);

    public override void Dispose() { source?.Dispose(); source = null; }
}
