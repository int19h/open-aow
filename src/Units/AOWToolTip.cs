// Translated from AOWToolTip.pas.
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
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;

namespace Aow.Units.AOWToolTip
{
    // { The AOWToolTip unit of aowInt.dpl: TAOWToolTip, the small window that TAOWControl shows with a
//   control's tool tip text after the mouse rested on it for ToolTipDelay updates. }
    // Fields: FItems, FOnDrawItem, FToolTipStatic, FToolTipDelay, FShowDuration, FItemHeight,
    // FShowing, FControlRect (the control on the screen. the tip shows at its bottom right),
    // FItemIndex, FItemRect (the line being drawn, for OnDrawItem), FLines (Items broken into lines
    // that fit)
    public partial class TAOWToolTip : Aow.Units.AoWWinGeneric.TAOWGenericWindow
    {
        public Aow.Rtl.TStringList FItems;
        public Aow.Rtl.TNotifyEvent FOnDrawItem;
        public bool FToolTipStatic;
        public int FToolTipDelay;
        public int FShowDuration;
        public int FItemHeight;
        public bool FShowing;
        public Aow.Rtl.TRect FControlRect;
        public int FItemIndex;
        public Aow.Rtl.TRect FItemRect;
        public Aow.Rtl.TStringList FLines;
        // AOWToolTip.pas:62
        public TAOWToolTip(TComponent owner) : base(owner)
        {
            var call__1 = new Aow.Rtl.TStringList();
            FLines = call__1;
            var call__2 = new Aow.Rtl.TStringList();
            FItems = call__2;
            FItems.OnChange = OnChangeText;
            FShowDuration = 100;
            FItemHeight = 10;
        }

        // AOWToolTip.pas:72
        public override void Destroy()
        {
            FLines.Destroy();
            FItems.Destroy();
            base.Destroy();
        }

        // AOWToolTip.pas:79
        public override void Validate()
        {
            if (FILFrame != null)
            {
                var call__3 = FILFrame.CheckIL(FILIndexFrame, 8, this);
                FILFrame = call__3;
            }

            if (FILTexture != null)
            {
                var call__4 = FILTexture.CheckIL(FILIndexTexture, 1, this);
                FILTexture = call__4;
            }

            if ((((FAOWOwner != null) && (FILFrame != null)) && (FILTexture != null)) && (FAOWFont != null))
            {
                FValid = true;
            }

            base.Validate();
        }

