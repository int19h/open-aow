// Translated from AoWPWindows.pas.
using Aow.Engine.Buffers;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

// published menu components
public partial class PopupWindows : DataModule
{
    public PopupWindows(Component owner) : base(owner)
    { }

    public StartMenu StartMenu;
    public AOWMenu OverViewsMenu;
    public AOWMenu SettingsMenu;
    public AOWMenu EndMenu;
    public AOWMenu DiskMenu;
    public StartMenu TCStartMenu;
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
                DialogHosts.VideoDialog.ShowModes();
                break;
            case 1:
                ShowSoundSettings(DialogHosts.SoundDlg);
                break;
            case 2:
                DialogHosts.GamePlayDialog.Root.Visible = true;
                break;
            case 3:
                DialogHosts.General.ResetLayout();
                break;
            case 4:
                EmailSettingsDlg.ShowEmailSettings(DialogHosts.EmailSettingsDlg);
                break;
            case 5:
                DialogHosts.DialogWin.DebugWin.Visible = true;
                break;
        }
    }

    // AoWPWindows.pas:83
    public void DiskMenuSelect(AOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                DialogHosts.FileDlgWindow.FMode = 0;
                DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                break;
            case 1:
                DialogHosts.FileDlgWindow.FMode = 1;
                DialogHosts.FileDlgWindow.FileDialog.Visible = true;
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
                ShowInformationBook(DialogHosts.WorldMap, null, null);
                break;
            case 1:
                if (GameTables.AoWEngine.FCampaign != null)
                {
                    DialogHosts.WorldMap.ShowCampaignHistory();
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
                var quitCombat = AbilityRules.TranslateRStr(QuitCombatRStr);
                var confirm = AbilityRules.TranslateRStr(LobbyText.ConfirmRStr);
                DialogHosts.DialogWin.Confirm(confirm, quitCombat, 6);
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
                HelpWindow.ShowHelp(507);
                break;
            case 1:
                HelpWindow.ShowHelp(500);
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
                DialogHosts.DialogWin.Confirm(
                    AbilityRules.TranslateRStr(LobbyText.ConfirmRStr),
                    AbilityRules.TranslateRStr(SurrenderYourEmpireRStr), 4);
                break;
            case 1:
                AoWHSMap.Current.ExitGame(ExitGameMode.Terminate, null);
                break;
            case 2:
                if (GameTables.AoWEngine.FDemo) DialogHosts.TitleWindow.ShowExitSplash();
                else GameHost.MainForm.CloseRequested();
                break;
        }
    }

    private static FixedArray_string_4 TVictoryConditionRStr = FixedArray_string_4.From(default(ReadOnlySpan<string>));
    private static FixedArray_string_3 TPlayerConnectionTypeRStr = FixedArray_string_3.From(default(ReadOnlySpan<string>));
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

    private static void ShowSoundSettings(SoundDlg Dialog)
    {
        Dialog.AddBtn.Visible = false;
        Dialog.FindFiles();
        Dialog.SoundSetup();
        Dialog.Dlg.Visible = true;
    }

    internal static void ShowInformationBook(WorldMap Dialog, ShowBookEventLog Event, ExecuteDoneEvent Done)
    {
        Dialog.FOnBookClosed = RtlUnit.RebindDelegate<NotifyEvent<object>>((Delegate)Done);
        Dialog.FBookContext = Event;
        if (Dialog.FBookContext != null)
        {
            Dialog.FBookContext.AddRef();
        }
        Dialog.LabelL.Text = AoWHSMap.Current.FMapName;
        ((StringCollection)Dialog.MemoL.Items).Clear();
        ((StringCollection)Dialog.MemoL.Items).AddStrings((StringCollection)(object)AoWHSMap.Current.FDescription);
        string text = AbilityRules.TranslateRStr(MapSettingsRStr);
        Dialog.LabelR.Text = text;
        ((StringCollection)Dialog.MemoR.Items).Clear();
        if (AoWHSMap.Current.FVictoryCondition == AoWHSMap.VictoryCondition.None)
        {
            string text2 = AbilityRules.TranslateRStr(ConquerTheMapRStr);
            string s = AbilityRules.TranslateRStr(ObjectiveXRStr);
            FormatArg val = (FormatArg)(text2);
            string text3 = AbilityRules.Format(s, new ReadOnlySpan<FormatArg>(ref val));
            ((StringCollection)Dialog.MemoR.Items).Add(text3);
        }
        else
        {
            string text4 = AbilityRules.TranslateRStr(TVictoryConditionRStr[(int)AoWHSMap.Current.FVictoryCondition]);
            string s2 = AbilityRules.TranslateRStr(ObjectiveXRStr);
            FormatArg val2 = (FormatArg)(text4);
            string text5 = AbilityRules.Format(s2, new ReadOnlySpan<FormatArg>(ref val2));
            ((StringCollection)Dialog.MemoR.Items).Add(text5);
        }
        if (AoWHSMap.Current.FDayLimit > 0)
        {
            string s3 = AbilityRules.TranslateRStr(DayLimitXRStr);
            FormatArg val3 = (FormatArg)(RtlUnit.IntToStr(AoWHSMap.Current.FDayLimit));
            string text6 = AbilityRules.Format(s3, new ReadOnlySpan<FormatArg>(ref val3));
            ((StringCollection)Dialog.MemoR.Items).Add(text6);
        }
        if (AoWHSMap.Current.FAlliedVictory)
        {
            string text7 = AbilityRules.TranslateRStr(EnabledRStr);
            string s4 = AbilityRules.TranslateRStr(AlliedVictoryXRStr);
            FormatArg val4 = (FormatArg)(text7);
            string text8 = AbilityRules.Format(s4, new ReadOnlySpan<FormatArg>(ref val4));
            ((StringCollection)Dialog.MemoR.Items).Add(text8);
        }
        else
        {
            string text9 = AbilityRules.TranslateRStr(DisabledRStr);
            string s5 = AbilityRules.TranslateRStr(AlliedVictoryXRStr);
            FormatArg val5 = (FormatArg)(text9);
            string text10 = AbilityRules.Format(s5, new ReadOnlySpan<FormatArg>(ref val5));
            ((StringCollection)Dialog.MemoR.Items).Add(text10);
        }
        int count = AoWHSMap.Current.FPlayers.GetCount();
        string s6 = AbilityRules.TranslateRStr(PlayersOnMapXRStr);
        FormatArg val6 = (FormatArg)(RtlUnit.IntToStr(count - 1));
        string text11 = AbilityRules.Format(s6, new ReadOnlySpan<FormatArg>(ref val6));
        ((StringCollection)Dialog.MemoR.Items).Add(text11);
        string s7 = AbilityRules.TranslateRStr(MaximumSphereLevelXRStr);
        FormatArg val7 = (FormatArg)(RtlUnit.IntToStr(AoWHSMap.Current.FSphereCount));
        string text12 = AbilityRules.Format(s7, new ReadOnlySpan<FormatArg>(ref val7));
        ((StringCollection)Dialog.MemoR.Items).Add(text12);
        string s8 = AbilityRules.TranslateRStr(MaximumPlayerHeroesXRStr);
        FormatArg val8 = (FormatArg)(RtlUnit.IntToStr((int)AoWHSMap.Current.FHeroCountMax));
        string text13 = AbilityRules.Format(s8, new ReadOnlySpan<FormatArg>(ref val8));
        ((StringCollection)Dialog.MemoR.Items).Add(text13);
        if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Local && AoWHSMap.Current.FRequestCombatType)
        {
            string text14 = AbilityRules.TranslateRStr(TPlayerConnectionTypeRStr[(int)AoWHSMap.Current.FConnectionType]);
            string text15 = AbilityRules.TranslateRStr(HotSeatRStr);
            string s9 = AbilityRules.TranslateRStr(GametypeXRStr);
            FormatArg val9 = (FormatArg)(text14 + "/" + text15);
            string text16 = AbilityRules.Format(s9, new ReadOnlySpan<FormatArg>(ref val9));
            ((StringCollection)Dialog.MemoR.Items).Add(text16);
        }
        else
        {
            string text17 = AbilityRules.TranslateRStr(TPlayerConnectionTypeRStr[(int)AoWHSMap.Current.FConnectionType]);
            string s10 = AbilityRules.TranslateRStr(GametypeXRStr);
            FormatArg val10 = (FormatArg)(text17);
            string text18 = AbilityRules.Format(s10, new ReadOnlySpan<FormatArg>(ref val10));
            ((StringCollection)Dialog.MemoR.Items).Add(text18);
        }
        string text19 = AbilityRules.TranslateRStr(GameText.TPlayModeRStr[(int)AoWHSMap.Current.FPlayMode]);
        string s11 = AbilityRules.TranslateRStr(TurnModeXRStr);
        FormatArg val11 = (FormatArg)(text19);
        string text20 = AbilityRules.Format(s11, new ReadOnlySpan<FormatArg>(ref val11));
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
        DialogHosts.General.Manager.OverlayPriority = 49;
    }
}
