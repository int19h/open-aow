// Translated from AoWButton.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.AOWMenu;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWButton;

// { The AoWButton unit of aowInt.dpl: TAOWBaseButton, the common ancestor of the push button and
//   the check box, which draws itself from a button library (three images: the left end, the
//   middle and the right end of a horizontal line) or from an icon library (one image), in the up,
//   down or lit (mouse over) state, with its text centred on it; and TAOWButton, the push button,
//   which calls OnClick when the left button is released over it. }
// Fields: FBorderHeightSelected (the text offset from the top when the button is down.
// FBorderHeight when it is up), FButtonType (btText: drawn from FILButton, btIcon: from
// FILIcon. set by Validate), FILButton, FILIcon, FILIndexUp, FILIndexDown, FILIndexLit,
// FPopupMenu, FLit (the mouse is over the button). FChecked is the down state
public partial class TAOWBaseButton : TAOWControl
{
    public TAOWBaseButton(TComponent owner) : base(owner)
    { }

    public int FBorderHeightSelected;
    public TButtonType FButtonType;
    public TInterfaceIL FILButton;
    public TInterfaceIL FILIcon;
    public int FILIndexUp;
    public int FILIndexDown;
    public int FILIndexLit;
    public TAOWMenu FPopupMenu;
    public bool FLit;

    // AoWButton.pas:85
    public override void Validate()
    {
        if (FILButton != null)
        {
            FILButton = FILButton.CheckIL(FILIndexUp, 3, this);
        }

        if (FILButton != null)
        {
            FILButton = FILButton.CheckIL(FILIndexDown, 3, this);
        }

        if (FILButton != null && FILIndexLit != FILIndexUp)
        {
            FILButton = FILButton.CheckIL(FILIndexLit, 3, this);
        }

        if (FILIcon != null)
        {
            FILIcon = FILIcon.CheckIL(FILIndexUp, 1, this);
        }

        if (FILIcon != null)
        {
            FILIcon = FILIcon.CheckIL(FILIndexDown, 1, this);
        }

        if (FILIcon != null && FILIndexLit != FILIndexUp)
        {
            FILIcon = FILIcon.CheckIL(FILIndexLit, 1, this);
        }

        if (FILButton != null)
        {
            FButtonType = TButtonType.btText;
            FValid = true;
        }
        else
        {
            if (FILIcon != null)
            {
                FButtonType = TButtonType.btIcon;
                FValid = true;
            }
        }

        base.Validate();
    }

    // AoWButton.pas:112
    public override void SetSize()
    {
        switch (FButtonType)
        {
            case TButtonType.btText:
                var ilButtonWidth = FILButton[FILIndexUp];
                var ilButton = FILButton[FILIndexUp + 2];
                if (ilButtonWidth.Width + ilButton.Width > FWinWidth)
                {
                    var ilButton2 = FILButton[FILIndexUp];
                    var ilButton3 = FILButton[FILIndexUp + 2];
                    FWinWidth = ilButton2.Width + ilButton3.Width;
                }

                FWinHeight = FILButton[FILIndexUp].Height;
                break;
            case TButtonType.btIcon:
                FWinWidth = FILIcon[FILIndexUp].Width;
                FWinHeight = FILIcon[FILIndexUp].Height;
                break;
        }

        base.SetSize();
    }

    // AoWButton.pas:78
    public override void SetVisible(bool Value)
    {
        if (Value != FVisible)
        {
            FLit = false;
        }

        base.SetVisible(Value);
    }

