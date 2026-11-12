// Translated from AoWGamePlayDlg.pas.
using System.Collections.Immutable;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// pending password
public partial class GamePlayDialog : UIModule
{
    public GamePlayDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public CheckBox AutoSaveCheckBox;
    public CheckBox SkipIntroCheckBox;
    public Edit PasswordEdit;
    public ComboBox CombatResolveModeComboBox;
    public CheckBox Classic1CheckBox;
    public CheckBox Classic2CheckBox;
    public CheckBox Classic3CheckBox;
    public CheckBox Classic4CheckBox;
    public CheckBox Classic5CheckBox;
    public CheckBox Classic6CheckBox;
    public CheckBox Simultaneous1CheckBox;
    public CheckBox Simultaneous2CheckBox;
    public CheckBox Simultaneous3CheckBox;
    public CheckBox Simultaneous4CheckBox;
    public CheckBox Simultaneous5CheckBox;
    public CheckBox Simultaneous6CheckBox;
    public CheckBox Classic7CheckBox;
    public CheckBox Simultaneous7CheckBox;
    public ComboBox MovementSpeedComboBox;
    public CheckBox Classic8CheckBox;
    public CheckBox Simultaneous8CheckBox;
    private string _newPassword = "";

    // AoWGamePlayDlg.pas:200
    public void PasswordEntered(EventLog log)
    {
        PasswordEventLog password = default;
        if (((PasswordEventLog)log).PressedButton == PressedEventLogButton.Ok)
        {
            if (!IsEmpty(((PasswordEventLog)log).Password))
            {
                _newPassword = ((PasswordEventLog)log).Password;
                password = new PasswordEventLog(null, holder: UI.Engine);
                password.Title = UI.Engine.TranslateResourceString(s_passwordConfirmationText);
                password.Prompt = UI.Engine.TranslateResourceString(s_reenterNewPasswordText);
                StrategicMap.Current.TriggerExecuteEventLog(password, true, PasswordConfirmed);
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
        if (((PasswordEventLog)log).Password == _newPassword)
        {
            var encode = AbilityRules.Encode(_newPassword, 0);
            PasswordEdit.Text = encode;
        }
        else
        {
            MessageDialog.ResetMessageDialog(UI.MessageDialog);
            UI.MessageDialog.MessageMemo.Items.Add("");
            var confirmationFailed = UI.Engine.TranslateResourceString(s_confirmationFailedText);
            UI.MessageDialog.MessageMemo.Items.Add(confirmationFailed);
            UI.MessageDialog.OKPanel.Visible = true;
            UI.MessageDialog.Window.Visible = true;
        }
    }

    // AoWGamePlayDlg.pas:95
    public void WindowKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 27:
                CancelClick();
                break;
        }
    }

    // AoWGamePlayDlg.pas:100
    public void WindowShow()
    {
        GameSettings.CombatResolveMode mode;
        Set<WindowPopupEvent> events;
        var autoSave = UI.Engine.Registry.GetAutoSave();
        AutoSaveCheckBox.Checked = autoSave;
        var skipIntro = UI.Engine.Registry.GetSkipIntro();
        SkipIntroCheckBox.Checked = skipIntro;
        var movementSpeed = UI.Engine.Registry.GetMovementSpeed();
        MovementSpeedComboBox.Selected = (int)movementSpeed;
        CombatResolveModeComboBox.Items.Clear();
        for (int modeOrdinal = (int)(GameSettings.CombatResolveMode)0; modeOrdinal <= (int)(GameSettings.CombatResolveMode)2; modeOrdinal++)
        {
            mode = (GameSettings.CombatResolveMode)modeOrdinal;
            var combatResolveMode = UI.Engine.TranslateResourceString(s_combatResolveModeTexts[(int)mode]);
            CombatResolveModeComboBox.Items.AddObject(combatResolveMode, IntToObject((int)mode));
        }

        var fRegistryCombatResolveMode = UI.Engine.Registry.GetCombatResolveMode();
        var indexOfObject = CombatResolveModeComboBox.Items.IndexOfObject(IntToObject((int)fRegistryCombatResolveMode));
        CombatResolveModeComboBox.Selected = indexOfObject;
        events = UI.Engine.Registry.GetClassicWindowPopupEvents();
        Classic1CheckBox.Checked = events.Contains(WindowPopupEvent.Combat);
        Classic2CheckBox.Checked = events.Contains(WindowPopupEvent.Diplomacy);
        Classic3CheckBox.Checked = events.Contains(WindowPopupEvent.SpellResearched);
        Classic4CheckBox.Checked = events.Contains(WindowPopupEvent.SpellReady);
        Classic5CheckBox.Checked = events.Contains(WindowPopupEvent.Rebellion);
        Classic6CheckBox.Checked = events.Contains(WindowPopupEvent.PartyDamage);
        Classic7CheckBox.Checked = events.Contains(WindowPopupEvent.HeroUpgrade);
        Classic8CheckBox.Checked = events.Contains(WindowPopupEvent.HeroJoin);
        events = UI.Engine.Registry.GetSimultaneousWindowPopupEvents();
        Simultaneous1CheckBox.Checked = events.Contains(WindowPopupEvent.Combat);
        Simultaneous2CheckBox.Checked = events.Contains(WindowPopupEvent.Diplomacy);
        Simultaneous3CheckBox.Checked = events.Contains(WindowPopupEvent.SpellResearched);
        Simultaneous4CheckBox.Checked = events.Contains(WindowPopupEvent.SpellReady);
        Simultaneous5CheckBox.Checked = events.Contains(WindowPopupEvent.Rebellion);
        Simultaneous6CheckBox.Checked = events.Contains(WindowPopupEvent.PartyDamage);
        Simultaneous7CheckBox.Checked = events.Contains(WindowPopupEvent.HeroUpgrade);
        Simultaneous8CheckBox.Checked = events.Contains(WindowPopupEvent.HeroJoin);
        var player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        PasswordEdit.Text = player.Settings;
    }