        // AOWToolTip.pas:95
        // { SetSize breaks the lines of Items into FLines so that each fits the width (moving the last
//   characters of a line to the next one up to a space), sizes the tool tip to the text and keeps
//   it on the screen. The width limit of the line breaking uses the images 3 and 4 of the frame
//   library instead of FILIndexFrame + 3 and + 4, and the minimum width adds the width of the
//   top left corner twice (original bugs). }
        public override void SetSize()
        {
            int I = default;
            int W = default;
            bool Moved = default;
            bool Done = default;
            if (FOwnerItem != 0)
            {
                FWinWidth = FOwnerItem;
            }
            else
            {
                FOwnerItem = FWinWidth;
            }

            var call__5 = FILFrame[FILIndexFrame];
            var call__6 = FILFrame[FILIndexFrame + 5];
            if ((call__5.Height + call__6.Height) > FWinHeight)
            {
                var call__7 = FILFrame[FILIndexFrame];
                var call__8 = FILFrame[FILIndexFrame + 5];
                FWinHeight = call__7.Height + call__8.Height;
            }

            var call__9 = FILFrame[FILIndexFrame];
            var call__10 = FILFrame[FILIndexFrame + 2];
            if ((call__9.Width + call__10.Width) > FWinWidth)
            {
                var call__11 = FILFrame[FILIndexFrame];
                var call__12 = FILFrame[FILIndexFrame];
                FWinWidth = call__11.Width + call__12.Width;
            }

            FLines.Clear();
            {
                I = 0;
                int end__13 = FItems.Count - 1;
                for (; I <= end__13; I += 1)
                {
                    var call__14 = FItems[I];
                    FLines.Add(call__14);
                }
            }

            var call__15 = FLines.Count;
            if (call__15 > 0)
            {
                I = 0;
                while (true)
                {
                    var call__16 = FLines.Count;
                    if (!(I < call__16))
                        break;
                    {
                        Moved = false;
                        Done = true;
                        while (true)
                        {
                            bool bool__17 = !Done;
                            if (!bool__17)
                            {
                                var call__18 = FLines[I];
                                var call__19 = FAOWFont.GetTextWidth(call__18);
                                var call__20 = FILFrame[3];
                                var call__21 = FILFrame[4];
                                bool__17 = (call__19 > (((FWinWidth - call__20.Width) - call__21.Width) - (FBorderWidth * 2)));
                            }

                            bool bool__22 = bool__17;
                            if (bool__22)
                            {
                                var call__23 = FLines[I];
                                bool__22 = (call__23.Length > 1);
                            }

                            if (!bool__22)
                                break;
                            {
                                Done = false;
                                var call__24 = FLines.Count;
                                if (I < (call__24 - 1))
                                {
                                    if (Moved == true)
                                    {
                                        var call__25 = FLines[I];
                                        var call__26 = FLines[I];
                                        var call__27 = FLines[I + 1];
                                        FLines[I + 1] = Aow.Rtl.Rtl.Copy(call__26, call__25.Length, 1) + call__27;
                                    }
                                    else
                                    {
                                        var call__28 = FLines[I];
                                        var call__29 = FLines[I];
                                        FLines.Insert(I + 1, Aow.Rtl.Rtl.Copy(call__29, call__28.Length, 1));
                                    }
                                }
                                else
                                {
                                    var call__30 = FLines[I];
                                    var call__31 = FLines[I];
                                    FLines.Add(Aow.Rtl.Rtl.Copy(call__31, call__30.Length, 1));
                                }

                                var call__32 = FLines[I];
                                var call__33 = FLines[I];
                                FLines[I] = Aow.Rtl.Rtl.Copy(call__33, 1, call__32.Length - 1);
                                Moved = true;
                                var call__34 = FLines[I];
                                var call__35 = FLines[I];
                                if (Aow.Rtl.Rtl.Copy(call__35, call__34.Length, 1) == AnsiString.FromBytes(32))
                                {
                                    Done = true;
                                }
                            }
                        }

                        I += 1;
                    }
                }

                var call__36 = FILFrame[FILIndexFrame + 1];
                var call__37 = FILFrame[FILIndexFrame + 6];
                var call__38 = FLines.Count;
                FWinHeight = ((call__36.Height + call__37.Height) + (FBorderHeight * 2)) + (call__38 * FItemHeight);
                var call__39 = FLines[0];
                var call__40 = FAOWFont.GetTextWidth(call__39);
                W = call__40;
                var call__41 = FILFrame[FILIndexFrame];
                var call__42 = FILFrame[FILIndexFrame + 2];
                if (W < (((FWinWidth - call__41.Width) - call__42.Width) - (FBorderWidth * 2)))
                {
                    var call__43 = FILFrame[FILIndexFrame];
                    var call__44 = FILFrame[FILIndexFrame + 2];
                    FWinWidth = ((W + call__43.Width) + call__44.Width) + (FBorderWidth * 2);
                }
            }

            if (FMasked)
            {
                var call__45 = FILFrame[FILIndexFrame + 3];
                FFrameLeft = call__45.Width;
                var call__46 = FILFrame[FILIndexFrame + 4];
                FFrameRight = FWinWidth - call__46.Width;
                var call__47 = FILFrame[FILIndexFrame + 1];
                FFrameTop = call__47.Height;
                var call__48 = FILFrame[FILIndexFrame + 6];
                FFrameBottom = FWinHeight - call__48.Height;
            }

            FWinLeft = FControlRect.Right;
            FWinTop = FControlRect.Bottom;
            if (FWinManager.FWinWidth < (FWinLeft + FWinWidth))
            {
                FWinLeft = FWinManager.FWinWidth - FWinWidth;
            }

            if ((FWinTop + FWinHeight) > FWinManager.FWinHeight)
            {
                FWinTop = FControlRect.Top - FWinHeight;
            }

            if (FWinTop < 0)
            {
                FWinTop = 0;
            }

            base.SetSize();
        }

        // AOWToolTip.pas:162
        public override void Update()
        {
            bool B = default;
            B = FDoubleBuffered;
            FDoubleBuffered = false;
            base.Update();
            FDoubleBuffered = B;
        }

        // AOWToolTip.pas:186
        public override void SetVisible(bool Value)
        {
            if (Value != FVisible)
            {
                FVisible = Value;
                if (!FVisible)
                {
                    FHidden = true;
                    if (FOnHide != null)
                    {
                        FOnHide(this);
                    }
                }
                else
                {
                    if (FDoubleBuffered)
                    {
                        FSurfaceValid = false;
                    }
                    else
                    {
                        FDrawn = false;
                    }

                    if (!FHidden)
                    {
                        FBackRect.Right = FWinWidth;
                        FBackRect.Bottom = FWinHeight;
                        FBackRect.Top = FWinTop;
                        FBackRect.Left = FWinLeft;
                    }

                    if (FOnShow != null)
                    {
                        FOnShow(this);
                    }
                }

                if (FVisible == true)
                {
                    FHidden = false;
                    SetPriority(FPriority);
                }
            }

            FBackRect.Right = 0;
            FBackRect.Bottom = 0;
        }

