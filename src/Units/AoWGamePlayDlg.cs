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
public partial class TGamePlayDlg : TDataModule
{
    public TGamePlayDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow GamePlayDlg;
    public TAOWButton CancelBtn;
    public TAoWFrame BCeltL;
    public TAOWLabel Title;
    public TAOWButton Close;
    public TAOWImage TCeltLR;
    public TAOWImage TCeltLL;
    public TAOWImage TitleIcon;
    public TAOWCheckBox AutoSaveCB;
    public TAOWLabel asLbl;
    public TAOWCheckBox SkipIntroCB;
    public TAOWLabel siLbl;
    public TAOWTabPanel Tab1;
    public TAOWPanel Pnl1;
    public TAOWPanel Pnl2;
    public TAOWLabel GamePasswordLbl;
    public TAOWEdit PasswordEdit;
    public TAOWComboBox CombatResolveModeCB;
    public TAOWVScrollBar CombatResolveModeSB;
    public TAOWCheckBox Classic1CB;
    public TAOWLabel PopupLbl1;
    public TAOWCheckBox Classic2CB;
    public TAOWLabel PopupLbl2;
    public TAOWCheckBox Classic3CB;
    public TAOWLabel PopupLbl3;
    public TAOWCheckBox Classic4CB;
    public TAOWCheckBox Classic5CB;
    public TAOWCheckBox Classic6CB;
    public TAOWLabel WindowPopupLbl;
    public TAOWLabel ClassicLbl;
    public TAOWLabel SimultaneousLbl;
    public TAOWCheckBox Sim1CB;
    public TAOWCheckBox Sim2CB;
    public TAOWCheckBox Sim3CB;
    public TAOWCheckBox Sim4CB;
    public TAOWCheckBox Sim5CB;
    public TAOWCheckBox Sim6CB;
    public TAOWLabel PopupLbl4;
    public TAOWLabel PopupLbl5;
    public TAOWLabel PopupLbl6;
    public TAOWCheckBox Classic7CB;
    public TAOWCheckBox Sim7CB;
    public TAOWLabel PopupLbl7;
    public TAOWButton ChangePasswordBtn;
    public TAOWComboBox MovementSpeedCB;
    public TAOWButton OKBtn;
    public TAOWCheckBox Classic8CB;
    public TAOWCheckBox Sim8CB;
    public TAOWLabel PopupLbl8;
    public TAOWLabel CombLbl;
    public TAOWPanel CombFr;
    public TAOWPanel CombPnl;
    public TAOWLabel Movelbl;
    public TAOWPanel MovFr;
    public TAOWPanel BtmPnl;
    public TAOWPanel MovPnl;
    public TAOWPanel AOWPanel1;
    public TAOWImage CeltLineL;
    public TIvTranslator Translator;
    public string FNewPassword = "";

