// Generated from AoWArnoLine.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWControl;
using static Aow.Units.AoWControl.AoWControl;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;

namespace Aow.Units.AoWArnoLine
{
    // { TAoWArnoLine: instance size $124; $114 top, $118 middle, $11C bottom,
//     $120 copied line image library. }
    public partial class TAoWArnoLine : Aow.Units.AoWControl.TAOWControl
    {
        public Aow.Units.ImageLib.TLibraryImage FImageTop;
        // { $114 }
        public Aow.Units.ImageLib.TLibraryImage FImageMiddle;
        // { $118 }
        public Aow.Units.ImageLib.TLibraryImage FImageBottom;
        // { $11C }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FLineLibrary;
        // AoWArnoLine.pas:47
        // { $120 }
        public override void Destroy()
        {
            FLineLibrary.Free();
            base.Destroy();
        }

        // AoWArnoLine.pas:41
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = Aow.Units.AOWInterfaceImgLib.TInterfaceIL.Create(this);
            FLineLibrary = call__1;
        }

        // AoWArnoLine.pas:53
        public override void Validate()
        {
            if (((FImageTop != null) && (FImageMiddle != null)) && (FImageBottom != null))
            {
                FLineLibrary.Clear();
                var call__2 = FImageTop.Copy();
                FLineLibrary.Add(call__2);
                var call__3 = FImageMiddle.Copy();
                FLineLibrary.Add(call__3);
                var call__4 = FImageBottom.Copy();
                FLineLibrary.Add(call__4);
                FValid = true;
            }

            base.Validate();
        }

        // AoWArnoLine.pas:93
        public override void SetSize()
        {
            FWinWidth = FImageTop.Width;
            base.SetSize();
        }

        // AoWArnoLine.pas:99
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWArnoLine.pas:66
        public void SetITop(Aow.Units.ImageLib.TLibraryImage Value)
        {
            if (Value != FImageTop)
            {
                FImageTop = Value;
                FValid = false;
            }
        }

        // AoWArnoLine.pas:75
        public void SetIMiddle(Aow.Units.ImageLib.TLibraryImage Value)
        {
            if (Value != FImageMiddle)
            {
                FImageMiddle = Value;
                FValid = false;
            }
        }

        // AoWArnoLine.pas:84
        public void SetIBottom(Aow.Units.ImageLib.TLibraryImage Value)
        {
            if (Value != FImageBottom)
            {
                FImageBottom = Value;
                FValid = false;
            }
        }

        [Published]
        public Aow.Units.ImageLib.TLibraryImage ImageTop { get => FImageTop; set => SetITop(value); }

        [Published]
        public Aow.Units.ImageLib.TLibraryImage ImageMiddle { get => FImageMiddle; set => SetIMiddle(value); }

        [Published]
        public Aow.Units.ImageLib.TLibraryImage ImageBottom { get => FImageBottom; set => SetIBottom(value); }

        // AoWArnoLine.pas:24
        public static new Aow.Units.AoWArnoLine.TAoWArnoLine Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AoWArnoLine.TAoWArnoLine o = new Aow.Units.AoWArnoLine.TAoWArnoLine();
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

    public static partial class AoWArnoLine
    {
        // AoWArnoLine.pas:119
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Tools", [typeof(Aow.Units.AoWArnoLine.TAoWArnoLine)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
