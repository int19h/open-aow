// Translated from AoWVScroll.pas.
using System.ComponentModel;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

public enum TaowScrollType : byte
{
    stScroll = 0,
    stSlider = 1,
    stIncDec = 2
}

public enum TaowTabType : byte
{
    ttIcon = 0,
    ttBar = 1
}

// Fields: FVanishWhenFull, FILIInc (the increment button: down or right), FILIDec (the
// decrement button), FILTBack (the background of a scroll bar), FILLBack (the background line
// of a slider), FILITab (an icon tab), FILLTab (a line tab), F130 (the length of the track the
// tab moves in), F134 (the length of the tab), FMin, FMax, FSmallChange, FLargeChange,
// FPosition, F14C (the number of positions, FMax - FMin + 1), F150 (the updates the mouse
// button has been held. 0 when it is up), F151 (the part that was clicked: 1 the decrement
// button, 2 the track before the tab, 3 the track after it, 4 the increment button, 5 the tab),
// FDelay (the updates before a held button repeats), F154 (the position a click in the track
// pages to. the mouse offset into the tab while the tab is dragged), FNumberVisible, F15C
// (unused), FScrollType, FTabType, FILIndexIncUp, FILIndexIncDown, FILIndexDecUp,
// FILIndexDecDown, FILIndexBack, FILIndexTab
public partial class TAOWScrollBar : TAOWControl
{
    public bool FVanishWhenFull;
    public TInterfaceIL FILIInc;
    public TInterfaceIL FILIDec;
    public TInterfaceIL FILTBack;
    public TInterfaceIL FILLBack;
    public TInterfaceIL FILITab;
    public TInterfaceIL FILLTab;
    public int F130;
    public int F134;
    public int FMin;
    public int FMax;
    public int FSmallChange;
    public int FLargeChange;
    public int FPosition;
    public int F14C;
    public byte F150;
    public byte F151;
    public byte FDelay;
    public int F154;
    public int FNumberVisible;
    public int F15C;
    public TaowScrollType FScrollType;
    public TaowTabType FTabType;
    public int FILIndexIncUp;
    public int FILIndexIncDown;
    public int FILIndexDecUp;
    public int FILIndexDecDown;
    public int FILIndexBack;
    public int FILIndexTab;

    // AoWVScroll.pas:130
    public TAOWScrollBar(TComponent owner) : base(owner)
    {
        FSmallChange = 1;
        FLargeChange = 1;
        FMax = 1;
        FNumberVisible = 1;
        FDelay = 20;
    }

    // AoWVScroll.pas:251
    // { Validate checks the image libraries and picks the kind of scroll bar: both buttons with a
    //   background and a tab make a scroll bar, both buttons alone make an increment/decrement pair,
    //   a background line with a tab makes a slider; anything else leaves the scroll bar invalid }
    public override void Validate()
    {
        if (FILIInc != null)
        {
            FILIInc = FILIInc.CheckIL(FILIndexIncUp, 1, this);
        }

        if (FILIInc != null)
        {
            FILIInc = FILIInc.CheckIL(FILIndexIncDown, 1, this);
        }

        if (FILIDec != null)
        {
            FILIDec = FILIDec.CheckIL(FILIndexDecUp, 1, this);
        }

        if (FILIDec != null)
        {
            FILIDec = FILIDec.CheckIL(FILIndexDecDown, 1, this);
        }

        if (FILTBack != null)
        {
            FILTBack = FILTBack.CheckIL(FILIndexBack, 1, this);
        }

        if (FILLBack != null)
        {
            FILLBack = FILLBack.CheckIL(FILIndexBack, 3, this);
        }

        if (FILITab != null)
        {
            FILITab = FILITab.CheckIL(FILIndexTab, 3, this);
        }

        if (FILLTab != null)
        {
            FILLTab = FILLTab.CheckIL(FILIndexTab, 3, this);
        }

        if (FILITab != null)
        {
            FTabType = TaowTabType.ttIcon;
        }
        else
        {
            FTabType = TaowTabType.ttBar;
        }

        if (FILIInc != null && FILIDec != null)
        {
            FValid = true;
            if (FILTBack != null && (FILITab != null || FILLTab != null))
            {
                FScrollType = TaowScrollType.stScroll;
            }
            else
            {
                FScrollType = TaowScrollType.stIncDec;
            }
        }
        else
        {
            if (FILLBack != null && (FILITab != null || FILLTab != null))
            {
                FValid = true;
                FScrollType = TaowScrollType.stSlider;
            }
        }

        base.Validate();
    }

