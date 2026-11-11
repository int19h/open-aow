// Translated from AOWMenuBar.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

// { The AOWMenuBar unit of aowInt.dpl: TAOWMenuBar, a horizontal bar of buttons, one per item, in a
//   window. Clicking an item selects it and shows the menus (TAOWMenu) that belong to the item
//   (their OwnerItem is the item's index); clicking it again closes it. }
// Fields: FItems, FSelected (-1 for none), FILButton, FLastSelected, FILIndexUp, FILIndexDown,
// FMenus (the TAOWMenu objects whose OwnerMenuBar is this bar), F130 (only cleared and tested:
// a click on the item last selected then does nothing)
public partial class AOWMenuBar : AOWControl
{
    public StringList FItems;
    public int FSelected;
    public InterfaceIL FILButton;
    public int FLastSelected;
    public int FILIndexUp;
    public int FILIndexDown;
    public List<Windows.AOWMenu> FMenus;
    public bool F130;

    // AOWMenuBar.pas:65
    public AOWMenuBar(Component owner) : base(owner)
    {
        FItems = new StringList();
        FItems.OnChange = OnChangeText;
        FMenus = new List<Windows.AOWMenu>();
        FSelected = -1;
    }

    // AOWMenuBar.pas:74
    public override void Dispose()
    {
        AOWWindowControl M;
        for (int I = 0, last = FMenus.Count - 1; I <= last; I++)
        {
            M = FMenus[I];
            M.DetachMenuBar();
        }

        FMenus.Clear();
        FItems.Dispose();
        base.Dispose();
    }

    // AOWMenuBar.pas:98
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

        if (FILButton != null)
        {
            FValid = true;
        }

        base.Validate();
    }

    // AOWMenuBar.pas:110
    // { The bar is at least as wide as one up and one down button per item }
    public override void SetSize()
    {
        var ilButtonWidth = FILButton[FILIndexUp];
        var ilButton = FILButton[FILIndexUp + 2];
        var count = FItems.Count;
        if ((ilButtonWidth.Width + ilButton.Width) * count > FWinWidth)
        {
            var ilButton2 = FILButton[FILIndexUp];
            var ilButton3 = FILButton[FILIndexUp + 2];
            var fItemsCount = FItems.Count;
            FWinWidth = (ilButton2.Width + ilButton3.Width) * fItemsCount;
        }

        FWinHeight = FILButton[FILIndexUp].Height;
        for (int I = 0, last = FMenus.Count - 1; I <= last; I++)
        {
            (FMenus[I]).FAligned = false;
        }

        base.SetSize();
    }

    // AOWMenuBar.pas:122
    public override void ReAlign()
    {
        for (int I = 0, last = FMenus.Count - 1; I <= last; I++)
        {
            (FMenus[I]).FAligned = false;
        }

        base.ReAlign();
    }

    // AOWMenuBar.pas:160
    // { Draw: the items side by side, each in an equal share of the width; the selected one with the up
    //   image and the text one pixel lower }
    public override void Draw(SurfaceDesc SurfDesc)
    {
        Rect OldClip = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                OldClip = SurfDesc.ClipRect;
                for (int I = 0, last = FItems.Count - 1; I <= last; I++)
                {
                    if (I != FSelected)
                    {
                        var count = FItems.Count;
                        var fItemsCount = FItems.Count;
                        var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * I / fItemsCount, FAOWWindow.FWinTop + FWinTop, FWinWidth / count, FWinHeight);
                        FILButton.DrawILLh(FILIndexDown, rect, ref SurfDesc);
                        if (FAOWFont != null)
                        {
                            FAOWFont.Color = FSelColor;
                            var item = FItems[I];
                            var count2 = FItems.Count;
                            FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * I / count2 + FBorderWidth, FAOWWindow.FWinTop + FWinTop + FBorderHeight, item, SurfDesc);
                        }
                    }
                    else
                    {
                        var count = FItems.Count;
                        var fItemsCount = FItems.Count;
                        var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * I / fItemsCount, FAOWWindow.FWinTop + FWinTop, FWinWidth / count, FWinHeight);
                        FILButton.DrawILLh(FILIndexUp, rect, ref SurfDesc);
                        if (FAOWFont != null)
                        {
                            FAOWFont.Color = FColor;
                            var item = FItems[I];
                            var count2 = FItems.Count;
                            FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * I / count2 + FBorderWidth, FAOWWindow.FWinTop + FWinTop + FBorderHeight + 1, item, SurfDesc);
                        }
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

    // AOWMenuBar.pas:206
    // { CheckMouseDown: a left click on an item selects it and shows its menus, or closes it when it
    //   was selected; a click outside the bar closes the selected item }
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        int I = default;
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            FRedraw = true;
            if (Button == MouseButton.Left)
            {
                var count = FItems.Count;
                I = (X - FAOWWindow.FWinLeft - FWinLeft) / (FWinWidth / count);
                if (I == FSelected)
                {
                    FSelected = -1;
                    if (FOnChange != null)
                    {
                        FOnChange(this);
                    }

                    FDrawn = false;
                }
                else
                {
                    if (!F130 || (I > -1 && I != FLastSelected))
                    {
                        FSelected = I;
                        FLastSelected = I;
                        I = 0;
                        int last = FMenus.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            var component = FMenus[I];
                            if (component.FOwnerItem == this.FSelected)
                            {
                                component.SetVisible(true);
                            }
                        }

                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        FDrawn = false;
                    }
                }
            }

            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
        else
        {
            if (FSelected > -1)
            {
                FSelected = -1;
                FDrawn = false;
            }
        }

        F130 = false;
    }

    // AOWMenuBar.pas:251
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight && FValid == true && FVisible == true)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AOWMenuBar.pas:264
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (X >= FAOWWindow.FWinLeft + FWinLeft && X <= FAOWWindow.FWinLeft + FWinLeft + FWinWidth - 1 && Y >= FAOWWindow.FWinTop + FWinTop && Y <= FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
        {
            FHandled = true;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // AOWMenuBar.pas:277
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AOWMenuBar.pas:89
    public virtual void SetILButton(InterfaceIL Value)
    {
        if (Value != FILButton)
        {
            FValid = false;
            FILButton = Value;
        }
    }

    // AOWMenuBar.pas:131
    public virtual void SetILUIndex(int Value)
    {
        if (Value != FILIndexUp)
        {
            FILIndexUp = Value;
            FValid = false;
        }
    }

    // AOWMenuBar.pas:140
    public virtual void SetILDIndex(int Value)
    {
        if (Value != FILIndexDown)
        {
            FILIndexDown = Value;
            FValid = false;
        }
    }

    // AOWMenuBar.pas:149
    public void SetFStrings(StringList Value)
    {
        FItems.Clear();
        for (int I = 0, last = Value.Count - 1; I <= last; I++)
        {
            var valueValue = Value[I];
            FItems.Add(valueValue);
        }
    }

    [Published]
    public ImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public StringList Items { get => FItems; set => SetFStrings(value); }

    [Published]
    public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

    [Published]
    public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

    [Published]
    public int Selected { get => FSelected; set => FSelected = value; }

    [Published]
    public InterfaceIL ILButton { get => FILButton; set => SetILButton(value); }

    [Published]
    public int ILIndexDown { get => FILIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ILIndexUp { get => FILIndexUp; set => SetILUIndex(value); }

    public new NotifyEvent<AOWMenuBar> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWMenuBar> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWMenuBar> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWMenuBar> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWMenuBar> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWMenuBar> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWMenuBar> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWMenuBar> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWMenuBar> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
