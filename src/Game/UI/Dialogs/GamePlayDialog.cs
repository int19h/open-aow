#nullable disable warnings
// Translated from AoWGamePlayDlg.pas.
using System.Collections.Immutable;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// pending password
public sealed partial class GamePlayDialog(UIContext ui) : UIModule(ui)
{
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
        if (log.PressedButton == PressedEventLogButton.Ok)
        {
            if (!IsEmpty(log.Password))
            {
                _newPassword = log.Password;
                PasswordEventLog password = new(null, holder: UI.Engine)
                {
                    Title = UI.Engine.TranslateResourceString(s_passwordConfirmationText),
                    Prompt = UI.Engine.TranslateResourceString(s_reenterNewPasswordText)
                };
                UI.Engine.StrategicMap.TriggerTypedExecuteEventLog(password, immediate: true, done: PasswordConfirmed);
                _ = password.Release();
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
            string encode = AbilityRules.Encode(_newPassword);
            PasswordEdit.Text = encode;
        }
        else
        {
            MessageDialog.ResetMessageDialog(UI.MessageDialog);
            _ = UI.MessageDialog.MessageMemo.Items.Add("");
            string confirmationFailed = UI.Engine.TranslateResourceString(s_confirmationFailedText);
            _ = UI.MessageDialog.MessageMemo.Items.Add(confirmationFailed);
            UI.MessageDialog.OKPanel.Visible = true;
            UI.MessageDialog.Window.Visible = true;
        }
    }

    // AoWGamePlayDlg.pas:95
    public void WindowKeyDown(in int key, ShiftStates shift)
    {
        switch (key)
        {
            case 27:
                CancelClick();
                break;
            default:
                break;
        }
    }

