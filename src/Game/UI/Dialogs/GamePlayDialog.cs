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
    public Panel Panel1;
    public Image CeltLineL;
    public IvTranslator Translator;
    public string NewPassword = "";

    // AoWGamePlayDlg.pas:200
    public void PasswordEntered(EventLog log)
    {
        PasswordEventLog password = default;
        if (((PasswordEventLog)log).PressedButton == PressedEventLogButton.Ok)
        {
            if (!IsEmpty(((PasswordEventLog)log).Password))
            {
                NewPassword = ((PasswordEventLog)log).Password;
                password = new PasswordEventLog(null);
                password.Title = AbilityRules.TranslateResourceString(s_passwordConfirmationText);
                password.Prompt = AbilityRules.TranslateResourceString(s_reenterNewPasswordText);
                AoWHSMap.Current.TriggerExecuteEventLog(password, true, PasswordConfirmed);
                password.Release();
            }
            else
            {
                PasswordEdit.Text = "";
            }
        }
    }

    // AoWGamePlayDlg.pas:217
    public void PasswordConfirmed(EventLog log)
    {
        if (((PasswordEventLog)log).Password == NewPassword)
        {
            var encode = AbilityRules.Encode(NewPassword, 0);
            PasswordEdit.Text = encode;
        }
        else
        {
            MessageDialog.ResetMessageDialog(DialogHosts.MessageDialog);
            DialogHosts.MessageDialog.MessageMemo.Items.Add("");
            var confirmationFailed = AbilityRules.TranslateResourceString(s_confirmationFailedText);
            DialogHosts.MessageDialog.MessageMemo.Items.Add(confirmationFailed);
            DialogHosts.MessageDialog.OKPnl.Visible = true;
            DialogHosts.MessageDialog.Dlg.Visible = true;
        }
    }

    // AoWGamePlayDlg.pas:95
    public void GamePlayDlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 27:
                CancelClick();
                break;
        }
    }

    // AoWGamePlayDlg.pas:100
    public void GamePlayDlgShow()
    {
        GameSettings.CombatResolveMode mode;
        Set<WindowPopupEvent> events;
        var autoSave = GameTables.GameEngine.Registry.GetAutoSave();
        AutoSaveCB.Checked = autoSave;
        var skipIntro = GameTables.GameEngine.Registry.GetSkipIntro();
        SkipIntroCB.Checked = skipIntro;
        var movementSpeed = GameTables.GameEngine.Registry.GetMovementSpeed();
        MovementSpeedCB.Selected = (int)movementSpeed;
        CombatResolveModeCB.Items.Clear();
        for (int modeOrdinal = (int)(GameSettings.CombatResolveMode)0; modeOrdinal <= (int)(GameSettings.CombatResolveMode)2; modeOrdinal++)
        {
            mode = (GameSettings.CombatResolveMode)modeOrdinal;
            var combatResolveMode = AbilityRules.TranslateResourceString(s_combatResolveModeTexts[(int)mode]);
            CombatResolveModeCB.Items.AddObject(combatResolveMode, IntToObject((int)mode));
        }

        var fRegistryCombatResolveMode = GameTables.GameEngine.Registry.GetCombatResolveMode();
        var indexOfObject = CombatResolveModeCB.Items.IndexOfObject(IntToObject((int)fRegistryCombatResolveMode));
        CombatResolveModeCB.Selected = indexOfObject;
        events = GameTables.GameEngine.Registry.GetClassicWindowPopupEvents();
        Classic1CB.Checked = events.Contains(WindowPopupEvent.Combat);
        Classic2CB.Checked = events.Contains(WindowPopupEvent.Diplomacy);
        Classic3CB.Checked = events.Contains(WindowPopupEvent.SpellResearched);
        Classic4CB.Checked = events.Contains(WindowPopupEvent.SpellReady);
        Classic5CB.Checked = events.Contains(WindowPopupEvent.Rebellion);
        Classic6CB.Checked = events.Contains(WindowPopupEvent.PartyDamage);
        Classic7CB.Checked = events.Contains(WindowPopupEvent.HeroUpgrade);
        Classic8CB.Checked = events.Contains(WindowPopupEvent.HeroJoin);
        events = GameTables.GameEngine.Registry.GetSimultaneousWindowPopupEvents();
        Sim1CB.Checked = events.Contains(WindowPopupEvent.Combat);
        Sim2CB.Checked = events.Contains(WindowPopupEvent.Diplomacy);
        Sim3CB.Checked = events.Contains(WindowPopupEvent.SpellResearched);
        Sim4CB.Checked = events.Contains(WindowPopupEvent.SpellReady);
        Sim5CB.Checked = events.Contains(WindowPopupEvent.Rebellion);
        Sim6CB.Checked = events.Contains(WindowPopupEvent.PartyDamage);
        Sim7CB.Checked = events.Contains(WindowPopupEvent.HeroUpgrade);
        Sim8CB.Checked = events.Contains(WindowPopupEvent.HeroJoin);
        var player = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        PasswordEdit.Text = player.Settings;
    }

    // AoWGamePlayDlg.pas:133
    public void ChangePasswordBtnClick()
    {
        PasswordEventLog log = default;
        if (AoWHSMap.Current.PlayMode != PlayMode.Turn || AoWHSMap.Current.ConnectionType != PlayerConnectionType.Network || AoWHSMap.Current.CurrentPlayer == AoWHSMap.Current.SeatedPlayer)
        {
            log = new PasswordEventLog(null);
            log.Prompt = AbilityRules.TranslateResourceString(s_enterNewPasswordText);
            AoWHSMap.Current.TriggerExecuteEventLog(log, true, PasswordEntered);
            log.Release();
        }
    }

    // AoWGamePlayDlg.pas:147
    public void OKBtnClick()
    {
        string error = "";
        Set<WindowPopupEvent> events;
        DialogHosts.General.PlayButtonSound(0);
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Settings != PasswordEdit.Text)
        {
            if (AoWHSMap.Current.ConnectionType != PlayerConnectionType.Network)
            {
                AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Settings = PasswordEdit.Text;
            }
            else
            {
                if (!AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).ChangePlayerSettings(PasswordEdit.Text, ref error))
                {
                    MessageDialog.ResetMessageDialog(DialogHosts.MessageDialog);
                    DialogHosts.MessageDialog.MessageMemo.Items.Add("");
                    var couldNotChangePassword = AbilityRules.TranslateResourceString(s_couldNotChangePasswordText);
                    DialogHosts.MessageDialog.MessageMemo.Items.Add(couldNotChangePassword);
                    DialogHosts.MessageDialog.OKPnl.Visible = true;
                    DialogHosts.MessageDialog.Dlg.Visible = true;
                }
            }
        }

        GameTables.GameEngine.Registry.SetAutoSave(AutoSaveCB.Checked);
        GameTables.GameEngine.Registry.SetSkipIntro(SkipIntroCB.Checked);
        GameTables.GameEngine.Registry.SetMovementSpeed((GameSettings.MovementSpeed)MovementSpeedCB.Selected);
        var objectValue = CombatResolveModeCB.Items.Objects[CombatResolveModeCB.Selected];
        GameTables.GameEngine.Registry.SetCombatResolveMode((GameSettings.CombatResolveMode)ObjectToInt(objectValue));
        events = Set<WindowPopupEvent>.Of();
        if (Classic1CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Combat);
        }

        if (Classic2CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Diplomacy);
        }

        if (Classic3CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellResearched);
        }

        if (Classic4CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellReady);
        }

        if (Classic5CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Rebellion);
        }

        if (Classic6CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.PartyDamage);
        }

        if (Classic7CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroUpgrade);
        }

        if (Classic8CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroJoin);
        }

        GameTables.GameEngine.Registry.SetClassicWindowPopupEvents(events);
        events = Set<WindowPopupEvent>.Of();
        if (Sim1CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Combat);
        }

        if (Sim2CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Diplomacy);
        }

        if (Sim3CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellResearched);
        }

        if (Sim4CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellReady);
        }

        if (Sim5CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Rebellion);
        }

        if (Sim6CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.PartyDamage);
        }

        if (Sim7CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroUpgrade);
        }

        if (Sim8CB.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroJoin);
        }

        GameTables.GameEngine.Registry.SetSimultaneousWindowPopupEvents(events);
        Root.Visible = false;
    }

    // AoWGamePlayDlg.pas:194
    public void CancelClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Root.Visible = false;
    }

    private static readonly string s_confirmationFailedText = "Confirmation failed";
    private static readonly string s_couldNotChangePasswordText = "Couldn't change password!";
    private static readonly string s_enterNewPasswordText = "Enter new password:";
    private static readonly string s_passwordConfirmationText = "Password confirmation";
    private static readonly string s_reenterNewPasswordText = "Confirm new password:";
    private static StringArray3 s_combatResolveModeTexts = StringArray3.From(default(ReadOnlySpan<string>));
}
