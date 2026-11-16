#nullable disable
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

    public Window Window { get; private set; }
    public CheckBox AutoSaveCheckBox { get; private set; }
    public CheckBox SkipIntroCheckBox { get; private set; }
    public Edit PasswordEdit { get; private set; }
    public ComboBox CombatResolveModeComboBox { get; private set; }
    public CheckBox Classic1CheckBox { get; private set; }
    public CheckBox Classic2CheckBox { get; private set; }
    public CheckBox Classic3CheckBox { get; private set; }
    public CheckBox Classic4CheckBox { get; private set; }
    public CheckBox Classic5CheckBox { get; private set; }
    public CheckBox Classic6CheckBox { get; private set; }
    public CheckBox Simultaneous1CheckBox { get; private set; }
    public CheckBox Simultaneous2CheckBox { get; private set; }
    public CheckBox Simultaneous3CheckBox { get; private set; }
    public CheckBox Simultaneous4CheckBox { get; private set; }
    public CheckBox Simultaneous5CheckBox { get; private set; }
    public CheckBox Simultaneous6CheckBox { get; private set; }
    public CheckBox Classic7CheckBox { get; private set; }
    public CheckBox Simultaneous7CheckBox { get; private set; }
    public ComboBox MovementSpeedComboBox { get; private set; }
    public CheckBox Classic8CheckBox { get; private set; }
    public CheckBox Simultaneous8CheckBox { get; private set; }

    private string _newPassword = "";

    // AoWGamePlayDlg.pas:200
    public void PasswordEntered(PasswordEventLog log)
    {
        PasswordEventLog password = default;
        if (log.PressedButton == PressedEventLogButton.Ok)
        {
            if (!IsEmpty(log.Password))
            {
                _newPassword = log.Password;
                password = new PasswordEventLog(null, holder: UI.Engine);
                password.Title = UI.Engine.TranslateResourceString(s_passwordConfirmationText);
                password.Prompt = UI.Engine.TranslateResourceString(s_reenterNewPasswordText);
                StrategicMap.Current.TriggerTypedExecuteEventLog(password, true, PasswordConfirmed);
                password.Release();
            }
            else
            {
                PasswordEdit.Text = "";
            }
        }
    }

    // AoWGamePlayDlg.pas:217
    public void PasswordConfirmed(PasswordEventLog log)
    {
        if (log.Password == _newPassword)
        {
            string encode = AbilityRules.Encode(_newPassword, 0);
            PasswordEdit.Text = encode;
        }
        else
        {
            MessageDialog.ResetMessageDialog(UI.MessageDialog);
            UI.MessageDialog.MessageMemo.Items.Add("");
            string confirmationFailed = UI.Engine.TranslateResourceString(s_confirmationFailedText);
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
        bool autoSave = UI.Engine.Registry.GetAutoSave();
        AutoSaveCheckBox.Checked = autoSave;
        bool skipIntro = UI.Engine.Registry.GetSkipIntro();
        SkipIntroCheckBox.Checked = skipIntro;
        GameSettings.MovementSpeed movementSpeed = UI.Engine.Registry.GetMovementSpeed();
        MovementSpeedComboBox.Selected = (int)movementSpeed;
        CombatResolveModeComboBox.Items.Clear();
        for (int modeOrdinal = (int)(GameSettings.CombatResolveMode)0; modeOrdinal <= (int)(GameSettings.CombatResolveMode)2; modeOrdinal++)
        {
            mode = (GameSettings.CombatResolveMode)modeOrdinal;
            string combatResolveMode = UI.Engine.TranslateResourceString(s_combatResolveModeTexts[(int)mode]);
            CombatResolveModeComboBox.Items.AddObject(combatResolveMode, IntToObject((int)mode));
        }

        GameSettings.CombatResolveMode fRegistryCombatResolveMode = UI.Engine.Registry.GetCombatResolveMode();
        int indexOfObject = CombatResolveModeComboBox.Items.IndexOfObject(IntToObject((int)fRegistryCombatResolveMode));
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
        Player player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
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
            StrategicMap.Current.TriggerTypedExecuteEventLog(log, true, PasswordEntered);
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
                    string couldNotChangePassword = UI.Engine.TranslateResourceString(s_couldNotChangePasswordText);
                    UI.MessageDialog.MessageMemo.Items.Add(couldNotChangePassword);
                    UI.MessageDialog.OKPanel.Visible = true;
                    UI.MessageDialog.Window.Visible = true;
                }
            }
        }

        UI.Engine.Registry.SetAutoSave(AutoSaveCheckBox.Checked);
        UI.Engine.Registry.SetSkipIntro(SkipIntroCheckBox.Checked);
        UI.Engine.Registry.SetMovementSpeed((GameSettings.MovementSpeed)MovementSpeedComboBox.Selected);
        object objectValue = CombatResolveModeComboBox.Items.Objects[CombatResolveModeComboBox.Selected];
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
