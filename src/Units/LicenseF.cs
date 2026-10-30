// Translated from LicenseF.pas.
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

namespace Aow.Units.LicenseF
{
    public partial class TLicenseForm : Aow.Rtl.TForm
    {
        public Aow.Rtl.TMemo LicenseMemo;
        public Aow.Rtl.TPanel Panel1;
        public Aow.Rtl.TImage Image1;
        public Aow.Rtl.TLabel Label1;
        public Aow.Rtl.TPanel Panel2;
        public Aow.Rtl.TBevel Bevel1;
        public Aow.Rtl.TLabel Label2;
        public Aow.Rtl.TButton YesBtn;
        public Aow.Rtl.TButton NoBtn;
        public Aow.Rtl.TImage Image2;
        // LicenseF.pas:35
        public void FormCreate(Aow.Rtl.TObject Sender)
        {
            /* TODO: Implement StdCtrls.Lines in the platform or editor service. */
            throw new System.NotImplementedException();
        }
    }

    public static partial class LicenseF
    {
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
