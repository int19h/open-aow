// Translated from AoWFrame.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AOWMenu;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;

namespace Aow.Units.AoWFrame
{
    public enum TFrameType : byte
    {
        frFilledBox = 0,
        frHollowBox = 1,
        frHorizLine = 2,
        frVertLine = 3
    }

    // Fields: FFrameType, FILFrame, FILTexture, FILLine, FILIndexFrame, FILIndexLine,
    // FILIndexTexture, FPopupMenu
    public partial class TAoWFrame : TAOWControl
    {
        public TAoWFrame(TComponent owner) : base(owner)
        { }

        public TFrameType FFrameType;
        public TInterfaceIL FILFrame;
        public TInterfaceIL FILTexture;
        public TInterfaceIL FILLine;
        public int FILIndexFrame;
        public int FILIndexLine;
        public int FILIndexTexture;
        public TAOWMenu FPopupMenu;
        // AoWFrame.pas:63
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

            if (FILFrame != null || FILLine != null)
            {
                FValid = true;
                if (FILFrame != null)
                {
                    if (FILTexture != null)
                    {
                        FFrameType = TFrameType.frFilledBox;
                    }
                    else
                    {
                        FFrameType = TFrameType.frHollowBox;
                    }
                }
                else
                {
                    if (FILLine != null)
                    {
                        if (FFrameType != TFrameType.frHorizLine)
                        {
                            FFrameType = TFrameType.frVertLine;
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
                case TFrameType.frFilledBox:
                case TFrameType.frHollowBox:
                    var call__4 = FILFrame[FILIndexFrame];
                    var call__5 = FILFrame[FILIndexFrame + 5];
                    if (call__4.Height + call__5.Height > FWinHeight)
                    {
                        var call__6 = FILFrame[FILIndexFrame];
                        var call__7 = FILFrame[FILIndexFrame + 5];
                        FWinHeight = call__6.Height + call__7.Height;
                    }

                    var call__8 = FILFrame[FILIndexFrame];
                    var call__9 = FILFrame[FILIndexFrame + 2];
                    if (call__8.Width + call__9.Width > FWinWidth)
                    {
                        var call__10 = FILFrame[FILIndexFrame];
                        var call__11 = FILFrame[FILIndexFrame + 2];
                        FWinWidth = call__10.Width + call__11.Width;
                    }

                    break;
                case TFrameType.frHorizLine:
                    var call__12 = FILLine[FILIndexLine];
                    FWinHeight = call__12.Height;
                    var call__13 = FILLine[FILIndexLine];
                    var call__14 = FILLine[FILIndexLine + 2];
                    if (call__13.Width + call__14.Width > FWinWidth)
                    {
                        var call__15 = FILLine[FILIndexLine];
                        var call__16 = FILLine[FILIndexLine + 2];
                        FWinWidth = call__15.Width + call__16.Width;
                    }

                    break;
                case TFrameType.frVertLine:
                    var call__17 = FILLine[FILIndexLine];
                    var call__18 = FILLine[FILIndexLine + 2];
                    if (call__17.Height + call__18.Height > FWinHeight)
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
        public override void Draw(TSurfaceDesc SurfDesc)
        {
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    switch (FFrameType)
                    {
                        case TFrameType.frVertLine:
                            var call__22 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILLine.DrawILLv(FILIndexLine, call__22, ref SurfDesc);
                            break;
                        case TFrameType.frHorizLine:
                            var call__23 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILLine.DrawILLh(FILIndexLine, call__23, ref SurfDesc);
                            break;
                        case TFrameType.frHollowBox:
                            var call__24 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__24, ref SurfDesc);
                            break;
                        case TFrameType.frFilledBox:
                            var call__25 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__25, ref SurfDesc);
                            var call__26 = FILFrame[FILIndexFrame + 1];
                            var call__27 = FILFrame[FILIndexFrame + 6];
                            var call__28 = FILFrame[FILIndexFrame + 3];
                            var call__29 = FILFrame[FILIndexFrame + 4];
                            var call__30 = FILFrame[FILIndexFrame + 1];
                            var call__31 = FILFrame[FILIndexFrame + 3];
                            var call__32 = Rect(FAOWWindow.FWinLeft + FWinLeft + call__31.Width, FAOWWindow.FWinTop + FWinTop + call__30.Height, FWinWidth - call__28.Width - call__29.Width, FWinHeight - call__26.Height - call__27.Height);
                            FILTexture.DrawILT(FILIndexTexture, call__32, ref SurfDesc);
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

        // AoWFrame.pas:202
        public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = true;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }

                if (Button == TMouseButton.mbRight && FPopupMenu != null)
                {
                    FPopupMenu.SetPopupOwner(this);
                    FPopupMenu.WinLeft = X;
                    FPopupMenu.WinTop = Y;
                    if (FPopupMenu.FWinManager != null)
                    {
                        if (FPopupMenu.FHidden)
                        {
                            if (FPopupMenu.FMenuLeft + FPopupMenu.FWinWidth > FPopupMenu.FWinManager.FWinWidth)
                            {
                                FPopupMenu.WinLeft = X - FPopupMenu.FWinWidth;
                            }

                            if (FPopupMenu.FMenuTop + FPopupMenu.FWinHeight > FPopupMenu.FWinManager.FWinHeight)
                            {
                                FPopupMenu.WinTop = Y - FPopupMenu.FWinHeight;
                            }
                        }
                        else
                        {
                            if (FPopupMenu.FWinLeft + FPopupMenu.FWinWidth > FPopupMenu.FWinManager.FWinWidth)
                            {
                                FPopupMenu.WinLeft = X - FPopupMenu.FWinWidth;
                            }

                            if (FPopupMenu.FWinTop + FPopupMenu.FWinHeight > FPopupMenu.FWinManager.FWinHeight)
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
        public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWFrame.pas:246
        public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
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
        public virtual void SetILFrame(TInterfaceIL Value)
        {
            if (Value != FILFrame)
            {
                FValid = false;
                FILFrame = Value;
            }
        }

        // AoWFrame.pas:148
        public virtual void SetILTexture(TInterfaceIL Value)
        {
            if (Value != FILTexture)
            {
                FValid = false;
                FILTexture = Value;
            }
        }

        // AoWFrame.pas:157
        public virtual void SetILLine(TInterfaceIL Value)
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
        public TFrameType FrameType { get => FFrameType; set => FFrameType = value; }

        [Published]
        public TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

        [Published]
        public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

        [Published]
        public int ILIndexLine { get => FILIndexLine; set => SetILLIndex(value); }

        [Published]
        public int ILIndexTexture { get => FILIndexTexture; set => SetILTIndex(value); }

        [Published]
        public TInterfaceIL ILLine { get => FILLine; set => SetILLine(value); }

        [Published]
        public TInterfaceIL ILTexture { get => FILTexture; set => SetILTexture(value); }

        [Published]
        public TAOWMenu PopupMenu { get => FPopupMenu; set => FPopupMenu = value; }
    }

    public static partial class AoWFrame
    {
        // AoWFrame.pas:260
        public static void Register()
        {
            RegisterComponents((AnsiString)"AOW Interface", [typeof(TAoWFrame)]);
        }
    }
}