    // AoWButton.pas:183
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        try
        {
            if (FVisible && FValid)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                if (FChecked)
                {
                    if (FButtonType == TButtonType.btText)
                    {
                        var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILButton.DrawILLh(FILIndexDown, rect, ref SurfDesc);
                    }
                    else
                    {
                        var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILIcon.DrawILI(FILIndexDown, rect, ref SurfDesc);
                    }

                    if (FAOWFont != null)
                    {
                        FAOWFont.Color = FSelColor;
                        var textWidth = FAOWFont.GetTextWidth(FText);
                        FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FWinWidth / 2 - textWidth / 2, FAOWWindow.FWinTop + FWinTop + FBorderHeightSelected, FText, SurfDesc);
                    }
                }
                else
                {
                    if (FButtonType == TButtonType.btText)
                    {
                        if (FLit)
                        {
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILButton.DrawILLh(FILIndexLit, rect, ref SurfDesc);
                        }
                        else
                        {
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILButton.DrawILLh(FILIndexUp, rect, ref SurfDesc);
                        }
                    }
                    else
                    {
                        if (FLit)
                        {
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILIcon.DrawILI(FILIndexLit, rect, ref SurfDesc);
                        }
                        else
                        {
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILIcon.DrawILI(FILIndexUp, rect, ref SurfDesc);
                        }
                    }

                    if (FAOWFont != null)
                    {
                        FAOWFont.Color = FColor;
                        var textWidth = FAOWFont.GetTextWidth(FText);
                        FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FWinWidth / 2 - textWidth / 2, FAOWWindow.FWinTop + FWinTop + FBorderHeight, FText, SurfDesc);
                    }
                }

                if (FOnDraw != null)
                {
                    FOnDraw(this, SurfDesc);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWButton.pas:136
    public virtual void SetILButton(TInterfaceIL Value)
    {
        if (Value != FILButton)
        {
            FILIcon = null;
            FValid = false;
            FILButton = Value;
        }
    }

    // AoWButton.pas:146
    public virtual void SetILIcon(TInterfaceIL Value)
    {
        if (Value != FILIcon)
        {
            FILButton = null;
            FValid = false;
            FILIcon = Value;
        }
    }

    // AoWButton.pas:156
    public virtual void SetILUIndex(int Value)
    {
        if (Value != FILIndexUp)
        {
            FILIndexUp = Value;
            FValid = false;
        }
    }

    // AoWButton.pas:165
    public virtual void SetILDIndex(int Value)
    {
        if (Value != FILIndexDown)
        {
            FILIndexDown = Value;
            FValid = false;
        }
    }

    // AoWButton.pas:174
    public virtual void SetILHIndex(int Value)
    {
        if (Value != FILIndexLit)
        {
            FILIndexLit = Value;
            FValid = false;
        }
    }

    // AoWButton.pas:130
    public void SetGText(string Value)
    {
        FText = Value;
        FDrawn = false;
    }

    [Published]
    public TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

    [Published]
    public int BorderHeightSelected { get => FBorderHeightSelected; set => FBorderHeightSelected = value; }

    [Published]
    public TInterfaceIL ILButton { get => FILButton; set => SetILButton(value); }

    [Published]
    public TInterfaceIL ILIcon { get => FILIcon; set => SetILIcon(value); }

    [Published]
    public int ILIndexDown { get => FILIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ILIndexLit { get => FILIndexLit; set => SetILHIndex(value); }

    [Published]
    public int ILIndexUp { get => FILIndexUp; set => SetILUIndex(value); }

    [Published]
    public TAOWMenu PopupMenu { get => FPopupMenu; set => FPopupMenu = value; }

    [Published]
    public string Text { get => FText; set => SetGText(value); }
}

// Fields: FOnClick
public partial class TAOWButton : TAOWBaseButton
{
    public TAOWButton(TComponent owner) : base(owner)
    { }

    public TNotifyEvent FOnClick;

    // AoWButton.pas:239
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        FLit = false;
        if (!FAOWWindow.FHandled)
        {
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = true;
                if (Button == TMouseButton.mbLeft)
                {
                    FMouseDown = true;
                    FChecked = true;
                    FDrawn = false;
                    FRedraw = true;
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }
    }

    // AoWButton.pas:261
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        FChecked = false;
        FLit = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (Button == TMouseButton.mbLeft)
            {
                if (FMouseDown && FOnClick != null)
                {
                    FOnClick(this);
                }

                FDrawn = false;
            }

            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }

        if (FMouseDown)
        {
            FDrawn = false;
        }

        FMouseDown = false;
    }

    // AoWButton.pas:284
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        bool OldChecked;
        bool OldLit;
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        OldChecked = FChecked;
        OldLit = FLit;
        FRedraw = false;
        if (FAOWWindow.FHandled || X < FAOWWindow.FWinLeft + FWinLeft || X > FAOWWindow.FWinLeft + FWinLeft + FWinWidth - 1 || Y < FAOWWindow.FWinTop + FWinTop || Y > FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
        {
            FChecked = false;
            FLit = false;
        }
        else
        {
            FHandled = true;
            FChecked = FMouseDown;
            FLit = !FMouseDown;
            FRedraw = true;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }

        if (OldLit != FLit || OldChecked != FChecked)
        {
            FDrawn = false;
        }
    }

    [Published]
    public TNotifyEvent OnClick { get => FOnClick; set => FOnClick = value; }
}

public static partial class AoWButtonUnit
{
    // AoWButton.pas:313
    public static void Register()
    {
        RegisterComponents("AOW Interface", [typeof(TAOWButton)]);
    }
}
