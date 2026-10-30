// Translated from UnitIDlg.pas.
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

namespace Aow.Units.UnitIDlg
{
    // { TUnitInfoDlg (instance size 564, VMT 557FF1B8) }
    public partial class TUnitInfoDlg : Aow.Rtl.TForm
    {
        public Aow.Rtl.TButton OKBtn;
        // { +$1DC }
        public Aow.Rtl.TPanel NamePnl;
        // { +$1E0 }
        public Aow.Rtl.TPanel Panel5;
        // { +$1E4 }
        public Aow.Rtl.TPanel Panel4;
        // { +$1E8 }
        public Aow.Rtl.TMemo AbilitiesMemo;
        // { +$1EC }
        public Aow.Rtl.TPanel Panel3;
        // { +$1F0 }
        public Aow.Rtl.TImage Image10;
        // { +$1F4 }
        public Aow.Rtl.TImage Image9;
        // { +$1F8 }
        public Aow.Rtl.TImage Image8;
        // { +$1FC }
        public Aow.Rtl.TImage Image7;
        // { +$200 }
        public Aow.Rtl.TImage Image6;
        // { +$204 }
        public Aow.Rtl.TLabel AttackLbl;
        // { +$208 }
        public Aow.Rtl.TLabel DefenseLbl;
        // { +$20C }
        public Aow.Rtl.TLabel DamageLbl;
        // { +$210 }
        public Aow.Rtl.TLabel ResistanceLbl;
        // { +$214 }
        public Aow.Rtl.TLabel HitPointLbl;
        // { +$218 }
        public Aow.Rtl.TPanel Panel1;
        // { +$21C }
        public Aow.Rtl.TButton CloseBtn;
        // { +$220 }
        public Aow.Rtl.TPanel AlignmentPnl;
        // { +$224 }
        public Aow.Rtl.TImage Image1;
        // { +$228 }
        public Aow.Rtl.TLabel MovesLbl;
        // { +$22C }
        public Aow.Rtl.TIvTranslator Translator;
        // UnitIDlg.pas:104
        // { +$230 }
        public int ShowUnit(Aow.Units.AoWE.TAbstractUnit AUnit)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Caption */
            throw new System.NotImplementedException();
        }

        // UnitIDlg.pas:120
        public int ShowUnitResource(Aow.Units.AoWE.TUnitResource Resource)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Caption */
            throw new System.NotImplementedException();
        }

        // Forms.pas:407
        public TUnitInfoDlg(TComponent owner) : base()
        { }
    }

    public partial class TUnitView : Aow.Units.AoWE.TAbilityOwner
    {
        public TUnitView(TECustomNode owner) : base(owner)
        { }

        // UnitIDlg.pas:54
        public virtual void Slot9C()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:55
        public virtual void SlotA0()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:56
        public virtual void SlotA4()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:57
        public virtual void SlotA8()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:58
        public virtual void SlotAC()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:59
        public virtual void SlotB0()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:60
        public virtual void SlotB4()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:61
        public virtual void SlotB8()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:62
        public virtual void SlotBC()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:63
        public virtual sbyte GetAttack()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:64
        // { +$C0 }
        public virtual sbyte GetDefense()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:65
        // { +$C4 }
        public virtual sbyte GetDamage()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:66
        // { +$C8 }
        public virtual sbyte GetResistance()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:67
        // { +$CC }
        public virtual sbyte GetHits()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:68
        // { +$D0 }
        public virtual sbyte GetMoves()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:69
        // { +$D4 }
        public virtual void SlotD8()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:70
        public virtual void SlotDC()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:71
        public virtual void SlotE0()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:72
        public virtual void SlotE4()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:73
        public virtual void SlotE8()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:74
        public virtual void SlotEC()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:75
        public virtual void SlotF0()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:76
        public virtual void SlotF4()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:77
        public virtual Aow.Rtl.AnsiString GetName()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:78
        // { +$F8 }
        public virtual Aow.Units.AoWE.TAlignment GetAlignment()
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    public struct TUnitResourceView
    {
        public Aow.Units.FixedArray_byte_40 Reserved00;
        public byte Alignment;
        public sbyte Attack;
        public sbyte Defense;
        public sbyte Damage;
        public sbyte Hits;
        public sbyte Moves;
        public sbyte Resistance;
        public Aow.Units.FixedArray_byte_9 Reserved2F;
        public Aow.Units.AoWE.TAbilityOwner AbilityOwner;
    }

    public static partial class UnitIDlg
    {

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
