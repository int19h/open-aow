// Translated from AoWGamePlayDlg.pas.
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWGamePlayDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// pending password
public partial class GamePlayDlg : DataModule
{
    public GamePlayDlg(Component owner) : base(owner)
    { }

    public AOWWindow Root;
    public AOWButton CancelBtn;
    public AoWFrame BCeltL;
    public AOWLabel Title;
    public AOWButton Close;
    public AOWImage TCeltLR;
    public AOWImage TCeltLL;
    public AOWImage TitleIcon;
    public AOWCheckBox AutoSaveCB;
    public AOWLabel asLbl;
    public AOWCheckBox SkipIntroCB;
    public AOWLabel siLbl;
    public AOWTabPanel Tab1;
    public AOWPanel Pnl1;
    public AOWPanel Pnl2;
    public AOWLabel GamePasswordLbl;
    public AOWEdit PasswordEdit;
    public AOWComboBox CombatResolveModeCB;
    public AOWVScrollBar CombatResolveModeSB;
    public AOWCheckBox Classic1CB;
    public AOWLabel PopupLbl1;
    public AOWCheckBox Classic2CB;
    public AOWLabel PopupLbl2;
    public AOWCheckBox Classic3CB;
    public AOWLabel PopupLbl3;
    public AOWCheckBox Classic4CB;
    public AOWCheckBox Classic5CB;
    public AOWCheckBox Classic6CB;
    public AOWLabel WindowPopupLbl;
    public AOWLabel ClassicLbl;
    public AOWLabel SimultaneousLbl;
    public AOWCheckBox Sim1CB;
    public AOWCheckBox Sim2CB;
    public AOWCheckBox Sim3CB;
    public AOWCheckBox Sim4CB;
    public AOWCheckBox Sim5CB;
    public AOWCheckBox Sim6CB;
    public AOWLabel PopupLbl4;
    public AOWLabel PopupLbl5;
    public AOWLabel PopupLbl6;
    public AOWCheckBox Classic7CB;
    public AOWCheckBox Sim7CB;
    public AOWLabel PopupLbl7;
    public AOWButton ChangePasswordBtn;
    public AOWComboBox MovementSpeedCB;
    public AOWButton OKBtn;
    public AOWCheckBox Classic8CB;
    public AOWCheckBox Sim8CB;
    public AOWLabel PopupLbl8;
    public AOWLabel CombLbl;
    public AOWPanel CombFr;
    public AOWPanel CombPnl;
    public AOWLabel Movelbl;
    public AOWPanel MovFr;
    public AOWPanel BtmPnl;
    public AOWPanel MovPnl;
    public AOWPanel AOWPanel1;
    public AOWImage CeltLineL;
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
                Password.FTitle = Magic.Abilities.AbilityRules.TranslateRStr(PasswordConfirmationRStr);
                Password.FPrompt = Magic.Abilities.AbilityRules.TranslateRStr(ReenterNewPasswordRStr);
                World.Map.AoWHSMap.Current.TriggerExecuteEventLog(Password, true, PasswordConfirmed);
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
            var encode = Magic.Abilities.AbilityRules.Encode(FNewPassword, 0);
            PasswordEdit.Text = encode;
        }
        else
        {
            MessageDlg.ResetMessageDialog(DialogHosts.MessageDlg);
            DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var confirmationFailed = Magic.Abilities.AbilityRules.TranslateRStr(ConfirmationFailedRStr);
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
        AoWRegistry.CombatResolveMode Mode;
        Set<WindowPopupEvent> Events;
        var autoSave = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetAutoSave();
        AutoSaveCB.Checked = autoSave;
        var skipIntro = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSkipIntro();
        SkipIntroCB.Checked = skipIntro;
        var movementSpeed = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetMovementSpeed();
        MovementSpeedCB.Selected = (int)movementSpeed;
        CombatResolveModeCB.Items.Clear();
        for (int modeOrdinal = (int)(AoWRegistry.CombatResolveMode)0; modeOrdinal <= (int)(AoWRegistry.CombatResolveMode)2; modeOrdinal++)
        {
            Mode = (AoWRegistry.CombatResolveMode)modeOrdinal;
            var combatResolveMode = Magic.Abilities.AbilityRules.TranslateRStr(TCombatResolveModeRStr[(int)Mode]);
            CombatResolveModeCB.Items.AddObject(combatResolveMode, IntToObject((int)Mode));
        }

        var fRegistryCombatResolveMode = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetCombatResolveMode();
        var indexOfObject = CombatResolveModeCB.Items.IndexOfObject(IntToObject((int)fRegistryCombatResolveMode));
        CombatResolveModeCB.Selected = indexOfObject;
        Events = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents();
        Classic1CB.Checked = Events.Contains(WindowPopupEvent.Combat);
        Classic2CB.Checked = Events.Contains(WindowPopupEvent.Diplomacy);
        Classic3CB.Checked = Events.Contains(WindowPopupEvent.SpellResearched);
        Classic4CB.Checked = Events.Contains(WindowPopupEvent.SpellReady);
        Classic5CB.Checked = Events.Contains(WindowPopupEvent.Rebellion);
        Classic6CB.Checked = Events.Contains(WindowPopupEvent.PartyDamage);
        Classic7CB.Checked = Events.Contains(WindowPopupEvent.HeroUpgrade);
        Classic8CB.Checked = Events.Contains(WindowPopupEvent.HeroJoin);
        Events = Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents();
        Sim1CB.Checked = Events.Contains(WindowPopupEvent.Combat);
        Sim2CB.Checked = Events.Contains(WindowPopupEvent.Diplomacy);
        Sim3CB.Checked = Events.Contains(WindowPopupEvent.SpellResearched);
        Sim4CB.Checked = Events.Contains(WindowPopupEvent.SpellReady);
        Sim5CB.Checked = Events.Contains(WindowPopupEvent.Rebellion);
        Sim6CB.Checked = Events.Contains(WindowPopupEvent.PartyDamage);
        Sim7CB.Checked = Events.Contains(WindowPopupEvent.HeroUpgrade);
        Sim8CB.Checked = Events.Contains(WindowPopupEvent.HeroJoin);
        var player = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer);
        PasswordEdit.Text = player.FSettings;
    }

    // AoWGamePlayDlg.pas:133
    public void ChangePasswordBtnClick()
    {
        PasswordEventLog Log = default;
        if (World.Map.AoWHSMap.Current.FPlayMode != PlayMode.Turn || World.Map.AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network || World.Map.AoWHSMap.Current.FCurrentPlayer == World.Map.AoWHSMap.Current.FSeatedPlayer)
        {
            Log = new PasswordEventLog(null);
            Log.FPrompt = Magic.Abilities.AbilityRules.TranslateRStr(EnterNewPasswordRStr);
            World.Map.AoWHSMap.Current.TriggerExecuteEventLog(Log, true, PasswordEntered);
            Log.Release();
        }
    }

    // AoWGamePlayDlg.pas:147
    public void OKBtnClick()
    {
        string Error = "";
        Set<WindowPopupEvent> Events;
        DialogHosts.General.PlayButtonSound(0);
        if (World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FSettings != PasswordEdit.Text)
        {
            if (World.Map.AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
            {
                World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FSettings = PasswordEdit.Text;
            }
            else
            {
                if (!World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).ChangePlayerSettings(PasswordEdit.Text, ref Error))
                {
                    MessageDlg.ResetMessageDialog(DialogHosts.MessageDlg);
                    DialogHosts.MessageDlg.MessageMemo.Items.Add("");
                    var couldntChangePassword = Magic.Abilities.AbilityRules.TranslateRStr(CouldntChangePasswordRStr);
                    DialogHosts.MessageDlg.MessageMemo.Items.Add(couldntChangePassword);
                    DialogHosts.MessageDlg.OKPnl.Visible = true;
                    DialogHosts.MessageDlg.Dlg.Visible = true;
                }
            }
        }

        Magic.Abilities.GameTables.AoWEngine.FRegistry.SetAutoSave(AutoSaveCB.Checked);
        Magic.Abilities.GameTables.AoWEngine.FRegistry.SetSkipIntro(SkipIntroCB.Checked);
        Magic.Abilities.GameTables.AoWEngine.FRegistry.SetMovementSpeed((AoWRegistry.MovementSpeed)MovementSpeedCB.Selected);
        var objectValue = CombatResolveModeCB.Items.Objects[CombatResolveModeCB.Selected];
        Magic.Abilities.GameTables.AoWEngine.FRegistry.SetCombatResolveMode((AoWRegistry.CombatResolveMode)ObjectToInt(objectValue));
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

        Magic.Abilities.GameTables.AoWEngine.FRegistry.SetClassicWindowPopupEvents(Events);
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

        Magic.Abilities.GameTables.AoWEngine.FRegistry.SetSimultaneousWindowPopupEvents(Events);
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
    private static Engine.Buffers.FixedArray_string_3 TCombatResolveModeRStr = Engine.Buffers.FixedArray_string_3.From(default(ReadOnlySpan<string>));
}
