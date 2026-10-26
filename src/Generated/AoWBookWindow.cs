// Generated from AoWBookWindow.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

        // AoWBookWindow.pas:120
        public override void Blt(Aow.Units.GFXE.TSurface Surface)
        {
            TSet<Aow.Units.GFXE.TDDSCreationFlagsElement> Flags = default;
            Aow.Units.GFXE.TSurfaceDesc Dest = default;
            Aow.Units.GFXE.TSurfaceDesc Source = default;
            Aow.Units.GFXE.TSurfaceDesc OldPage = default;
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            // { Match trials (3): declaration/operand order, named temporaries, zero-shift scheduling; still differs. }
            if (FPageSurface != null)
            {
                var call__3 = FPageSurface.PixelFormatID;
                var call__4 = FBackSurface.PixelFormatID;
                bool bool__5 = (call__3 != call__4);
                if (!bool__5)
                {
                    var call__6 = FPageSurface.Width;
                    var call__7 = FBackSurface.Width;
                    bool__5 = (call__6 != call__7);
                }

                bool bool__8 = bool__5;
                if (!bool__8)
                {
                    var call__9 = FPageSurface.Height;
                    var call__10 = FBackSurface.Height;
                    bool__8 = (call__9 != call__10);
                }

                if (bool__8 || ((FPageSurface is Aow.Units.GFXE.TDDrawSurface) ^ (FBackSurface is Aow.Units.GFXE.TDDrawSurface)))
                {
                    FPageSurface.Free();
                    FPageSurface = null;
                }
            }

            if (FPageSurface == null)
            {
                if (FBackSurface is Aow.Units.GFXE.TDDrawSurface)
                {
                    if (((Aow.Units.GFXE.TDDrawSurface)FBackSurface).Status.Contains(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssVideoMemory) && FUseVMem)
                    {
                        Flags = TSet<Aow.Units.GFXE.TDDSCreationFlagsElement>.Of(Aow.Units.GFXE.TDDSCreationFlagsElement.cfVideoMemory, Aow.Units.GFXE.TDDSCreationFlagsElement.cfSystemMemory);
                    }
                    else
                    {
                        Flags = TSet<Aow.Units.GFXE.TDDSCreationFlagsElement>.Of(Aow.Units.GFXE.TDDSCreationFlagsElement.cfSystemMemory);
                    }

                    var call__11 = Aow.Units.GFXE.TDDrawSurface.Create();
                    FPageSurface = call__11;
                    var call__12 = FBackSurface.PixelFormatID;
                    ((Aow.Units.GFXE.TDDrawSurface)FPageSurface).CreateSurface(FWinWidth, FWinHeight, call__12, Flags);
                }
                else
                {
                    var call__13 = FBackSurface.PixelFormatID;
                    var call__14 = Aow.Units.GFXE.TDataSurface.Create(FWinWidth, FWinHeight, call__13, null);
                    FPageSurface = call__14;
                }

                var call__15 = Aow.Rtl.Rtl.Rect(0, 0, FWinWidth, FWinHeight);
                R = call__15;
                FBackSurface.BltClipped(FPageSurface, Ptr<Aow.Rtl.TRect>.To(R__cell, 0), 0, 0);
            }

            if (FAnimating)
            {
                if (FFirstAnimationFrame || !FPreviousSurfaceValid)
                {
                    var call__16 = Aow.Rtl.Rtl.Rect(0, 0, FWinWidth, FWinHeight);
                    R = call__16;
                    switch (FPageTurnMode)
                    {
                        case Aow.Units.PageTurnCtrl.TPageTurnMode.ptLeft:
                            if (!FMasked)
                            {
                                FBackSurface.BltClipped(Surface, Ptr<Aow.Rtl.TRect>.To(R__cell, 0), FWinLeft, FWinTop);
                            }
                            else
                            {
                                var call__17 = FBackSurface.PixelFormatID;
                                var call__18 = Aow.Units.GFXE.GFXE.Color2PixelInfo(FMaskColor, call__17, null);
                                FBackSurface.BltColorKeyClipped(Surface, Ptr<Aow.Rtl.TRect>.To(R__cell, 0), FWinLeft, FWinTop, call__18);
                            }

                            break;
                        case Aow.Units.PageTurnCtrl.TPageTurnMode.ptRight:
                            if (!FMasked)
                            {
                                FPageSurface.BltClipped(Surface, Ptr<Aow.Rtl.TRect>.To(R__cell, 0), FWinLeft, FWinTop);
                            }
                            else
                            {
                                var call__19 = FPageSurface.PixelFormatID;
                                var call__20 = Aow.Units.GFXE.GFXE.Color2PixelInfo(FMaskColor, call__19, null);
                                FPageSurface.BltColorKeyClipped(Surface, Ptr<Aow.Rtl.TRect>.To(R__cell, 0), FWinLeft, FWinTop, call__20);
                            }

                            break;
                    }

                    FFirstAnimationFrame = false;
                }

                Surface.Lock(ref Dest);
                FBackSurface.Lock(ref Source);
                FPageSurface.Lock(ref OldPage);
                FPageTurn.RenderScene(FWinLeft + FBorderWidth, FWinTop + FBorderHeight, Dest, FBorderWidth, FBorderHeight, OldPage, FBorderWidth, FBorderHeight, Source, ref R);
                FPageSurface.Unlock();
                FBackSurface.Unlock();
                Surface.Unlock();
                FBackRect.Top = R.Top;
                // { Original also resets positions inside the top frame. }
                if ((FBackRect.Top < FWinTop) || (FBackRect.Top <= (FWinTop + FFrameTop)))
                {
                    FBackRect.Top = FWinTop - 1;
                }

                FBackRect.Bottom = (FWinTop - R.Top) + FWinHeight;
                FSurfaceValid = true;
                if (FOnDrawSurface != null)
                {
                    FOnDrawSurface(this, Surface, FWinLeft, FWinTop);
                }
            }
            else
            {
                base.Blt(Surface);
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
                var call__21 = Aow.Rtl.Rtl.Rect(0, 0, FWinWidth, FWinHeight);
                R = call__21;
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
