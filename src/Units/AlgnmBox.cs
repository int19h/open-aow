// Translated from AlgnmBox.pas.
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

namespace Aow.Units.AlgnmBox
{
    // { TAlignmentBox (instance size 372, VMT 557FEB3C) }
    public partial class TAlignmentBox : Aow.Rtl.TComboBox
    {
        public Aow.Units.AoWE.TAlignment FAlignment;
        // { +$170 }
        public TSet<Aow.Units.AoWE.TAlignment> FAlignments;
        // { +$171 }
        public bool FSetMode;
        // { +$172 }
        public Aow.Units.FixedArray_byte_1 F_AlignmentBox_173;
        // AlgnmBox.pas:133
        public override void Loaded()
        {
            base.Loaded();
            GenerateItems();
        }

        // AlgnmBox.pas:54
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Create */
            throw new System.NotImplementedException();
        }

        // AlgnmBox.pas:60
        public void GenerateItems()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        // AlgnmBox.pas:100
        public void SetAlignments(TSet<Aow.Units.AoWE.TAlignment> Value)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        // AlgnmBox.pas:110
        public void SetAlignment(Aow.Units.AoWE.TAlignment Value)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        // AlgnmBox.pas:120
        protected internal override void Change()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.ItemIndex */
            throw new System.NotImplementedException();
        }

        [Published]
        public bool UseSets { get => FSetMode; set => FSetMode = value; }

        // AlgnmBox.pas:24
        public static Aow.Units.AlgnmBox.TAlignmentBox Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AlgnmBox.TAlignmentBox o = new Aow.Units.AlgnmBox.TAlignmentBox();
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

    public static partial class AlgnmBox
    {
        // AlgnmBox.pas:139
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.AlgnmBox.TAlignmentBox)]);
        }

        public static Aow.Rtl.AnsiString NoneRStr = (AnsiString)"None";

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
