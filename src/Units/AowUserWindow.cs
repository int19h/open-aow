// Translated from AowUserWindow.pas.
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

namespace Aow.Units.AowUserWindow
{
    // { TAoWUserWindow: instance size $1F4; $1EC extended drawing event. }
    public partial class TAoWUserWindow : Aow.Units.AoWWindow.TAOWWindow
    {
        public Aow.Units.AowUserWindow.TDrawSurfaceExEvent FOnDrawSurfaceEx;
        // AowUserWindow.pas:29
        // { $1EC }
        public override void DrawSurface(Aow.Units.GFXE.TSurface Surface)
        {
            bool Handled = default;
            Handled = false;
            if (FOnDrawSurfaceEx != null)
            {
                FOnDrawSurfaceEx(Surface, ref Handled);
            }

            if (!Handled)
            {
                base.DrawSurface(Surface);
            }
        }

        [Published]
        public Aow.Units.AowUserWindow.TDrawSurfaceExEvent OnDrawSurfaceEx { get => FOnDrawSurfaceEx; set => FOnDrawSurfaceEx = value; }
    }

    public delegate void TDrawSurfaceExEvent(Aow.Units.GFXE.TSurface Surface, ref bool Handled);
    public static partial class AowUserWindow
    {
        // AowUserWindow.pas:38
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Tools", [typeof(Aow.Units.AowUserWindow.TAoWUserWindow)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
