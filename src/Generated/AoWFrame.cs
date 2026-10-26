// Generated from AoWFrame.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AOWMenuBar;
using static Aow.Units.AOWMenuBar.AOWMenuBar;
using Aow.Units.AOWMenu;
using static Aow.Units.AOWMenu.AOWMenu;
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

namespace Aow.Units.AoWFrame
{
    public enum TFrameType : byte
    {
        frFilledBox = 0,
        frHollowBox = 1,
        frHorizLine = 2,
        frVertLine = 3
    }

    // { AoWFrame.TAoWFrame (instance size 308). Fields: +$114 FFrameType, +$118 FILFrame, +$11C
//     FILTexture, +$120 FILLine, +$124 FILIndexFrame, +$128 FILIndexLine, +$12C FILIndexTexture,
//     +$130 FPopupMenu.
//     VMT (new): SetILFrame, SetILTexture, SetILLine }
    public partial class TAoWFrame : Aow.Units.AoWControl.TAOWControl
    {
        public Aow.Units.AoWFrame.TFrameType FFrameType;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILFrame;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTexture;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILLine;
        public int FILIndexFrame;
        public int FILIndexLine;
        public int FILIndexTexture;
        public Aow.Units.AOWMenu.TAOWMenu FPopupMenu;
        // AoWFrame.pas:63
        // { TAoWFrame }
        public override void Validate()
        {
            if (FILFrame != null)
            {
                var call__1 = FILFrame.CheckIL(FILIndexFrame, 8, this);
                FILFrame = call__1;
            }

            if (FILTexture != null)
            {
                var call__2 = FILTexture.CheckIL(FILIndexTexture, 1, this);
                FILTexture = call__2;
            }

            if (FILLine != null)
            {
                var call__3 = FILLine.CheckIL(FILIndexLine, 3, this);
                FILLine = call__3;
            }

            if ((FILFrame != null) || (FILLine != null))
            {
                FValid = true;
                if (FILFrame != null)
                {
                    if (FILTexture != null)
                    {
                        FFrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox;
                    }
                    else
                    {
                        FFrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox;
                    }
                }
                else
                {
                    if (FILLine != null)
                    {
                        if (FFrameType != Aow.Units.AoWFrame.TFrameType.frHorizLine)
                        {
                            FFrameType = Aow.Units.AoWFrame.TFrameType.frVertLine;
                        }
                    }
                }
            }

            base.Validate();
        }

        // AoWFrame.pas:113
        public override void SetSize()
        {
            switch (FFrameType)
            {
                case Aow.Units.AoWFrame.TFrameType.frFilledBox:
                case Aow.Units.AoWFrame.TFrameType.frHollowBox:
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
                case Aow.Units.AoWFrame.TFrameType.frHorizLine:
                    var call__12 = FILLine[FILIndexLine];
                    FWinHeight = call__12.Height;
                    var call__13 = FILLine[FILIndexLine];
                    var call__14 = FILLine[FILIndexLine + 2];
                    if ((call__13.Width + call__14.Width) > FWinWidth)
                    {
                        var call__15 = FILLine[FILIndexLine];
                        var call__16 = FILLine[FILIndexLine + 2];
                        FWinWidth = call__15.Width + call__16.Width;
                    }

                    break;
                case Aow.Units.AoWFrame.TFrameType.frVertLine:
                    var call__17 = FILLine[FILIndexLine];
                    var call__18 = FILLine[FILIndexLine + 2];
                    if ((call__17.Height + call__18.Height) > FWinHeight)
                    {
                        var call__19 = FILLine[FILIndexLine];
                        var call__20 = FILLine[FILIndexLine + 2];
                        FWinHeight = call__19.Height + call__20.Height;
                    }

                    var call__21 = FILLine[FILIndexLine];
                    FWinWidth = call__21.Width;
                    break;
            }

            base.SetSize();
        }

        // AoWFrame.pas:166
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWFrame.pas:202
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried Self identity, receiver scope and local/field staging; register/stack allocation still differs. }
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }

                if ((Button == Aow.Rtl.TMouseButton.mbRight) && (FPopupMenu != null))
                {
                    FPopupMenu.SetPopupOwner(this);
                    FPopupMenu.WinLeft = X;
                    FPopupMenu.WinTop = Y;
                    if (FPopupMenu.FWinManager != null)
                    {
                        if (FPopupMenu.FHidden)
                        {
                            if ((FPopupMenu.FMenuLeft + FPopupMenu.FWinWidth) > FPopupMenu.FWinManager.FWinWidth)
                            {
                                FPopupMenu.WinLeft = X - FPopupMenu.FWinWidth;
                            }

                            if ((FPopupMenu.FMenuTop + FPopupMenu.FWinHeight) > FPopupMenu.FWinManager.FWinHeight)
                            {
                                FPopupMenu.WinTop = Y - FPopupMenu.FWinHeight;
                            }
                        }
                        else
                        {
                            if ((FPopupMenu.FWinLeft + FPopupMenu.FWinWidth) > FPopupMenu.FWinManager.FWinWidth)
                            {
                                FPopupMenu.WinLeft = X - FPopupMenu.FWinWidth;
                            }

                            if ((FPopupMenu.FWinTop + FPopupMenu.FWinHeight) > FPopupMenu.FWinManager.FWinHeight)
                            {
                                FPopupMenu.WinTop = Y - FPopupMenu.FWinHeight;
                            }
                        }
                    }

                    FPopupMenu.Visible = true;
                }
            }
        }

        // AoWFrame.pas:237
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWFrame.pas:246
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
        }

        // AoWFrame.pas:255
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AoWFrame.pas:139
        public virtual void SetILFrame(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILFrame)
            {
                FValid = false;
                FILFrame = Value;
            }
        }

        // AoWFrame.pas:148
        public virtual void SetILTexture(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTexture)
            {
                FValid = false;
                FILTexture = Value;
            }
        }

        // AoWFrame.pas:157
        public virtual void SetILLine(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILLine)
            {
                FValid = false;
                FILLine = Value;
            }
        }

        // AoWFrame.pas:86
        public void SetILFIndex(int Value)
        {
            if (Value != FILIndexFrame)
            {
                FILIndexFrame = Value;
                FValid = false;
            }
        }

        // AoWFrame.pas:95
        public void SetILLIndex(int Value)
        {
            if (Value != FILIndexLine)
            {
                FILIndexLine = Value;
                FValid = false;
            }
        }

        // AoWFrame.pas:104
        public void SetILTIndex(int Value)
        {
            if (Value != FILIndexTexture)
            {
                FILIndexTexture = Value;
                FValid = false;
            }
        }

        [Published]
        public Aow.Units.AoWFrame.TFrameType FrameType { get => FFrameType; set => FFrameType = value; }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

        [Published]
        public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

        [Published]
        public int ILIndexLine { get => FILIndexLine; set => SetILLIndex(value); }

        [Published]
        public int ILIndexTexture { get => FILIndexTexture; set => SetILTIndex(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILLine { get => FILLine; set => SetILLine(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTexture { get => FILTexture; set => SetILTexture(value); }

        [Published]
        public Aow.Units.AOWMenu.TAOWMenu PopupMenu { get => FPopupMenu; set => FPopupMenu = value; }
    }

    public static partial class AoWFrame
    {
        // AoWFrame.pas:260
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWFrame.TAoWFrame)]);
        }
    }
}
