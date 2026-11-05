// Translated from E2Globals.pas.
using Aow.Rtl;
using Aow.Units.AoWTutorialWin;
using Aow.Units.AoWCampaignJoin;
using Aow.Units.AoWProgressDialog;
using Aow.Units.AoWSyncDlg;
using Aow.Units.AoWTitleScene;
using Aow.Units.AOWTitleWindow;
using Aow.Units.AoWCampaignTransferDlg;
using Aow.Units.AoWGamePlayDlg;
using Aow.Units.AoWPowerDlg;
using Aow.Units.AoWIntroScene;
using Aow.Units.AOWDialogWin;
using Aow.Units.AoWCampaignSettings;

namespace Aow.Units.E2Globals
{
    public static partial class E2Globals
    {
        // { Form references and resource strings accessed by executable E2 routines. }
        public static TGamePlayDlg GamePlayDialog;
        public static TTitleScene TitleScene;
        public static TCampaignJoinDlg CampaignJoinDlg;
        public static TTutorialWindow TutorialWindow;
        public static TTitleWindow TitleWindow;
        public static TCampaignTransferDlg CampaignTransferDlg;
        public static TIntroScene IntroScene;
        public static TPowerDlg PowerDlg;
        public static TSynchronizeDlg SynchronizeDlg;
        public static TDWin DialogWin;
        public static TCampaignSettingsDlg CampaignSettingsDlg;
        public static TProgressDialog ProgressDialog;
        public static readonly AnsiString LoadingRStr = (AnsiString)"Loading";
        public static readonly AnsiString FileNotFoundRStr = (AnsiString)"File not found";
        public static readonly AnsiString WarningRStr = (AnsiString)"Warning";
        public static readonly AnsiString ConnectionLostRStr = (AnsiString)"Connection Lost!";
        public static readonly AnsiString BackgroundRStr = (AnsiString)"Background";
        public static readonly AnsiString SurfaceRStr = (AnsiString)"Surface";
        public static readonly AnsiString CavernsRStr = (AnsiString)"Caverns";
        public static readonly AnsiString DepthsRStr = (AnsiString)"Depths";
        public static readonly AnsiString ChooseFactionRStr = (AnsiString)"Choose a Faction";
        public static readonly AnsiString HideInfoRStr = (AnsiString)"Hide Info";
        public static readonly AnsiString ShowInfoRStr = (AnsiString)"Show Info";
        public static readonly AnsiString ViewSummaryRStr = (AnsiString)"View Summary";
        public static readonly AnsiString CultOfStormsRStr = (AnsiString)"The Cult of Storms";
        public static readonly AnsiString KeepersRStr = (AnsiString)"The Keepers";
        public static readonly AnsiString QuitCampaignRStr = (AnsiString)"Quit the Campaign?";
        public static readonly AnsiString QuitCombatRStr = (AnsiString)"Surrender your forces?";
        public static readonly AnsiString ConfirmRStr = (AnsiString)"Confirm";
    }
}
