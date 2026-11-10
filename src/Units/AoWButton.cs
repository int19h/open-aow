// Translated from AoWButton.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

// { The AoWButton unit of aowInt.dpl: TAOWBaseButton, the common ancestor of the push button and
//   the check box, which draws itself from a button library (three images: the left end, the
//   middle and the right end of a horizontal line) or from an icon library (one image), in the up,
//   down or lit (mouse over) state, with its text centred on it; and TAOWButton, the push button,
//   which calls OnClick when the left button is released over it. }
// Fields: FBorderHeightSelected (the text offset from the top when the button is down.
// FBorderHeight when it is up), FButtonType (btText: drawn from FILButton, btIcon: from
// FILIcon. set by Validate), FILButton, FILIcon, FILIndexUp, FILIndexDown, FILIndexLit,
// FPopupMenu, FLit (the mouse is over the button). FChecked is the down state
public partial class AOWBaseButton : AOWControl
{
    public AOWBaseButton(Component owner) : base(owner)
    { }

    public int FBorderHeightSelected;
    public ButtonType FButtonType;
    public InterfaceIL FILButton;
    public InterfaceIL FILIcon;
    public int FILIndexUp;
    public int FILIndexDown;
    public int FILIndexLit;
    public AOWMenu FPopupMenu;
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
            FButtonType = ButtonType.Text;
            FValid = true;
        }
        else
        {
            if (FILIcon != null)
            {
                FButtonType = ButtonType.Icon;
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
            case ButtonType.Text:
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
            case ButtonType.Icon:
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
    public override void Draw(SurfaceDesc SurfDesc)
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
                    if (FButtonType == ButtonType.Text)
                    {
                        var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILButton.DrawILLh(FILIndexDown, rect, ref SurfDesc);
                    }
                    else
                    {
                        var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
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
                    if (FButtonType == ButtonType.Text)
                    {
                        if (FLit)
                        {
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILButton.DrawILLh(FILIndexLit, rect, ref SurfDesc);
                        }
                        else
                        {
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILButton.DrawILLh(FILIndexUp, rect, ref SurfDesc);
                        }
                    }
                    else
                    {
                        if (FLit)
                        {
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILIcon.DrawILI(FILIndexLit, rect, ref SurfDesc);
                        }
                        else
                        {
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
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
    public virtual void SetILButton(InterfaceIL Value)
    {
        if (Value != FILButton)
        {
            FILIcon = null;
            FValid = false;
            FILButton = Value;
        }
    }

    // AoWButton.pas:146
    public virtual void SetILIcon(InterfaceIL Value)
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
    public ImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

    [Published]
    public int BorderHeightSelected { get => FBorderHeightSelected; set => FBorderHeightSelected = value; }

    [Published]
    public InterfaceIL ILButton { get => FILButton; set => SetILButton(value); }

    [Published]
    public InterfaceIL ILIcon { get => FILIcon; set => SetILIcon(value); }

    [Published]
    public int ILIndexDown { get => FILIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ILIndexLit { get => FILIndexLit; set => SetILHIndex(value); }

    [Published]
    public int ILIndexUp { get => FILIndexUp; set => SetILUIndex(value); }

    [Published]
    public AOWMenu PopupMenu { get => FPopupMenu; set => FPopupMenu = value; }

    [Published]
    public string Text { get => FText; set => SetGText(value); }

    public new NotifyEvent<AOWBaseButton> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWBaseButton> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWBaseButton> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWBaseButton> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWBaseButton> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWBaseButton> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWBaseButton> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWBaseButton> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWBaseButton> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}

// Fields: FOnClick
public partial class AOWButton : AOWBaseButton
{
    public AOWButton(Component owner) : base(owner)
    { }

    public NotifyEvent<AOWButton> FOnClick;

    // AoWButton.pas:239
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        FLit = false;
        if (!FAOWWindow.FHandled)
        {
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = true;
                if (Button == MouseButton.Left)
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
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        FChecked = false;
        FLit = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (Button == MouseButton.Left)
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
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
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
    public NotifyEvent<AOWButton> OnClick { get => FOnClick; set => FOnClick = value; }

    public new NotifyEvent<AOWButton> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWButton> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWButton> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWButton> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWButton> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWButton> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWButton> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWButton> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWButton> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
