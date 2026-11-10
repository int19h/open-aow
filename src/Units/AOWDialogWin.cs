// Translated from AOWDialogWin.pas.
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AOWDialogWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class DWin : DataModule
{
    public DWin(Component owner) : base(owner)
    { }

    public AOWWindow Dialog;
    public AOWLabel DTitle;
    public AOWLabel DMessage;
    public AOWButton DButton;
    public AOWWindow DebugWin;
    public AOWLabel DebugDTitle;
    public AOWButton DebugDOkBtn;
    public AOWLabel FrameLbl;
    public AOWLabel DebugDfrTitle;
    public AOWMemo DebugMemo;
    public AOWVScrollBar DMemoScroll;
    public AOWPanel okPnl;
    public AOWPanel yesPnl;
    public AOWButton yesBtn;
    public AOWButton noBtn;
    public AoWFrame BCeltL;
    public AoWFrame TCeltL;
    public AOWImage TCeltLineL;
    public AOWImage AOWImage1;
    public AOWImage AOWImage2;
    public AoWFrame AoWFrame1;
    public AoWFrame SignBLine;
    public AOWImage SIcon;
    public IvTranslator Translator;
    public byte FAction;
    public bool FSkipCombatChoice;

    // AOWDialogWin.pas:121
    public void Confirm(string Title, string Text, byte Action)
    {
        FAction = Action;
        DTitle.Text = Title;
        DMessage.Text = Text;
        yesPnl.Visible = true;
        okPnl.Visible = false;
        Dialog.Visible = true;
    }

    // AOWDialogWin.pas:113
    public void AddDebugMessage(string Text)
    {
        DebugMemo.Items.Add(Text);
        if (DebugMemo.Items.Count > 99)
        {
            DebugMemo.Items.Delete(0);
        }

        DebugMemo.FSized = false;
        DMemoScroll.Position = DMemoScroll.Max;
    }

    // AOWDialogWin.pas:60
    public void DButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dialog.Visible = false;
    }

    // AOWDialogWin.pas:67
    public void DebugDOkBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DebugWin.Visible = false;
    }

    // AOWDialogWin.pas:73
    public void yesBtnClick()
    {
        string Reason = "";
        Structure Structure;
        DialogHosts.General.PlayButtonSound(0);
        switch (FAction)
        {
            case 0:
            case 1:
                break;
            case 2:
                DialogHosts.General.FCity.Loot();
                break;
            case 3:
                if (DialogHosts.General.FCity.CanRaze(AoWHSMap.Current.FSeatedPlayer, ref Reason))
                {
                    if (DialogHosts.General.FCity.Raze(AoWHSMap.Current.FSeatedPlayer))
                    {
                        Structure = DialogHosts.General.FStructure;
                        DialogHosts.MapEvents.TheMapStructureDeselected();
                        DialogHosts.MapEvents.TheMapStructureSelected(Structure);
                    }
                }
                else
                {
                    AoWHSMap.Current.ShowWarning(Reason);
                }

                break;
            case 4:
                AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).Surrender();
                break;
            case 5:
                DialogHosts.WorldMap.WorldPnl.Visible = false;
                DialogHosts.General.ReturnToTitle();
                break;
            case 6:
                if (Combat.AoWCombatMap.Current != null)
                {
                    Combat.AoWCombatMap.Current.Surrender(Combat.AoWCombatMap.Current.FSeatedCombatPlayer);
                }

                break;
            case 7:
                if (DialogHosts.General.FSelectedUnit != null)
                {
                    if (DialogHosts.General.FSelectedUnit.CanDisband(ref Reason))
                    {
                        if (DialogHosts.General.FSelectedUnit.Disband())
                        {
                            DialogHosts.UnitWindow.UnitWin.Visible = false;
                        }
                    }
                    else
                    {
                        AoWHSMap.Current.ShowWarning(Reason);
                    }
                }

                break;
        }

        Dialog.Visible = false;
    }

    // AOWDialogWin.pas:104
    public void DialogKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        switch (Key)
        {
            case 27:
                DButtonClick();
                break;
            case 13:
                if (yesPnl.Visible)
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

    internal static void ConfirmDialog(DWin Dialog, string Title, string Message, byte Kind)
    {
        Dialog.FAction = Kind;
        Dialog.DTitle.Text = Title;
        Dialog.DMessage.Text = Message;
        Dialog.yesPnl.Visible = true;
        Dialog.okPnl.Visible = false;
        Dialog.Dialog.Visible = true;
    }

}
