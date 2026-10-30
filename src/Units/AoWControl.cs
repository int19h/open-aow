// Translated from AoWControl.pas.
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

namespace Aow.Units.AoWControl
{
    // { The AoWControl unit of aowInt.dpl: TAOWControl, the ancestor of the controls that sit in a
//   window (buttons, edits, lists, scroll bars, ...). It knows its window and shows its tool tip. }
    // { AoWControl.TAOWControl (instance size 276). Fields: +$104 FToolTipText, +$108 FToolTipShown
//     (updates the tool tip has been visible), +$10C FAOWWindow, +$110 FToolTip }
    public partial class TAOWControl : Aow.Units.AoWComp.TAoWComponent
    {
        public Aow.Rtl.TStringList FToolTipText;
        public int FToolTipShown;
        public Aow.Units.AoWBaseWin.TAOWBaseWindow FAOWWindow;
        public Aow.Units.AOWToolTip.TAOWToolTip FToolTip;
        // AoWControl.pas:46
        // { TAOWControl }
        public TAOWControl(TComponent owner) : base(owner)
        {
            var call__1 = new Aow.Rtl.TStringList();
            FToolTipText = call__1;
            FCaptureInput = true;
        }

        // AoWControl.pas:53
        public override void Destroy()
        {
            FToolTipText.Destroy();
            base.Destroy();
        }

        // AoWControl.pas:84
        // { Update counts the tool tip delay and shows or hides the tool tip, and asks the owner to redraw
//   the control's old and new places when it moved, changed size or became hidden. FOldRect holds
//   the place of the last update as (Left, Top, Width, Height); the rectangles to redraw are in the
//   coordinates of the window manager (the owner's position is added unless the owner is the
//   manager's direct child... of a window placed in the manager) }
        public override void Update()
        {
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWBaseWin.TAOWBaseWindow W = default;
            // { Tried Self/field casts, receiver scope and receiver/local staging; Self/temporary registers still differ. }
            if (FValid)
            {
                if (FVisible)
                {
                    base.Update();
                    if (FToolTip != null)
                    {
                        if (FToolTipCount > 0)
                        {
                            if (((FToolTipShown > 0) && (FMouseIn == true)) && (FToolTip.FShowDuration > 0))
                            {
                                if (FToolTipShown < FToolTip.FShowDuration)
                                {
                                    FToolTipShown += 1;
                                }
                                else
                                {
                                    if (!FToolTip.FToolTipStatic && (FToolTip != null))
                                    {
                                        FToolTip.SetVisible(false);
                                    }

                                    FToolTipShown = 0;
                                }
                            }

                            if (FToolTip.FToolTipDelay > FToolTipCount)
                            {
                                FToolTipCount += 1;
                            }
                            else
                            {
                                var call__2 = FToolTipText.Count;
                                if ((call__2 > 0) && (FMouseIn == false))
                                {
                                    FToolTip.SetFStrings(FToolTipText);
                                    if (!FToolTip.FToolTipStatic)
                                    {
                                        FToolTip.SetVisible(true);
                                        FAOWWindow.FRedraw = true;
                                        FToolTip.FSized = false;
                                    }

                                    W = FAOWWindow;
                                    while (W.FAOWOwner is Aow.Units.AoWBaseWin.TAOWBaseWindow)
                                    {
                                        W = (Aow.Units.AoWBaseWin.TAOWBaseWindow)W.FAOWOwner;
                                    }

                                    if (FAOWWindow != W)
                                    {
                                        var call__3 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + W.FWinLeft) + FWinLeft, (FAOWWindow.FWinTop + W.FWinTop) + FWinTop, (FAOWWindow.FWinLeft + W.FWinLeft) + FToolTipLeft, ((FAOWWindow.FWinTop + W.FWinTop) + FWinTop) + FWinHeight);
                                        FToolTip.FControlRect = call__3;
                                    }
                                    else
                                    {
                                        var call__4 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FAOWWindow.FWinLeft + FToolTipLeft, (FAOWWindow.FWinTop + FWinTop) + FWinHeight);
                                        FToolTip.FControlRect = call__4;
                                    }

                                    FMouseIn = true;
                                    FToolTipShown = 1;
                                }
                            }
                        }
                        else
                        {
                            if (FMouseIn == true)
                            {
                                if (!FToolTip.FToolTipStatic && (FToolTip != null))
                                {
                                    FToolTip.SetVisible(false);
                                }

                                FMouseIn = false;
                            }
                        }
                    }

