// Generated from StartMenu.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // StartMenu.pas:123
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (FValid)
            {
                var call__1 = FILFrame[FILIndexFrame + 6];
                FWinHeight -= call__1.Height;
                base.CheckMouseDown(Button, Shift, X, Y);
                var call__2 = FILFrame[FILIndexFrame + 6];
                FWinHeight += call__2.Height;
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
