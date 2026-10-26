using Aow.Rtl;

namespace Aow.Units.GFXE;

public static partial class GFXE
{
    // Gfxepack/GFXE.pas:1046. Milestone 3 supplies the pixel implementation.
    public static void MMXBltRect(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.MMXBltRect");
    // Gfxepack/GFXE.pas:1143. Milestone 3 supplies the pixel implementation.
    public static void FPBltRect(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.FPBltRect");
    // Gfxepack/GFXE.pas:1239. Milestone 3 supplies the pixel implementation.
    public static void RegisterBltRect(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RegisterBltRect");
    // Gfxepack/GFXE.pas:1345. Milestone 3 supplies the pixel implementation.
    public static void RepMovsdBltRect(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RepMovsdBltRect");
    // Gfxepack/GFXE.pas:1411. Milestone 3 supplies the pixel implementation.
    public static void BltRect(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.BltRect");
    // Gfxepack/GFXE.pas:1798. Milestone 3 supplies the pixel implementation.
    public static void BltTransparent565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.BltTransparent565");
    // Gfxepack/GFXE.pas:1905. Milestone 3 supplies the pixel implementation.
    public static void BltTransparent555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.BltTransparent555");
    // Gfxepack/GFXE.pas:2013. Milestone 3 supplies the pixel implementation.
    public static void BltColorKey16(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, int ColorKey) => throw new NotImplementedException("GFXE.BltColorKey16");
    // Gfxepack/GFXE.pas:2207. Milestone 3 supplies the pixel implementation.
    public static void BltStretched16(object RowTable, object ColTable, object Dst, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.BltStretched16");
    // Gfxepack/GFXE.pas:7580. Milestone 3 supplies the pixel implementation.
    public static void RemapSurface16to16(object Src, object Dst, object Table, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RemapSurface16to16");
    // Gfxepack/GFXE.pas:7684. Milestone 3 supplies the pixel implementation.
    public static void RemapSurface16to08(object Src, object Dst, object Table, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RemapSurface16to08");
    // Gfxepack/GFXE.pas:7779. Milestone 3 supplies the pixel implementation.
    public static void RemapSurface16to32(object Src, object Dst, object Table, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RemapSurface16to32");
    // Gfxepack/GFXE.pas:7873. Milestone 3 supplies the pixel implementation.
    public static void RemapSurface555to32(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RemapSurface555to32");
    // Gfxepack/GFXE.pas:7992. Milestone 3 supplies the pixel implementation.
    public static void RemapSurface565to32(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RemapSurface565to32");
    // Gfxepack/GFXE.pas:8111. Milestone 3 supplies the pixel implementation.
    public static void RemapSurface555to565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RemapSurface555to565");
    // Gfxepack/GFXE.pas:8218. Milestone 3 supplies the pixel implementation.
    public static void RemapSurface565to555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height) => throw new NotImplementedException("GFXE.RemapSurface565to555");
    // Gfxepack/GFXE.pas:9737. Milestone 3 supplies the pixel implementation.
    public static void BlendSurface555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, object Table) => throw new NotImplementedException("GFXE.BlendSurface555");
    // Gfxepack/GFXE.pas:9952. Milestone 3 supplies the pixel implementation.
    public static void BlendSurface565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, object Table) => throw new NotImplementedException("GFXE.BlendSurface565");
    // Gfxepack/GFXE.pas:10170. Milestone 3 supplies the pixel implementation.
    public static void BlendColorKey555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, int ColorKey, object Table) => throw new NotImplementedException("GFXE.BlendColorKey555");
    // Gfxepack/GFXE.pas:10525. Milestone 3 supplies the pixel implementation.
    public static void BlendColorKey565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, int ColorKey, object Table) => throw new NotImplementedException("GFXE.BlendColorKey565");
    // Gfxepack/GFXE.pas:10884. Milestone 3 supplies the pixel implementation.
    public static void BlendStretched555(object RowTable, object ColTable, object Dst, int DstPitch, int Width, int Height, object Table) => throw new NotImplementedException("GFXE.BlendStretched555");
    // Gfxepack/GFXE.pas:10961. Milestone 3 supplies the pixel implementation.
    public static void BlendStretched565(object RowTable, object ColTable, object Dst, int DstPitch, int Width, int Height, object Table) => throw new NotImplementedException("GFXE.BlendStretched565");
}
