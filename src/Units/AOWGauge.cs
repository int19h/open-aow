// Translated from AOWGauge.pas.
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
using Aow.Units.AOWToolTip;
using static Aow.Units.AOWToolTip.AOWToolTip;
using Aow.Units.AoWControl;
using static Aow.Units.AoWControl.AoWControl;
using Aow.Units.AOWWinManager;
using static Aow.Units.AOWWinManager.AOWWinManager;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;
using static Aow.Units.AoWWinControl.AoWWinControl;
using Aow.Units.AoWBaseWin;
using static Aow.Units.AoWBaseWin.AoWBaseWin;
using Aow.Units.AoWWinGeneric;
using static Aow.Units.AoWWinGeneric.AoWWinGeneric;

namespace Aow.Units.AOWGauge
{
    public enum TAOWGaugeType : byte
    {
        gtNormal = 0,
        gtBorderless = 1
    }

    // { AOWGauge.TAoWGauge (instance size 316). Fields: +$114 FMin, +$118 FMax, +$11C FPosition,
//     +$120 FILFrame, +$124 FILTextureNormal, +$128 FILTextureSelected, +$12C FStyle, +$130
//     FILIndexFrame, +$134 FILIndexTNormal, +$138 FILIndexTSelected.
//     VMT (new): SetILFrame, SetILTNorm, SetILTSel }
    public partial class TAoWGauge : Aow.Units.AoWControl.TAOWControl
    {
        public int FMin;
        public int FMax;
        public int FPosition;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILFrame;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTextureNormal;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTextureSelected;
        public Aow.Units.AOWGauge.TAOWGaugeType FStyle;
        public int FILIndexFrame;
        public int FILIndexTNormal;
        public int FILIndexTSelected;
        // AOWGauge.pas:69
        // { TAoWGauge }
        public TAoWGauge(TComponent owner) : base(owner)
        {
            FMax = 1;
        }

        // AOWGauge.pas:75
        public override void Validate()
        {
            if (FILTextureNormal != null)
            {
                var call__1 = FILTextureNormal.CheckIL(FILIndexTNormal, 1, this);
                FILTextureNormal = call__1;
            }

            if (FILTextureSelected != null)
            {
                var call__2 = FILTextureSelected.CheckIL(FILIndexTSelected, 1, this);
                FILTextureSelected = call__2;
            }

            if ((FILTextureNormal != null) && (FILTextureSelected != null))
            {
                if (FILFrame != null)
                {
                    var call__3 = FILFrame.CheckIL(FILIndexFrame, 8, this);
                    FILFrame = call__3;
                }

                if (FILFrame != null)
                {
                    FStyle = Aow.Units.AOWGauge.TAOWGaugeType.gtNormal;
                }
                else
                {
                    FStyle = Aow.Units.AOWGauge.TAOWGaugeType.gtBorderless;
                }

                FValid = true;
            }

            base.Validate();
        }

        // AOWGauge.pas:94
        public override void SetSize()
        {
            switch (FStyle)
            {
                case Aow.Units.AOWGauge.TAOWGaugeType.gtNormal:
                    var call__4 = FILFrame[FILIndexFrame];
                    var call__5 = FILFrame[FILIndexFrame + 5];
                    if ((call__4.Height + call__5.Height) > FWinHeight)
                    {
                        var call__6 = FILFrame[FILIndexFrame];
                        var call__7 = FILFrame[FILIndexFrame + 5];
                        FWinHeight = call__6.Height + call__7.Height;
                    }

                    var call__8 = FILFrame[FILIndexFrame];
                    var call__9 = FILFrame[FILIndexFrame + 2];
                    if ((call__8.Width + call__9.Width) > FWinWidth)
                    {
                        var call__10 = FILFrame[FILIndexFrame];
                        var call__11 = FILFrame[FILIndexFrame + 2];
                        FWinWidth = call__10.Width + call__11.Width;
                    }

                    break;
            }

            if ((FMin + 1) > FMax)
            {
                FMax = FMin + 1;
            }

            if (FMin > FPosition)
            {
                FPosition = FMin;
            }

            if (FMax < FPosition)
            {
                FPosition = FMax;
            }

            base.SetSize();
        }

        // AOWGauge.pas:183
        // { Draw: the filled width P is computed from Position without subtracting Min (original bug:
//   the gauge is only right for Min = 0) }
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Rtl.TRect OldClip = default;
            int P = default;
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    OldClip = SurfDesc.ClipRect;
                    switch (FStyle)
                    {
                        case Aow.Units.AOWGauge.TAOWGaugeType.gtNormal:
                            var call__12 = FILFrame[FILIndexFrame + 3];
                            var call__13 = FILFrame[FILIndexFrame + 4];
                            P = (((FWinWidth - call__12.Width) - call__13.Width) * FPosition) / (FMax - FMin);
                            var call__14 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__14, ref SurfDesc);
                            var call__15 = FILFrame[FILIndexFrame + 3];
                            if ((((FAOWWindow.FWinLeft + P) + FWinLeft) + call__15.Width) > SurfDesc.ClipRect.Left)
                            {
                                var call__16 = FILFrame[FILIndexFrame + 3];
                                SurfDesc.ClipRect.Left = ((FAOWWindow.FWinLeft + P) + FWinLeft) + call__16.Width;
                            }

