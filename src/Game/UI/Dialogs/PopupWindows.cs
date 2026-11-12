// Translated from AoWPWindows.pas.
using System.Collections.Immutable;

using Aow.Game.Combat;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// published menu components
public partial class PopupWindows : UIModule
{
    public PopupWindows(UIContext ui) : base(ui)
    { }

    public Menu OverviewsMenu;

    // AoWPWindows.pas:61
    public void SettingsMenuSelect(Menu sender)
    {
        switch (sender.SelectedItem)
        {
            case 0:
                UI.VideoDialog.ShowModes();
                break;
            case 1:
                ShowSoundSettings(UI.SoundDialog);
                break;
            case 2:
                UI.GamePlayDialog.Window.Visible = true;
                break;
            case 3:
                UI.General.ResetLayout();
                break;
            case 4:
                EmailSettingsDialog.ShowEmailSettings(UI.EmailSettingsDialog);
                break;
            case 5:
                UI.DebugWindow.Show();
                break;
        }
    }

    // AoWPWindows.pas:83
    public void DiskMenuSelect(Menu sender)
    {
        switch (sender.SelectedItem)
        {
            case 0:
                UI.FileDialogWindow.Mode = 0;
                UI.FileDialogWindow.Window.Visible = true;
                break;
            case 1:
                UI.FileDialogWindow.Mode = 1;
                UI.FileDialogWindow.Window.Visible = true;
                break;
        }
    }

    // AoWPWindows.pas:91
    public void OverViewsMenuSelect(Menu sender)
    {
        switch (sender.SelectedItem)
        {
            case 0:
                // { Original opens the information book. }
                ShowInformationBook(UI.WorldMap, null, null);
                break;
            case 1:
                if (UI.Engine.Campaign != null)
                {
                    UI.WorldMap.ShowCampaignHistory();
                }

                break;
        }
    }

    // AoWPWindows.pas:99
    public void TCEndMenuSelect(Menu sender)
    {
        switch (sender.SelectedItem)
        {
            case 0:
                var quitCombat = UI.Engine.TranslateResourceString(QuitCombatText);
                var confirm = UI.Engine.TranslateResourceString(LobbyText.ConfirmText);
                UI.ConfirmationDialog.Confirm(confirm, text: quitCombat, onConfirmed: SurrenderCombat);
                break;
        }
    }

    // AOWDialogWin.pas:73, action 6: the seated player gives up the battle.
    internal static void SurrenderCombat()
    {
        if (CombatMap.Current != null)
        {
            CombatMap.Current.Surrender(CombatMap.Current.SeatedCombatPlayer);
        }
    }

    // AOWDialogWin.pas:73, action 4: the seated player gives up the game.
    private static void SurrenderEmpire() => StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Surrender();

    // AoWPWindows.pas:115
    public const string AgeOfWondersUrl = "http://www.ageofwonders.com";

    // AoWPWindows.pas:107
    public void HelpMenuSelect(Menu sender)
    {
        switch (sender.SelectedItem)
        {
            case 0:
                UI.HelpWindow.ShowHelp(507);
                break;
            case 1:
                UI.HelpWindow.ShowHelp(500);
                break;
            case 2:
                // The original switches to windowed mode first. The port runs borderless
                // fullscreen, and the browser opens over it.
                RtlUnit.OpenUrl(AgeOfWondersUrl);
                break;
        }
    }

    // AoWPWindows.pas:73. Keep all three branches across the host boundary.
    public void EndMenuSelect(Menu sender)
    {
        switch (sender.SelectedItem)
        {
            case 0:
                UI.ConfirmationDialog.Confirm(
                    UI.Engine.TranslateResourceString(LobbyText.ConfirmText),
                    text: UI.Engine.TranslateResourceString(s_surrenderYourEmpireText),
                    onConfirmed: SurrenderEmpire);
                break;
            case 1:
                StrategicMap.Current.ExitGame(ExitGameMode.Terminate, null);
                break;
            case 2:
                if (UI.Engine.Demo) UI.TitleWindow.ShowExitSplash();
                else GameHost.MainForm.CloseRequested();
                break;
        }
    }

