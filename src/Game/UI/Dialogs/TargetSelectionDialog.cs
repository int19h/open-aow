using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class TargetSelectionDialog : DataModule
{
    public TargetSelectionDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Frame CeltFrame;
    public Button Close;
    public Label Title;
    public Image SIcon;
    public Button MainBtn;
    public Memo DefaultMemo;
    public Panel DefaultPnl;
    public Label HeaderLbl;
    public Panel HeaderPnl;
    public ArnoLine BorderImage;
    public Panel SpellPnl;
    public Memo SpellMemo;
    public Label TargetRangeLabel;
    public Label TargetLbl;
    public IvTranslator Translator;

    // AoWTargetSelection.pas:45
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        AoWHSMap.Current.TargetSelection.Deactivate();
    }

    // AoWTargetSelection.pas:51
    public void MainBtnClick()
    {
        CloseClick();
    }
}
