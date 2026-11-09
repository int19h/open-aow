// Translated from AoWListBox.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.AOWMenu;
using Aow.Units.AoWVScroll;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWListBox;

public enum TLBType : byte
{
    lbNormal = 0,
    lbBlank = 1
}

// Fields: FItems, FVScrollBar, F11C (the rows shown), FSelected, F124 (set to -1 by SetSize,
// otherwise unused), FOnPreDrawItem, FOnDrawItem, F138 (the width the frame or the scroll bar
// takes at the right), F13C (the scroll bar is visible), FILFrame, FILTextureNormal,
// FILTextureSelected, FItemHeight, FOnDoubleClick, FTextAlign, F15C (the first item shown),
// F160 (0: drawn with the frame and the textures, 1: text only), FILIndexFrame,
// FILIndexTNormal, FILIndexTSelected, FPopupMenu, FDrawItem, FDrawItemRect (the item and its
// rectangle while OnPreDrawItem and OnDrawItem run), F188 (the item under the mouse, -1 for
// none)
public partial class TAOWListBox : TAOWControl
{
    public TStringList FItems;
    public TAOWVScrollBar FVScrollBar;
    public int F11C;
    public int FSelected;
    public int F124;
    public TAoWOnDrawItemEvent<TAOWListBox> FOnPreDrawItem;
    public TAoWOnDrawItemEvent<TAOWListBox> FOnDrawItem;
    public int F138;
    public bool F13C;
    public TInterfaceIL FILFrame;
    public TInterfaceIL FILTextureNormal;
    public TInterfaceIL FILTextureSelected;
    public int FItemHeight;
    public TNotifyEvent<TAOWListBox> FOnDoubleClick;
    public TaowTextAlign FTextAlign;
    public int F15C;
    public byte F160;
    public int FILIndexFrame;
    public int FILIndexTNormal;
    public int FILIndexTSelected;
    public TAOWMenu FPopupMenu;
    public int FDrawItem;
    public TRect FDrawItemRect;
    public int F188;

    // AoWListBox.pas:107
    public TAOWListBox(TComponent owner) : base(owner)
    {
        FItems = new TStringList();
        FItems.OnChange = OnChangeText;
        F11C = 3;
        FItemHeight = 10;
    }

    // AoWListBox.pas:116
    public override void Dispose()
    {
        FItems.Dispose();
        base.Dispose();
    }

    // AoWListBox.pas:124
    // { Validate: a list box needs only its font; without all three image libraries it is drawn as
    //   text only }
    public override void Validate()
    {
        if (FAOWFont != null)
        {
            if (FILFrame != null)
            {
                FILFrame = FILFrame.CheckIL(FILIndexFrame, 8, this);
            }

            if (FILTextureNormal != null)
            {
                FILTextureNormal = FILTextureNormal.CheckIL(FILIndexTNormal, 1, this);
            }

            if (FILTextureSelected != null)
            {
                FILTextureSelected = FILTextureSelected.CheckIL(FILIndexTSelected, 1, this);
            }

            if (FILFrame != null && FILTextureNormal != null && FILTextureSelected != null)
            {
                F160 = 0;
            }
            else
            {
                F160 = 1;
            }

            FValid = true;
        }

        base.Validate();
    }

