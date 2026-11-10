// Translated from AoWComboBox.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

// { The AoWComboBox unit of aowInt.dpl: TAOWComboBox, a drop-down list. Closed, it shows the
//   selected item in an edit field; a click opens the list below the field (at PriorityOpened, with
//   a vertical scroll bar when one is attached), a click on an item selects it and closes the list. }
// Fields: F114 (the mouse button went down on the field), FOpen (the list is shown), FItems,
// FVScrollBar, F124 (the rows the open list shows), FNumberVisible, F12C (the first item
// shown), FListBorderWidth, FListBorderHeight, FSelected, F13C (the item under the mouse, -1
// for none), FPriorityOpened, F144 (the priority before the list was opened), FOnDrawItem, F150
// (the width the scroll bar takes from the list), F154 (the scroll bar is visible), FILEdit,
// FILFrame, FILTextureNormal, FILTextureSelected, FItemHeight, FOnOpen, FOnClose, FILEIndex,
// FILFIndex, FILTnIndex, FILTsIndex, FDrawItem, FDrawItemRect (the item and its rectangle while
// OnDrawItem runs)
public partial class AOWComboBox : AOWControl
{
    public bool F114;
    public bool FOpen;
    public StringList FItems;
    public AOWVScrollBar FVScrollBar;
    public int F124;
    public int FNumberVisible;
    public int F12C;
    public int FListBorderWidth;
    public int FListBorderHeight;
    public int FSelected;
    public int F13C;
    public int FPriorityOpened;
    public int F144;
    public NotifyEvent<AOWComboBox> FOnDrawItem;
    public int F150;
    public bool F154;
    public InterfaceIL FILEdit;
    public InterfaceIL FILFrame;
    public InterfaceIL FILTextureNormal;
    public InterfaceIL FILTextureSelected;
    public int FItemHeight;
    public NotifyEvent<AOWComboBox> FOnOpen;
    public NotifyEvent<AOWComboBox> FOnClose;
    public int FILEIndex;
    public int FILFIndex;
    public int FILTnIndex;
    public int FILTsIndex;
    public int FDrawItem;
    public Rect FDrawItemRect;

    // AoWComboBox.pas:114
    public AOWComboBox(Component owner) : base(owner)
    {
        FItems = new StringList();
        FItems.OnChange = OnChangeText;
        F13C = -1;
    }

    // AoWComboBox.pas:122
    public override void Dispose()
    {
        FItems.Dispose();
        base.Dispose();
    }

    // AoWComboBox.pas:128
    public override void Validate()
    {
        if (FILEdit != null)
        {
            FILEdit = FILEdit.CheckIL(FILEIndex, 3, this);
        }

        if (FILFrame != null)
        {
            FILFrame = FILFrame.CheckIL(FILFIndex, 8, this);
        }

        if (FILTextureNormal != null)
        {
            FILTextureNormal = FILTextureNormal.CheckIL(FILTnIndex, 1, this);
        }

        if (FILTextureSelected != null)
        {
            FILTextureSelected = FILTextureSelected.CheckIL(FILTsIndex, 1, this);
        }

        if (FAOWFont != null && FILEdit != null && FILFrame != null && FILTextureNormal != null && FILTextureSelected != null)
        {
            FValid = true;
        }

        base.Validate();
    }

    // AoWComboBox.pas:146
    // { SetSize closes the list and sets up the scroll bar. The scroll bar's range and page are one
    //   item short of the open list's, and its top is taken from the first edit image (original) }
    public override void SetSize()
    {
        FOpen = false;
        var ilEditWidth = FILEdit[FILEIndex];
        var ilEdit = FILEdit[FILEIndex + 2];
        if (ilEditWidth.Width + ilEdit.Width > FWinWidth)
        {
            var ilEdit2 = FILEdit[FILEIndex];
            var ilEdit3 = FILEdit[FILEIndex + 2];
            FWinWidth = ilEdit2.Width + ilEdit3.Width;
        }

        F124 = FNumberVisible;
        var count = FItems.Count;
        if (F124 > count)
        {
            F124 = FItems.Count;
        }

        if (FItems.Count <= FSelected)
        {
            FSelected = FItems.Count - 1;
        }

        FWinHeight = FILEdit[FILEIndex].Height;
        if (FVScrollBar != null)
        {
            if (FVScrollBar.FAOWWindow != FAOWWindow)
            {
                FVScrollBar.SetAOWWindow(FAOWWindow);
            }

            FVScrollBar.Priority = FPriority + 1;
            FVScrollBar.SetFMin(0);
            var fItemsCount = FItems.Count;
            FVScrollBar.SetFMax(fItemsCount - 2);
            FVScrollBar.SetFPos(F12C);
            FVScrollBar.SetFNum(F124 - 1);
            var ilFrame = FILFrame[FILFIndex + 6];
            FVScrollBar.WinHeight = ilFrame.Height + (FItemHeight * F124 + FBorderHeight);
            FVScrollBar.SetLarge(F124 - 1);
            FVScrollBar.Visible = FOpen;
            if (FVScrollBar.FVanishWhenFull && FItems.Count - 1 < F124)
            {
                FVScrollBar.Visible = false;
            }

            FVScrollBar.FSized = false;
            FVScrollBar.WinLeft = FWinLeft + FWinWidth - FVScrollBar.FWinWidth;
            var ilEditHeight = FILEdit[0];
            FVScrollBar.WinTop = ilEditHeight.Height + FWinTop;
            F154 = FVScrollBar.FVisible;
        }

        base.SetSize();
    }