        // AOWToolTip.pas:223
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
            Aow.Rtl.TRect OldClip = default;
            try
            {
                if (FValid && FVisible)
                {
                    var call__49 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                    FILFrame.DrawILF(FILIndexFrame, call__49, ref SurfDesc);
                    var call__50 = FILFrame[FILIndexFrame + 1];
                    var call__51 = FILFrame[FILIndexFrame + 6];
                    var call__52 = FILFrame[FILIndexFrame + 3];
                    var call__53 = FILFrame[FILIndexFrame + 4];
                    var call__54 = FILFrame[FILIndexFrame + 1];
                    var call__55 = FILFrame[FILIndexFrame + 3];
                    var call__56 = Aow.Rtl.Rtl.Rect(FWinLeft + call__55.Width, FWinTop + call__54.Height, (FWinWidth - call__52.Width) - call__53.Width, (FWinHeight - call__50.Height) - call__51.Height);
                    FILTexture.DrawILT(FILIndexTexture, call__56, ref SurfDesc);
                    FAOWFont.Color = FColor;
                    OldClip = SurfDesc.ClipRect;
                    var call__57 = FLines.Count;
                    if (call__57 > 0)
                    {
                        {
                            I = 0;
                            int end__58 = FLines.Count - 1;
                            for (; I <= end__58; I += 1)
                            {
                                var call__59 = FILFrame[FILIndexFrame + 4];
                                if (((FWinLeft + FWinWidth) - call__59.Width) < OldClip.Right)
                                {
                                    var call__60 = FILFrame[FILIndexFrame + 4];
                                    SurfDesc.ClipRect.Right = (FWinLeft + FWinWidth) - call__60.Width;
                                }

                                var call__61 = FLines.Count;
                                bool bool__62 = (I < call__61);
                                if (bool__62)
                                {
                                    var call__63 = FLines.Count;
                                    bool__62 = (call__63 > 0);
                                }

                                if (bool__62)
                                {
                                    var call__64 = FLines[I];
                                    var call__65 = FILFrame[FILIndexFrame];
                                    var call__66 = FILFrame[FILIndexFrame + 3];
                                    FAOWFont.SetText((FWinLeft + FBorderWidth) + call__66.Width, ((FWinTop + FBorderHeight) + call__65.Height) + (FItemHeight * I), call__64, SurfDesc);
                                    FItemIndex = I;
                                    var call__67 = FILFrame[FILIndexFrame + 1];
                                    var call__68 = FILFrame[FILIndexFrame + 1];
                                    var call__69 = FILFrame[FILIndexFrame + 3];
                                    var call__70 = Aow.Rtl.Rtl.Rect(FWinLeft + call__69.Width, (FWinTop + call__68.Height) + (FItemHeight * I), FWinLeft + FWinWidth, (FWinTop + call__67.Height) + ((I + 1) * FItemHeight));
                                    FItemRect = call__70;
                                    if (FOnDrawItem != null)
                                    {
                                        FOnDrawItem(this);
                                    }
                                }

                                SurfDesc.ClipRect = OldClip;
                            }
                        }
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

        // AOWToolTip.pas:266
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FShowing = false;
            SetVisible(false);
        }

        // AOWToolTip.pas:172
        public void SetFStrings(Aow.Rtl.TStringList Value)
        {
            int I = default;
            FLines.Clear();
            FItems.Clear();
            {
                I = 0;
                int end__71 = Value.Count - 1;
                for (; I <= end__71; I += 1)
                {
                    var call__72 = Value[I];
                    FLines.Add(call__72);
                    var call__73 = Value[I];
                    FItems.Add(call__73);
                }
            }

            FSized = false;
        }

        [Published]
        public Aow.Units.ImageLib.TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

        [Published]
        public Aow.Rtl.TStringList Items { get => FItems; set => SetFStrings(value); }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

        [Published]
        public bool ToolTipStatic { get => FToolTipStatic; set => FToolTipStatic = value; }

        [Published]
        public int ToolTipDelay { get => FToolTipDelay; set => FToolTipDelay = value; }

        [Published]
        public int ShowDuration { get => FShowDuration; set => FShowDuration = value; }

        [Published]
        public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }
    }

    public static partial class AOWToolTip
    {
        // AOWToolTip.pas:272
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AOWToolTip.TAOWToolTip)]);
        }
    }
}
