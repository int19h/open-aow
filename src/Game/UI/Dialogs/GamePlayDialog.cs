// Translated from AoWGamePlayDlg.pas.
using Aow.Engine.Buffers;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// pending password
public partial class GamePlayDialog : DataModule
{
    public GamePlayDialog(Component owner) : base(owner)
    { }

    public Window Root;
    public Button CancelBtn;
    public Frame BCeltL;
    public Label Title;
    public Button Close;
    public Image TCeltLR;
    public Image TCeltLL;
    public Image TitleIcon;
    public CheckBox AutoSaveCB;
    public Label asLbl;
    public CheckBox SkipIntroCB;
    public Label siLbl;
    public TabPanel Tab1;
    public Panel Pnl1;
    public Panel Pnl2;
    public Label GamePasswordLbl;
    public Edit PasswordEdit;
    public ComboBox CombatResolveModeCB;
    public VerticalScrollBar CombatResolveModeSB;
    public CheckBox Classic1CB;
    public Label PopupLbl1;
    public CheckBox Classic2CB;
    public Label PopupLbl2;
    public CheckBox Classic3CB;
    public Label PopupLbl3;
    public CheckBox Classic4CB;
    public CheckBox Classic5CB;
    public CheckBox Classic6CB;
    public Label WindowPopupLbl;
    public Label ClassicLbl;
    public Label SimultaneousLbl;
    public CheckBox Sim1CB;
    public CheckBox Sim2CB;
    public CheckBox Sim3CB;
    public CheckBox Sim4CB;
    public CheckBox Sim5CB;
    public CheckBox Sim6CB;
    public Label PopupLbl4;
    public Label PopupLbl5;
    public Label PopupLbl6;
    public CheckBox Classic7CB;
    public CheckBox Sim7CB;
    public Label PopupLbl7;
    public Button ChangePasswordBtn;
    public ComboBox MovementSpeedCB;
    public Button OKBtn;
    public CheckBox Classic8CB;
    public CheckBox Sim8CB;
    public Label PopupLbl8;
    public Label CombLbl;
    public Panel CombFr;
    public Panel CombPnl;
    public Label Movelbl;
    public Panel MovFr;
    public Panel BtmPnl;
    public Panel MovPnl;
    public Panel AOWPanel1;
    public Image CeltLineL;
    public IvTranslator Translator;
    public string FNewPassword = "";

    // AoWGamePlayDlg.pas:200
    public void PasswordEntered(EventLog Log)
    {
        PasswordEventLog Password = default;
        if (((PasswordEventLog)Log).FPressedButton == PressedEventLogButton.Ok)
        {
            if (!IsEmpty(((PasswordEventLog)Log).FPassword))
            {
                FNewPassword = ((PasswordEventLog)Log).FPassword;
                Password = new PasswordEventLog(null);
                Password.FTitle = AbilityRules.TranslateRStr(PasswordConfirmationRStr);
                Password.FPrompt = AbilityRules.TranslateRStr(ReenterNewPasswordRStr);
                AoWHSMap.Current.TriggerExecuteEventLog(Password, true, PasswordConfirmed);
                Password.Release();
            }
            else
            {
                PasswordEdit.Text = "";
            }
        }
    }

    // AoWGamePlayDlg.pas:217
    public void PasswordConfirmed(EventLog Log)
    {
        if (((PasswordEventLog)Log).FPassword == FNewPassword)
        {
            var encode = AbilityRules.Encode(FNewPassword, 0);
            PasswordEdit.Text = encode;
        }
        else
        {
            MessageDialog.ResetMessageDialog(DialogHosts.MessageDlg);
            DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var confirmationFailed = AbilityRules.TranslateRStr(ConfirmationFailedRStr);
            DialogHosts.MessageDlg.MessageMemo.Items.Add(confirmationFailed);
            DialogHosts.MessageDlg.OKPnl.Visible = true;
            DialogHosts.MessageDlg.Dlg.Visible = true;
        }
    }