    // AoWGamePlayDlg.pas:200
    public void PasswordEntered(TEventLog Log)
    {
        TPasswordEventLog Password = default;
        if (((TPasswordEventLog)Log).FPressedButton == TPressedEventLogButton.pebOk)
        {
            if (!IsEmpty(((TPasswordEventLog)Log).FPassword))
            {
                FNewPassword = ((TPasswordEventLog)Log).FPassword;
                Password = new TPasswordEventLog(null);
                Password.FTitle = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TGamePlayDlg.PasswordConfirmationRStr);
                Password.FPrompt = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TGamePlayDlg.ReenterNewPasswordRStr);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.TriggerExecuteEventLog(Password, true, PasswordConfirmed);
                Password.Release();
            }
            else
            {
                PasswordEdit.Text = "";
            }
        }
    }

    // AoWGamePlayDlg.pas:217
    public void PasswordConfirmed(TEventLog Log)
    {
        if (((TPasswordEventLog)Log).FPassword == FNewPassword)
        {
            var encode = Aow.Game.Magic.Abilities.AbilityRules.Encode(FNewPassword, 0);
            PasswordEdit.Text = encode;
        }
        else
        {
            Aow.Game.Ui.Dialogs.TMessageDlg.ResetMessageDialog(Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var confirmationFailed = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TGamePlayDlg.ConfirmationFailedRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add(confirmationFailed);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.OKPnl.Visible = true;
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.Dlg.Visible = true;
        }
    }

    // AoWGamePlayDlg.pas:95
    public void GamePlayDlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
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
        TCombatResolveMode Mode;
        TSet<TWindowPopupEvent> Events;
        var autoSave = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetAutoSave();
        AutoSaveCB.Checked = autoSave;
        var skipIntro = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSkipIntro();
        SkipIntroCB.Checked = skipIntro;
        var movementSpeed = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetMovementSpeed();
        MovementSpeedCB.Selected = (int)movementSpeed;
        CombatResolveModeCB.Items.Clear();
        for (int modeOrdinal = (int)(TCombatResolveMode)0; modeOrdinal <= (int)(TCombatResolveMode)2; modeOrdinal++)
        {
            Mode = (TCombatResolveMode)modeOrdinal;
            var combatResolveMode = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TGamePlayDlg.TCombatResolveModeRStr[(int)Mode]);
            CombatResolveModeCB.Items.AddObject(combatResolveMode, IntToObject((int)Mode));
        }

        var fRegistryCombatResolveMode = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetCombatResolveMode();
        var indexOfObject = CombatResolveModeCB.Items.IndexOfObject(IntToObject((int)fRegistryCombatResolveMode));
        CombatResolveModeCB.Selected = indexOfObject;
        Events = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents();
        Classic1CB.Checked = Events.Contains(TWindowPopupEvent.wpCombat);
        Classic2CB.Checked = Events.Contains(TWindowPopupEvent.wpDiplomacy);
        Classic3CB.Checked = Events.Contains(TWindowPopupEvent.wpSpellResearched);
        Classic4CB.Checked = Events.Contains(TWindowPopupEvent.wpSpellReady);
        Classic5CB.Checked = Events.Contains(TWindowPopupEvent.wpRebellion);
        Classic6CB.Checked = Events.Contains(TWindowPopupEvent.wpPartyDamage);
        Classic7CB.Checked = Events.Contains(TWindowPopupEvent.wpHeroUpgrade);
        Classic8CB.Checked = Events.Contains(TWindowPopupEvent.wpHeroJoin);
        Events = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents();
        Sim1CB.Checked = Events.Contains(TWindowPopupEvent.wpCombat);
        Sim2CB.Checked = Events.Contains(TWindowPopupEvent.wpDiplomacy);
        Sim3CB.Checked = Events.Contains(TWindowPopupEvent.wpSpellResearched);
        Sim4CB.Checked = Events.Contains(TWindowPopupEvent.wpSpellReady);
        Sim5CB.Checked = Events.Contains(TWindowPopupEvent.wpRebellion);
        Sim6CB.Checked = Events.Contains(TWindowPopupEvent.wpPartyDamage);
        Sim7CB.Checked = Events.Contains(TWindowPopupEvent.wpHeroUpgrade);
        Sim8CB.Checked = Events.Contains(TWindowPopupEvent.wpHeroJoin);
        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        PasswordEdit.Text = player.FSettings;
    }

    // AoWGamePlayDlg.pas:133
    public void ChangePasswordBtnClick()
    {
        TPasswordEventLog Log = default;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayMode != TPlayMode.pmTurn || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FConnectionType != TPlayerConnectionType.pcNetwork || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCurrentPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
        {
            Log = new TPasswordEventLog(null);
            Log.FPrompt = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TGamePlayDlg.EnterNewPasswordRStr);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.TriggerExecuteEventLog(Log, true, PasswordEntered);
            Log.Release();
        }
    }

    // AoWGamePlayDlg.pas:147
    public void OKBtnClick()
    {
        string Error = "";
        TSet<TWindowPopupEvent> Events;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FSettings != PasswordEdit.Text)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FConnectionType != TPlayerConnectionType.pcNetwork)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FSettings = PasswordEdit.Text;
            }
            else
            {
                if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).ChangePlayerSettings(PasswordEdit.Text, ref Error))
                {
                    Aow.Game.Ui.Dialogs.TMessageDlg.ResetMessageDialog(Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg);
                    Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add("");
                    var couldntChangePassword = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TGamePlayDlg.CouldntChangePasswordRStr);
                    Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add(couldntChangePassword);
                    Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.OKPnl.Visible = true;
                    Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.Dlg.Visible = true;
                }
            }
        }

        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetAutoSave(AutoSaveCB.Checked);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetSkipIntro(SkipIntroCB.Checked);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetMovementSpeed((TMovementSpeed)MovementSpeedCB.Selected);
        var objectValue = CombatResolveModeCB.Items.Objects[CombatResolveModeCB.Selected];
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetCombatResolveMode((TCombatResolveMode)ObjectToInt(objectValue));
        Events = TSet<TWindowPopupEvent>.Of();
        if (Classic1CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpCombat);
        }

        if (Classic2CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpDiplomacy);
        }

        if (Classic3CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpSpellResearched);
        }

        if (Classic4CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpSpellReady);
        }

        if (Classic5CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpRebellion);
        }

        if (Classic6CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpPartyDamage);
        }

        if (Classic7CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpHeroUpgrade);
        }

        if (Classic8CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpHeroJoin);
        }

        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetClassicWindowPopupEvents(Events);
        Events = TSet<TWindowPopupEvent>.Of();
        if (Sim1CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpCombat);
        }

        if (Sim2CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpDiplomacy);
        }

        if (Sim3CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpSpellResearched);
        }

        if (Sim4CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpSpellReady);
        }

        if (Sim5CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpRebellion);
        }

        if (Sim6CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpPartyDamage);
        }

        if (Sim7CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpHeroUpgrade);
        }

        if (Sim8CB.Checked)
        {
            Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpHeroJoin);
        }

        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetSimultaneousWindowPopupEvents(Events);
        GamePlayDlg.Visible = false;
    }

    // AoWGamePlayDlg.pas:194
    public void CancelClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        GamePlayDlg.Visible = false;
    }

    private static readonly string ConfirmationFailedRStr = "Confirmation failed";
    private static readonly string CouldntChangePasswordRStr = "Couldn't change password!";
    private static readonly string EnterNewPasswordRStr = "Enter new password:";
    private static readonly string PasswordConfirmationRStr = "Password confirmation";
    private static readonly string ReenterNewPasswordRStr = "Confirm new password:";
    private static global::Aow.Game.Engine.Buffers.FixedArray_string_3 TCombatResolveModeRStr = global::Aow.Game.Engine.Buffers.FixedArray_string_3.From(default(global::System.ReadOnlySpan<string>));
}
