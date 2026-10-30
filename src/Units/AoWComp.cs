// Translated from AoWComp.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;

namespace Aow.Units.AoWComp
{
    public enum TButtonType : byte
    {
        btText = 0,
        btIcon = 1
    }

    public enum TaowTextAlign : byte
    {
        taLeft = 0,
        taCenter = 1,
        taRight = 2
    }

    public enum TAOWAlignHeight : byte
    {
        ahNone = 0,
        ahTop = 1,
        ahBottom = 2,
        ahBoth = 3,
        ahCenter = 4
    }

    public enum TAOWAlignWidth : byte
    {
        awNone = 0,
        awLeft = 1,
        awRight = 2,
        awBoth = 3,
        awCenter = 4
    }

    public enum TAOWDrawMethod : byte
    {
        dmBlank = 0,
        dmBorderless = 1,
        dmHollowBox = 2,
        dmFilledBox = 3
    }

    // { AoWComp.TAoWComponent (instance size 260). Fields: +$24 FColor, +$28 FSelColor, +$2C FValid
//     (Validate found everything it needs), +$2D FAligned, +$2E FSized, +$30 FCursor, +$34 FOnDraw,
//     +$3C FOnPreDraw, +$44 FOnMouseDown, +$4C FOnMouseUp, +$54 FOnMouseMove, +$5C FOnKeyPress, +$64
//     FOnKeyDown, +$6C FOnChange, +$74 FMouseDown, +$75 FVisible, +$78 FHandled (a Check* method
//     consumed the input), +$7C FWinWidth, +$80 FWinHeight, +$84 FWinLeft, +$88 FWinTop, +$8C
//     FAOWFont, +$90 FText, +$94 FRedraw, +$98 FBorderWidth, +$9C FBorderHeight, +$A0 FPriority,
//     +$A4 FDrawn (the component is drawn as it is now), +$A5 FFirstDraw, +$A6 FMouseIn, +$A8
//     FToolTipCount, +$AC FRadioIndex, +$B0 FChecked, +$B4 FOwnerItem, +$B8 FOnUpdate, +$C0
//     FCaptureInput, +$C4 FOnDebugMessage, +$CC FAOWOwner, +$D0 FChildren (sorted by FPriority),
//     +$D4 FAlignment, +$D8 FMasked, +$DC FBltList, +$E0 FDrawList (TaowRect), +$E4 FBltCount, +$E8
//     FDrawCount, +$EC FToolTipLeft, +$F0 FOldRect, +$100 FHidden, +$101 FDoubleBuffered, +$102
//     FSurfaceValid.
//     VMT (after TComponent's): Create, OnChangeText, Validate, SetSize, ReAlign, Setup, Update,
//     ClearDrawList, ClearBltList, InvalidateRect, AddBltRect, AddDrawRect, SetOwner, SetWinLeft,
//     SetWinTop, SetWidth, SetHeight, SetPriority, SetVisible, SetColor, SetSelColor, DrawSurface,
//     DrawSurfDesc, Draw, CheckMouseDown, CheckMouseUp, CheckMouseMove, CheckKeyPress,
//     CheckKeyDown }
    public partial class TAoWComponent : Aow.Rtl.TComponent
    {
        public int FColor;
        public int FSelColor;
        public bool FValid;
        public bool FAligned;
        public bool FSized;
        public int FCursor;
        public Aow.Units.AoWComp.TAoWOnDrawEvent FOnDraw;
        public Aow.Units.AoWComp.TAoWOnDrawEvent FOnPreDraw;
        public Aow.Rtl.TMouseEvent FOnMouseDown;
        public Aow.Rtl.TMouseEvent FOnMouseUp;
        public Aow.Rtl.TMouseMoveEvent FOnMouseMove;
        public Aow.Rtl.TKeyPressEvent FOnKeyPress;
        public Aow.Rtl.TKeyEvent FOnKeyDown;
        public Aow.Rtl.TNotifyEvent FOnChange;
        public bool FMouseDown;
        public bool FVisible;
        public bool FHandled;
        public int FWinWidth;
        public int FWinHeight;
        public int FWinLeft;
        public int FWinTop;
        public Aow.Units.ImageLib.TImageLibraryFont FAOWFont;
        public Aow.Rtl.AnsiString FText;
        public bool FRedraw;
        public int FBorderWidth;
        public int FBorderHeight;
        public int FPriority;
        public bool FDrawn;
        public bool FFirstDraw;
        public bool FMouseIn;
        public int FToolTipCount;
        public int FRadioIndex;
        public bool FChecked;
        public int FOwnerItem;
        public Aow.Rtl.TNotifyEvent FOnUpdate;
        public bool FCaptureInput;
        public Aow.Units.AoWComp.TAoWOnDebugEvent FOnDebugMessage;
        public Aow.Units.AoWComp.TAoWComponent FAOWOwner;
        public Aow.Rtl.TList FChildren;
        public Aow.Units.AoWComp.TAOWAlignment FAlignment;
        public bool FMasked;
        public Aow.Rtl.TList FBltList;
        public Aow.Rtl.TList FDrawList;
        public int FBltCount;
        public int FDrawCount;
        public int FToolTipLeft;
        public Aow.Rtl.TRect FOldRect;
        public bool FHidden;
        public bool FDoubleBuffered;
        public bool FSurfaceValid;
        // AoWComp.pas:222
        public TAoWComponent(TComponent owner) : base(owner)
        {
            FVisible = true;
            FFirstDraw = true;
            var call__1 = new Aow.Units.AoWComp.TAOWAlignment();
            FAlignment = call__1;
            var call__2 = new Aow.Rtl.TList();
            FChildren = call__2;
            var call__3 = new Aow.Rtl.TList();
            FBltList = call__3;
            var call__4 = new Aow.Rtl.TList();
            FDrawList = call__4;
        }

