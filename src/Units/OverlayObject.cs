// Translated from OverlayObject.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
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

namespace Aow.Units.OverlayObject
{
    // { TOverlayObject (instance size 28, VMT 557D3C78) }
    public partial class TOverlayObject : Aow.Units.ILTer.TFixedILTerrainMO
    {
        public TOverlayObject(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // OverlayObject.pas:36
        public new const int ClassID = 131228;

    }

    // { TOverlayObjectResource (instance size 72, VMT 557D3E30): $44 FOverlayType. }
    public partial class TOverlayObjectResource : Aow.Units.ILTer.TFixedILTerrainMOR
    {
        public TOverlayObjectResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // OverlayObject.pas:42
        public new const int ClassID = 131229;

        public int FOverlayType;
        // OverlayObject.pas:48
        // { $44 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FOverlayType, 40);
        }

        // OverlayObject.pas:45
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131228;
            return Result;
        }

        // OverlayObject.pas:39
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__1 = Aow.Units.AoWE.AoWE.OverlayObjectRStr;
            var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(call__1);
            Result = call__2;
            return Result;
        }

    }

    public static partial class OverlayObject
    {
        // OverlayObject.pas:54
        public static void RegisterOverlayObject(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.OverlayObject.TOverlayObject>(), Aow.Units.Engine.EClass.Of<Aow.Units.OverlayObject.TOverlayObjectResource>()]);
        }

        // OverlayObject.pas:59
        public static void UnRegisterOverlayObject(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.OverlayObject.TOverlayObject>(), Aow.Units.Engine.EClass.Of<Aow.Units.OverlayObject.TOverlayObjectResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