    // AoWListBox.pas:143
    public override void SetSize()
    {
        if (FItemHeight < 1)
        {
            FItemHeight = 10;
        }

        switch (F160)
        {
            case 0:
                var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                var ilFrame = FILFrame[FILIndexFrame + 6];
                if (ilFrameHeight.Height + ilFrame.Height > FWinHeight)
                {
                    var ilFrame2 = FILFrame[FILIndexFrame + 1];
                    var ilFrame3 = FILFrame[FILIndexFrame + 6];
                    FWinHeight = ilFrame2.Height + ilFrame3.Height;
                }

                var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                var ilFrame4 = FILFrame[FILIndexFrame + 4];
                if (ilFrameWidth.Width + ilFrame4.Width > FWinWidth)
                {
                    var ilFrame2 = FILFrame[FILIndexFrame + 3];
                    var ilFrame3 = FILFrame[FILIndexFrame + 4];
                    FWinWidth = ilFrame2.Width + ilFrame3.Width;
                }

                var ilFrame5 = FILFrame[FILIndexFrame + 1];
                var ilFrame6 = FILFrame[FILIndexFrame + 6];
                F11C = (FWinHeight - ilFrame5.Height - ilFrame6.Height) / FItemHeight;
                F138 = FILFrame[FILIndexFrame + 4].Width;
                FMasked = false;
                break;
            case 1:
                F11C = FWinHeight / FItemHeight;
                F138 = 0;
                FMasked = true;
                break;
        }

        if (FItems.Count < F11C)
        {
            F11C = FItems.Count;
        }

        if (F11C < 1)
        {
            F11C = 1;
        }

        if (FItems.Count <= FSelected)
        {
            FSelected = FItems.Count - 1;
        }

        if (FItems.Count < F15C + F11C)
        {
            F15C = FItems.Count - F11C;
        }

        if (F15C < 0)
        {
            F15C = 0;
        }

        F13C = false;
        if (FVScrollBar != null)
        {
            F124 = -1;
            if (FVScrollBar.FAOWWindow != FAOWWindow)
            {
                FVScrollBar.SetAOWWindow(FAOWWindow);
            }

            FVScrollBar.Priority = FPriority + 1;
            FVScrollBar.SetFMin(0);
            var count = FItems.Count;
            FVScrollBar.SetFMax(count - 1);
            FVScrollBar.SetFPos(F15C);
            FVScrollBar.SetFNum(F11C);
            FVScrollBar.WinHeight = FWinHeight;
            FVScrollBar.SetLarge(F11C - 1);
            FVScrollBar.Visible = FVisible;
            if (FVScrollBar.FVanishWhenFull && FItems.Count - 1 < F11C)
            {
                FVScrollBar.Visible = false;
            }
            else
            {
                F138 = FVScrollBar.FWinWidth;
            }

            FVScrollBar.FSized = false;
            FVScrollBar.WinLeft = FWinLeft + FWinWidth - FVScrollBar.FWinWidth;
            FVScrollBar.WinTop = FWinTop;
            F13C = FVScrollBar.FVisible;
        }

        base.SetSize();
    }

    // AoWListBox.pas:386
    // { Update keeps the first item shown in step with the scroll bar }
    public override void Update()
    {
        if (FVisible && FVScrollBar != null && FItems.Count > 0)
        {
            if (FVScrollBar.FPosition != F15C)
            {
                F15C = FVScrollBar.FPosition;
                var count = FItems.Count;
                if (F15C + F11C > count)
                {
                    F15C = FItems.Count - F11C;
                }

                if (F15C < 0)
                {
                    F15C = 0;
                }

                FDrawn = false;
            }
        }

        base.Update();
    }

    // AoWListBox.pas:241
    public override void SetVisible(bool Value)
    {
        base.SetVisible(Value);
        if (FVScrollBar != null)
        {
            FVScrollBar.Visible = FVisible;
        }
    }