        // AoWComp.pas:233
        public override void Destroy()
        {
            int I = default;
            SetOwner(null);
            while (FChildren.Count > 0)
            {
                var call__5 = FChildren[0];
                ((Aow.Units.AoWComp.TAoWComponent)call__5).SetOwner(null);
            }

            FChildren.Free();
            FAlignment.Free();
            {
                I = 0;
                int end__6 = FDrawList.Count - 1;
                for (; I <= end__6; I += 1)
                {
                    var call__7 = FDrawList[I];
                    ((Aow.Units.AoWComp.TaowRect)call__7).Free();
                }
            }

            {
                I = 0;
                int end__8 = FBltList.Count - 1;
                for (; I <= end__8; I += 1)
                {
                    var call__9 = FBltList[I];
                    ((Aow.Units.AoWComp.TaowRect)call__9).Free();
                }
            }

            FBltList.Free();
            FDrawList.Free();
            base.Destroy();
        }

        // AoWComp.pas:251
        public virtual void OnChangeText(Aow.Rtl.TObject Sender)
        {
            FSized = false;
        }

        // AoWComp.pas:300
        public virtual void Validate()
        {
            int I = default;
            FSized = false;
            FAligned = false;
            {
                I = 0;
                int end__10 = FChildren.Count - 1;
                for (; I <= end__10; I += 1)
                {
                    var call__11 = FChildren[I];
                    ((Aow.Units.AoWComp.TAoWComponent)call__11).FValid = false;
                }
            }
        }

        // AoWComp.pas:678
        public virtual void SetSize()
        {
            int I = default;
            FSized = true;
            FDrawn = false;
            FBltCount = 0;
            FDrawCount = 0;
            {
                I = 0;
                int end__12 = FChildren.Count - 1;
                for (; I <= end__12; I += 1)
                {
                    var call__13 = FChildren[I];
                    ((Aow.Units.AoWComp.TAoWComponent)call__13).FAligned = false;
                }
            }
        }