    // AoWComboBox.pas:356
    // { Update keeps the first item shown in step with the scroll bar }
    public override void Update()
    {
        base.Update();
        if (FVisible)
        {
            FRedraw = FOpen;
            if (FVScrollBar != null && FItems.Count > 0)
            {
                if (FVScrollBar.FPosition != F12C)
                {
                    F12C = FVScrollBar.FPosition;
                    var count = FItems.Count;
                    if (F12C + F124 > count)
                    {
                        F12C = FItems.Count - F124;
                    }

                    FDrawn = false;
                }
            }
        }
    }

    // AoWComboBox.pas:204
    public override void SetVisible(bool Value)
    {
        base.SetVisible(Value);
        if (FOpen && FVScrollBar != null)
        {
            FVScrollBar.Visible = FVisible;
        }
    }

    // AoWComboBox.pas:286
    // { Draw draws the edit field with the selected item and, when the list is open, the frame, the
    //   texture of the list, the highlight of the item under the mouse and the items, clipped short of
    //   the scroll bar; OnDrawItem is called for every item with FDrawItem and FDrawItemRect set }
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

                FAOWFont.Color = FColor;
                OldClip = SurfDesc.ClipRect;
                var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                FILEdit.DrawILLh(FILEIndex, rect, ref SurfDesc);
                if (FSelected > -1 && FItems.Count > FSelected)
                {
                    var item = FItems[FSelected];
                    FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FBorderWidth, FAOWWindow.FWinTop + FWinTop + FBorderHeight, item, SurfDesc);
                }

                SurfDesc.ClipRect = OldClip;
                if (FOpen == true)
                {
                    var ilFrameHeight = FILFrame[FILFIndex + 1];
                    var ilFrame = FILFrame[FILFIndex + 6];
                    var ilEditHeight = FILEdit[FILEIndex];
                    var rect2 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + ilEditHeight.Height, FWinWidth, ilFrameHeight.Height + ilFrame.Height + FItemHeight * F124 + FListBorderHeight);
                    FILFrame.DrawILF(FILFIndex, rect2, ref SurfDesc);
                    var ilFrameWidth = FILFrame[FILFIndex + 3];
                    var ilFrame2 = FILFrame[FILFIndex + 4];
                    var ilEdit = FILEdit[FILEIndex];
                    var ilFrame3 = FILFrame[FILFIndex + 1];
                    var ilFrame4 = FILFrame[FILFIndex + 3];
                    var rect3 = new Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrame4.Width, FAOWWindow.FWinTop + FWinTop + ilEdit.Height + ilFrame3.Height, FWinWidth - ilFrameWidth.Width - ilFrame2.Width, FItemHeight * F124 + FListBorderHeight);
                    FILTextureNormal.DrawILT(FILTnIndex, rect3, ref SurfDesc);
                    if (F13C >= F12C && F13C < F124 + F12C)
                    {
                        var ilFrame5 = FILFrame[FILFIndex + 3];
                        var ilFrame6 = FILFrame[FILFIndex + 4];
                        var ilEdit2 = FILEdit[FILEIndex];
                        var ilFrame7 = FILFrame[FILFIndex + 1];
                        var ilFrame8 = FILFrame[FILFIndex + 3];
                        var rect4 = new Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrame8.Width, FAOWWindow.FWinTop + FWinTop + ilEdit2.Height + ilFrame7.Height + (F13C - F12C) * FItemHeight, FWinWidth - ilFrame5.Width - ilFrame6.Width, FItemHeight);
                        FILTextureSelected.DrawILT(FILTsIndex, rect4, ref SurfDesc);
                    }

                    if (FAOWWindow.FWinLeft + FWinLeft + FWinWidth - F150 < OldClip.Right)
                    {
                        SurfDesc.ClipRect.Right = FAOWWindow.FWinLeft + FWinLeft + FWinWidth - F150;
                    }

                    for (int I = 0, last = F124 - 1; I <= last; I++)
                    {
                        if (I < FItems.Count)
                        {
                            if (F12C + I == F13C)
                            {
                                FAOWFont.Color = FSelColor;
                            }

                            var ilEdit2 = FILEdit[FILEIndex];
                            var ilFrame5 = FILFrame[FILFIndex + 1];
                            var item = FItems[F12C + I];
                            FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FListBorderWidth, FAOWWindow.FWinTop + FWinTop + ilEdit2.Height + ilFrame5.Height + FListBorderHeight + FItemHeight * I, item, SurfDesc);
                            if (F12C + I == F13C)
                            {
                                FAOWFont.Color = FColor;
                            }

                            FDrawItem = F12C + I;
                            var ilEdit3 = FILEdit[FILEIndex + 1];
                            var ilFrame6 = FILFrame[FILFIndex + 1];
                            var ilEdit4 = FILEdit[FILEIndex + 1];
                            var ilFrame7 = FILFrame[FILFIndex + 1];
                            var ilFrame8 = FILFrame[FILFIndex + 3];
                            var ilFrame9 = FILFrame[FILFIndex + 3];
                            FDrawItemRect = new Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrame9.Width, FAOWWindow.FWinTop + FWinTop + ilEdit4.Height + ilFrame7.Height + FItemHeight * I, FAOWWindow.FWinLeft + FWinLeft + ilFrame8.Width + FWinWidth, FAOWWindow.FWinTop + FWinTop + ilEdit3.Height + ilFrame6.Height + (I + 1) * FItemHeight);
                            if (FOnDrawItem != null)
                            {
                                FOnDrawItem(this);
                            }
                        }
                    }

                    SurfDesc.ClipRect = OldClip;
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

    // AoWComboBox.pas:375
    // { CheckMouseDown opens or closes the list with a click on the edit field and closes it with a
    //   click outside the control }
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        bool condition = X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1;
        if (condition)
        {
            var ilEdit = FILEdit[FILEIndex];
            condition = Y < FAOWWindow.FWinTop + FWinTop + ilEdit.Height;
        }

        if (condition)
        {
            FHandled = true;
            if (Button == MouseButton.Left)
            {
                FMouseDown = true;
                F114 = true;
                if (FValid == true && FVisible == true)
                {
                    if (FOpen)
                    {
                        if (F144 != FPriority)
                        {
                            Priority = F144;
                        }

                        FOpen = false;
                        FWinHeight = FILEdit[FILEIndex].Height;
                        if (FVScrollBar != null)
                        {
                            FVScrollBar.Visible = false;
                        }

                        if (FOnClose != null)
                        {
                            FOnClose(this);
                        }
                    }
                    else
                    {
                        F144 = FPriority;
                        if (FPriorityOpened != FPriority)
                        {
                            Priority = FPriorityOpened;
                        }

                        FOpen = true;
                        if (FItems.Count < F124)
                        {
                            F124 = FItems.Count;
                        }

                        var ilEditHeight = FILEdit[FILEIndex];
                        var ilFrame = FILFrame[FILFIndex + 1];
                        var ilFrameHeight = FILFrame[FILFIndex + 6];
                        FWinHeight = ilEditHeight.Height + FListBorderHeight + FItemHeight * F124 + ilFrame.Height + ilFrameHeight.Height;
                        if (FItems.Count < F12C + F124)
                        {
                            F12C = FItems.Count - F124;
                        }

                        FDrawn = false;
                        F150 = FILFrame[FILFIndex + 4].Width;
                        F154 = false;
                        if (FVScrollBar != null && F124 > 0)
                        {
                            if (FPriority + 1 != FVScrollBar.FPriority)
                            {
                                FVScrollBar.Priority = FPriority + 1;
                            }

                            FVScrollBar.SetFMin(0);
                            var count = FItems.Count;
                            FVScrollBar.SetFMax(count - 1);
                            FVScrollBar.SetFPos(F12C);
                            FVScrollBar.SetFNum(F124);
                            var ilFrame2 = FILFrame[FILFIndex + 6];
                            FVScrollBar.WinHeight = ilFrame2.Height + (FItemHeight * F124 + FBorderHeight);
                            FVScrollBar.Visible = true;
                            if (FVScrollBar.FVanishWhenFull == true && FItems.Count - 1 < F124)
                            {
                                FVScrollBar.Visible = false;
                            }
                            else
                            {
                                F150 = FVScrollBar.FWinWidth;
                            }

                            F154 = FVScrollBar.FVisible;
                            FVScrollBar.FSized = false;
                            FVScrollBar.WinLeft = FWinLeft + FWinWidth - FVScrollBar.FWinWidth;
                            var ilEdit = FILEdit[FILEIndex];
                            FVScrollBar.WinTop = ilEdit.Height + FWinTop;
                            FVScrollBar.FDrawn = false;
                        }

                        if (FOnOpen != null)
                        {
                            FOnOpen(this);
                        }
                    }
                }
                else
                {
                    if (FOpen == true)
                    {
                        if (F144 != FPriority)
                        {
                            Priority = F144;
                        }

                        FOpen = false;
                        FWinHeight = FILEdit[FILEIndex].Height;
                        if (FVScrollBar != null)
                        {
                            FVScrollBar.Visible = false;
                        }

                        if (FOnClose != null)
                        {
                            FOnClose(this);
                        }
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
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = true;
            }
            else
            {
                if (FOpen)
                {
                    if (F144 != FPriority)
                    {
                        Priority = F144;
                    }

                    FOpen = false;
                    FWinHeight = FILEdit[FILEIndex].Height;
                    if (FVScrollBar != null)
                    {
                        FVScrollBar.Visible = false;
                    }

                    if (FOnClose != null)
                    {
                        FOnClose(this);
                    }
                }
            }
        }
    }

    // AoWComboBox.pas:476
    // { CheckMouseUp selects the item under the mouse in the open list and closes the list }
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        int I = default;
        FHandled = false;
        FMouseDown = false;
        F114 = false;
        FRedraw = false;
        bool condition = X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1;
        if (condition)
        {
            var ilEdit = FILEdit[FILEIndex];
            condition = Y < FAOWWindow.FWinTop + FWinTop + ilEdit.Height;
        }

        if (condition)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
        else
        {
            if (Button == MouseButton.Left && FValid == true && FVisible == true && FOpen == true)
            {
                bool test = X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth;
                if (test)
                {
                    var ilEdit = FILEdit[FILEIndex];
                    test = Y > FAOWWindow.FWinTop + FWinTop + ilEdit.Height - 1;
                }

                if (test && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
                {
                    if (X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth - F150)
                    {
                        var ilEdit = FILEdit[FILEIndex];
                        I = (Y - FAOWWindow.FWinTop - FWinTop - FBorderHeight - ilEdit.Height) / FItemHeight + F12C;
                        if (I < FItems.Count)
                        {
                            FSelected = I;
                        }

                        FOpen = false;
                        FRedraw = false;
                        FWinHeight = FILEdit[FILEIndex].Height;
                        if (FVScrollBar != null)
                        {
                            FVScrollBar.Visible = false;
                        }

                        FHandled = true;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        if (FOnClose != null)
                        {
                            FOnClose(this);
                        }
                    }
                }
                else
                {
                    FOpen = false;
                    FRedraw = false;
                    FWinHeight = FILEdit[FILEIndex].Height;
                    if (FVScrollBar != null)
                    {
                        FVScrollBar.Visible = false;
                    }

                    if (FOnClose != null)
                    {
                        FOnClose(this);
                    }
                }
            }
        }
    }

    // AoWComboBox.pas:530
    // { CheckMouseMove tracks the item under the mouse in the open list }
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        int OldItem;
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        OldItem = F13C;
        F13C = -1;
        if (X >= FAOWWindow.FWinLeft + FWinLeft && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth - F150 && Y >= FAOWWindow.FWinTop + FWinTop && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOpen)
            {
                var ilEditHeight = FILEdit[FILEIndex];
                bool condition = Y >= FAOWWindow.FWinTop + FWinTop + FBorderHeight + ilEditHeight.Height;
                if (condition)
                {
                    var ilEdit = FILEdit[FILEIndex];
                    condition = Y < FAOWWindow.FWinTop + FWinTop + ilEdit.Height + FBorderHeight + FItemHeight * F124;
                }

                if (condition)
                {
                    var ilEdit = FILEdit[FILEIndex];
                    F13C = (Y - FAOWWindow.FWinTop - FWinTop - FBorderHeight - ilEdit.Height) / FItemHeight + F12C;
                }
            }

            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }

        if (FOpen == true && OldItem != F13C)
        {
            FDrawn = false;
            if (FVScrollBar != null)
            {
                FVScrollBar.FDrawn = false;
            }
        }
    }

    // AoWComboBox.pas:211
    public virtual void SetILEdit(InterfaceIL Value)
    {
        if (Value != FILEdit)
        {
            FValid = false;
            FILEdit = Value;
        }
    }

    // AoWComboBox.pas:220
    public virtual void SetILEIndex(int Value)
    {
        if (Value != FILEIndex)
        {
            FILEIndex = Value;
            FValid = false;
        }
    }

    // AoWComboBox.pas:229
    public virtual void SetILFrame(InterfaceIL Value)
    {
        if (Value != FILFrame)
        {
            FValid = false;
            FILFrame = Value;
        }
    }

    // AoWComboBox.pas:238
    public virtual void SetILFIndex(int Value)
    {
        if (Value != FILFIndex)
        {
            FILFIndex = Value;
            FValid = false;
        }
    }

    // AoWComboBox.pas:247
    public virtual void SetILTNorm(InterfaceIL Value)
    {
        if (Value != FILTextureNormal)
        {
            FValid = false;
            FILTextureNormal = Value;
        }
    }

    // AoWComboBox.pas:256
    public virtual void SetILTnIndex(int Value)
    {
        if (Value != FILTnIndex)
        {
            FILTnIndex = Value;
            FValid = false;
        }
    }

    // AoWComboBox.pas:265
    public virtual void SetILTSel(InterfaceIL Value)
    {
        if (Value != FILTextureSelected)
        {
            FValid = false;
            FILTextureSelected = Value;
        }
    }

    // AoWComboBox.pas:274
    public virtual void SetILTsIndex(int Value)
    {
        if (Value != FILTsIndex)
        {
            FILTsIndex = Value;
            FValid = false;
        }
    }

    // AoWComboBox.pas:179
    public virtual void SetIndex(int Value)
    {
        if (Value >= FItems.Count)
        {
            Value = FItems.Count - 1;
        }

        if (Value != FSelected)
        {
            FSelected = Value;
            FDrawn = false;
        }
    }

    // AoWComboBox.pas:190
    public void SetFStrings(StringList Value)
    {
        FItems.Clear();
        FItems.AddStrings(Value);
    }

    // AoWComboBox.pas:196
    public void SetVScroll(AOWVScrollBar Value)
    {
        FVScrollBar = Value;
        if (Value != null)
        {
            Value.Priority = FPriority;
        }

        FSized = false;
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
    public int ListBorderWidth { get => FListBorderWidth; set => FListBorderWidth = value; }

    [Published]
    public int ListBorderHeight { get => FListBorderHeight; set => FListBorderHeight = value; }

    [Published]
    public AOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

    [Published]
    public int NumberVisible { get => FNumberVisible; set => FNumberVisible = value; }

    [Published]
    public int Selected { get => FSelected; set => SetIndex(value); }

    [Published]
    public int PriorityOpened { get => FPriorityOpened; set => FPriorityOpened = value; }

    [Published]
    public NotifyEvent<AOWComboBox> OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

    [Published]
    public InterfaceIL ILEdit { get => FILEdit; set => SetILEdit(value); }

    [Published]
    public int ILEIndex { get => FILEIndex; set => SetILEIndex(value); }

    [Published]
    public InterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

    [Published]
    public int ILFIndex { get => FILFIndex; set => SetILFIndex(value); }

    [Published]
    public InterfaceIL ILTextureNormal { get => FILTextureNormal; set => SetILTNorm(value); }

    [Published]
    public int ILTnIndex { get => FILTnIndex; set => SetILTnIndex(value); }

    [Published]
    public InterfaceIL ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

    [Published]
    public int ILTsIndex { get => FILTsIndex; set => SetILTsIndex(value); }

    [Published]
    public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

    [Published]
    public NotifyEvent<AOWComboBox> OnOpen { get => FOnOpen; set => FOnOpen = value; }

    [Published]
    public NotifyEvent<AOWComboBox> OnClose { get => FOnClose; set => FOnClose = value; }

    public new NotifyEvent<AOWComboBox> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWComboBox> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWComboBox> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWComboBox> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWComboBox> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWComboBox> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWComboBox> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWComboBox> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWComboBox> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