    // AoWGamePlayDlg.pas:133
    public void ChangePasswordBtnClick()
    {
        PasswordEventLog log = default;
        if (StrategicMap.Current.PlayMode != PlayMode.Turn || StrategicMap.Current.ConnectionType != PlayerConnectionType.Network || StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer)
        {
            log = new PasswordEventLog(null, holder: UI.Engine);
            log.Prompt = UI.Engine.TranslateResourceString(s_enterNewPasswordText);
            StrategicMap.Current.TriggerExecuteEventLog(log, true, PasswordEntered);
            log.Release();
        }
    }

    // AoWGamePlayDlg.pas:147
    public void OKBtnClick()
    {
        string error = "";
        Set<WindowPopupEvent> events;
        UI.General.PlayButtonSound(0);
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Settings != PasswordEdit.Text)
        {
            if (StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
            {
                StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Settings = PasswordEdit.Text;
            }
            else
            {
                if (!StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).ChangePlayerSettings(PasswordEdit.Text, ref error))
                {
                    MessageDialog.ResetMessageDialog(UI.MessageDialog);
                    UI.MessageDialog.MessageMemo.Items.Add("");
                    var couldNotChangePassword = UI.Engine.TranslateResourceString(s_couldNotChangePasswordText);
                    UI.MessageDialog.MessageMemo.Items.Add(couldNotChangePassword);
                    UI.MessageDialog.OKPanel.Visible = true;
                    UI.MessageDialog.Window.Visible = true;
                }
            }
        }

        UI.Engine.Registry.SetAutoSave(AutoSaveCheckBox.Checked);
        UI.Engine.Registry.SetSkipIntro(SkipIntroCheckBox.Checked);
        UI.Engine.Registry.SetMovementSpeed((GameSettings.MovementSpeed)MovementSpeedComboBox.Selected);
        var objectValue = CombatResolveModeComboBox.Items.Objects[CombatResolveModeComboBox.Selected];
        UI.Engine.Registry.SetCombatResolveMode((GameSettings.CombatResolveMode)ObjectToInt(objectValue));
        events = Set<WindowPopupEvent>.Of();
        if (Classic1CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Combat);
        }

        if (Classic2CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Diplomacy);
        }

        if (Classic3CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellResearched);
        }

        if (Classic4CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellReady);
        }

        if (Classic5CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Rebellion);
        }

        if (Classic6CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.PartyDamage);
        }

        if (Classic7CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroUpgrade);
        }

        if (Classic8CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroJoin);
        }

        UI.Engine.Registry.SetClassicWindowPopupEvents(events);
        events = Set<WindowPopupEvent>.Of();
        if (Simultaneous1CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Combat);
        }

        if (Simultaneous2CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Diplomacy);
        }

        if (Simultaneous3CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellResearched);
        }

        if (Simultaneous4CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.SpellReady);
        }

        if (Simultaneous5CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.Rebellion);
        }

        if (Simultaneous6CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.PartyDamage);
        }

        if (Simultaneous7CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroUpgrade);
        }

        if (Simultaneous8CheckBox.Checked)
        {
            events = events + Set<WindowPopupEvent>.Of(WindowPopupEvent.HeroJoin);
        }

        UI.Engine.Registry.SetSimultaneousWindowPopupEvents(events);
        Window.Visible = false;
    }

    // AoWGamePlayDlg.pas:194
    public void CancelClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    private static readonly string s_confirmationFailedText = "Confirmation failed";
    private static readonly string s_couldNotChangePasswordText = "Couldn't change password!";
    private static readonly string s_enterNewPasswordText = "Enter new password:";
    private static readonly string s_passwordConfirmationText = "Password confirmation";
    private static readonly string s_reenterNewPasswordText = "Confirm new password:";
    private static readonly ImmutableArray<string> s_combatResolveModeTexts = ["", "", ""];
}
