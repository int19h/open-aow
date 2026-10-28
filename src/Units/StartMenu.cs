// Translated from StartMenu.pas.
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
using Aow.Units.AOWMenu;
using static Aow.Units.AOWMenu.AOWMenu;
using Aow.Units.AOWWinManager;
using static Aow.Units.AOWWinManager.AOWWinManager;
using Aow.Units.AOWMenuBar;
using static Aow.Units.AOWMenuBar.AOWMenuBar;

namespace Aow.Units.StartMenu
{
    // { TStartMenu: instance size $258; $254 whole-menu image library. }
    public partial class TStartMenu : Aow.Units.AOWMenu.TAOWMenu
    {
        public Aow.Units.ImageLib.TImageLibrary FILWholeMenu;
        // StartMenu.pas:30
        // { $254 }
        public override void SetSize()
        {
            Aow.Units.AOWWinManager.TAOWWinManager M = default;
            base.SetSize();
            FWinHeight = 377;
            M = FWinManager;
            if ((M != null) && (FOwnerMenuBar != null))
            {
                FWinTop = M.FWinHeight - 393;
            }

            FWinLeft = 0;
        }

        // StartMenu.pas:41
        public override void SetVisible(bool Value)
        {
            Aow.Units.AOWWinManager.TAOWWinManager M = default;
            Aow.Units.AOWMenuBar.TAOWMenuBar B = default;
            // { Match trials (4): named temporaries, with scopes, Pointer casts, zero-shift scheduling, identity assignments; still differs. }
            if (Value != FVisible)
            {
                FVisible = Value;
                if (FVisible == true)
                {
                    FHighlighted = -1;
                    FWinHeight = 377;
                    M = FWinManager;
                    if ((M != null) && (FOwnerMenuBar != null))
                    {
                        FWinTop = M.FWinHeight - 393;
                    }

                    FWinLeft = 0;
                    FDrawn = false;
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
                else
                {
                    B = FOwnerMenuBar;
                    if (B != null)
                    {
                        B.FSelected = -1;
                        B.FDrawn = false;
                    }

                    FHidden = true;
                    if (FOnHide != null)
                    {
                        FOnHide(this);
                    }
                }
            }

            if (FVisible)
            {
                if ((!FValid || !FAligned) || !FSized)
                {
                    Setup();
                }

                ClearBltList();
                ClearDrawList();
                FDrawn = false;
                SetPriority(FPriority);
            }
        }

        // StartMenu.pas:87
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
            Aow.Rtl.TRect OldClip = default;
            try
            {
                if (FValid && FVisible)
                {
                    if (!FDrawn)
                    {
                        OldClip = SurfDesc.ClipRect;
                        FAOWFont.Color = FColor;
                        var call__1 = FILWholeMenu[FHighlighted + 1];
                        call__1.ShowClipped(0, 0, SurfDesc);
                        {
                            I = 0;
                            int end__2 = FItems.Count - 1;
                            for (; I <= end__2; I += 1)
                            {
                                var call__3 = FItems.Count;
                                if (call__3 > 0)
                                {
                                    var call__4 = FILFrame[FILIndexFrame + 2];
                                    if (((FWinLeft + FWinWidth) - call__4.Width) < OldClip.Right)
                                    {
                                        var call__5 = FILFrame[FILIndexFrame + 2];
                                        SurfDesc.ClipRect.Right = (FWinLeft + FWinWidth) - call__5.Width;
                                    }

                                    if (I == FHighlighted)
                                    {
                                        FAOWFont.Color = FSelColor;
                                    }

                                    var call__6 = FItems[I];
                                    var call__7 = FILFrame[FILIndexFrame];
                                    var call__8 = FILFrame[FILIndexFrame + 3];
                                    FAOWFont.SetText((FWinLeft + FBorderWidth) + call__8.Width, ((FWinTop + FBorderHeight) + call__7.Height) + (FItemHeight * I), call__6, SurfDesc);
                                    if (I == FHighlighted)
                                    {
                                        FAOWFont.Color = FColor;
                                    }

                                    FDrawItem = I;
                                    var call__9 = FILFrame[FILIndexFrame + 1];
                                    var call__10 = FILFrame[FILIndexFrame + 1];
                                    var call__11 = FILFrame[FILIndexFrame + 3];
                                    var call__12 = Aow.Rtl.Rtl.Rect(FWinLeft + call__11.Width, (FWinTop + call__10.Height) + (FItemHeight * I), FWinLeft + FWinWidth, (FWinTop + call__9.Height) + ((I + 1) * FItemHeight));
                                    FDrawItemRect = call__12;
                                    if (FOnDrawItem != null)
                                    {
                                        FOnDrawItem(this);
                                    }

                                    SurfDesc.ClipRect = OldClip;
                                }
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

        // StartMenu.pas:123
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (FValid)
            {
                var call__13 = FILFrame[FILIndexFrame + 6];
                FWinHeight -= call__13.Height;
                base.CheckMouseDown(Button, Shift, X, Y);
                var call__14 = FILFrame[FILIndexFrame + 6];
                FWinHeight += call__14.Height;
            }
        }

        [Published]
        public Aow.Units.ImageLib.TImageLibrary ILWholeMenu { get => FILWholeMenu; set => FILWholeMenu = value; }
    }

    public static partial class StartMenu
    {
        // StartMenu.pas:133
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Tools", [typeof(Aow.Units.StartMenu.TStartMenu)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
