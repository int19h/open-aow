#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWLabel.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// { The AoWLabel unit of aowInt.dpl: TAOWLabel, a line of text in an image library font; it
//   sizes itself to its text. }
// no fields of its own
public sealed partial class Label(Component owner) : Control(owner)
{

    // AoWLabel.pas:38
    public override void Validate()
    {
        if (base.Font is not null)
        {
            Valid = true;
        }

        Masked = true;
        base.Validate();
    }

    // AoWLabel.pas:46
    public override void SetSize()
    {
        SetWinWidthWithoutInvalidate(base.Font.GetTextWidth(base.Text));
        SetWinHeightWithoutInvalidate(base.Font.GetTextHeight(base.Text));
        base.SetSize();
    }

    // AoWLabel.pas:72
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(surfDesc: lockedSurface);

                base.Font.Color = Color;
                base.Font.SetText(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, base.Text, lockedSurface);
                RaiseDraw(surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWLabel.pas:90
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseDown(button, shift, X, Y);
        }
    }

    // AoWLabel.pas:103
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseUp(button, shift, X, Y);
        }
    }

    // AoWLabel.pas:115
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseMove(shift, X, Y);
        }
    }

    // AoWLabel.pas:128
    public override void CheckKeyPress(ref byte key) => Handled = false;

    // AoWLabel.pas:53
    public void SetGFont(ImageLibraryFont value)
    {
        if (value != base.Font)
        {
            base.Font = value;
            Valid = false;
        }
    }

    // AoWLabel.pas:62
    public void SetGText(string value)
    {
        if (base.Text != value)
        {
            base.Text = value;
            Sized = false;
            Aligned = false;
        }
    }

    [Published]
    public override ImageLibraryFont Font { get => base.Font; set => SetGFont(value); }

    [Published]
    public override string Text { get => base.Text; set => SetGText(value); }
}
