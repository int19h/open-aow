// Translated from Crypt.pas.
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
using Aow.Units.ItemExpS;
using static Aow.Units.ItemExpS.ItemExpS;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Crypt
{
    // { TCrypt (instance size 64, no added fields, VMT 557C64D0) }
    public partial class TCrypt : Aow.Units.ItemExpS.TItemExplorationSite
    {
        public TCrypt(TECustomNode owner) : base(owner)
        {}

        // Crypt.pas:34
        public new const int ClassID = 131850;

    }

    // { TCryptResource (instance size 104, no added fields, VMT 557C6740) }
    public partial class TCryptResource : Aow.Units.ItemExpS.TItemExplorationSiteResource
    {
        public TCryptResource(TECustomNode owner) : base(owner)
        {}

        // Crypt.pas:40
        public new const int ClassID = 131851;

        // Crypt.pas:43
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131850;
            return Result;
        }

        // Crypt.pas:37
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CryptRStr);
            return Result;
        }

    }

    public static partial class Crypt
    {
        // Crypt.pas:46
        public static void RegisterCrypt(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Crypt.TCrypt>(), Aow.Units.Engine.EClass.Of<Aow.Units.Crypt.TCryptResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
