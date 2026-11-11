using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class Image
{
    // FastBattleWindow.GlowAnimation is a plain TImageLibrary in the recovered form.
    public ImageLibrary FImageLib;

    [Published]
    public ImageLibrary ImageLib { get => FImageLib; set => SetILB(value); }

    public void SetILB(ImageLibrary value)
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

    public override void Draw(LockedSurface surface)
    {
        try
        {
            if (!FValid || !FVisible) return;
            FOnPreDraw?.Invoke(this, surface);
            var rectangle = new Rect(FAOWWindow.FWinLeft + FWinLeft,
                FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
            FImageLib.DrawILT(FCurrentImage, rectangle, ref surface);
            FOnDraw?.Invoke(this, surface);
        }
        catch
        {
            RtlUnit.MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }
}