        // AoWComp.pas:690
        public virtual void ReAlign()
        {
            int I = default;
            if ((FAOWOwner != null) && ((FAlignment.FAlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awNone) || (FAlignment.FAlignHeight != Aow.Units.AoWComp.TAOWAlignHeight.ahNone)))
            {
                switch (this.FAlignment.FAlignWidth)
                {
                    case Aow.Units.AoWComp.TAOWAlignWidth.awLeft:
                        FWinLeft = ((FAOWOwner.FWinWidth * this.FAlignment.FLeftOffPercent) / 100) + this.FAlignment.FLeftOffset;
                        if (FAlignment.FWidthPercent > 0)
                        {
                            FWinWidth = (FAOWOwner.FWinWidth * FAlignment.FWidthPercent) / 100;
                        }

                        break;
                    case Aow.Units.AoWComp.TAOWAlignWidth.awRight:
                        if (this.FAlignment.FWidthPercent > 0)
                        {
                            FWinWidth = (FAOWOwner.FWinWidth * this.FAlignment.FWidthPercent) / 100;
                        }

                        FWinLeft = ((FAOWOwner.FWinWidth - FAlignment.FRightOffset) - FWinWidth) - ((FAOWOwner.FWinWidth * FAlignment.FRightOffPercent) / 100);
                        break;
                    case Aow.Units.AoWComp.TAOWAlignWidth.awBoth:
                        FWinLeft = ((FAOWOwner.FWinWidth * this.FAlignment.FLeftOffPercent) / 100) + this.FAlignment.FLeftOffset;
                        FWinWidth = ((FAOWOwner.FWinWidth - FWinLeft) - FAlignment.FRightOffset) - ((FAOWOwner.FWinWidth * FAlignment.FRightOffPercent) / 100);
                        break;
                    case Aow.Units.AoWComp.TAOWAlignWidth.awCenter:
                        FWinLeft = (FAOWOwner.FWinWidth / 2) - (FWinWidth / 2);
                        break;
                }

                switch (this.FAlignment.FAlignHeight)
                {
                    case Aow.Units.AoWComp.TAOWAlignHeight.ahTop:
                        FWinTop = ((FAOWOwner.FWinHeight * this.FAlignment.FTopOffPercent) / 100) + this.FAlignment.FTopOffset;
                        if (FAlignment.FHeightPercent > 0)
                        {
                            FWinHeight = (FAOWOwner.FWinHeight * FAlignment.FHeightPercent) / 100;
                        }

                        break;
                    case Aow.Units.AoWComp.TAOWAlignHeight.ahBottom:
                        if (this.FAlignment.FHeightPercent > 0)
                        {
                            FWinHeight = (FAOWOwner.FWinHeight * this.FAlignment.FHeightPercent) / 100;
                        }

                        FWinTop = ((FAOWOwner.FWinHeight - FAlignment.FBottomOffset) - FWinHeight) - ((FAOWOwner.FWinHeight * FAlignment.FBottomOffPercent) / 100);
                        break;
                    case Aow.Units.AoWComp.TAOWAlignHeight.ahBoth:
                        FWinTop = ((FAOWOwner.FWinHeight * this.FAlignment.FTopOffPercent) / 100) + this.FAlignment.FTopOffset;
                        FWinHeight = ((FAOWOwner.FWinHeight - FWinTop) - FAlignment.FBottomOffset) - ((FAOWOwner.FWinHeight * FAlignment.FBottomOffPercent) / 100);
                        break;
                    case Aow.Units.AoWComp.TAOWAlignHeight.ahCenter:
                        FWinTop = (FAOWOwner.FWinHeight / 2) - (FWinHeight / 2);
                        break;
                }
            }

            FAligned = true;
            FSized = false;
            {
                I = 0;
                int end__16 = FChildren.Count - 1;
                for (; I <= end__16; I += 1)
                {
                    var call__17 = FChildren[I];
                    ((Aow.Units.AoWComp.TAoWComponent)call__17).FAligned = false;
                }
            }
        }

