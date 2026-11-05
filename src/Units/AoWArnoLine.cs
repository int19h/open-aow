// Translated from AoWArnoLine.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;

namespace Aow.Units.AoWArnoLine
{
    // top, middle, bottom, copied line image library
    public partial class TAoWArnoLine : TAOWControl
    {
        public ImageLib.TLibraryImage FImageTop;
        public ImageLib.TLibraryImage FImageMiddle;
        public ImageLib.TLibraryImage FImageBottom;
        public TInterfaceIL FLineLibrary;
        // AoWArnoLine.pas:47
        public override void Destroy()
        {
            FLineLibrary.Free();
            base.Destroy();
        }

        // AoWArnoLine.pas:41
        public TAoWArnoLine(TComponent owner) : base(owner)
        {
            FLineLibrary = new TInterfaceIL(this);
        }

        // AoWArnoLine.pas:53
        public override void Validate()
        {
            if (FImageTop != null && FImageMiddle != null && FImageBottom != null)
            {
                FLineLibrary.Clear();
                var copy = FImageTop.Copy();
                FLineLibrary.Add(copy);
                var fImageMiddleCopy = FImageMiddle.Copy();
                FLineLibrary.Add(fImageMiddleCopy);
                var fImageBottomCopy = FImageBottom.Copy();
                FLineLibrary.Add(fImageBottomCopy);
                FValid = true;
            }

            base.Validate();
        }

        // AoWArnoLine.pas:93
        public override void SetSize()
        {
            FWinWidth = FImageTop.Width;
            base.SetSize();
        }

        // AoWArnoLine.pas:99
        public override void Draw(TSurfaceDesc SurfDesc)
        {
            TRect OldClip = default;
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    OldClip = SurfDesc.ClipRect;
                    if (FWinTop > SurfDesc.ClipRect.Top)
                    {
                        SurfDesc.ClipRect.Top = FWinTop;
                    }

                    var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                    FLineLibrary.DrawILLv(0, rect, ref SurfDesc);
                    if (FOnDraw != null)
                    {
                        FOnDraw(this, SurfDesc);
                    }

                    SurfDesc.ClipRect = OldClip;
                }
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Exception during Draw", (AnsiString)"Error - " + Name, 0);
            }
        }

        // AoWArnoLine.pas:66
        public void SetITop(ImageLib.TLibraryImage Value)
        {
            if (Value != FImageTop)
            {
                FImageTop = Value;
                FValid = false;
            }
        }

        // AoWArnoLine.pas:75
        public void SetIMiddle(ImageLib.TLibraryImage Value)
        {
            if (Value != FImageMiddle)
            {
                FImageMiddle = Value;
                FValid = false;
            }
        }

        // AoWArnoLine.pas:84
        public void SetIBottom(ImageLib.TLibraryImage Value)
        {
            if (Value != FImageBottom)
            {
                FImageBottom = Value;
                FValid = false;
            }
        }

        [Published]
        public ImageLib.TLibraryImage ImageTop { get => FImageTop; set => SetITop(value); }

        [Published]
        public ImageLib.TLibraryImage ImageMiddle { get => FImageMiddle; set => SetIMiddle(value); }

        [Published]
        public ImageLib.TLibraryImage ImageBottom { get => FImageBottom; set => SetIBottom(value); }
    }

    public static partial class AoWArnoLine
    {
        // AoWArnoLine.pas:119
        public static void Register()
        {
            RegisterComponents((AnsiString)"AOW Tools", [typeof(TAoWArnoLine)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