    // AoWListBox.pas:304
    // { Draw draws the frame and the texture, then the visible items (one more than fit: the loop runs
    //   to F11C, original) clipped to the inside, the selected one on its highlight texture }
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        int L = default;
        int W = default;
        int T = default;
        int H = default;
        TRect OldClip = default;
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
                switch (F160)
                {
                    case 0:
                        var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect, ref SurfDesc);
                        var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                        var ilFrame = FILFrame[FILIndexFrame + 6];
                        var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                        var ilFrame2 = FILFrame[FILIndexFrame + 1];
                        var ilFrame3 = FILFrame[FILIndexFrame + 3];
                        var rect2 = Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrame3.Width, FAOWWindow.FWinTop + FWinTop + ilFrame2.Height, FWinWidth - ilFrameWidth.Width - F138, FWinHeight - ilFrameHeight.Height - ilFrame.Height);
                        FILTextureNormal.DrawILT(FILIndexTNormal, rect2, ref SurfDesc);
                        var ilFrame4 = FILFrame[FILIndexFrame + 3];
                        L = FAOWWindow.FWinLeft + FWinLeft + ilFrame4.Width;
                        var ilFrame5 = FILFrame[FILIndexFrame + 1];
                        T = FAOWWindow.FWinTop + FWinTop + ilFrame5.Height;
                        var ilFrame6 = FILFrame[FILIndexFrame + 1];
                        var ilFrame7 = FILFrame[FILIndexFrame + 6];
                        H = FWinHeight - ilFrame6.Height - ilFrame7.Height;
                        var ilFrame8 = FILFrame[FILIndexFrame + 3];
                        W = FWinWidth - ilFrame8.Width - F138;
                        break;
                    default:
                        L = FAOWWindow.FWinLeft + FWinLeft;
                        T = FAOWWindow.FWinTop + FWinTop;
                        H = FWinHeight;
                        W = FWinWidth - F138;
                        break;
                }

                if (L + W < OldClip.Right)
                {
                    SurfDesc.ClipRect.Right = L + W;
                }

                if (T + H < OldClip.Bottom)
                {
                    SurfDesc.ClipRect.Bottom = T + H;
                }

                for (int I = 0, last = F11C; I <= last; I++)
                {
                    var count = FItems.Count;
                    if (F15C + I < count)
                    {
                        FDrawItem = F15C + I;
                        FDrawItemRect = Rect(L, FItemHeight * I + T, L + W, (I + 1) * FItemHeight + T);
                        if (FDrawItemRect.Bottom > SurfDesc.ClipRect.Bottom)
                        {
                            FDrawItemRect.Bottom = SurfDesc.ClipRect.Bottom;
                        }

                        if (FOnPreDrawItem != null)
                        {
                            FOnPreDrawItem(this, FDrawItemRect, FDrawItem, ref SurfDesc);
                        }

                        if (F15C + I == FSelected)
                        {
                            FAOWFont.Color = FSelColor;
                            if (F160 == 0)
                            {
                                var rect = Rect(L, FItemHeight * I + T, W, FItemHeight);
                                FILTextureSelected.DrawILT(FILIndexTSelected, rect, ref SurfDesc);
                            }
                        }
                        else
                        {
                            FAOWFont.Color = FColor;
                        }

                        switch (FTextAlign)
                        {
                            case TaowTextAlign.taLeft:
                                var item = FItems[F15C + I];
                                FAOWFont.SetText(FBorderWidth + L, FBorderHeight + T + FItemHeight * I, item, SurfDesc);
                                break;
                            case TaowTextAlign.taRight:
                                var item2 = FItems[F15C + I];
                                var textWidth = FAOWFont.GetTextWidth(item2);
                                var item3 = FItems[F15C + I];
                                FAOWFont.SetText(L + W - FBorderWidth - textWidth, FBorderHeight + T + FItemHeight * I, item3, SurfDesc);
                                break;
                            default:
                                var item4 = FItems[F15C + I];
                                var faowFontTextWidth = FAOWFont.GetTextWidth(item4);
                                var item5 = FItems[F15C + I];
                                FAOWFont.SetText(L + W / 2 - faowFontTextWidth / 2, FBorderHeight + T + FItemHeight * I, item5, SurfDesc);
                                break;
                        }

                        if (FOnDrawItem != null)
                        {
                            FOnDrawItem(this, FDrawItemRect, FDrawItem, ref SurfDesc);
                        }
                    }
                }

                SurfDesc.ClipRect = OldClip;
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

    // AoWListBox.pas:404
    // { CheckMouseDown selects the item under the mouse unless the owner already handled the click;
    //   a double click calls OnDoubleClick, the right button opens the popup menu at the mouse, moved
    //   back inside the window manager when it would stick out }
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        int I;
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (!FAOWOwner.FHandled && X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth - F138 && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (F160 == 0)
            {
                var ilFrame = FILFrame[FILIndexFrame];
                I = (Y - FAOWWindow.FWinTop - FWinTop - ilFrame.Height) / FItemHeight + F15C;
            }
            else
            {
                I = (Y - FAOWWindow.FWinTop - FWinTop) / FItemHeight + F15C;
            }

            if (FItems.Count > I)
            {
                if (I != FSelected)
                {
                    FSelected = I;
                    FDrawn = false;
                    if (FOnChange != null)
                    {
                        FOnChange(this);
                    }
                }

                if (Button == TMouseButton.mbLeft && (TSet<TShiftState>.Of(TShiftState.ssDouble) & Shift) == TSet<TShiftState>.Of(TShiftState.ssDouble) && FOnDoubleClick != null)
                {
                    FOnDoubleClick(this);
                }

                if (FSelected > -1 && Button == TMouseButton.mbRight && FPopupMenu != null)
                {
                    FPopupMenu.SetPopupOwner(this);
                    FPopupMenu.WinLeft = X;
                    FPopupMenu.WinTop = Y;
                    if (FPopupMenu.WinManager != null)
                    {
                        if (FPopupMenu.FHidden)
                        {
                            if (FPopupMenu.FMenuLeft + FPopupMenu.WinWidth > FPopupMenu.WinManager.WinWidth)
                            {
                                FPopupMenu.WinLeft = X - FPopupMenu.WinWidth;
                            }

                            if (FPopupMenu.FMenuTop + FPopupMenu.WinHeight > FPopupMenu.WinManager.WinHeight)
                            {
                                FPopupMenu.WinTop = Y - FPopupMenu.WinHeight;
                            }
                        }
                        else
                        {
                            if (FPopupMenu.WinLeft + FPopupMenu.WinWidth > FPopupMenu.WinManager.WinWidth)
                            {
                                FPopupMenu.WinLeft = X - FPopupMenu.WinWidth;
                            }

                            if (FPopupMenu.WinTop + FPopupMenu.WinHeight > FPopupMenu.WinManager.WinHeight)
                            {
                                FPopupMenu.WinTop = Y - FPopupMenu.WinHeight;
                            }
                        }
                    }

                    FPopupMenu.Visible = true;
                }
            }

            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWListBox.pas:460
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth - F138 && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWListBox.pas:475
    // { CheckMouseMove tracks the item under the mouse }
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (X < FAOWWindow.FWinLeft + FWinLeft || X > FAOWWindow.FWinLeft + FWinLeft + FWinWidth - F138 - 1 || Y < FAOWWindow.FWinTop + FWinTop || Y > FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
        {
            F188 = -1;
        }
        else
        {
            FHandled = true;
            if (F160 == 0)
            {
                var ilFrame = FILFrame[FILIndexFrame];
                F188 = (Y - FAOWWindow.FWinTop - FWinTop - ilFrame.Height) / FItemHeight + F15C;
            }
            else
            {
                F188 = (Y - FAOWWindow.FWinTop - FWinTop) / FItemHeight + F15C;
            }

            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // AoWListBox.pas:497
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AoWListBox.pas:275
    public virtual void SetILFrame(TInterfaceIL Value)
    {
        if (Value != FILFrame)
        {
            FValid = false;
            FILFrame = Value;
        }
    }

    // AoWListBox.pas:284
    public virtual void SetILTNorm(TInterfaceIL Value)
    {
        if (Value != FILTextureNormal)
        {
            FValid = false;
            FILTextureNormal = Value;
        }
    }

    // AoWListBox.pas:293
    public virtual void SetILTSel(TInterfaceIL Value)
    {
        if (Value != FILTextureSelected)
        {
            FValid = false;
            FILTextureSelected = Value;
        }
    }

    // AoWListBox.pas:202
    public void SetFStrings(TStringList Value)
    {
        FItems.Clear();
        FItems.AddStrings(Value);
    }

    // AoWListBox.pas:208
    public void SetIndex(int Value)
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

    // AoWListBox.pas:219
    public void SetListOff(int Value)
    {
        if (Value >= FItems.Count)
        {
            Value = FItems.Count - 1;
        }

        if (Value < 0)
        {
            Value = 0;
        }

        if (Value != F15C)
        {
            F15C = Value;
            FDrawn = false;
        }
    }

    // AoWListBox.pas:232
    public void SetVScroll(TAOWVScrollBar Value)
    {
        FVScrollBar = Value;
        if (Value != null)
        {
            Value.Priority = FPriority;
        }

        Priority = FPriority;
        FSized = false;
    }

    // AoWListBox.pas:248
    public void SetILFIndex(int Value)
    {
        if (Value != FILIndexFrame)
        {
            FILIndexFrame = Value;
            FValid = false;
        }
    }

    // AoWListBox.pas:257
    public void SetILTnIndex(int Value)
    {
        if (Value != FILIndexTNormal)
        {
            FILIndexTNormal = Value;
            FValid = false;
        }
    }

    // AoWListBox.pas:266
    public void SetILTsIndex(int Value)
    {
        if (Value != FILIndexTSelected)
        {
            FILIndexTSelected = Value;
            FValid = false;
        }
    }

    public int ListOffset { get => F15C; set => SetListOff(value); }

    [Published]
    public TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public TStringList Items { get => FItems; set => SetFStrings(value); }

    [Published]
    public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

    [Published]
    public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

    [Published]
    public TAOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

    [Published]
    public TNotifyEvent<TAOWListBox> OnDoubleClick { get => FOnDoubleClick; set => FOnDoubleClick = value; }

    [Published]
    public int Selected { get => FSelected; set => SetIndex(value); }

    [Published]
    public TAoWOnDrawItemEvent<TAOWListBox> OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

    [Published]
    public TAoWOnDrawItemEvent<TAOWListBox> OnPreDrawItem { get => FOnPreDrawItem; set => FOnPreDrawItem = value; }

    [Published]
    public TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

    [Published]
    public TInterfaceIL ILTextureNormal { get => FILTextureNormal; set => SetILTNorm(value); }

    [Published]
    public TInterfaceIL ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

    [Published]
    public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

    [Published]
    public int ILIndexTNormal { get => FILIndexTNormal; set => SetILTnIndex(value); }

    [Published]
    public int ILIndexTSelected { get => FILIndexTSelected; set => SetILTsIndex(value); }

    [Published]
    public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

    [Published]
    public TAOWMenu PopupMenu { get => FPopupMenu; set => FPopupMenu = value; }

    [Published]
    public TaowTextAlign TextAlign { get => FTextAlign; set => FTextAlign = value; }

    public new TNotifyEvent<TAOWListBox> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAOWListBox> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAOWListBox> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAOWListBox> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAOWListBox> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAOWListBox> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAOWListBox> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAOWListBox> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAOWListBox> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}

public static partial class AoWListBoxUnit
{
}
