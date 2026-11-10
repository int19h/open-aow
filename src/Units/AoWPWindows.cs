// Translated from AoWPWindows.pas.
using Aow.Game.Host;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

// published menu components
public partial class PopupWindows : DataModule
{
    public PopupWindows(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public global::Aow.Game.Ui.Windows.StartMenu StartMenu;
    public AOWMenu OverViewsMenu;
    public AOWMenu SettingsMenu;
    public AOWMenu EndMenu;
    public AOWMenu DiskMenu;
    public global::Aow.Game.Ui.Windows.StartMenu TCStartMenu;
    public IvTranslator Translator;
    public AOWMenu HelpMenu;
    public AOWMenu TCOVMenu;
    public AOWMenu TCDiskMenu;
    public AOWMenu TCHelpMenu;
    public AOWMenu TCSettingsMenu;
    public AOWMenu TCEndMenu;

    // AoWPWindows.pas:61
    public void SettingsMenuSelect(AOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ShowModes();
                break;
            case 1:
                Aow.Game.Ui.Dialogs.PopupWindows.ShowSoundSettings(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg);
                break;
            case 2:
                Aow.Game.Ui.Dialogs.DialogHosts.GamePlayDialog.Root.Visible = true;
                break;
            case 3:
                Aow.Game.Ui.Dialogs.DialogHosts.General.ResetLayout();
                break;
            case 4:
                Aow.Game.Ui.Dialogs.EmailSettingsDlg.ShowEmailSettings(Aow.Game.Ui.Dialogs.DialogHosts.EmailSettingsDlg);
                break;
            case 5:
                Aow.Game.Ui.Dialogs.DialogHosts.DialogWin.DebugWin.Visible = true;
                break;
        }
    }

