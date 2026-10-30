// Translated from CenterViewLoc.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CenterViewLoc
{
    public partial class TCenterViewLocation : Aow.Units.HSEngine.TMapObject
    {
        public TCenterViewLocation(TECustomNode owner) : base(owner)
        {}

        // CenterViewLoc.pas:46
        public new const int ClassID = 2233640;

        // CenterViewLoc.pas:80
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // CenterViewLoc.pas:63
        public override void Activate()
        {
            base.Activate();
            if (!GetEditMode())
            {
                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCenterViewLocation != null)
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCenterViewLocation.Free();
                }

                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCenterViewLocation = this;
            }
        }

        // CenterViewLoc.pas:73
        public override void Deactivate()
        {
            base.Deactivate();
            if (!GetEditMode())
            {
                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCenterViewLocation == this)
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCenterViewLocation = null;
                }
            }
        }

        // CenterViewLoc.pas:55
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // CenterViewLoc.pas:49
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        // CenterViewLoc.pas:52
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // CenterViewLoc.pas:58
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            return Result;
        }

    }

    public partial class TCenterViewLocationResource : Aow.Units.HSEngine.TMapObjectResource
    {
        public TCenterViewLocationResource(TECustomNode owner) : base(owner)
        {
            SetLevel(27);
        }

        // CenterViewLoc.pas:91
        public new const int ClassID = 2233641;

        // CenterViewLoc.pas:85


        // CenterViewLoc.pas:102
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2233640;
            return Result;
        }

        // CenterViewLoc.pas:94
        public override void SetLevel(byte Value)
        {
            base.SetLevel(27);
        }

        // CenterViewLoc.pas:34


        // CenterViewLoc.pas:99
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Center View Location";
            return Result;
        }

    }

    public static partial class CenterViewLoc
    {
        // CenterViewLoc.pas:105
        public static void RegisterCenterViewLocation(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.CenterViewLoc.TCenterViewLocationResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.CenterViewLoc.TCenterViewLocation>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
