// Translated from AoWLabel.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.AoWControl;

namespace Aow.Units.AoWLabel
{
    // { The AoWLabel unit of aowInt.dpl: TAOWLabel, a line of text in an image library font; it
//   sizes itself to its text. }
    // no fields of its own
    public partial class TAOWLabel : TAOWControl
    {
        public TAOWLabel(TComponent owner) : base(owner)
        { }

        // AoWLabel.pas:38
        public override void Validate()
        {
            if (FAOWFont != null)
            {
                FValid = true;
            }

            FMasked = true;
            base.Validate();
        }

        // AoWLabel.pas:46
        public override void SetSize()
        {
            FWinWidth = FAOWFont.GetTextWidth(FText);
            FWinHeight = FAOWFont.GetTextHeight(FText);
            base.SetSize();
        }

        // AoWLabel.pas:72
        public override void Draw(TSurfaceDesc SurfDesc)
        {
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    FAOWFont.Color = FColor;
                    FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FText, SurfDesc);
                    if (FOnDraw != null)
                    {
                        FOnDraw(this, SurfDesc);
                    }
                }
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Exception during Draw", (AnsiString)"Error - " + Name, 0);
            }
        }

        // AoWLabel.pas:90
        public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = FCaptureInput;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWLabel.pas:103
        public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = FCaptureInput;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWLabel.pas:115
        public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = FCaptureInput;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
        }

        // AoWLabel.pas:128
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AoWLabel.pas:53
        public void SetGFont(TImageLibraryFont Value)
        {
            if (Value != FAOWFont)
            {
                FAOWFont = Value;
                FValid = false;
            }
        }

        // AoWLabel.pas:62
        public void SetGText(AnsiString Value)
        {
            if (FText != Value)
            {
                FText = Value;
                FSized = false;
                FAligned = false;
            }
        }

        [Published]
        public TImageLibraryFont AOWFont { get => FAOWFont; set => SetGFont(value); }

        [Published]
        public AnsiString Text { get => FText; set => SetGText(value); }
    }

    public static partial class AoWLabel
    {
        // AoWLabel.pas:133
        public static void Register()
        {
            RegisterComponents((AnsiString)"AOW Interface", [typeof(TAOWLabel)]);
        }
    }
}