                            var call__17 = FILFrame[FILIndexFrame + 1];
                            var call__18 = FILFrame[FILIndexFrame + 6];
                            var call__19 = FILFrame[FILIndexFrame + 3];
                            var call__20 = FILFrame[FILIndexFrame + 4];
                            var call__21 = FILFrame[FILIndexFrame + 1];
                            var call__22 = FILFrame[FILIndexFrame + 3];
                            var call__23 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + call__22.Width, (FAOWWindow.FWinTop + FWinTop) + call__21.Height, (FWinWidth - call__19.Width) - call__20.Width, (FWinHeight - call__17.Height) - call__18.Height);
                            FILTextureNormal.DrawILT(FILIndexTNormal, call__23, ref SurfDesc);
                            SurfDesc.ClipRect = OldClip;
                            var call__24 = FILFrame[FILIndexFrame + 1];
                            var call__25 = FILFrame[FILIndexFrame + 6];
                            var call__26 = FILFrame[FILIndexFrame + 1];
                            var call__27 = FILFrame[FILIndexFrame + 3];
                            var call__28 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + call__27.Width, (FAOWWindow.FWinTop + FWinTop) + call__26.Height, P, (FWinHeight - call__24.Height) - call__25.Height);
                            FILTextureSelected.DrawILT(FILIndexTSelected, call__28, ref SurfDesc);
                            break;
                        case Aow.Units.AOWGauge.TAOWGaugeType.gtBorderless:
                            P = (FPosition * FWinWidth) / (FMax - FMin);
                            if (((FAOWWindow.FWinLeft + P) + FWinLeft) > SurfDesc.ClipRect.Left)
                            {
                                SurfDesc.ClipRect.Left = (FAOWWindow.FWinLeft + P) + FWinLeft;
                            }

                            var call__29 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + P) + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth - P, FWinHeight);
                            FILTextureNormal.DrawILT(FILIndexTNormal, call__29, ref SurfDesc);
                            SurfDesc.ClipRect = OldClip;
                            var call__30 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, P, FWinHeight);
                            FILTextureSelected.DrawILT(FILIndexTSelected, call__30, ref SurfDesc);
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
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Draw", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AOWGauge.pas:237
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AOWGauge.pas:250
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AOWGauge.pas:262
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
        }

        // AOWGauge.pas:274
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AOWGauge.pas:141
        public virtual void SetILFrame(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILFrame)
            {
                FValid = false;
                FILFrame = Value;
            }
        }

        // AOWGauge.pas:150
        public virtual void SetILTNorm(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTextureNormal)
            {
                FValid = false;
                FILTextureNormal = Value;
            }
        }

        // AOWGauge.pas:159
        public virtual void SetILTSel(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTextureSelected)
            {
                FValid = false;
                FILTextureSelected = Value;
            }
        }

        // AOWGauge.pas:114
        public void SetILFIndex(int Value)
        {
            if (Value != FILIndexFrame)
            {
                FILIndexFrame = Value;
                FValid = false;
            }
        }

        // AOWGauge.pas:123
        public void SetILTnIndex(int Value)
        {
            if (Value != FILIndexTNormal)
            {
                FILIndexTNormal = Value;
                FValid = false;
            }
        }

        // AOWGauge.pas:132
        public void SetILTsIndex(int Value)
        {
            if (Value != FILIndexTSelected)
            {
                FILIndexTSelected = Value;
                FValid = false;
            }
        }

        // AOWGauge.pas:168
        public void SetPos(int Value)
        {
            if (Value != FPosition)
            {
                FPosition = Value;
                if (FPosition > FMax)
                {
                    FPosition = FMax;
                }

                if (FMin > FPosition)
                {
                    FPosition = FMin;
                }

                FDrawn = false;
            }
        }

        [Published]
        public int Min { get => FMin; set => FMin = value; }

        [Published]
        public int Max { get => FMax; set => FMax = value; }

        [Published]
        public int Position { get => FPosition; set => SetPos(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTextureNormal { get => FILTextureNormal; set => SetILTNorm(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

        [Published]
        public int ILIndexFrame { get => FILIndexFrame; set => FILIndexFrame = value; }

        [Published]
        public int ILIndexTNormal { get => FILIndexTNormal; set => SetILTnIndex(value); }

        [Published]
        public int ILIndexTSelected { get => FILIndexTSelected; set => SetILTsIndex(value); }
    }

    public static partial class AOWGauge
    {
        // AOWGauge.pas:279
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AOWGauge.TAoWGauge)]);
        }
    }
}