    // AoWGamePlayDlg.pas:100
    public void WindowShow()
    {
        bool autoSave = UI.Engine.Registry.GetAutoSave();
        AutoSaveCheckBox.Checked = autoSave;
        bool skipIntro = UI.Engine.Registry.GetSkipIntro();
        SkipIntroCheckBox.Checked = skipIntro;
        GameSettings.MovementSpeed movementSpeed = UI.Engine.Registry.GetMovementSpeed();
        MovementSpeedComboBox.Selected = (int)movementSpeed;
        CombatResolveModeComboBox.Items.Clear();
        for (int modeOrdinal = (int)GameSettings.CombatResolveMode.Choose; modeOrdinal <= (int)GameSettings.CombatResolveMode.AlwaysTacticalBattle; modeOrdinal++)
        {
            GameSettings.CombatResolveMode mode = (GameSettings.CombatResolveMode)modeOrdinal;
            string combatResolveMode = UI.Engine.TranslateResourceString(s_combatResolveModeTexts[(int)mode]);
            _ = CombatResolveModeComboBox.Items.AddObject(combatResolveMode, item: IntToObject((int)mode));
        }

        GameSettings.CombatResolveMode fRegistryCombatResolveMode = UI.Engine.Registry.GetCombatResolveMode();
        int indexOfObject = CombatResolveModeComboBox.Items.IndexOfObject(IntToObject((int)fRegistryCombatResolveMode));
        CombatResolveModeComboBox.Selected = indexOfObject;
        WindowPopupEvents events = UI.Engine.Registry.GetClassicWindowPopupEvents();
        Classic1CheckBox.Checked = events.Contains(WindowPopupEvents.Combat);
        Classic2CheckBox.Checked = events.Contains(WindowPopupEvents.Diplomacy);
        Classic3CheckBox.Checked = events.Contains(WindowPopupEvents.SpellResearched);
        Classic4CheckBox.Checked = events.Contains(WindowPopupEvents.SpellReady);
        Classic5CheckBox.Checked = events.Contains(WindowPopupEvents.Rebellion);
        Classic6CheckBox.Checked = events.Contains(WindowPopupEvents.PartyDamage);
        Classic7CheckBox.Checked = events.Contains(WindowPopupEvents.HeroUpgrade);
        Classic8CheckBox.Checked = events.Contains(WindowPopupEvents.HeroJoin);
        events = UI.Engine.Registry.GetSimultaneousWindowPopupEvents();
        Simultaneous1CheckBox.Checked = events.Contains(WindowPopupEvents.Combat);
        Simultaneous2CheckBox.Checked = events.Contains(WindowPopupEvents.Diplomacy);
        Simultaneous3CheckBox.Checked = events.Contains(WindowPopupEvents.SpellResearched);
        Simultaneous4CheckBox.Checked = events.Contains(WindowPopupEvents.SpellReady);
        Simultaneous5CheckBox.Checked = events.Contains(WindowPopupEvents.Rebellion);
        Simultaneous6CheckBox.Checked = events.Contains(WindowPopupEvents.PartyDamage);
        Simultaneous7CheckBox.Checked = events.Contains(WindowPopupEvents.HeroUpgrade);
        Simultaneous8CheckBox.Checked = events.Contains(WindowPopupEvents.HeroJoin);
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        Player? player = strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer);
        PasswordEdit.Text = player.Settings;
    }

    // AoWGamePlayDlg.pas:133
    public void ChangePasswordBtnClick()
    {
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (strategicMap.PlayMode != PlayMode.Turn || strategicMap.ConnectionType != PlayerConnectionType.Network || strategicMap.CurrentPlayer == strategicMap.SeatedPlayer)
        {
            PasswordEventLog log = new(null, holder: UI.Engine)
            {
                Prompt = UI.Engine.TranslateResourceString(s_enterNewPasswordText)
            };
            UI.Engine.StrategicMap.TriggerTypedExecuteEventLog(log, immediate: true, done: PasswordEntered);
            _ = log.Release();
        }
    }

    // AoWGamePlayDlg.pas:147
    public void OKBtnClick()
    {
        UI.General.PlayButtonSound(0);
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).Settings != PasswordEdit.Text)
        {
            if (strategicMap.ConnectionType != PlayerConnectionType.Network)
            {
                strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).Settings = PasswordEdit.Text;
            }
            else
            {
                string error = "";
                if (!strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).ChangePlayerSettings(PasswordEdit.Text, error))
                {
                    MessageDialog.ResetMessageDialog(UI.MessageDialog);
                    _ = UI.MessageDialog.MessageMemo.Items.Add("");
                    string couldNotChangePassword = UI.Engine.TranslateResourceString(s_couldNotChangePasswordText);
                    _ = UI.MessageDialog.MessageMemo.Items.Add(couldNotChangePassword);
                    UI.MessageDialog.OKPanel.Visible = true;
                    UI.MessageDialog.Window.Visible = true;
                }
            }
        }

        UI.Engine.Registry.SetAutoSave(AutoSaveCheckBox.Checked);
        UI.Engine.Registry.SetSkipIntro(SkipIntroCheckBox.Checked);
        UI.Engine.Registry.SetMovementSpeed((GameSettings.MovementSpeed)MovementSpeedComboBox.Selected);
        object? objectValue = CombatResolveModeComboBox.Items.Objects[CombatResolveModeComboBox.Selected];
        UI.Engine.Registry.SetCombatResolveMode((GameSettings.CombatResolveMode)ObjectToInt(objectValue));
        WindowPopupEvents events = WindowPopupEvents.None;
        if (Classic1CheckBox.Checked)
        {
            events |= WindowPopupEvents.Combat;
        }

        if (Classic2CheckBox.Checked)
        {
            events |= WindowPopupEvents.Diplomacy;
        }

        if (Classic3CheckBox.Checked)
        {
            events |= WindowPopupEvents.SpellResearched;
        }

        if (Classic4CheckBox.Checked)
        {
            events |= WindowPopupEvents.SpellReady;
        }

        if (Classic5CheckBox.Checked)
        {
            events |= WindowPopupEvents.Rebellion;
        }

        if (Classic6CheckBox.Checked)
        {
            events |= WindowPopupEvents.PartyDamage;
        }

        if (Classic7CheckBox.Checked)
        {
            events |= WindowPopupEvents.HeroUpgrade;
        }

        if (Classic8CheckBox.Checked)
        {
            events |= WindowPopupEvents.HeroJoin;
        }

        UI.Engine.Registry.SetClassicWindowPopupEvents(events);
        events = WindowPopupEvents.None;
        if (Simultaneous1CheckBox.Checked)
        {
            events |= WindowPopupEvents.Combat;
        }

        if (Simultaneous2CheckBox.Checked)
        {
            events |= WindowPopupEvents.Diplomacy;
        }

        if (Simultaneous3CheckBox.Checked)
        {
            events |= WindowPopupEvents.SpellResearched;
        }

        if (Simultaneous4CheckBox.Checked)
        {
            events |= WindowPopupEvents.SpellReady;
        }

        if (Simultaneous5CheckBox.Checked)
        {
            events |= WindowPopupEvents.Rebellion;
        }

        if (Simultaneous6CheckBox.Checked)
        {
            events |= WindowPopupEvents.PartyDamage;
        }

        if (Simultaneous7CheckBox.Checked)
        {
            events |= WindowPopupEvents.HeroUpgrade;
        }

        if (Simultaneous8CheckBox.Checked)
        {
            events |= WindowPopupEvents.HeroJoin;
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
