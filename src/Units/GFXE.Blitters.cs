using Aow.Graphics;

namespace Aow.Game.Draw;

// The assembler blitters of GFXE.pas (graphics.md 5.2). Each untyped pointer and pitch pair of
// the original is a SurfaceView, the tables are byte spans.
public static partial class GFXEUnit
{
    public static void MMXBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.MMXBltRect(Src, Dst, Width, Height);

    public static void FPBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.FPBltRect(Src, Dst, Width, Height);

    public static void RegisterBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RegisterBltRect(Src, Dst, Width, Height);

    public static void RepMovsdBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RepMovsdBltRect(Src, Dst, Width, Height);

    public static void BltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.BltRect(Src, Dst, Width, Height);

    public static void BltTransparent565(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.BltTransparent565(Src, Dst, Width, Height);

    public static void BltTransparent555(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.BltTransparent555(Src, Dst, Width, Height);

    public static void BltColorKey16(SurfaceView Src, SurfaceView Dst, int Width, int Height, int ColorKey) => Blit.BltColorKey16(Src, Dst, Width, Height, ColorKey);

    // The original's RowTable holds row addresses. The port's holds byte offsets from Source.
    public static void BltStretched16(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height) => Blit.BltStretched16(Source, RowTable, ColTable, Dst, Width, Height);

    public static void RemapSurface16to16(SurfaceView Src, SurfaceView Dst, ReadOnlySpan<byte> Table, int Width, int Height) => Blit.RemapSurface16to16(Src, Dst, Table, Width, Height);

    public static void RemapSurface16to08(SurfaceView Src, SurfaceView Dst, ReadOnlySpan<byte> Table, int Width, int Height) => Blit.RemapSurface16to08(Src, Dst, Table, Width, Height);

    public static void RemapSurface16to32(SurfaceView Src, SurfaceView Dst, ReadOnlySpan<byte> Table, int Width, int Height) => Blit.RemapSurface16to32(Src, Dst, Table, Width, Height);

    public static void RemapSurface555to32(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RemapSurface555to32(Src, Dst, Width, Height);

    public static void RemapSurface565to32(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RemapSurface565to32(Src, Dst, Width, Height);

    public static void RemapSurface555to565(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RemapSurface555to565(Src, Dst, Width, Height);

    public static void RemapSurface565to555(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RemapSurface565to555(Src, Dst, Width, Height);

    public static void BlendSurface555(SurfaceView Src, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table) => Blit.BlendSurface555(Src, Dst, Width, Height, Table);

    public static void BlendSurface565(SurfaceView Src, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table) => Blit.BlendSurface565(Src, Dst, Width, Height, Table);

    public static void BlendColorKey555(SurfaceView Src, SurfaceView Dst, int Width, int Height, int ColorKey, ReadOnlySpan<byte> Table) => Blit.BlendColorKey555(Src, Dst, Width, Height, ColorKey, Table);

    public static void BlendColorKey565(SurfaceView Src, SurfaceView Dst, int Width, int Height, int ColorKey, ReadOnlySpan<byte> Table) => Blit.BlendColorKey565(Src, Dst, Width, Height, ColorKey, Table);

    public static void BlendStretched555(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table) => Blit.BlendStretched555(Source, RowTable, ColTable, Dst, Width, Height, Table);

    public static void BlendStretched565(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table) => Blit.BlendStretched565(Source, RowTable, ColTable, Dst, Width, Height, Table);
}
