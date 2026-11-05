using Aow.Units.AoWE;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;

namespace Aow.Units.AoWHex;

public partial class TAoWWaterHexagon
{
    // AoWHex.pas:315. The base texture includes an owner-address phase.
    public override void Show(int xSurface, int ySurface, in TSurfaceDesc surface)
    {
        var field = (TMapField)Owner;
        int x = 0, y = 0;
        HSEngine.HSEngine.HXtoHP(field.Xhx, field.Yhx, ref x, ref y);
        x -= xSurface + 8;
        y -= ySurface;
        var library = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        int phase = FrameFixtureInputs.WaterVariantPhase ?? (int)((uint)Rtl.Rtl.ObjectToInt(Owner) >> 3);
        library[((phase + field.Xhx + field.Yhx * 2 + (FSides >> 6)) & 15) + 140].Show(x, y, surface);
        x -= 6;
        y -= 9;
        if ((FSides & 1) != 0) library[36].Show(x, y, surface);
        if ((FSides & 2) != 0) library[37].Show(x, y, surface);
        if ((FSides & 32) != 0) library[35].Show(x, y, surface);
    }
}
