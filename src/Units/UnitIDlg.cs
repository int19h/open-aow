// Translated from UnitIDlg.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from UnitIDlg.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Controls
{

    public partial class UnitInfoDlg : Form
    {
        public Button OKBtn;
        public Panel NamePnl;
        public Panel Panel5;
        public Panel Panel4;
        public Memo AbilitiesMemo;
        public Panel Panel3;
        public Image Image10;
        public Image Image9;
        public Image Image8;
        public Image Image7;
        public Image Image6;
        public Label AttackLbl;
        public Label DefenseLbl;
        public Label DamageLbl;
        public Label ResistanceLbl;
        public Label HitPointLbl;
        public Panel Panel1;
        public Button CloseBtn;
        public Panel AlignmentPnl;
        public Image Image1;
        public Label MovesLbl;
        public IvTranslator Translator;

        // UnitIDlg.pas:104
        public int ShowUnit(AbstractUnit AUnit)
        {
            /* TODO: Implement StdCtrls.Caption in the platform or editor service. */
            throw new NotImplementedException();
        }

        // UnitIDlg.pas:120
        public int ShowUnitResource(UnitResource Resource)
        {
            /* TODO: Implement StdCtrls.Caption in the platform or editor service. */
            throw new NotImplementedException();
        }

        // Forms.pas:407
        public UnitInfoDlg(global::Aow.Rtl.Component owner) : base()
        { }
    }
}

namespace Aow.Game.Magic.Abilities
{

    public partial class UnitView : AbilityOwner
    {
        public UnitView(ECustomNode owner) : base(owner)
        { }

        // UnitIDlg.pas:54
        public virtual void Slot9C()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:55
        public virtual void SlotA0()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:56
        public virtual void SlotA4()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:57
        public virtual void SlotA8()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:58
        public virtual void SlotAC()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:59
        public virtual void SlotB0()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:60
        public virtual void SlotB4()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:61
        public virtual void SlotB8()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:62
        public virtual void SlotBC()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:63
        public virtual sbyte GetAttack()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:64
        public virtual sbyte GetDefense()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:65
        public virtual sbyte GetDamage()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:66
        public virtual sbyte GetResistance()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:67
        public virtual sbyte GetHits()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:68
        public virtual sbyte GetMoves()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:69
        public virtual void SlotD8()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:70
        public virtual void SlotDC()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:71
        public virtual void SlotE0()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:72
        public virtual void SlotE4()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:73
        public virtual void SlotE8()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:74
        public virtual void SlotEC()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:75
        public virtual void SlotF0()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:76
        public virtual void SlotF4()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:77
        public virtual string GetName()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // UnitIDlg.pas:78
        public virtual Alignment GetAlignment()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{

    public struct UnitResourceView
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
        public global::Aow.Game.Magic.Abilities.AbilityOwner AbilityOwner;
    }
}