    // AoWGamePlayDlg.pas:95
    public void GamePlayDlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        switch (Key)
        {
            case 27:
                CancelClick();
                break;
        }
    }

    // AoWGamePlayDlg.pas:100
    public void GamePlayDlgShow()
    {
        GameSettings.CombatResolveMode Mode;
        Set<WindowPopupEvent> Events;
        var autoSave = GameTables.AoWEngine.FRegistry.GetAutoSave();
        AutoSaveCB.Checked = autoSave;
        var skipIntro = GameTables.AoWEngine.FRegistry.GetSkipIntro();
        SkipIntroCB.Checked = skipIntro;
        var movementSpeed = GameTables.AoWEngine.FRegistry.GetMovementSpeed();
        MovementSpeedCB.Selected = (int)movementSpeed;
        CombatResolveModeCB.Items.Clear();
        for (int modeOrdinal = (int)(GameSettings.CombatResolveMode)0; modeOrdinal <= (int)(GameSettings.CombatResolveMode)2; modeOrdinal++)
        {
            Mode = (GameSettings.CombatResolveMode)modeOrdinal;
            var combatResolveMode = AbilityRules.TranslateRStr(TCombatResolveModeRStr[(int)Mode]);
            CombatResolveModeCB.Items.AddObject(combatResolveMode, IntToObject((int)Mode));
        }

        var fRegistryCombatResolveMode = GameTables.AoWEngine.FRegistry.GetCombatResolveMode();
        var indexOfObject = CombatResolveModeCB.Items.IndexOfObject(IntToObject((int)fRegistryCombatResolveMode));
        CombatResolveModeCB.Selected = indexOfObject;
        Events = GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents();
        Classic1CB.Checked = Events.Contains(WindowPopupEvent.Combat);
        Classic2CB.Checked = Events.Contains(WindowPopupEvent.Diplomacy);
        Classic3CB.Checked = Events.Contains(WindowPopupEvent.SpellResearched);
        Classic4CB.Checked = Events.Contains(WindowPopupEvent.SpellReady);
        Classic5CB.Checked = Events.Contains(WindowPopupEvent.Rebellion);
        Classic6CB.Checked = Events.Contains(WindowPopupEvent.PartyDamage);
        Classic7CB.Checked = Events.Contains(WindowPopupEvent.HeroUpgrade);
        Classic8CB.Checked = Events.Contains(WindowPopupEvent.HeroJoin);
        Events = GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents();
        Sim1CB.Checked = Events.Contains(WindowPopupEvent.Combat);
        Sim2CB.Checked = Events.Contains(WindowPopupEvent.Diplomacy);
        Sim3CB.Checked = Events.Contains(WindowPopupEvent.SpellResearched);
        Sim4CB.Checked = Events.Contains(WindowPopupEvent.SpellReady);
        Sim5CB.Checked = Events.Contains(WindowPopupEvent.Rebellion);
        Sim6CB.Checked = Events.Contains(WindowPopupEvent.PartyDamage);
        Sim7CB.Checked = Events.Contains(WindowPopupEvent.HeroUpgrade);
        Sim8CB.Checked = Events.Contains(WindowPopupEvent.HeroJoin);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
        PasswordEdit.Text = player.FSettings;
    }

    // AoWGamePlayDlg.pas:133
    public void ChangePasswordBtnClick()
    {
        PasswordEventLog Log = default;
        if (AoWHSMap.Current.FPlayMode != PlayMode.Turn || AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network || AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer)
        {
            Log = new PasswordEventLog(null);
            Log.FPrompt = AbilityRules.TranslateRStr(EnterNewPasswordRStr);
            AoWHSMap.Current.TriggerExecuteEventLog(Log, true, PasswordEntered);
            Log.Release();
        }
    }

    // AoWGamePlayDlg.pas:147
    public void OKBtnClick()
    {
        string Error = "";
        Set<WindowPopupEvent> Events;
        DialogHosts.General.PlayButtonSound(0);
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FSettings != PasswordEdit.Text)
        {
            if (AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FSettings = PasswordEdit.Text;
            }
            else
            {
                if (!AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).ChangePlayerSettings(PasswordEdit.Text, ref Error))
                {
                    MessageDialog.ResetMessageDialog(DialogHosts.MessageDlg);
                    DialogHosts.MessageDlg.MessageMemo.Items.Add("");
                    var couldntChangePassword = AbilityRules.TranslateRStr(CouldntChangePasswordRStr);
                    DialogHosts.MessageDlg.MessageMemo.Items.Add(couldntChangePassword);
                    DialogHosts.MessageDlg.OKPnl.Visible = true;
                    DialogHosts.MessageDlg.Dlg.Visible = true;
                }
            }
        }

        GameTables.AoWEngine.FRegistry.SetAutoSave(AutoSaveCB.Checked);
        GameTables.AoWEngine.FRegistry.SetSkipIntro(SkipIntroCB.Checked);
        GameTables.AoWEngine.FRegistry.SetMovementSpeed((GameSettings.MovementSpeed)MovementSpeedCB.Selected);
        var objectValue = CombatResolveModeCB.Items.Objects[CombatResolveModeCB.Selected];
        GameTables.AoWEngine.FRegistry.SetCombatResolveMode((GameSettings.CombatResolveMode)ObjectToInt(objectValue));
        Events = Set<WindowPopupEvent>.Of();
        if (Classic1CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Combat);
        }

        if (Classic2CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Diplomacy);
        }

        if (Classic3CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellResearched);
        }

        if (Classic4CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellReady);
        }

        if (Classic5CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Rebellion);
        }

        if (Classic6CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.PartyDamage);
        }

        if (Classic7CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroUpgrade);
        }

        if (Classic8CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroJoin);
        }

        GameTables.AoWEngine.FRegistry.SetClassicWindowPopupEvents(Events);
        Events = Set<WindowPopupEvent>.Of();
        if (Sim1CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Combat);
        }

        if (Sim2CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Diplomacy);
        }

        if (Sim3CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellResearched);
        }

        if (Sim4CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellReady);
        }

        if (Sim5CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Rebellion);
        }

        if (Sim6CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.PartyDamage);
        }

        if (Sim7CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroUpgrade);
        }

        if (Sim8CB.Checked)
        {
            Events = Events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroJoin);
        }

        GameTables.AoWEngine.FRegistry.SetSimultaneousWindowPopupEvents(Events);
        Root.Visible = false;
    }

    // AoWGamePlayDlg.pas:194
    public void CancelClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Root.Visible = false;
    }

    private static readonly string ConfirmationFailedRStr = "Confirmation failed";
    private static readonly string CouldntChangePasswordRStr = "Couldn't change password!";
    private static readonly string EnterNewPasswordRStr = "Enter new password:";
    private static readonly string PasswordConfirmationRStr = "Password confirmation";
    private static readonly string ReenterNewPasswordRStr = "Confirm new password:";
    private static StringArray3 TCombatResolveModeRStr = StringArray3.From(default(ReadOnlySpan<string>));
}
