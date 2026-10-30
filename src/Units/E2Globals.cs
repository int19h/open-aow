// Translated from E2Globals.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.AoWWorldMap;
using static Aow.Units.AoWWorldMap.AoWWorldMap;
using Aow.Units.AoWTutorialWin;
using static Aow.Units.AoWTutorialWin.AoWTutorialWin;
using Aow.Units.AoWCampaignJoin;
using static Aow.Units.AoWCampaignJoin.AoWCampaignJoin;
using Aow.Units.AoWProgressDialog;
using static Aow.Units.AoWProgressDialog.AoWProgressDialog;
using Aow.Units.AoWSyncDlg;
using static Aow.Units.AoWSyncDlg.AoWSyncDlg;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWTitleScene;
using static Aow.Units.AoWTitleScene.AoWTitleScene;
using Aow.Units.AOWTitleWindow;
using static Aow.Units.AOWTitleWindow.AOWTitleWindow;
using Aow.Units.AoWCampaignTransferDlg;
using static Aow.Units.AoWCampaignTransferDlg.AoWCampaignTransferDlg;
using Aow.Units.AoWTargetSelection;
using static Aow.Units.AoWTargetSelection.AoWTargetSelection;
using Aow.Units.AoWLeaderSetup;
using static Aow.Units.AoWLeaderSetup.AoWLeaderSetup;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.AoWGamePlayDlg;
using static Aow.Units.AoWGamePlayDlg.AoWGamePlayDlg;
using Aow.Units.AoWPowerDlg;
using static Aow.Units.AoWPowerDlg.AoWPowerDlg;
using Aow.Units.AoWIntroScene;
using static Aow.Units.AoWIntroScene.AoWIntroScene;
using Aow.Units.AOWDialogWin;
using static Aow.Units.AOWDialogWin.AOWDialogWin;
using Aow.Units.AoWCampaignSettings;
using static Aow.Units.AoWCampaignSettings.AoWCampaignSettings;

namespace Aow.Units.E2Globals
{
    public static partial class E2Globals
    {
        // { Form references and resource strings accessed by executable E2 routines. }
        public static Aow.Units.AoWGamePlayDlg.TGamePlayDlg GamePlayDialog;
        public static Aow.Units.AoWTitleScene.TTitleScene TitleScene;
        public static Aow.Units.AoWCampaignJoin.TCampaignJoinDlg CampaignJoinDlg;
        public static Aow.Units.AoWTutorialWin.TTutorialWindow TutorialWindow;
        public static Aow.Units.AOWTitleWindow.TTitleWindow TitleWindow;
        public static Aow.Units.AoWCampaignTransferDlg.TCampaignTransferDlg CampaignTransferDlg;
        public static Aow.Units.AoWIntroScene.TIntroScene IntroScene;
        public static Aow.Units.AoWPowerDlg.TPowerDlg PowerDlg;
        public static Aow.Units.AoWSyncDlg.TSynchronizeDlg SynchronizeDlg;
        public static Aow.Units.AOWDialogWin.TDWin DialogWin;
        public static Aow.Units.AoWCampaignSettings.TCampaignSettingsDlg CampaignSettingsDlg;
        public static Aow.Units.AoWProgressDialog.TProgressDialog ProgressDialog;
        public static readonly Aow.Rtl.AnsiString LoadingRStr = (AnsiString)"Loading";
        public static readonly Aow.Rtl.AnsiString FileNotFoundRStr = (AnsiString)"File not found";
        public static readonly Aow.Rtl.AnsiString WarningRStr = (AnsiString)"Warning";
        public static readonly Aow.Rtl.AnsiString ConnectionLostRStr = (AnsiString)"Connection Lost!";
        public static readonly Aow.Rtl.AnsiString BackgroundRStr = (AnsiString)"Background";
        public static readonly Aow.Rtl.AnsiString SurfaceRStr = (AnsiString)"Surface";
        public static readonly Aow.Rtl.AnsiString CavernsRStr = (AnsiString)"Caverns";
        public static readonly Aow.Rtl.AnsiString DepthsRStr = (AnsiString)"Depths";
        public static readonly Aow.Rtl.AnsiString ChooseFactionRStr = (AnsiString)"Choose a Faction";
        public static readonly Aow.Rtl.AnsiString HideInfoRStr = (AnsiString)"Hide Info";
        public static readonly Aow.Rtl.AnsiString ShowInfoRStr = (AnsiString)"Show Info";
        public static readonly Aow.Rtl.AnsiString ViewSummaryRStr = (AnsiString)"View Summary";
        public static readonly Aow.Rtl.AnsiString CultOfStormsRStr = (AnsiString)"The Cult of Storms";
        public static readonly Aow.Rtl.AnsiString KeepersRStr = (AnsiString)"The Keepers";
        public static readonly Aow.Rtl.AnsiString QuitCampaignRStr = (AnsiString)"Quit the Campaign?";
        public static readonly Aow.Rtl.AnsiString QuitCombatRStr = (AnsiString)"Surrender your forces?";
        public static readonly Aow.Rtl.AnsiString ConfirmRStr = (AnsiString)"Confirm";
    }
}
