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

public partial class TDWin : TDataModule
{
    public TDWin(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dialog;
    public TAOWLabel DTitle;
    public TAOWLabel DMessage;
    public TAOWButton DButton;
    public TAOWWindow DebugWin;
    public TAOWLabel DebugDTitle;
    public TAOWButton DebugDOkBtn;
    public TAOWLabel FrameLbl;
    public TAOWLabel DebugDfrTitle;
    public TAOWMemo DebugMemo;
    public TAOWVScrollBar DMemoScroll;
    public TAOWPanel okPnl;
    public TAOWPanel yesPnl;
    public TAOWButton yesBtn;
    public TAOWButton noBtn;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TAoWFrame AoWFrame1;
    public TAoWFrame SignBLine;
    public TAOWImage SIcon;
    public TIvTranslator Translator;
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dialog.Visible = false;
    }

    // AOWDialogWin.pas:67
    public void DebugDOkBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        DebugWin.Visible = false;
    }

    // AOWDialogWin.pas:73
    public void yesBtnClick()
    {
        string Reason = "";
        TStructure Structure;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        switch (FAction)
        {
            case 0:
            case 1:
                break;
            case 2:
                Aow.Game.Ui.Dialogs.DialogHosts.General.FCity.Loot();
                break;
            case 3:
                if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCity.CanRaze(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer, ref Reason))
                {
                    if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCity.Raze(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer))
                    {
                        Structure = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure;
                        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapStructureDeselected();
                        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapStructureSelected(Structure);
                    }
                }
                else
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                }

                break;
            case 4:
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).Surrender();
                break;
            case 5:
                Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.WorldPnl.Visible = false;
                Aow.Game.Ui.Dialogs.DialogHosts.General.ReturnToTitle();
                break;
            case 6:
                if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
                {
                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Surrender(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedCombatPlayer);
                }

                break;
            case 7:
                if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit != null)
                {
                    if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.CanDisband(ref Reason))
                    {
                        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.Disband())
                        {
                            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = false;
                        }
                    }
                    else
                    {
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                    }
                }

                break;
        }

        Dialog.Visible = false;
    }

    // AOWDialogWin.pas:104
    public void DialogKeyDown(ref ushort Key, TSet<TShiftState> Shift)
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

    internal static void ConfirmDialog(TDWin Dialog, string Title, string Message, byte Kind)
    {
        Dialog.FAction = Kind;
        Dialog.DTitle.Text = Title;
        Dialog.DMessage.Text = Message;
        Dialog.yesPnl.Visible = true;
        Dialog.okPnl.Visible = false;
        Dialog.Dialog.Visible = true;
    }

}
