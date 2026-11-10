
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class DDrawSurface
{
    private int memoryPitch;
    private byte[] memoryBits;

    public int CreateSurface(int AWidth, int AHeight, int APixelFormatID, Set<DDSCreationFlagsElement> AFlags) { if (FPixelFormatID != APixelFormatID) memoryBits = null; FPixelFormatID = APixelFormatID; FCreationFlags = AFlags; FStatus = Set<DDrawSurfaceStatusElement>.Of(ssSystemMemory); return Resize(AWidth, AHeight); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && memoryBits != null) return PixelFormats.DD_OK; (memoryPitch, memoryBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 8); FWidth = AWidth; FHeight = AHeight; return PixelFormats.DD_OK; }

    public override int Lock(ref SurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, memoryPitch, FPixelFormatID, memoryBits); return PixelFormats.DD_OK; }

    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int BltColorKey(Surface Dest, Rect? SrcRect, int X, int Y, int ColorKey) => base.BltColorKey(Dest, SrcRect, X, Y, ColorKey);
    public override int Cls(int Color) => base.Cls(Color);
    public int AsyncCls(int Color) => Cls(Color);
    public bool Lost() => false;
    public int Restore() => PixelFormats.DD_OK;
    public override int Unlock() => PixelFormats.DD_OK;
    public override int LockEx(ref SurfaceDesc SD, in Rect R) => base.LockEx(ref SD, R);
    public override int UnlockEx(in SurfaceDesc SD) => Unlock();

    public override void Dispose() { memoryBits = null; FWidth = FHeight = 0; base.Dispose(); }
}