        // AoWComp.pas:753
        // { Setup brings the component up to date: Validate, SetSize and ReAlign when their flag is clear,
//   then Setup of every visible child; an exception in any step is reported in a message box }
        public virtual void Setup()
        {
            int I = default;
            if (FVisible)
            {
                try
                {
                    if (!FValid)
                    {
                        Validate();
                    }
                }
                catch (System.Exception)
                {
                    Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Validate", ((AnsiString)"Error - ") + Name, (int)0);
                }

                if (FValid)
                {
                    try
                    {
                        if (!FSized)
                        {
                            SetSize();
                        }
                    }
                    catch (System.Exception)
                    {
                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during SetSize", ((AnsiString)"Error - ") + Name, (int)0);
                    }

                    try
                    {
                        if (!FAligned)
                        {
                            ReAlign();
                        }
                    }
                    catch (System.Exception)
                    {
                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during ReAlign", ((AnsiString)"Error - ") + Name, (int)0);
                    }

                    try
                    {
                        if (!FSized)
                        {
                            SetSize();
                        }
                    }
                    catch (System.Exception)
                    {
                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during SetSize", ((AnsiString)"Error - ") + Name, (int)0);
                    }

                    try
                    {
                        {
                            I = 0;
                            int end__18 = FChildren.Count - 1;
                            for (; I <= end__18; I += 1)
                            {
                                var call__19 = FChildren[I];
                                if (((Aow.Units.AoWComp.TAoWComponent)call__19).FVisible)
                                {
                                    var call__20 = FChildren[I];
                                    ((Aow.Units.AoWComp.TAoWComponent)call__20).Setup();
                                }
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Setup", ((AnsiString)"Error - ") + Name, (int)0);
                    }
                }
            }
        }

        // AoWComp.pas:796
        public virtual void Update()
        {
            int I = default;
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnUpdate != null)
                    {
                        FOnUpdate(this);
                    }

                    {
                        I = FChildren.Count - 1;
                        byte end__21 = 0;
                        for (; I >= end__21; I -= 1)
                        {
                            var call__22 = FChildren[I];
                            ((Aow.Units.AoWComp.TAoWComponent)call__22).Update();
                        }
                    }

                    if ((!FValid || !FAligned) || !FSized)
                    {
                        Setup();
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Update", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AoWComp.pas:202
        // { TAoWComponent }
        public virtual void ClearDrawList()
        {
            int I = default;
            FDrawCount = 0;
            {
                I = 0;
                int end__23 = FChildren.Count - 1;
                for (; I <= end__23; I += 1)
                {
                    var call__24 = FChildren[I];
                    if (((Aow.Units.AoWComp.TAoWComponent)call__24).FVisible)
                    {
                        var call__25 = FChildren[I];
                        ((Aow.Units.AoWComp.TAoWComponent)call__25).ClearDrawList();
                    }
                }
            }
        }

        // AoWComp.pas:212
        public virtual void ClearBltList()
        {
            int I = default;
            FBltCount = 0;
            {
                I = 0;
                int end__26 = FChildren.Count - 1;
                for (; I <= end__26; I += 1)
                {
                    var call__27 = FChildren[I];
                    if (((Aow.Units.AoWComp.TAoWComponent)call__27).FVisible)
                    {
                        var call__28 = FChildren[I];
                        ((Aow.Units.AoWComp.TAoWComponent)call__28).ClearBltList();
                    }
                }
            }
        }

        // AoWComp.pas:576
        public virtual void InvalidateRect(int Index, ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
        }

        // AoWComp.pas:314
        // { AddBltRect adds ARect to the rectangles to blit, clipped against the rectangles already in the
//   list so that no two overlap: the parts of ARect that another rectangle covers are cut off (a
//   cut that leaves two pieces adds one of them as a new rectangle), and an old rectangle that ARect
//   covers on one side is trimmed or emptied. }
        public virtual void AddBltRect(ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
            Aow.Units.AoWComp.TaowRect N = default;
            int I = default;
            int Count = default;
            bool Add = default;
            Aow.Units.AoWComp.TaowRect R = default;
            if (FValid && FVisible)
            {
                I = 0;
                Count = FBltCount;
                Add = true;
                if (ARect.Left < 0)
                {
                    ARect.Left = 0;
                }

                if (ARect.Top < 0)
                {
                    ARect.Top = 0;
                }

                if ((ARect.Right < ARect.Left) || (ARect.Bottom < ARect.Top))
                {
                    Add = false;
                }
                else
                {
                    while ((I < Count) && Add)
                    {
                        var call__29 = FBltList[I];
                        R = (Aow.Units.AoWComp.TaowRect)call__29;
                        if ((((ARect.Right >= R.R.Left) && (ARect.Left <= R.R.Right)) && (ARect.Bottom >= R.R.Top)) && (ARect.Top <= R.R.Bottom))
                        {
                            if (R.R.Left <= ARect.Left)
                            {
                                if (ARect.Right <= R.R.Right)
                                {
                                    if (ARect.Top >= R.R.Top)
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            Add = false;
                                        }
                                        else
                                        {
                                            ARect.Top = R.R.Bottom + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            ARect.Bottom = R.R.Top - 1;
                                        }
                                        else
                                        {
                                            if (FBltCount == FBltList.Count)
                                            {
                                                var call__30 = new Aow.Units.AoWComp.TaowRect();
                                                FBltList.Add(call__30);
                                            }

                                            var call__31 = FBltList[FBltCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__31;
                                            var call__32 = Aow.Rtl.Rtl.Rect(ARect.Left, ARect.Top, ARect.Right, R.R.Top - 1);
                                            N.R = call__32;
                                            FBltCount += 1;
                                            ARect.Top = R.R.Bottom + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ARect.Top >= R.R.Top)
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            ARect.Left = R.R.Right + 1;
                                        }
                                        else
                                        {
                                            if (FBltCount == FBltList.Count)
                                            {
                                                var call__33 = new Aow.Units.AoWComp.TaowRect();
                                                FBltList.Add(call__33);
                                            }

                                            var call__34 = FBltList[FBltCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__34;
                                            var call__35 = Aow.Rtl.Rtl.Rect(ARect.Left, R.R.Bottom + 1, R.R.Right, ARect.Bottom);
                                            N.R = call__35;
                                            FBltCount += 1;
                                            ARect.Left = R.R.Right + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            if (FBltCount == FBltList.Count)
                                            {
                                                var call__36 = new Aow.Units.AoWComp.TaowRect();
                                                FBltList.Add(call__36);
                                            }

                                            var call__37 = FBltList[FBltCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__37;
                                            var call__38 = Aow.Rtl.Rtl.Rect(ARect.Left, ARect.Top, R.R.Right, R.R.Top - 1);
                                            N.R = call__38;
                                            FBltCount += 1;
                                            ARect.Left = R.R.Right + 1;
                                        }
                                        else
                                        {
                                            R.R.Right = ARect.Left - 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (ARect.Right <= R.R.Right)
                                {
                                    if (ARect.Top >= R.R.Top)
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            ARect.Right = R.R.Left - 1;
                                        }
                                        else
                                        {
                                            if (FBltCount == FBltList.Count)
                                            {
                                                var call__39 = new Aow.Units.AoWComp.TaowRect();
                                                FBltList.Add(call__39);
                                            }

                                            var call__40 = FBltList[FBltCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__40;
                                            var call__41 = Aow.Rtl.Rtl.Rect(R.R.Left, R.R.Bottom + 1, ARect.Right, ARect.Bottom);
                                            N.R = call__41;
                                            FBltCount += 1;
                                            ARect.Right = R.R.Left - 1;
                                        }
                                    }
                                    else
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            if (FBltCount == FBltList.Count)
                                            {
                                                var call__42 = new Aow.Units.AoWComp.TaowRect();
                                                FBltList.Add(call__42);
                                            }

                                            var call__43 = FBltList[FBltCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__43;
                                            var call__44 = Aow.Rtl.Rtl.Rect(R.R.Left, ARect.Top, ARect.Right, R.R.Top - 1);
                                            N.R = call__44;
                                            FBltCount += 1;
                                            ARect.Right = R.R.Left - 1;
                                        }
                                        else
                                        {
                                            R.R.Left = ARect.Right + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ARect.Top >= R.R.Top)
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            if (FBltCount == FBltList.Count)
                                            {
                                                var call__45 = new Aow.Units.AoWComp.TaowRect();
                                                FBltList.Add(call__45);
                                            }

                                            var call__46 = FBltList[FBltCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__46;
                                            var call__47 = Aow.Rtl.Rtl.Rect(ARect.Left, ARect.Top, R.R.Left - 1, ARect.Bottom);
                                            N.R = call__47;
                                            FBltCount += 1;
                                            ARect.Left = R.R.Right + 1;
                                        }
                                        else
                                        {
                                            R.R.Bottom = ARect.Top - 1;
                                        }
                                    }
                                    else
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            R.R.Top = ARect.Bottom + 1;
                                        }
                                        else
                                        {
                                            var call__48 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                                            R.R = call__48;
                                        }
                                    }
                                }
                            }
                        }

                        I += 1;
                    }
                }

                if (Add)
                {
                    if (FBltCount == FBltList.Count)
                    {
                        var call__49 = new Aow.Units.AoWComp.TaowRect();
                        FBltList.Add(call__49);
                    }

                    var call__50 = FBltList[FBltCount];
                    R = (Aow.Units.AoWComp.TaowRect)call__50;
                    R.R = ARect;
                    FBltCount += 1;
                }
            }
        }

        // AoWComp.pas:445
        // { AddDrawRect: the same for the rectangles to redraw; the rectangle added last records Sender }
        public virtual void AddDrawRect(ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
            Aow.Units.AoWComp.TaowRect N = default;
            int I = default;
            int Count = default;
            bool Add = default;
            Aow.Units.AoWComp.TaowRect R = default;
            if (FValid && FVisible)
            {
                I = 0;
                Count = FDrawCount;
                Add = true;
                if (ARect.Left < 0)
                {
                    ARect.Left = 0;
                }

                if (ARect.Top < 0)
                {
                    ARect.Top = 0;
                }

                if ((ARect.Right < ARect.Left) || (ARect.Bottom < ARect.Top))
                {
                    Add = false;
                }
                else
                {
                    while ((I < Count) && Add)
                    {
                        var call__51 = FDrawList[I];
                        R = (Aow.Units.AoWComp.TaowRect)call__51;
                        if ((((ARect.Right >= R.R.Left) && (ARect.Left <= R.R.Right)) && (ARect.Bottom >= R.R.Top)) && (ARect.Top <= R.R.Bottom))
                        {
                            if (R.R.Left <= ARect.Left)
                            {
                                if (ARect.Right <= R.R.Right)
                                {
                                    if (ARect.Top >= R.R.Top)
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            Add = false;
                                        }
                                        else
                                        {
                                            ARect.Top = R.R.Bottom + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            ARect.Bottom = R.R.Top - 1;
                                        }
                                        else
                                        {
                                            if (FDrawCount == FDrawList.Count)
                                            {
                                                var call__52 = new Aow.Units.AoWComp.TaowRect();
                                                FDrawList.Add(call__52);
                                            }

                                            var call__53 = FDrawList[FDrawCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__53;
                                            var call__54 = Aow.Rtl.Rtl.Rect(ARect.Left, ARect.Top, ARect.Right, R.R.Top - 1);
                                            N.R = call__54;
                                            FDrawCount += 1;
                                            ARect.Top = R.R.Bottom + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ARect.Top >= R.R.Top)
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            ARect.Left = R.R.Right + 1;
                                        }
                                        else
                                        {
                                            if (FDrawCount == FDrawList.Count)
                                            {
                                                var call__55 = new Aow.Units.AoWComp.TaowRect();
                                                FDrawList.Add(call__55);
                                            }

                                            var call__56 = FDrawList[FDrawCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__56;
                                            var call__57 = Aow.Rtl.Rtl.Rect(ARect.Left, R.R.Bottom + 1, R.R.Right, ARect.Bottom);
                                            N.R = call__57;
                                            FDrawCount += 1;
                                            ARect.Left = R.R.Right + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            if (FDrawCount == FDrawList.Count)
                                            {
                                                var call__58 = new Aow.Units.AoWComp.TaowRect();
                                                FDrawList.Add(call__58);
                                            }

                                            var call__59 = FDrawList[FDrawCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__59;
                                            var call__60 = Aow.Rtl.Rtl.Rect(ARect.Left, ARect.Top, R.R.Right, R.R.Top - 1);
                                            N.R = call__60;
                                            FDrawCount += 1;
                                            ARect.Left = R.R.Right + 1;
                                        }
                                        else
                                        {
                                            R.R.Right = ARect.Left - 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (ARect.Right <= R.R.Right)
                                {
                                    if (ARect.Top >= R.R.Top)
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            ARect.Right = R.R.Left - 1;
                                        }
                                        else
                                        {
                                            if (FDrawCount == FDrawList.Count)
                                            {
                                                var call__61 = new Aow.Units.AoWComp.TaowRect();
                                                FDrawList.Add(call__61);
                                            }

                                            var call__62 = FDrawList[FDrawCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__62;
                                            var call__63 = Aow.Rtl.Rtl.Rect(R.R.Left, R.R.Bottom + 1, ARect.Right, ARect.Bottom);
                                            N.R = call__63;
                                            FDrawCount += 1;
                                            ARect.Right = R.R.Left - 1;
                                        }
                                    }
                                    else
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            if (FDrawCount == FDrawList.Count)
                                            {
                                                var call__64 = new Aow.Units.AoWComp.TaowRect();
                                                FDrawList.Add(call__64);
                                            }

                                            var call__65 = FDrawList[FDrawCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__65;
                                            var call__66 = Aow.Rtl.Rtl.Rect(R.R.Left, ARect.Top, ARect.Right, R.R.Top - 1);
                                            N.R = call__66;
                                            FDrawCount += 1;
                                            ARect.Right = R.R.Left - 1;
                                        }
                                        else
                                        {
                                            R.R.Left = ARect.Right + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ARect.Top >= R.R.Top)
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            if (FDrawCount == FDrawList.Count)
                                            {
                                                var call__67 = new Aow.Units.AoWComp.TaowRect();
                                                FDrawList.Add(call__67);
                                            }

                                            var call__68 = FDrawList[FDrawCount];
                                            N = (Aow.Units.AoWComp.TaowRect)call__68;
                                            var call__69 = Aow.Rtl.Rtl.Rect(ARect.Left, ARect.Top, R.R.Left - 1, ARect.Bottom);
                                            N.R = call__69;
                                            FDrawCount += 1;
                                            ARect.Left = R.R.Right + 1;
                                        }
                                        else
                                        {
                                            R.R.Bottom = ARect.Top - 1;
                                        }
                                    }
                                    else
                                    {
                                        if (ARect.Bottom <= R.R.Bottom)
                                        {
                                            R.R.Top = ARect.Bottom + 1;
                                        }
                                        else
                                        {
                                            var call__70 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                                            R.R = call__70;
                                        }
                                    }
                                }
                            }
                        }

                        I += 1;
                    }
                }

                if (Add)
                {
                    if (FDrawCount == FDrawList.Count)
                    {
                        var call__71 = new Aow.Units.AoWComp.TaowRect();
                        FDrawList.Add(call__71);
                    }

                    var call__72 = FDrawList[FDrawCount];
                    R = (Aow.Units.AoWComp.TaowRect)call__72;
                    R.R = ARect;
                    R.Owner = Sender;
                    FDrawCount += 1;
                }
            }
        }

        // AoWComp.pas:262
        public virtual void SetOwner(Aow.Units.AoWComp.TAoWComponent Value)
        {
            int I = default;
            int NewIndex = default;
            int OldIndex = default;
            if (FAOWOwner != null)
            {
                var call__73 = FAOWOwner.FChildren.IndexOf(this);
                OldIndex = call__73;
                FAOWOwner.FChildren.Delete(OldIndex);
            }
            else
            {
                OldIndex = -2;
            }

            if (Value != FAOWOwner)
            {
                FValid = false;
            }

            FAOWOwner = Value;
            NewIndex = -1;
            if (FAOWOwner != null)
            {
                I = 0;
                while (I < FAOWOwner.FChildren.Count)
                {
                    var call__74 = FAOWOwner.FChildren[I];
                    if (((Aow.Units.AoWComp.TAoWComponent)call__74).FPriority > FPriority)
                    {
                        NewIndex = I;
                        I = FAOWOwner.FChildren.Count;
                    }
                    else
                    {
                        I += 1;
                    }
                }

                if (NewIndex == -1)
                {
                    FAOWOwner.FChildren.Add(this);
                }
                else
                {
                    FAOWOwner.FChildren.Insert(NewIndex, this);
                }
            }

            if (NewIndex != OldIndex)
            {
                if (FDoubleBuffered)
                {
                    FSurfaceValid = false;
                }
                else
                {
                    FDrawn = false;
                }
            }
        }

        // AoWComp.pas:580
        public virtual void SetWinLeft(int Value)
        {
            FWinLeft = Value;
            FDrawn = false;
        }

        // AoWComp.pas:586
        public virtual void SetWinTop(int Value)
        {
            FWinTop = Value;
            FDrawn = false;
        }

        // AoWComp.pas:592
        public virtual void SetWidth(int Value)
        {
            FWinWidth = Value;
            FSized = false;
        }

        // AoWComp.pas:598
        public virtual void SetHeight(int Value)
        {
            FWinHeight = Value;
            FSized = false;
        }

        // AoWComp.pas:604
        public virtual void SetPriority(int Value)
        {
            FPriority = Value;
            SetOwner(FAOWOwner);
        }

        // AoWComp.pas:610
        public virtual void SetVisible(bool Value)
        {
            if (Value != FVisible)
            {
                FVisible = Value;
                FDrawn = false;
                if (FVisible)
                {
                    FHidden = false;
                    ClearBltList();
                    ClearDrawList();
                    if ((!FValid || !FAligned) || !FSized)
                    {
                        Setup();
                    }
                }
                else
                {
                    FHidden = true;
                }
            }
        }

        // AoWComp.pas:629
        public virtual void SetColor(int Value)
        {
            FColor = Value;
            FDrawn = false;
        }

        // AoWComp.pas:635
        public virtual void SetSelColor(int Value)
        {
            FSelColor = Value;
            FDrawn = false;
        }

        // AoWComp.pas:641
        public virtual void DrawSurface(Aow.Units.GFXE.TSurface Surface)
        {
        }

        // AoWComp.pas:645
        public virtual void DrawSurfDesc(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
        }

        // AoWComp.pas:649
        public virtual void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
        }

        // AoWComp.pas:653
        public virtual void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
        }

