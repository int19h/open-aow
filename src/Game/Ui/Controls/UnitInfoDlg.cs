// Translated from UnitIDlg.pas.
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Controls;

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
    public UnitInfoDlg(Component owner) : base()
    { }
}
