using Aow.Layout;
using Aow.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;

namespace Aow.Units.AoWImage;

public partial class TAOWImage
{
    // FastCombatWindow.GlowAnimation is a plain TImageLibrary in the recovered form.
    public TImageLibrary FImageLib;

    [Published]
    public TImageLibrary ImageLib { get => FImageLib; set => SetILB(value); }

    public void SetILB(TImageLibrary value)
    {
        if (value == FImageLib) return;
        FValid = false;
        FImageLib = value;
    }

    public override void Validate()
    {
        if (FImageLib != null)
            FImageLib = FAnimate
                ? FImageLib.CheckIL(FAnimStart, FAnimEnd + 1 - FAnimStart, this)
                : FImageLib.CheckIL(FCurrentImage, 1, this);
        if (FImageLib != null) FValid = true;
        base.Validate();
    }

    public override void Draw(TSurfaceDesc surface)
    {
        try
        {
            if (!FValid || !FVisible) return;
            FOnPreDraw?.Invoke(this, surface);
            var rectangle = Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft,
                FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
            FImageLib.DrawILT(FCurrentImage, rectangle, ref surface);
            FOnDraw?.Invoke(this, surface);
        }
        catch (System.Exception)
        {
            Rtl.Rtl.MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }
}
