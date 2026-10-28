// Translated from AoWBookWindow.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.PageTurnCtrl;
using static Aow.Units.PageTurnCtrl.PageTurnCtrl;

namespace Aow.Units.AoWBookWindow
{
    // { TAoWBookWindow: instance size $208; $1EC completion event, $1F4 first frame,
//     $1F5 previous surface validity, $1F8 page turn, $1FC animating,
//     $200 saved page surface, $204 page-turn mode. }
    public partial class TAoWBookWindow : Aow.Units.AoWWindow.TAOWWindow
    {
        public Aow.Rtl.TNotifyEvent FOnAnimationComplete;
        // { $1EC }
        public bool FFirstAnimationFrame;
        // { $1F4 }
        public bool FPreviousSurfaceValid;
        // { $1F5 }
        public Aow.Units.PageTurnCtrl.TPageTurnControl FPageTurn;
        // { $1F8 }
        public bool FAnimating;
        // { $1FC }
        public Aow.Units.GFXE.TSurface FPageSurface;
        // { $200 }
        public Aow.Units.PageTurnCtrl.TPageTurnMode FPageTurnMode;
        // AoWBookWindow.pas:50
        // { $204 }
        public override void Destroy()
        {
            Aow.Units.GFXE.TSurface S = default;
            FPageTurn.Free();
            S = FPageSurface;
            if (S != null)
            {
                S.Free();
            }

            base.Destroy();
        }

        // AoWBookWindow.pas:44
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = Aow.Units.PageTurnCtrl.TPageTurnControl.Create();
            FPageTurn = call__1;
        }

        // AoWBookWindow.pas:78
        public override void Update()
        {
            if (FVisible && FAnimating)
            {
                FSurfaceValid = false;
                var call__2 = FPageTurn.UpdateScene();
                if (!call__2)
                {
                    FAnimating = false;
                    if (FOnAnimationComplete != null)
                    {
                        FOnAnimationComplete(this);
                    }
                }
            }

            base.Update();
        }

        // AoWBookWindow.pas:59
        public override void SetVisible(bool Value)
        {
            Aow.Units.GFXE.TSurface S = default;
            if (Value != FVisible)
            {
                FAnimating = false;
                if (FFreeSurfaceOnHide)
                {
                    S = FPageSurface;
                    if (S != null)
                    {
                        S.Free();
                        FPageSurface = null;
                    }
                }
            }

            base.SetVisible(Value);
        }

        // AoWBookWindow.pas:113
        public override void DrawSurface(Aow.Units.GFXE.TSurface Surface)
        {
            FPreviousSurfaceValid = FSurfaceValid;
            base.DrawSurface(Surface);
            if (FPreviousSurfaceValid && FAnimating)
            {
                Blt(Surface);
            }
        }

        // AoWBookWindow.pas:186
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = FAnimating;
            if (!FAnimating)
            {
                base.CheckMouseDown(Button, Shift, X, Y);
            }
            else
            {
                if ((((X > (FWinLeft - 1)) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
                {
                    FSurfaceValid = false;
                    FAnimating = false;
                    if (FOnAnimationComplete != null)
                    {
                        FOnAnimationComplete(this);
                    }
                }
            }
        }

        // AoWBookWindow.pas:199
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = FAnimating;
            if (!FAnimating)
            {
                base.CheckMouseUp(Button, Shift, X, Y);
            }
        }

        // AoWBookWindow.pas:205
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = FAnimating;
            if (!FAnimating)
            {
                base.CheckMouseMove(Shift, X, Y);
            }
        }

        // AoWBookWindow.pas:92
        public void StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode Mode)
        {
            Aow.Units.GFXE.TSurfaceDesc Source = default;
            Aow.Units.GFXE.TSurfaceDesc Dest = default;
            Aow.Rtl.TRect R = default;
            if ((FBackSurface != null) && (FPageSurface != null))
            {
                var call__3 = Aow.Rtl.Rtl.Rect(0, 0, FWinWidth, FWinHeight);
                R = call__3;
                FBackSurface.Lock(ref Source);
                FPageSurface.Lock(ref Dest);
                Aow.Units.GFXE.GFXE.BltSDRectClipped(Source, R, Dest, 0, 0);
                FPageSurface.Unlock();
                FBackSurface.Unlock();
                FPageTurn.SetPageDimensions(271, 368);
                FPageTurn.FSpeed = 15;
                FPageTurn.Start(Mode);
                FPageTurnMode = Mode;
                FDrawn = false;
                FAnimating = true;
                FFirstAnimationFrame = true;
            }
        }

        [Published]
        public Aow.Rtl.TNotifyEvent OnAnimDone { get => FOnAnimationComplete; set => FOnAnimationComplete = value; }

        // AoWBookWindow.pas:27
        public static new Aow.Units.AoWBookWindow.TAoWBookWindow Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AoWBookWindow.TAoWBookWindow o = new Aow.Units.AoWBookWindow.TAoWBookWindow();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class AoWBookWindow
    {
        // AoWBookWindow.pas:211
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Tools", [typeof(Aow.Units.AoWBookWindow.TAoWBookWindow)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