        // AoWComp.pas:658
        public virtual void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
        }

        // AoWComp.pas:663
        public virtual void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
        }

        // AoWComp.pas:668
        public virtual void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AoWComp.pas:673
        public virtual void CheckKeyDown(ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            FHandled = false;
        }

        // AoWComp.pas:256
        public void SetAlign(Aow.Units.AoWComp.TAOWAlignment Value)
        {
            FAlignment.Assign(Value);
            FAligned = false;
        }

        public int Priority { get => FPriority; set => SetPriority(value); }
        public int WinLeft { get => FWinLeft; set => SetWinLeft(value); }
        public int WinTop { get => FWinTop; set => SetWinTop(value); }
        public int WinWidth { get => FWinWidth; set => SetWidth(value); }
        public int WinHeight { get => FWinHeight; set => SetHeight(value); }
        public int TextColor { get => FColor; set => SetColor(value); }
        public int TextSelColor { get => FSelColor; set => SetSelColor(value); }
        public Aow.Units.AoWComp.TAoWComponent AOWOwner { get => FAOWOwner; set => SetOwner(value); }
        public Aow.Units.AoWComp.TAOWAlignment Alignment { get => FAlignment; set => SetAlign(value); }
        public bool Masked { get => FMasked; set => FMasked = value; }
        public bool CaptureInput { get => FCaptureInput; set => FCaptureInput = value; }
        public Aow.Rtl.TNotifyEvent OnChange { get => FOnChange; set => FOnChange = value; }

        [Published]
        public Aow.Rtl.TMouseEvent OnMouseDown { get => FOnMouseDown; set => FOnMouseDown = value; }

        [Published]
        public Aow.Rtl.TMouseEvent OnMouseUp { get => FOnMouseUp; set => FOnMouseUp = value; }

        [Published]
        public Aow.Rtl.TMouseMoveEvent OnMouseMove { get => FOnMouseMove; set => FOnMouseMove = value; }

        [Published]
        public Aow.Rtl.TKeyPressEvent OnKeyPress { get => FOnKeyPress; set => FOnKeyPress = value; }

        [Published]
        public Aow.Rtl.TKeyEvent OnKeyDown { get => FOnKeyDown; set => FOnKeyDown = value; }

        [Published]
        public Aow.Units.AoWComp.TAoWOnDrawEvent OnPreDraw { get => FOnPreDraw; set => FOnPreDraw = value; }

        [Published]
        public Aow.Units.AoWComp.TAoWOnDrawEvent OnDraw { get => FOnDraw; set => FOnDraw = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnUpdate { get => FOnUpdate; set => FOnUpdate = value; }

        [Published]
        [System.ComponentModel.DefaultValue(true)]
        public bool Visible { get => FVisible; set => SetVisible(value); }

        [Published]
        public Aow.Units.AoWComp.TAoWOnDebugEvent OnDebugMessage { get => FOnDebugMessage; set => FOnDebugMessage = value; }
    }

    public delegate void TAoWOnDrawEvent(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc);
    public delegate void TAoWOnDrawBackEvent(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc);
    public delegate void TAoWOnDrawItemEvent(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc);
    public delegate void TAoWOnDebugEvent(Aow.Rtl.AnsiString Dtext);
    // { AoWComp.TAOWAlignment (instance size 44): how a component sizes and places itself inside its
