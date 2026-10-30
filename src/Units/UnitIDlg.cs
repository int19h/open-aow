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
    public partial class TUnitInfoDlg : Aow.Rtl.TForm
    {
        public Aow.Rtl.TButton OKBtn;
        public Aow.Rtl.TPanel NamePnl;
        public Aow.Rtl.TPanel Panel5;
        public Aow.Rtl.TPanel Panel4;
        public Aow.Rtl.TMemo AbilitiesMemo;
        public Aow.Rtl.TPanel Panel3;
        public Aow.Rtl.TImage Image10;
        public Aow.Rtl.TImage Image9;
        public Aow.Rtl.TImage Image8;
        public Aow.Rtl.TImage Image7;
        public Aow.Rtl.TImage Image6;
        public Aow.Rtl.TLabel AttackLbl;
        public Aow.Rtl.TLabel DefenseLbl;
        public Aow.Rtl.TLabel DamageLbl;
        public Aow.Rtl.TLabel ResistanceLbl;
        public Aow.Rtl.TLabel HitPointLbl;
        public Aow.Rtl.TPanel Panel1;
        public Aow.Rtl.TButton CloseBtn;
        public Aow.Rtl.TPanel AlignmentPnl;
        public Aow.Rtl.TImage Image1;
        public Aow.Rtl.TLabel MovesLbl;
        public Aow.Rtl.TIvTranslator Translator;
        // UnitIDlg.pas:104
        public int ShowUnit(Aow.Units.AoWE.TAbstractUnit AUnit)
        {
            /* TODO: Implement StdCtrls.Caption in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitIDlg.pas:120
        public int ShowUnitResource(Aow.Units.AoWE.TUnitResource Resource)
        {
            /* TODO: Implement StdCtrls.Caption in the platform or editor service. */
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
        public virtual sbyte GetDefense()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:65
        public virtual sbyte GetDamage()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:66
        public virtual sbyte GetResistance()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:67
        public virtual sbyte GetHits()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:68
        public virtual sbyte GetMoves()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // UnitIDlg.pas:69
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
