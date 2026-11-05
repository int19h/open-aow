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

namespace Aow.Units.E2Globals;

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
    public static readonly string LoadingRStr = "Loading";
    public static readonly string FileNotFoundRStr = "File not found";
    public static readonly string WarningRStr = "Warning";
    public static readonly string ConnectionLostRStr = "Connection Lost!";
    public static readonly string BackgroundRStr = "Background";
    public static readonly string SurfaceRStr = "Surface";
    public static readonly string CavernsRStr = "Caverns";
    public static readonly string DepthsRStr = "Depths";
    public static readonly string ChooseFactionRStr = "Choose a Faction";
    public static readonly string HideInfoRStr = "Hide Info";
    public static readonly string ShowInfoRStr = "Show Info";
    public static readonly string ViewSummaryRStr = "View Summary";
    public static readonly string CultOfStormsRStr = "The Cult of Storms";
    public static readonly string KeepersRStr = "The Keepers";
    public static readonly string QuitCampaignRStr = "Quit the Campaign?";
    public static readonly string QuitCombatRStr = "Surrender your forces?";
    public static readonly string ConfirmRStr = "Confirm";
}