                    if (((FOldRect.Right > 0) && (FOldRect.Bottom > 0)) && ((((FOldRect.Left != FWinLeft) || (FOldRect.Top != FWinTop)) || (FOldRect.Right != FWinWidth)) || (FOldRect.Bottom != FWinHeight)))
                    {
                        if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                        {
                            var call__5 = Aow.Rtl.Rtl.Rect(FOldRect.Left, FOldRect.Top, (FOldRect.Left + FOldRect.Right) - 1, (FOldRect.Top + FOldRect.Bottom) - 1);
                            R = call__5;
                        }
                        else
                        {
                            var call__6 = Aow.Rtl.Rtl.Rect(FAOWOwner.FWinLeft + FOldRect.Left, FAOWOwner.FWinTop + FOldRect.Top, ((FAOWOwner.FWinLeft + FOldRect.Left) + FOldRect.Right) - 1, ((FAOWOwner.FWinTop + FOldRect.Top) + FOldRect.Bottom) - 1);
                            R = call__6;
                        }

                        FAOWOwner.AddDrawRect(ref R, this);
                        FDrawn = false;
                    }
                    else
                    {
                        if (FMasked && !FDrawn)
                        {
                            if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                            {
                                var call__7 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, (FWinLeft + FWinWidth) - 1, (FWinTop + FWinHeight) - 1);
                                R = call__7;
                            }
                            else
                            {
                                var call__8 = Aow.Rtl.Rtl.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, ((FAOWOwner.FWinLeft + FWinLeft) + FWinWidth) - 1, ((FAOWOwner.FWinTop + FWinTop) + FWinHeight) - 1);
                                R = call__8;
                            }

                            FAOWOwner.AddDrawRect(ref R, this);
                        }
                    }

                    if (!FDrawn)
                    {
                        if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                        {
                            var call__9 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, (FWinLeft + FWinWidth) - 1, (FWinTop + FWinHeight) - 1);
                            R = call__9;
                        }
                        else
                        {
                            var call__10 = Aow.Rtl.Rtl.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, ((FAOWOwner.FWinLeft + FWinLeft) + FWinWidth) - 1, ((FAOWOwner.FWinTop + FWinTop) + FWinHeight) - 1);
                            R = call__10;
                        }

                        var call__11 = FAOWOwner.FChildren.IndexOf(this);
                        FAOWOwner.InvalidateRect(call__11, ref R, this);
                    }

                    FOldRect.Left = FWinLeft;
                    FOldRect.Top = FWinTop;
                    FOldRect.Right = FWinWidth;
                    FOldRect.Bottom = FWinHeight;
                }
                else
                {
                    if (FHidden)
                    {
                        if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                        {
                            var call__12 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, (FWinLeft + FWinWidth) - 1, (FWinTop + FWinHeight) - 1);
                            R = call__12;
                        }
                        else
                        {
                            var call__13 = Aow.Rtl.Rtl.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, ((FAOWOwner.FWinLeft + FWinLeft) + FWinWidth) - 1, ((FAOWOwner.FWinTop + FWinTop) + FWinHeight) - 1);
                            R = call__13;
                        }

                        FAOWOwner.AddDrawRect(ref R, this);
                        FHidden = false;
                    }
                }
            }
        }

        // AoWControl.pas:64
        public override void SetOwner(Aow.Units.AoWComp.TAoWComponent Value)
        {
            base.SetOwner(Value);
            FAOWWindow = (Aow.Units.AoWBaseWin.TAOWBaseWindow)FAOWOwner;
        }

        // AoWControl.pas:190
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (FToolTip != null)
            {
                FToolTip.SetVisible(false);
                FToolTipShown = FToolTip.FShowDuration;
                FMouseIn = true;
                FToolTipCount = FToolTip.FToolTipDelay;
            }
        }

        // AoWControl.pas:201
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried Self/coordinate identities and receiver scope; Y/sum registers still differ. }
            if ((((X < (FAOWWindow.FWinLeft + FWinLeft)) || (X > (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - 1))) || (Y < (FAOWWindow.FWinTop + FWinTop))) || (Y > (((FAOWWindow.FWinTop + FWinTop) + FWinHeight) - 1)))
            {
                FToolTipCount = 0;
            }
            else
            {
                if ((FToolTip != null) && (FToolTip.FToolTipDelay > FToolTipCount))
                {
                    FToolTipLeft = X - FAOWWindow.FWinLeft;
                    FToolTipCount = 1;
                }
            }
        }

        // AoWControl.pas:59
        public void SetAOWWindow(Aow.Units.AoWBaseWin.TAOWBaseWindow Value)
        {
            SetOwner(Value);
        }

        // AoWControl.pas:70
        public void SetToolStrings(Aow.Rtl.TStringList Value)
        {
            int I = default;
            FToolTipText.Clear();
            {
                I = 0;
                int end__14 = Value.Count - 1;
                for (; I <= end__14; I += 1)
                {
                    var call__15 = Value[I];
                    FToolTipText.Add(call__15);
                }
            }
        }

        [Published]
        public Aow.Units.AoWBaseWin.TAOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

        [Published]
        public Aow.Units.AOWToolTip.TAOWToolTip ToolTip { get => FToolTip; set => FToolTip = value; }

        [Published]
        public Aow.Rtl.TStringList ToolTipText { get => FToolTipText; set => SetToolStrings(value); }
    }

    public static partial class AoWControl
    {
    }
}
