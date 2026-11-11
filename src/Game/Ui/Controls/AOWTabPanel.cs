// Translated from AoWTabPanel.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

// { The AoWTabPanel unit of aowInt.dpl: TAOWTabPanel, a row of tabs, one per item, in a window.
//   Clicking a tab selects it; the panels (TAOWPanel) whose TabPanel is this control are shown when
//   their OwnerItem is the selected tab and hidden otherwise. }
// Fields: FItems, FSelected, FILButton, FBorderHeightSelected (the text offset of the selected
// tab), FILIndexUp, FILIndexDown, FPanels (the TAOWPanel objects of the tabs)
public partial class AOWTabPanel : AOWControl
{
    public StringList FItems;
    public int FSelected;
    public InterfaceIL FILButton;
    public int FBorderHeightSelected;
    public int FILIndexUp;
    public int FILIndexDown;
    public List<Windows.AOWPanel> FPanels;

    // AoWTabPanel.pas:62
    public AOWTabPanel(Component owner) : base(owner)
    {
        FItems = new StringList();
        FItems.OnChange = OnChangeText;
        FPanels = new List<Windows.AOWPanel>();
    }

    // AoWTabPanel.pas:70
    public override void Dispose()
    {
        AOWWindowControl P;
        for (int I = 0, last = FPanels.Count - 1; I <= last; I++)
        {
            P = FPanels[I];
            P.DetachMenuBar();
        }

        FPanels.Clear();
        FItems.Dispose();
        base.Dispose();
    }

    // AoWTabPanel.pas:112
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

        if (FILButton != null && FAOWFont != null)
        {
            FValid = true;
        }

        base.Validate();
    }

    // AoWTabPanel.pas:123
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
        if (FItems.Count <= FSelected)
        {
            FSelected = FItems.Count - 1;
        }

        base.SetSize();
    }

    // AoWTabPanel.pas:164
    // { Draw: the tabs side by side in equal shares of the width, the text centered; the selected tab
    //   with the down image and its text BorderHeightSelected pixels down }
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
                        FILButton.DrawILLh(FILIndexUp, rect, ref SurfDesc);
                        if (FAOWFont != null)
                        {
                            FAOWFont.Color = FSelColor;
                            var item = FItems[I];
                            var textWidth = FAOWFont.GetTextWidth(item);
                            var count2 = FItems.Count;
                            var item2 = FItems[I];
                            FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * (2 * I + 1) / (2 * count2) - textWidth / 2, FAOWWindow.FWinTop + FWinTop + FBorderHeight, item2, SurfDesc);
                        }
                    }
                    else
                    {
                        var count = FItems.Count;
                        var fItemsCount = FItems.Count;
                        var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * I / fItemsCount, FAOWWindow.FWinTop + FWinTop, FWinWidth / count, FWinHeight);
                        FILButton.DrawILLh(FILIndexDown, rect, ref SurfDesc);
                        if (FAOWFont != null)
                        {
                            FAOWFont.Color = FColor;
                            var item = FItems[I];
                            var textWidth = FAOWFont.GetTextWidth(item);
                            var count2 = FItems.Count;
                            var item2 = FItems[I];
                            FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * (2 * I + 1) / (2 * count2) - textWidth / 2, FAOWWindow.FWinTop + FWinTop + FBorderHeightSelected, item2, SurfDesc);
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

    // AoWTabPanel.pas:211
    // { CheckMouseDown: a left click on a tab selects it }
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (Button == MouseButton.Left)
            {
                var count = FItems.Count;
                SetIndex((X - FAOWWindow.FWinLeft - FWinLeft) / (FWinWidth / count));
            }

            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWTabPanel.pas:226
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

    // AoWTabPanel.pas:239
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

    // AoWTabPanel.pas:252
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AoWTabPanel.pas:85
    public virtual void SetILButton(InterfaceIL Value)
    {
        if (Value != FILButton)
        {
            FValid = false;
            FILButton = Value;
        }
    }

    // AoWTabPanel.pas:94
    public virtual void SetILUIndex(int Value)
    {
        if (Value != FILIndexUp)
        {
            FILIndexUp = Value;
            FValid = false;
        }
    }

    // AoWTabPanel.pas:103
    public virtual void SetILDIndex(int Value)
    {
        if (Value != FILIndexDown)
        {
            FILIndexDown = Value;
            FValid = false;
        }
    }

    // AoWTabPanel.pas:133
    public void SetFStrings(StringList Value)
    {
        FItems.Clear();
        for (int I = 0, last = Value.Count - 1; I <= last; I++)
        {
            var valueValue = Value[I];
            FItems.Add(valueValue);
        }
    }

    // AoWTabPanel.pas:143
    // { SetIndex selects a tab: the panels of the tab are shown, the others hidden }
    public void SetIndex(int Value)
    {
        if (Value != FSelected)
        {
            FSelected = Value;
            for (int I = 0, last = FPanels.Count - 1; I <= last; I++)
            {
                var component = FPanels[I];
                if (component.FOwnerItem == this.FSelected)
                {
                    component.SetVisible(true);
                }
                else
                {
                    component.SetVisible(false);
                }
            }

            if (FOnChange != null)
            {
                FOnChange(this);
            }

            FDrawn = false;
        }
    }

    [Published]
    public ImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public StringList Items { get => FItems; set => SetFStrings(value); }

    [Published]
    public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

    [Published]
    public int Selected { get => FSelected; set => SetIndex(value); }

    [Published]
    public InterfaceIL ILButton { get => FILButton; set => SetILButton(value); }

    [Published]
    public int ILIndexDown { get => FILIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ILIndexUp { get => FILIndexUp; set => SetILUIndex(value); }

    [Published]
    public int BorderHeightSelected { get => FBorderHeightSelected; set => FBorderHeightSelected = value; }

    public new NotifyEvent<AOWTabPanel> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWTabPanel> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWTabPanel> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWTabPanel> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWTabPanel> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWTabPanel> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWTabPanel> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWTabPanel> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWTabPanel> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