    // AoWPWindows.pas:83
    public void DiskMenuSelect(AOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 0;
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                break;
            case 1:
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 1;
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                break;
        }
    }

    // AoWPWindows.pas:91
    public void OverViewsMenuSelect(AOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                // { Original opens the information book. }
                Aow.Game.Ui.Dialogs.PopupWindows.ShowInformationBook(Aow.Game.Ui.Dialogs.DialogHosts.WorldMap, null, null);
                break;
            case 1:
                if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.ShowCampaignHistory();
                }

                break;
        }
    }

    // AoWPWindows.pas:99
    public void TCEndMenuSelect(AOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                var quitCombat = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.PopupWindows.QuitCombatRStr);
                var confirm = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.ConfirmRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.DialogWin.Confirm(confirm, quitCombat, 6);
                break;
        }
    }

    // AoWPWindows.pas:115
    public const string AgeOfWondersUrl = "http://www.ageofwonders.com";

    // AoWPWindows.pas:107
    public void HelpMenuSelect(AOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                Aow.Game.Ui.Dialogs.HelpWindow.ShowHelp(507);
                break;
            case 1:
                Aow.Game.Ui.Dialogs.HelpWindow.ShowHelp(500);
                break;
            case 2:
                // The original switches to windowed mode first. The port runs borderless
                // fullscreen, and the browser opens over it.
                RtlUnit.OpenUrl(AgeOfWondersUrl);
                break;
        }
    }

    // AoWPWindows.pas:73. Keep all three branches across the host boundary.
    public void EndMenuSelect(AOWMenu sender)
    {
        switch (sender.FSelectedItem)
        {
            case 0:
                Aow.Game.Ui.Dialogs.DialogHosts.DialogWin.Confirm(
                    Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.ConfirmRStr),
                    Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.PopupWindows.SurrenderYourEmpireRStr), 4);
                break;
            case 1:
                Aow.Game.World.Map.AoWHSMap.Current.ExitGame(ExitGameMode.Terminate, null);
                break;
            case 2:
                if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo) Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.ShowExitSplash();
                else GameHost.MainForm.CloseRequested();
                break;
        }
    }

    private static global::Aow.Game.Engine.Buffers.FixedArray_string_4 TVictoryConditionRStr = global::Aow.Game.Engine.Buffers.FixedArray_string_4.From(default(global::System.ReadOnlySpan<string>));
    private static global::Aow.Game.Engine.Buffers.FixedArray_string_3 TPlayerConnectionTypeRStr = global::Aow.Game.Engine.Buffers.FixedArray_string_3.From(default(global::System.ReadOnlySpan<string>));
    private static readonly string AlliedVictoryXRStr = "Allied victory: %s";
    private static readonly string ConquerTheMapRStr = "Conquer the map";
    private static readonly string DisabledRStr = "Disabled";
    private static readonly string EnabledRStr = "Enabled";
    private static readonly string GametypeXRStr = "Game type: %s";
    private static readonly string HotSeatRStr = "Hot Seat";
    private static readonly string MaximumPlayerHeroesXRStr = "Maximum player heroes: %s";
    private static readonly string MaximumSphereLevelXRStr = "Maximum sphere level: %s";
    private static readonly string ObjectiveXRStr = "Objective: %s";
    private static readonly string PlayersOnMapXRStr = "Players on map: %s";
    private static readonly string SurrenderYourEmpireRStr = "Surrender your empire?";
    private static readonly string TurnModeXRStr = "Turn mode: %s";
    internal static readonly string QuitCombatRStr = "Surrender your forces?";
    private static readonly string MapSettingsRStr = "Scenario Info:";
    private static readonly string DayLimitXRStr = "Turn Limit: %s";

    private static void ShowSoundSettings(global::Aow.Game.Ui.Dialogs.SoundDlg Dialog)
    {
        Dialog.AddBtn.Visible = false;
        Dialog.FindFiles();
        Dialog.SoundSetup();
        Dialog.Dlg.Visible = true;
    }

    internal static void ShowInformationBook(global::Aow.Game.Ui.Dialogs.WorldMap Dialog, global::Aow.Game.Events.ShowBookEventLog Event, global::Aow.Game.Events.ExecuteDoneEvent Done)
    {
        Dialog.FOnBookClosed = RtlUnit.RebindDelegate<NotifyEvent<object>>((global::System.Delegate)Done);
        Dialog.FBookContext = Event;
        if (Dialog.FBookContext != null)
        {
            Dialog.FBookContext.AddRef();
        }
        Dialog.LabelL.Text = global::Aow.Game.World.Map.AoWHSMap.Current.FMapName;
        ((StringCollection)Dialog.MemoL.Items).Clear();
        ((StringCollection)Dialog.MemoL.Items).AddStrings((StringCollection)(object)global::Aow.Game.World.Map.AoWHSMap.Current.FDescription);
        string text = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MapSettingsRStr);
        Dialog.LabelR.Text = text;
        ((StringCollection)Dialog.MemoR.Items).Clear();
        if (global::Aow.Game.World.Map.AoWHSMap.Current.FVictoryCondition == Aow.Game.World.Map.AoWHSMap.VictoryCondition.None)
        {
            string text2 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ConquerTheMapRStr);
            string s = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ObjectiveXRStr);
            FormatArg val = (FormatArg)(text2);
            string text3 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s, new global::System.ReadOnlySpan<FormatArg>(ref val));
            ((StringCollection)Dialog.MemoR.Items).Add(text3);
        }
        else
        {
            string text4 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(TVictoryConditionRStr[(int)global::Aow.Game.World.Map.AoWHSMap.Current.FVictoryCondition]);
            string s2 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ObjectiveXRStr);
            FormatArg val2 = (FormatArg)(text4);
            string text5 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s2, new global::System.ReadOnlySpan<FormatArg>(ref val2));
            ((StringCollection)Dialog.MemoR.Items).Add(text5);
        }
        if (global::Aow.Game.World.Map.AoWHSMap.Current.FDayLimit > 0)
        {
            string s3 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(DayLimitXRStr);
            FormatArg val3 = (FormatArg)(RtlUnit.IntToStr(global::Aow.Game.World.Map.AoWHSMap.Current.FDayLimit));
            string text6 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s3, new global::System.ReadOnlySpan<FormatArg>(ref val3));
            ((StringCollection)Dialog.MemoR.Items).Add(text6);
        }
        if (global::Aow.Game.World.Map.AoWHSMap.Current.FAlliedVictory)
        {
            string text7 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(EnabledRStr);
            string s4 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(AlliedVictoryXRStr);
            FormatArg val4 = (FormatArg)(text7);
            string text8 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s4, new global::System.ReadOnlySpan<FormatArg>(ref val4));
            ((StringCollection)Dialog.MemoR.Items).Add(text8);
        }
        else
        {
            string text9 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(DisabledRStr);
            string s5 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(AlliedVictoryXRStr);
            FormatArg val5 = (FormatArg)(text9);
            string text10 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s5, new global::System.ReadOnlySpan<FormatArg>(ref val5));
            ((StringCollection)Dialog.MemoR.Items).Add(text10);
        }
        int count = global::Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetCount();
        string s6 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(PlayersOnMapXRStr);
        FormatArg val6 = (FormatArg)(RtlUnit.IntToStr(count - 1));
        string text11 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s6, new global::System.ReadOnlySpan<FormatArg>(ref val6));
        ((StringCollection)Dialog.MemoR.Items).Add(text11);
        string s7 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MaximumSphereLevelXRStr);
        FormatArg val7 = (FormatArg)(RtlUnit.IntToStr(global::Aow.Game.World.Map.AoWHSMap.Current.FSphereCount));
        string text12 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s7, new global::System.ReadOnlySpan<FormatArg>(ref val7));
        ((StringCollection)Dialog.MemoR.Items).Add(text12);
        string s8 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MaximumPlayerHeroesXRStr);
        FormatArg val8 = (FormatArg)(RtlUnit.IntToStr((int)global::Aow.Game.World.Map.AoWHSMap.Current.FHeroCountMax));
        string text13 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s8, new global::System.ReadOnlySpan<FormatArg>(ref val8));
        ((StringCollection)Dialog.MemoR.Items).Add(text13);
        if (global::Aow.Game.World.Map.AoWHSMap.Current.FConnectionType == global::Aow.Game.Session.PlayerConnectionType.Local && global::Aow.Game.World.Map.AoWHSMap.Current.FRequestCombatType)
        {
            string text14 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(TPlayerConnectionTypeRStr[(int)global::Aow.Game.World.Map.AoWHSMap.Current.FConnectionType]);
            string text15 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(HotSeatRStr);
            string s9 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GametypeXRStr);
            FormatArg val9 = (FormatArg)(text14 + "/" + text15);
            string text16 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s9, new global::System.ReadOnlySpan<FormatArg>(ref val9));
            ((StringCollection)Dialog.MemoR.Items).Add(text16);
        }
        else
        {
            string text17 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(TPlayerConnectionTypeRStr[(int)global::Aow.Game.World.Map.AoWHSMap.Current.FConnectionType]);
            string s10 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GametypeXRStr);
            FormatArg val10 = (FormatArg)(text17);
            string text18 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s10, new global::System.ReadOnlySpan<FormatArg>(ref val10));
            ((StringCollection)Dialog.MemoR.Items).Add(text18);
        }
        string text19 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameText.TPlayModeRStr[(int)global::Aow.Game.World.Map.AoWHSMap.Current.FPlayMode]);
        string s11 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(TurnModeXRStr);
        FormatArg val11 = (FormatArg)(text19);
        string text20 = global::Aow.Game.Magic.Abilities.AbilityRules.Format(s11, new global::System.ReadOnlySpan<FormatArg>(ref val11));
        ((StringCollection)Dialog.MemoR.Items).Add(text20);
        Dialog.PicL.Visible = false;
        Dialog.PicR.Visible = false;
        Dialog.MemoL.Visible = true;
        Dialog.MemoR.Visible = true;
        Dialog.Back.Visible = false;
        Dialog.Cancel.Visible = false;
        Dialog.Next.Visible = false;
        Dialog.BookWin.Visible = true;
        if (Dialog.BookWin.FBackSurface != null)
        {
            Dialog.BookWin.FBackSurface.Cls(Dialog.BookWin.FMaskColor);
        }
        global::Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 49;
    }
}
