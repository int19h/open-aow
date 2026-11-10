// Translated from DisplayC.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Draw;

// 8 bit back surfaces drawn with StretchDIBits through a palette. FBack1, FBack2
// (TWindows08DIBSurface, declared in the implementation), FPalette, FBitmapInfo (header and 256
// colors), FHPalette, FOldPalette
public partial class Windows08DC : WindowsDC
{

    internal Surface FBack1;
    internal Surface FBack2;
    internal GFXEPalette FPalette;
    internal BitmapInfoHeader FBitmapInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal int FHPalette;
    internal int FOldPalette;

    // DisplayC.pas:2165
    public Windows08DC(Component owner) : base(owner)
    {
        FPalette = new GFXEPalette();
        FPalette.PixelFormatID = PixelFormats.pfRGB24;
    }

    // DisplayC.pas:2172
    public override void Dispose()
    {
        base.Dispose();
    }

    // DisplayC.pas:2178
    public override GFXEPalette GetPalette() => FPalette;

    // DisplayC.pas:2183
    public override int GetHPalette() => FHPalette;

    // DisplayC.pas:2188
    public override Surface GetSurface(int Index)
    {
        Surface Result;
        switch (Index)
        {
            case 0:
                Result = FGDISurface;
                break;
            case 1:
                Result = FBack1;
                break;
            case 2:
                Result = FBack2;
                break;
            default:
                Result = null;
                break;
        }

        return Result;
    }

    // DisplayC.pas:2199
    public override int Deactivate()
    {
        if (FStatus.Contains(DisplayCtrlStatusElement.Activated))
        {
            FBack1?.Dispose();
            FBack1 = null;
            FBack2?.Dispose();
            FBack2 = null;
        }

        ReleaseHPalette();
        return base.Deactivate();
    }

    // DisplayC.pas:2212
    public override int Activate(CustomDisplay ADisplay)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2254
    public void CreateHPalette()
    {
        /* TODO: Implement Windows.CreatePalette in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2270
    public void ReleaseHPalette()
    {
        /* TODO: Implement Windows.SelectPalette in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2281
    public override void WindowRecreated()
    {
        /* TODO: Implement Windows.DeleteObject in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2290
    public void UpdatePalette()
    {
        /* TODO: Implement Windows.SetPaletteEntries in the platform or editor service. */
        throw new NotImplementedException();
    }

}