    private static readonly ImmutableArray<string> s_victoryConditionTexts = ["", "", "", ""];
    private static readonly ImmutableArray<string> s_playerConnectionTypeTexts = ["", "", ""];
    private static readonly string s_alliedVictoryFormat = "Allied victory: %s";
    private static readonly string s_conquerMapText = "Conquer the map";
    private static readonly string s_disabledText = "Disabled";
    private static readonly string s_enabledText = "Enabled";
    private static readonly string s_gameTypeFormat = "Game type: %s";
    private static readonly string s_hotSeatText = "Hot Seat";
    private static readonly string s_maximumPlayerHeroesFormat = "Maximum player heroes: %s";
    private static readonly string s_maximumSphereLevelFormat = "Maximum sphere level: %s";
    private static readonly string s_objectiveFormat = "Objective: %s";
    private static readonly string s_playersOnMapFormat = "Players on map: %s";
    private static readonly string s_surrenderYourEmpireText = "Surrender your empire?";
    private static readonly string s_turnModeFormat = "Turn mode: %s";

    internal static string QuitCombatText { get; } = "Surrender your forces?";

    private static readonly string s_mapSettingsText = "Scenario Info:";
    private static readonly string s_dayLimitFormat = "Turn Limit: %s";

    private static void ShowSoundSettings(SoundDialog dialog)
    {
        dialog.AddButton.Visible = false;
        dialog.FindFiles();
        dialog.SoundSetup();
        dialog.Window.Visible = true;
    }

