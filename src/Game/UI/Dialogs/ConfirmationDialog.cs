using Aow.Game.Combat;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class ConfirmationDialog : DataModule
{
    public ConfirmationDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label TitleLabel;
    public Label MessageLabel;
    public Window DebugWindow;
    public Label FrameRateLabel;
    public Memo DebugMemo;
    public VerticalScrollBar DebugMemoScrollBar;
    public Panel OKPanel;
    public Panel YesNoPanel;
    private byte _action;

    public bool SkipCombatChoice { get; }

    // AOWDialogWin.pas:121
    public void Confirm(string title, string text, byte action)
    {
        _action = action;
        TitleLabel.Text = title;
        MessageLabel.Text = text;
        YesNoPanel.Visible = true;
        OKPanel.Visible = false;
        Window.Visible = true;
    }

    // AOWDialogWin.pas:113
    public void AddDebugMessage(string text)
    {
        DebugMemo.Items.Add(text);
        if (DebugMemo.Items.Count > 99)
        {
            DebugMemo.Items.Delete(0);
        }

        DebugMemo.Sized = false;
        DebugMemoScrollBar.Position = DebugMemoScrollBar.Max;
    }

    // AOWDialogWin.pas:60
    public void DButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AOWDialogWin.pas:67
    public void DebugDOkBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DebugWindow.Visible = false;
    }

    // AOWDialogWin.pas:73
    public void yesBtnClick()
    {
        string reason = "";
        Structure structure;
        DialogHosts.General.PlayButtonSound(0);
        switch (_action)
        {
            case 0:
            case 1:
                break;
            case 2:
                DialogHosts.General.City.Loot();
                break;
            case 3:
                if (DialogHosts.General.City.CanRaze(AoWHSMap.Current.SeatedPlayer, ref reason))
                {
                    if (DialogHosts.General.City.Raze(AoWHSMap.Current.SeatedPlayer))
                    {
                        structure = DialogHosts.General.Structure;
                        DialogHosts.MapEvents.TheMapStructureDeselected();
                        DialogHosts.MapEvents.TheMapStructureSelected(structure);
                    }
                }
                else
                {
                    AoWHSMap.Current.ShowWarning(reason);
                }

                break;
            case 4:
                AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Surrender();
                break;
            case 5:
                DialogHosts.WorldMap.WorldPanel.Visible = false;
                DialogHosts.General.ReturnToTitle();
                break;
            case 6:
                if (CombatMap.Current != null)
                {
                    CombatMap.Current.Surrender(CombatMap.Current.SeatedCombatPlayer);
                }

                break;
            case 7:
                if (DialogHosts.General.SelectedUnit != null)
                {
                    if (DialogHosts.General.SelectedUnit.CanDisband(ref reason))
                    {
                        if (DialogHosts.General.SelectedUnit.Disband())
                        {
                            DialogHosts.UnitWindow.MainWindow.Visible = false;
                        }
                    }
                    else
                    {
                        AoWHSMap.Current.ShowWarning(reason);
                    }
                }

                break;
        }

        Window.Visible = false;
    }

    // AOWDialogWin.pas:104
    public void DialogKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 27:
                DButtonClick();
                break;
            case 13:
                if (YesNoPanel.Visible)
                {
                    yesBtnClick();
                }
                else
                {
                    DButtonClick();
                }

                break;
        }
    }

    internal static void ConfirmDialog(ConfirmationDialog dialog, string title, string message, byte kind)
    {
        dialog._action = kind;
        dialog.TitleLabel.Text = title;
        dialog.MessageLabel.Text = message;
        dialog.YesNoPanel.Visible = true;
        dialog.OKPanel.Visible = false;
        dialog.Window.Visible = true;
    }

}
