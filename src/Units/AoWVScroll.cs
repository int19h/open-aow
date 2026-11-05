// Translated from AoWVScroll.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;
using System.ComponentModel;

namespace Aow.Units.AoWVScroll
{
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
                var call__1 = FILIInc.CheckIL(FILIndexIncUp, 1, this);
                FILIInc = call__1;
            }

            if (FILIInc != null)
            {
                var call__2 = FILIInc.CheckIL(FILIndexIncDown, 1, this);
                FILIInc = call__2;
            }

            if (FILIDec != null)
            {
                var call__3 = FILIDec.CheckIL(FILIndexDecUp, 1, this);
                FILIDec = call__3;
            }

            if (FILIDec != null)
            {
                var call__4 = FILIDec.CheckIL(FILIndexDecDown, 1, this);
                FILIDec = call__4;
            }

            if (FILTBack != null)
            {
                var call__5 = FILTBack.CheckIL(FILIndexBack, 1, this);
                FILTBack = call__5;
            }

            if (FILLBack != null)
            {
                var call__6 = FILLBack.CheckIL(FILIndexBack, 3, this);
                FILLBack = call__6;
            }

            if (FILITab != null)
            {
                var call__7 = FILITab.CheckIL(FILIndexTab, 3, this);
                FILITab = call__7;
            }

            if (FILLTab != null)
            {
                var call__8 = FILLTab.CheckIL(FILIndexTab, 3, this);
                FILLTab = call__8;
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
                            FPosition = FPosition - FSmallChange;
                            if (FMin > FPosition)
                            {
                                FPosition = FMin;
                            }

                            FDrawn = false;
                            if (FOnChange != null)
                            {
                                FOnChange(this);
                            }

                            break;
                        case 4:
                            FPosition = FPosition + FSmallChange;
                            if (FMax + 1 - FNumberVisible < FPosition)
                            {
                                FPosition = FMax + 1 - FNumberVisible;
                            }

                            FDrawn = false;
                            if (FOnChange != null)
                            {
                                FOnChange(this);
                            }

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
                    var call__9 = FILTBack[FILIndexBack];
                    FWinWidth = call__9.Width;
                    var call__10 = FILIDec[FILIndexDecUp];
                    var call__11 = FILIInc[FILIndexIncUp];
                    if (call__10.Height + call__11.Height > FWinHeight)
                    {
                        var call__12 = FILIDec[FILIndexDecUp];
                        var call__13 = FILIInc[FILIndexIncUp];
                        FWinHeight = call__12.Height + call__13.Height;
                    }

                    var call__14 = FILIDec[FILIndexDecUp];
                    var call__15 = FILIInc[FILIndexIncUp];
                    F130 = FWinHeight - call__14.Height - call__15.Height;
                    if (F130 < 1)
                    {
                        F130 = 1;
                    }

                    F134 = F130 * FNumberVisible / F14C;
                    switch (FTabType)
                    {
                        case TaowTabType.ttIcon:
                            var call__16 = FILITab[FILIndexTab];
                            F134 = call__16.Height;
                            break;
                        case TaowTabType.ttBar:
                            var call__17 = FILLTab[FILIndexTab];
                            var call__18 = FILLTab[FILIndexTab + 2];
                            if (F134 < call__17.Height + call__18.Height)
                            {
                                var call__19 = FILLTab[FILIndexTab];
                                var call__20 = FILLTab[FILIndexTab + 2];
                                F134 = call__19.Height + call__20.Height;
                            }

                            break;
                    }

                    break;
                case TaowScrollType.stSlider:
                    var call__21 = FILLBack[FILIndexBack];
                    FWinWidth = call__21.Width;
                    F130 = FWinHeight;
                    F134 = F130 * FNumberVisible / F14C;
                    switch (FTabType)
                    {
                        case TaowTabType.ttIcon:
                            var call__22 = FILITab[FILIndexTab];
                            F134 = call__22.Height;
                            break;
                        case TaowTabType.ttBar:
                            var call__23 = FILLTab[FILIndexTab];
                            var call__24 = FILLTab[FILIndexTab + 2];
                            if (F134 < call__23.Height + call__24.Height)
                            {
                                var call__25 = FILLTab[FILIndexTab];
                                var call__26 = FILLTab[FILIndexTab + 2];
                                F134 = call__25.Height + call__26.Height;
                            }

                            break;
                    }

                    break;
                case TaowScrollType.stIncDec:
                    var call__27 = FILIDec[FILIndexDecUp];
                    FWinWidth = call__27.Width;
                    var call__28 = FILIDec[FILIndexDecUp];
                    var call__29 = FILIInc[FILIndexIncUp];
                    FWinHeight = call__28.Height + call__29.Height;
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
                                var call__30 = FILIInc[FILIndexIncDown];
                                var call__31 = FILIInc[FILIndexIncDown];
                                var call__32 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - call__31.Height, FWinWidth, call__30.Height);
                                FILIInc.DrawILT(FILIndexIncDown, call__32, ref SurfDesc);
                            }
                            else
                            {
                                var call__33 = FILIInc[FILIndexIncUp];
                                var call__34 = FILIInc[FILIndexIncDown];
                                var call__35 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - call__34.Height, FWinWidth, call__33.Height);
                                FILIInc.DrawILT(FILIndexIncUp, call__35, ref SurfDesc);
                            }

                            if (F151 == 1 && F150 > 0)
                            {
                                var call__36 = FILIDec[FILIndexDecDown];
                                var call__37 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, call__36.Height);
                                FILIDec.DrawILT(FILIndexDecDown, call__37, ref SurfDesc);
                            }
                            else
                            {
                                var call__38 = FILIDec[FILIndexDecUp];
                                var call__39 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, call__38.Height);
                                FILIDec.DrawILT(FILIndexDecUp, call__39, ref SurfDesc);
                            }

                            var call__40 = FILTBack[FILIndexBack];
                            var call__41 = FILIDec[FILIndexDecUp];
                            var call__42 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + call__41.Height, call__40.Width, F130);
                            FILTBack.DrawILT(FILIndexBack, call__42, ref SurfDesc);
                            TabPos = F130 * FPosition / F14C;
                            if (F134 + TabPos > F130)
                            {
                                TabPos = F130 - F134;
                            }

                            switch (FTabType)
                            {
                                case TaowTabType.ttIcon:
                                    var call__43 = FILIDec[FILIndexDecUp];
                                    var call__44 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + call__43.Height + TabPos, FWinWidth, F134);
                                    FILITab.DrawILT(FILIndexTab, call__44, ref SurfDesc);
                                    break;
                                case TaowTabType.ttBar:
                                    var call__45 = FILIDec[FILIndexDecUp];
                                    var call__46 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + call__45.Height + TabPos, FWinWidth, F134);
                                    FILLTab.DrawILLv(FILIndexTab, call__46, ref SurfDesc);
                                    break;
                            }

                            break;
                        case TaowScrollType.stSlider:
                            var call__47 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILLBack.DrawILLv(FILIndexBack, call__47, ref SurfDesc);
                            TabPos = F130 * FPosition / F14C;
                            if (F134 + TabPos > F130)
                            {
                                TabPos = F130 - F134;
                            }

                            switch (FTabType)
                            {
                                case TaowTabType.ttIcon:
                                    var call__48 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + TabPos, FWinWidth, F134);
                                    FILITab.DrawILT(FILIndexTab, call__48, ref SurfDesc);
                                    break;
                                case TaowTabType.ttBar:
                                    var call__49 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + TabPos, FWinWidth, F134);
                                    FILLTab.DrawILLv(FILIndexTab, call__49, ref SurfDesc);
                                    break;
                            }

                            break;
                        case TaowScrollType.stIncDec:
                            if (F151 == 4 && F150 > 0)
                            {
                                var call__50 = FILIInc[FILIndexIncDown];
                                var call__51 = FILIInc[FILIndexIncDown];
                                var call__52 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - call__51.Height, FWinWidth, call__50.Height);
                                FILIInc.DrawILT(FILIndexIncDown, call__52, ref SurfDesc);
                            }
                            else
                            {
                                var call__53 = FILIInc[FILIndexIncUp];
                                var call__54 = FILIInc[FILIndexIncDown];
                                var call__55 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - call__54.Height, FWinWidth, call__53.Height);
                                FILIInc.DrawILT(FILIndexIncUp, call__55, ref SurfDesc);
                            }

                            if (F151 == 1 && F150 > 0)
                            {
                                var call__56 = FILIDec[FILIndexDecDown];
                                var call__57 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, call__56.Height);
                                FILIDec.DrawILT(FILIndexDecDown, call__57, ref SurfDesc);
                            }
                            else
                            {
                                var call__58 = FILIDec[FILIndexDecUp];
                                var call__59 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, call__58.Height);
                                FILIDec.DrawILT(FILIndexDecUp, call__59, ref SurfDesc);
                            }

                            break;
                    }

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
                        var call__60 = FILIDec[FILIndexDecUp];
                        if (Y < FAOWWindow.FWinTop + FWinTop + call__60.Height - 1)
                        {
                            F151 = 1;
                            FPosition = FPosition - FSmallChange;
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

                        var call__61 = FILIInc[FILIndexIncUp];
                        if (Y > FAOWWindow.FWinTop + FWinTop + FWinHeight - call__61.Height)
                        {
                            F151 = 4;
                            FPosition = FPosition + FSmallChange;
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
                    }

                    if (F151 == 5)
                    {
                        switch (FScrollType)
                        {
                            case TaowScrollType.stScroll:
                                var call__62 = FILIDec[FILIndexDecUp];
                                bool bool__63 = Y > FAOWWindow.FWinTop + FWinTop + call__62.Height;
                                if (bool__63)
                                {
                                    var call__64 = FILIInc[FILIndexIncUp];
                                    bool__63 = Y < FAOWWindow.FWinTop + FWinTop + TabPos + call__64.Height;
                                }

                                if (bool__63)
                                {
                                    var call__65 = FILIDec[FILIndexDecUp];
                                    F154 = (Y - FAOWWindow.FWinTop - FWinTop - call__65.Height) * F14C / F130 + FMin;
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

                                var call__66 = FILIDec[FILIndexDecUp];
                                bool bool__67 = Y > FAOWWindow.FWinTop + FWinTop + call__66.Height + TabPos + F134;
                                if (bool__67)
                                {
                                    var call__68 = FILIInc[FILIndexIncUp];
                                    bool__67 = Y < FAOWWindow.FWinTop + FWinTop + FWinHeight - call__68.Height;
                                }

                                if (bool__67)
                                {
                                    var call__69 = FILIDec[FILIndexDecUp];
                                    F154 = (Y - FAOWWindow.FWinTop - FWinTop - call__69.Height) * F14C / F130 + FMin;
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
    }

    public static partial class AoWVScroll
    {
        // AoWVScroll.pas:706
        public static void Register()
        {
            RegisterComponents((AnsiString)"AOW Interface", [typeof(TAOWVScrollBar)]);
        }
    }
}