    internal static void ShowInformationBook(WorldMap dialog, ShowBookEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.SetBookClosedHandler(RtlUnit.RebindDelegate<NotifyEvent<object>>((Delegate)done));
        dialog.BookContext = eventValue;
        if (dialog.BookContext != null)
        {
            dialog.BookContext.AddRef();
        }
        dialog.LeftPageLabel.Text = StrategicMap.Current.MapName;
        ((StringCollection)dialog.LeftPageMemo.Items).Clear();
        ((StringCollection)dialog.LeftPageMemo.Items).AddStrings((StringCollection)(object)StrategicMap.Current.Description);
        string text = dialog.UI.Engine.TranslateResourceString(s_mapSettingsText);
        dialog.RightPageLabel.Text = text;
        ((StringCollection)dialog.RightPageMemo.Items).Clear();
        if (StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.None)
        {
            string text2 = dialog.UI.Engine.TranslateResourceString(s_conquerMapText);
            string s = dialog.UI.Engine.TranslateResourceString(s_objectiveFormat);
            FormatArg value = (FormatArg)(text2);
            string text3 = AbilityRules.Format(s, new ReadOnlySpan<FormatArg>(ref value));
            ((StringCollection)dialog.RightPageMemo.Items).Add(text3);
        }
        else
        {
            string text4 = dialog.UI.Engine.TranslateResourceString(s_victoryConditionTexts[(int)StrategicMap.Current.VictoryCondition]);
            string s2 = dialog.UI.Engine.TranslateResourceString(s_objectiveFormat);
            FormatArg value2 = (FormatArg)(text4);
            string text5 = AbilityRules.Format(s2, new ReadOnlySpan<FormatArg>(ref value2));
            ((StringCollection)dialog.RightPageMemo.Items).Add(text5);
        }
        if (StrategicMap.Current.DayLimit > 0)
        {
            string s3 = dialog.UI.Engine.TranslateResourceString(s_dayLimitFormat);
            FormatArg value3 = (FormatArg)(RtlUnit.IntToStr(StrategicMap.Current.DayLimit));
            string text6 = AbilityRules.Format(s3, new ReadOnlySpan<FormatArg>(ref value3));
            ((StringCollection)dialog.RightPageMemo.Items).Add(text6);
        }
        if (StrategicMap.Current.AlliedVictory)
        {
            string text7 = dialog.UI.Engine.TranslateResourceString(s_enabledText);
            string s4 = dialog.UI.Engine.TranslateResourceString(s_alliedVictoryFormat);
            FormatArg value4 = (FormatArg)(text7);
            string text8 = AbilityRules.Format(s4, new ReadOnlySpan<FormatArg>(ref value4));
            ((StringCollection)dialog.RightPageMemo.Items).Add(text8);
        }
        else
        {
            string text9 = dialog.UI.Engine.TranslateResourceString(s_disabledText);
            string s5 = dialog.UI.Engine.TranslateResourceString(s_alliedVictoryFormat);
            FormatArg value5 = (FormatArg)(text9);
            string text10 = AbilityRules.Format(s5, new ReadOnlySpan<FormatArg>(ref value5));
            ((StringCollection)dialog.RightPageMemo.Items).Add(text10);
        }
        int count = StrategicMap.Current.Players.GetCount();
        string s6 = dialog.UI.Engine.TranslateResourceString(s_playersOnMapFormat);
        FormatArg value6 = (FormatArg)(RtlUnit.IntToStr(count - 1));
        string text11 = AbilityRules.Format(s6, new ReadOnlySpan<FormatArg>(ref value6));
        ((StringCollection)dialog.RightPageMemo.Items).Add(text11);
        string s7 = dialog.UI.Engine.TranslateResourceString(s_maximumSphereLevelFormat);
        FormatArg value7 = (FormatArg)(RtlUnit.IntToStr(StrategicMap.Current.SphereCount));
        string text12 = AbilityRules.Format(s7, new ReadOnlySpan<FormatArg>(ref value7));
        ((StringCollection)dialog.RightPageMemo.Items).Add(text12);
        string s8 = dialog.UI.Engine.TranslateResourceString(s_maximumPlayerHeroesFormat);
        FormatArg value8 = (FormatArg)(RtlUnit.IntToStr((int)StrategicMap.Current.HeroCountMax));
        string text13 = AbilityRules.Format(s8, new ReadOnlySpan<FormatArg>(ref value8));
        ((StringCollection)dialog.RightPageMemo.Items).Add(text13);
        if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Local && StrategicMap.Current.RequestCombatType)
        {
            string text14 = dialog.UI.Engine.TranslateResourceString(s_playerConnectionTypeTexts[(int)StrategicMap.Current.ConnectionType]);
            string text15 = dialog.UI.Engine.TranslateResourceString(s_hotSeatText);
            string s9 = dialog.UI.Engine.TranslateResourceString(s_gameTypeFormat);
            FormatArg value9 = (FormatArg)(text14 + "/" + text15);
            string text16 = AbilityRules.Format(s9, new ReadOnlySpan<FormatArg>(ref value9));
            ((StringCollection)dialog.RightPageMemo.Items).Add(text16);
        }
        else
        {
            string text17 = dialog.UI.Engine.TranslateResourceString(s_playerConnectionTypeTexts[(int)StrategicMap.Current.ConnectionType]);
            string s10 = dialog.UI.Engine.TranslateResourceString(s_gameTypeFormat);
            FormatArg value10 = (FormatArg)(text17);
            string text18 = AbilityRules.Format(s10, new ReadOnlySpan<FormatArg>(ref value10));
            ((StringCollection)dialog.RightPageMemo.Items).Add(text18);
        }
        string text19 = dialog.UI.Engine.TranslateResourceString(GameText.PlayModeTexts[(int)StrategicMap.Current.PlayMode]);
        string s11 = dialog.UI.Engine.TranslateResourceString(s_turnModeFormat);
        FormatArg value11 = (FormatArg)(text19);
        string text20 = AbilityRules.Format(s11, new ReadOnlySpan<FormatArg>(ref value11));
        ((StringCollection)dialog.RightPageMemo.Items).Add(text20);
        dialog.LeftPagePicture.Visible = false;
        dialog.RightPagePicture.Visible = false;
        dialog.LeftPageMemo.Visible = true;
        dialog.RightPageMemo.Visible = true;
        dialog.BackButton.Visible = false;
        dialog.CancelButton.Visible = false;
        dialog.NextButton.Visible = false;
        dialog.Book.Visible = true;
        if (dialog.Book.BackSurface != null)
        {
            dialog.Book.BackSurface.Clear(dialog.Book._maskColor);
        }
        dialog.UI.General.Manager.OverlayPriority = 49;
    }
}