//     owner (ReAlign). +4 FWidthPercent, +5 FHeightPercent, +8 FLeftOffPercent, +$C
//     FRightOffPercent, +$10 FTopOffPercent, +$14 FBottomOffPercent, +$18 FLeftOffset, +$1C
//     FRightOffset, +$20 FTopOffset, +$24 FBottomOffset, +$28 FAlignWidth, +$29 FAlignHeight.
//     It does not override Assign, so TAoWComponent.SetAlign raises EConvertError (original bug). }
    public partial class TAOWAlignment : Aow.Rtl.TPersistent
    {
        internal byte FWidthPercent;
        internal byte FHeightPercent;
        internal int FLeftOffPercent;
        internal int FRightOffPercent;
        internal int FTopOffPercent;
        internal int FBottomOffPercent;
        internal int FLeftOffset;
        internal int FRightOffset;
        internal int FTopOffset;
        internal int FBottomOffset;
        internal Aow.Units.AoWComp.TAOWAlignWidth FAlignWidth;
        internal Aow.Units.AoWComp.TAOWAlignHeight FAlignHeight;
        [Published]
        public Aow.Units.AoWComp.TAOWAlignWidth AlignWidth { get => FAlignWidth; set => FAlignWidth = value; }

        [Published]
        public Aow.Units.AoWComp.TAOWAlignHeight AlignHeight { get => FAlignHeight; set => FAlignHeight = value; }

        [Published]
        public byte HeightPercent { get => FHeightPercent; set => FHeightPercent = value; }

        [Published]
        public byte WidthPercent { get => FWidthPercent; set => FWidthPercent = value; }

        [Published]
        public int LeftOffPercent { get => FLeftOffPercent; set => FLeftOffPercent = value; }

        [Published]
        public int RightOffPercent { get => FRightOffPercent; set => FRightOffPercent = value; }

        [Published]
        public int TopOffPercent { get => FTopOffPercent; set => FTopOffPercent = value; }

        [Published]
        public int BottomOffPercent { get => FBottomOffPercent; set => FBottomOffPercent = value; }

        [Published]
        public int LeftOffset { get => FLeftOffset; set => FLeftOffset = value; }

        [Published]
        public int RightOffset { get => FRightOffset; set => FRightOffset = value; }

        [Published]
        public int TopOffset { get => FTopOffset; set => FTopOffset = value; }

        [Published]
        public int BottomOffset { get => FBottomOffset; set => FBottomOffset = value; }

    }

    // { AoWComp.TaowRect (instance size 24): one entry of the dirty rectangle lists. +4 R, +$14 Owner
//     (the component that asked for the redraw; AddDrawRect only) }
    public partial class TaowRect : Aow.Rtl.TObject
    {
        public Aow.Rtl.TRect R;
        public Aow.Units.AoWComp.TAoWComponent Owner;
    }

    public static partial class AoWComp
    {
    }
}
