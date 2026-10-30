using Aow.Rtl;

namespace Aow.Units.ImageLib;

public partial class TFastLoadFileStream
{
    private TFileStream source;
    public TFastLoadFileStream(AnsiString name) : base(0) => source = new TFileStream(name, Rtl.Rtl.fmOpenRead | Rtl.Rtl.fmShareDenyWrite);
    public override int Read(Span<byte> buffer) => source.Read(buffer);
    public override int Write(ReadOnlySpan<byte> buffer) => source.Write(buffer);
    public override int Seek(int offset, int origin) => source.Seek(offset, origin);
    public override void SetSize(int size) => source.SetSize(size);
    public override void Destroy() { source.Free(); source = null; }
}
