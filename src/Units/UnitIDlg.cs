// Translated from UnitIDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;

namespace Aow.Units.UnitIDlg;

public partial class TUnitInfoDlg : TForm
{
    public TButton OKBtn;
    public TPanel NamePnl;
    public TPanel Panel5;
    public TPanel Panel4;
    public TMemo AbilitiesMemo;
    public TPanel Panel3;
    public TImage Image10;
    public TImage Image9;
    public TImage Image8;
    public TImage Image7;
    public TImage Image6;
    public TLabel AttackLbl;
    public TLabel DefenseLbl;
    public TLabel DamageLbl;
    public TLabel ResistanceLbl;
    public TLabel HitPointLbl;
    public TPanel Panel1;
    public TButton CloseBtn;
    public TPanel AlignmentPnl;
    public TImage Image1;
    public TLabel MovesLbl;
    public TIvTranslator Translator;

    // UnitIDlg.pas:104
    public int ShowUnit(TAbstractUnit AUnit)
    {
        /* TODO: Implement StdCtrls.Caption in the platform or editor service. */
        throw new NotImplementedException();
    }

    // UnitIDlg.pas:120
    public int ShowUnitResource(TUnitResource Resource)
    {
        /* TODO: Implement StdCtrls.Caption in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Forms.pas:407
    public TUnitInfoDlg(TComponent owner) : base()
    { }
}

public partial class TUnitView : TAbilityOwner
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
    public virtual string GetName()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // UnitIDlg.pas:78
    public virtual TAlignment GetAlignment()
    {
        throw EAbstractError.Create("Abstract Error");
    }
}

public struct TUnitResourceView
{
    public FixedArray_byte_40 Reserved00;
    public byte Alignment;
    public sbyte Attack;
    public sbyte Defense;
    public sbyte Damage;
    public sbyte Hits;
    public sbyte Moves;
    public sbyte Resistance;
    public FixedArray_byte_9 Reserved2F;
    public TAbilityOwner AbilityOwner;
}

public static partial class UnitIDlgUnit
{

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