    // AoWVScroll.pas:351
    // { Update repeats the action of a held mouse button once the button has been held for Delay
    //   updates: a button steps by SmallChange, the track pages by LargeChange up to the position that
    //   was clicked }
    public override void Update()
    {
        if (FVisible && F150 > 0)
        {
            if (F150 < FDelay)
            {
                F150 = (byte)(F150 + 1);
            }
            else
            {
                switch (F151)
                {
                    case 1:
                        ApplySmallStep(true);
                        break;
                    case 4:
                        ApplySmallStep(false);
                        break;
                    case 2:
                        FPosition = FPosition - FLargeChange;
                        if (F154 > FPosition)
                        {
                            FPosition = F154;
                            F150 = 0;
                        }

                        FDrawn = false;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        break;
                    case 3:
                        FPosition = FPosition + FLargeChange;
                        if (F154 + 1 < FPosition)
                        {
                            FPosition = F154 + 1;
                            F150 = 0;
                        }

                        FDrawn = false;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        break;
                }
            }
        }

        base.Update();
    }

    // AoWVScroll.pas:194
    public virtual void SetILIUIndex(int Value)
    {
        if (Value != FILIndexIncUp)
        {
            FILIndexIncUp = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:203
    public virtual void SetILIDIndex(int Value)
    {
        if (Value != FILIndexIncDown)
        {
            FILIndexIncDown = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:212
    public virtual void SetILDUIndex(int Value)
    {
        if (Value != FILIndexDecUp)
        {
            FILIndexDecUp = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:221
    public virtual void SetILDDIndex(int Value)
    {
        if (Value != FILIndexDecDown)
        {
            FILIndexDecDown = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:230
    public virtual void SetILBIndex(int Value)
    {
        if (Value != FILIndexBack)
        {
            FILIndexBack = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:239
    public virtual void SetILTIndex(int Value)
    {
        if (Value != FILIndexTab)
        {
            FILIndexTab = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:140
    public virtual void SetILInc(TInterfaceIL Value)
    {
        if (Value != FILIInc)
        {
            FValid = false;
            FILIInc = Value;
        }
    }

    // AoWVScroll.pas:149
    public virtual void SetILDec(TInterfaceIL Value)
    {
        if (Value != FILIDec)
        {
            FValid = false;
            FILIDec = Value;
        }
    }

    // AoWVScroll.pas:158
    public virtual void SetILBack(TInterfaceIL Value)
    {
        if (Value != FILTBack)
        {
            FValid = false;
            FILTBack = Value;
        }
    }

    // AoWVScroll.pas:167
    public virtual void SetILLBack(TInterfaceIL Value)
    {
        if (Value != FILLBack)
        {
            FValid = false;
            FILLBack = Value;
        }
    }

    // AoWVScroll.pas:176
    public virtual void SetILITab(TInterfaceIL Value)
    {
        if (Value != FILITab)
        {
            FValid = false;
            FILITab = Value;
        }
    }

    // AoWVScroll.pas:185
    public virtual void SetILLTab(TInterfaceIL Value)
    {
        if (Value != FILLTab)
        {
            FValid = false;
            FILLTab = Value;
        }
    }

    // AoWVScroll.pas:289
    public void SetSmall(int Value)
    {
        if (Value < 1)
        {
            Value = 1;
        }

        FSmallChange = Value;
    }

    // AoWVScroll.pas:296
    public void SetLarge(int Value)
    {
        if (Value < 1)
        {
            Value = 1;
        }

        FLargeChange = Value;
    }

    // AoWVScroll.pas:303
    public void SetFMin(int Value)
    {
        if (Value > FMax)
        {
            Value = FMax;
        }

        if (Value != FMin)
        {
            FMin = Value;
            FSized = false;
        }
    }

    // AoWVScroll.pas:314
    public void SetFMax(int Value)
    {
        if (Value < FMin)
        {
            Value = FMin;
        }

        if (Value != FMax)
        {
            FMax = Value;
            FSized = false;
        }
    }

    // AoWVScroll.pas:325
    public void SetFNum(int Value)
    {
        if (Value > FMax - FMin + 1)
        {
            Value = FMax - FMin + 1;
        }

        if (Value < 1)
        {
            Value = 1;
        }

        if (Value != FNumberVisible)
        {
            FNumberVisible = Value;
            FSized = false;
        }
    }

    // AoWVScroll.pas:338
    public void SetFPos(int Value)
    {
        FPosition = Value;
        if (FPosition + FNumberVisible > FMax - FMin + 1)
        {
            FPosition = FMax - FMin - FNumberVisible + 1;
        }

        if (FPosition < 0)
        {
            FPosition = 0;
        }

        FDrawn = false;
    }

    [Published]
    [DefaultValue(20)]
    public byte Delay { get => FDelay; set => FDelay = value; }

    [Published]
    public int Min { get => FMin; set => SetFMin(value); }

    [Published]
    public int Max { get => FMax; set => SetFMax(value); }

    [Published]
    public int SmallChange { get => FSmallChange; set => SetSmall(value); }

    [Published]
    public int LargeChange { get => FLargeChange; set => SetLarge(value); }

    [Published]
    public int Position { get => FPosition; set => SetFPos(value); }

    [Published]
    public int NumberVisible { get => FNumberVisible; set => SetFNum(value); }

    [Published]
    public bool VanishWhenFull { get => FVanishWhenFull; set => FVanishWhenFull = value; }

    [Published]
    public TInterfaceIL ILIInc { get => FILIInc; set => SetILInc(value); }

    [Published]
    public TInterfaceIL ILIDec { get => FILIDec; set => SetILDec(value); }

    [Published]
    public TInterfaceIL ILTBack { get => FILTBack; set => SetILBack(value); }

    [Published]
    public TInterfaceIL ILLBack { get => FILLBack; set => SetILLBack(value); }

    [Published]
    public TInterfaceIL ILITab { get => FILITab; set => SetILITab(value); }

    [Published]
    public TInterfaceIL ILLTab { get => FILLTab; set => SetILLTab(value); }

    [Published]
    public int ILIndexIncUp { get => FILIndexIncUp; set => SetILIUIndex(value); }

    [Published]
    public int ILIndexIncDown { get => FILIndexIncDown; set => SetILIDIndex(value); }

    [Published]
    public int ILIndexDecUp { get => FILIndexDecUp; set => SetILDUIndex(value); }

    [Published]
    public int ILIndexDecDown { get => FILIndexDecDown; set => SetILDDIndex(value); }

    [Published]
    public int ILIndexBack { get => FILIndexBack; set => SetILBIndex(value); }

    [Published]
    public int ILIndexTab { get => FILIndexTab; set => SetILTIndex(value); }

    public new TNotifyEvent<TAOWScrollBar> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAOWScrollBar> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAOWScrollBar> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAOWScrollBar> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAOWScrollBar> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAOWScrollBar> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAOWScrollBar> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAOWScrollBar> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAOWScrollBar> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}

// the vertical scroll bar, the decrement button at the top and the increment button at the
// bottom
public partial class TAOWVScrollBar : TAOWScrollBar
{
    public TAOWVScrollBar(TComponent owner) : base(owner)
    { }

    // AoWVScroll.pas:408
    // { SetSize takes the width from the images and sizes the track and the tab: the tab of a line is
    //   proportional to the visible part of the range, but no shorter than its two end images }
    public override void SetSize()
    {
        F14C = FMax - FMin + 1;
        if (F14C < FNumberVisible)
        {
            FNumberVisible = F14C;
        }

        switch (FScrollType)
        {
            case TaowScrollType.stScroll:
                FWinWidth = FILTBack[FILIndexBack].Width;
                var iliDecHeight = FILIDec[FILIndexDecUp];
                var iliIncHeight = FILIInc[FILIndexIncUp];
                if (iliDecHeight.Height + iliIncHeight.Height > FWinHeight)
                {
                    var iliDec = FILIDec[FILIndexDecUp];
                    var iliInc = FILIInc[FILIndexIncUp];
                    FWinHeight = iliDec.Height + iliInc.Height;
                }

                var iliDec2 = FILIDec[FILIndexDecUp];
                var iliInc2 = FILIInc[FILIndexIncUp];
                F130 = FWinHeight - iliDec2.Height - iliInc2.Height;
                if (F130 < 1)
                {
                    F130 = 1;
                }

                F134 = F130 * FNumberVisible / F14C;
                switch (FTabType)
                {
                    case TaowTabType.ttIcon:
                        F134 = FILITab[FILIndexTab].Height;
                        break;
                    case TaowTabType.ttBar:
                        var illTabHeight = FILLTab[FILIndexTab];
                        var illTab = FILLTab[FILIndexTab + 2];
                        if (F134 < illTabHeight.Height + illTab.Height)
                        {
                            var illTab2 = FILLTab[FILIndexTab];
                            var illTab3 = FILLTab[FILIndexTab + 2];
                            F134 = illTab2.Height + illTab3.Height;
                        }

                        break;
                }

                break;
            case TaowScrollType.stSlider:
                FWinWidth = FILLBack[FILIndexBack].Width;
                F130 = FWinHeight;
                F134 = F130 * FNumberVisible / F14C;
                switch (FTabType)
                {
                    case TaowTabType.ttIcon:
                        F134 = FILITab[FILIndexTab].Height;
                        break;
                    case TaowTabType.ttBar:
                        var illTabHeight = FILLTab[FILIndexTab];
                        var illTab = FILLTab[FILIndexTab + 2];
                        if (F134 < illTabHeight.Height + illTab.Height)
                        {
                            var illTab2 = FILLTab[FILIndexTab];
                            var illTab3 = FILLTab[FILIndexTab + 2];
                            F134 = illTab2.Height + illTab3.Height;
                        }

                        break;
                }

                break;
            case TaowScrollType.stIncDec:
                FWinWidth = FILIDec[FILIndexDecUp].Width;
                var iliDec3 = FILIDec[FILIndexDecUp];
                var iliInc3 = FILIInc[FILIndexIncUp];
                FWinHeight = iliDec3.Height + iliInc3.Height;
                break;
        }

        if (FPosition + FNumberVisible > F14C)
        {
            FPosition = F14C - FNumberVisible;
        }

        base.SetSize();
    }

    // AoWVScroll.pas:457
    // { Draw draws the buttons (pressed while held), the background and the tab. The top of the
    //   increment button is computed from the height of its pressed image in both states (original) }
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        int TabPos = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                switch (FScrollType)
                {
                    case TaowScrollType.stScroll:
                        if (F151 == 4 && F150 > 0)
                        {
                            var iliIncHeight = FILIInc[FILIndexIncDown];
                            var iliInc = FILIInc[FILIndexIncDown];
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - iliInc.Height, FWinWidth, iliIncHeight.Height);
                            FILIInc.DrawILT(FILIndexIncDown, rect, ref SurfDesc);
                        }
                        else
                        {
                            var iliInc = FILIInc[FILIndexIncUp];
                            var iliIncHeight = FILIInc[FILIndexIncDown];
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - iliIncHeight.Height, FWinWidth, iliInc.Height);
                            FILIInc.DrawILT(FILIndexIncUp, rect, ref SurfDesc);
                        }

                        if (F151 == 1 && F150 > 0)
                        {
                            var iliDec = FILIDec[FILIndexDecDown];
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                            FILIDec.DrawILT(FILIndexDecDown, rect, ref SurfDesc);
                        }
                        else
                        {
                            var iliDec = FILIDec[FILIndexDecUp];
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                            FILIDec.DrawILT(FILIndexDecUp, rect, ref SurfDesc);
                        }

                        var iltBack = FILTBack[FILIndexBack];
                        var iliDecHeight = FILIDec[FILIndexDecUp];
                        var rect2 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + iliDecHeight.Height, iltBack.Width, F130);
                        FILTBack.DrawILT(FILIndexBack, rect2, ref SurfDesc);
                        TabPos = F130 * FPosition / F14C;
                        if (F134 + TabPos > F130)
                        {
                            TabPos = F130 - F134;
                        }

                        switch (FTabType)
                        {
                            case TaowTabType.ttIcon:
                                var iliDec = FILIDec[FILIndexDecUp];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + iliDec.Height + TabPos, FWinWidth, F134);
                                FILITab.DrawILT(FILIndexTab, rect, ref SurfDesc);
                                break;
                            case TaowTabType.ttBar:
                                var iliDec2 = FILIDec[FILIndexDecUp];
                                var rect3 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + iliDec2.Height + TabPos, FWinWidth, F134);
                                FILLTab.DrawILLv(FILIndexTab, rect3, ref SurfDesc);
                                break;
                        }

                        break;
                    case TaowScrollType.stSlider:
                        var rect4 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILLBack.DrawILLv(FILIndexBack, rect4, ref SurfDesc);
                        TabPos = F130 * FPosition / F14C;
                        if (F134 + TabPos > F130)
                        {
                            TabPos = F130 - F134;
                        }

                        switch (FTabType)
                        {
                            case TaowTabType.ttIcon:
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + TabPos, FWinWidth, F134);
                                FILITab.DrawILT(FILIndexTab, rect, ref SurfDesc);
                                break;
                            case TaowTabType.ttBar:
                                var rect3 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + TabPos, FWinWidth, F134);
                                FILLTab.DrawILLv(FILIndexTab, rect3, ref SurfDesc);
                                break;
                        }

                        break;
                    case TaowScrollType.stIncDec:
                        if (F151 == 4 && F150 > 0)
                        {
                            var iliIncHeight = FILIInc[FILIndexIncDown];
                            var iliInc = FILIInc[FILIndexIncDown];
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - iliInc.Height, FWinWidth, iliIncHeight.Height);
                            FILIInc.DrawILT(FILIndexIncDown, rect, ref SurfDesc);
                        }
                        else
                        {
                            var iliInc = FILIInc[FILIndexIncUp];
                            var iliIncHeight = FILIInc[FILIndexIncDown];
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - iliIncHeight.Height, FWinWidth, iliInc.Height);
                            FILIInc.DrawILT(FILIndexIncUp, rect, ref SurfDesc);
                        }

                        if (F151 == 1 && F150 > 0)
                        {
                            var iliDec = FILIDec[FILIndexDecDown];
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                            FILIDec.DrawILT(FILIndexDecDown, rect, ref SurfDesc);
                        }
                        else
                        {
                            var iliDec = FILIDec[FILIndexDecUp];
                            var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                            FILIDec.DrawILT(FILIndexDecUp, rect, ref SurfDesc);
                        }

                        break;
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

    // AoWVScroll.pas:549
    // { CheckMouseDown: a click on a button steps the position, a click in the track pages towards
    //   the click, a click on the tab starts dragging it (F151 stays 5). The track before the tab ends
    //   at the height of the increment button (original) }
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        int TabPos = default;
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (FVisible == true && X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (Button == TMouseButton.mbLeft)
            {
                F150 = 1;
                F151 = 5;
                TabPos = F130 * FPosition / F14C;
                if (TabPos + F134 > F130)
                {
                    TabPos = F130 - F134;
                }

                if (FScrollType != TaowScrollType.stSlider)
                {
                    var iliDec = FILIDec[FILIndexDecUp];
                    if (Y < FAOWWindow.FWinTop + FWinTop + iliDec.Height - 1)
                    {
                        F151 = 1;
                        ApplySmallStep(true);
                    }

                    var iliInc = FILIInc[FILIndexIncUp];
                    if (Y > FAOWWindow.FWinTop + FWinTop + FWinHeight - iliInc.Height)
                    {
                        F151 = 4;
                        ApplySmallStep(false);
                    }
                }

                if (F151 == 5)
                {
                    switch (FScrollType)
                    {
                        case TaowScrollType.stScroll:
                            var iliDecHeight = FILIDec[FILIndexDecUp];
                            bool condition = Y > FAOWWindow.FWinTop + FWinTop + iliDecHeight.Height;
                            if (condition)
                            {
                                var iliInc = FILIInc[FILIndexIncUp];
                                condition = Y < FAOWWindow.FWinTop + FWinTop + TabPos + iliInc.Height;
                            }

                            if (condition)
                            {
                                var iliDec = FILIDec[FILIndexDecUp];
                                F154 = (Y - FAOWWindow.FWinTop - FWinTop - iliDec.Height) * F14C / F130 + FMin;
                                F151 = 2;
                                FPosition = FPosition - FLargeChange;
                                if (FMin > FPosition)
                                {
                                    FPosition = FMin;
                                }

                                FDrawn = false;
                                if (FOnChange != null)
                                {
                                    FOnChange(this);
                                }
                            }

                            var iliDec2 = FILIDec[FILIndexDecUp];
                            bool test = Y > FAOWWindow.FWinTop + FWinTop + iliDec2.Height + TabPos + F134;
                            if (test)
                            {
                                var iliInc = FILIInc[FILIndexIncUp];
                                test = Y < FAOWWindow.FWinTop + FWinTop + FWinHeight - iliInc.Height;
                            }

                            if (test)
                            {
                                var iliDec = FILIDec[FILIndexDecUp];
                                F154 = (Y - FAOWWindow.FWinTop - FWinTop - iliDec.Height) * F14C / F130 + FMin;
                                F151 = 3;
                                FPosition = FPosition + FLargeChange;
                                if (FMax + 1 - FNumberVisible < FPosition)
                                {
                                    FPosition = FMax + 1 - FNumberVisible;
                                }

                                FDrawn = false;
                                if (FOnChange != null)
                                {
                                    FOnChange(this);
                                }
                            }

                            if (F151 == 5)
                            {
                                F154 = Y - TabPos;
                            }

                            break;
                        case TaowScrollType.stSlider:
                            if (Y > FAOWWindow.FWinTop + FWinTop && Y < FAOWWindow.FWinTop + FWinTop + TabPos)
                            {
                                F154 = (Y - FAOWWindow.FWinTop - FWinTop) * F14C / F130 + FMin;
                                F151 = 2;
                                FPosition = FPosition - FLargeChange;
                                if (FMin > FPosition)
                                {
                                    FPosition = FMin;
                                }

                                FDrawn = false;
                                if (FOnChange != null)
                                {
                                    FOnChange(this);
                                }
                            }

                            if (Y > FAOWWindow.FWinTop + FWinTop + TabPos + F134 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
                            {
                                F154 = (Y - FAOWWindow.FWinTop - FWinTop) * F14C / F130 + FMin;
                                F151 = 3;
                                FPosition = FPosition + FLargeChange;
                                if (FMax + 1 - FNumberVisible < FPosition)
                                {
                                    FPosition = FMax + 1 - FNumberVisible;
                                }

                                FDrawn = false;
                                if (FOnChange != null)
                                {
                                    FOnChange(this);
                                }
                            }

                            if (F151 == 5)
                            {
                                F154 = Y - TabPos;
                            }

                            break;
                    }
                }
            }

            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWVScroll.pas:661
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (F151 != 0)
        {
            FDrawn = false;
        }

        F150 = 0;
        F151 = 0;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWVScroll.pas:680
    // { CheckMouseMove drags the tab }
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (F151 == 5 && F150 > 0)
        {
            FPosition = (Y - F154) * F14C / F130 + FMin;
            if (FPosition < FMin)
            {
                FPosition = FMin;
            }

            if (FMax + 1 - FNumberVisible < FPosition)
            {
                FPosition = FMax + 1 - FNumberVisible;
            }

            FDrawn = false;
            if (FOnChange != null)
            {
                FOnChange(this);
            }
        }

        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    public new TNotifyEvent<TAOWVScrollBar> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAOWVScrollBar> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAOWVScrollBar> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAOWVScrollBar> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAOWVScrollBar> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAOWVScrollBar> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAOWVScrollBar> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAOWVScrollBar> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAOWVScrollBar> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
