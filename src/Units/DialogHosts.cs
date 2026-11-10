using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public static class DialogHosts
{
    public static TSoundDlg SoundDlg;

    public static TMultiWindow MultiWindow;

    public static TMagicWin MagicWin;

    public static TTBWindow TBWindow;

    public static TChatWindow ChatWindow;

    public static TConstructDlg ConstructDlg;

    public static TDisjunctionDlg DisjunctionDlg;

    public static TGamePlayDlg GamePlayDialog;

    public static TTitleScene TitleScene;

    public static TCampaignJoinDlg CampaignJoinDlg;

    public static TTitleWindow TitleWindow;

    public static TCampaignTransferDlg CampaignTransferDlg;

    public static TPowerDlg PowerDlg;

    public static TSynchronizeDlg SynchronizeDlg;

    public static TDWin DialogWin;

    public static TCampaignSettingsDlg CampaignSettingsDlg;

    public static TProgressDialog ProgressDialog;

    public static TUnitSelectionDlg UnitSelectionDlg;

    public static TEmailSettingsDlg EmailSettingsDlg;

    public static TEmailGameSettingsDlg EmailGameSettingsDlg;

    public static TPOP3Dlg POP3Dlg;

    public static TCastUnitSpellDlg CastUnitSpellDlg;

    public static TDeformFaceDlg DeformFaceDlg;

    public static TSMTPDlg SMTPDlg;

    public static TTargetSelectionDlg TargetSelectionDlg;

    public static TWorldMap WorldMap;

    public static TControlWin ControlWin;

    public static TMWindow MWindow;

    public static TPyreScreen PyreScreen;

    public static TCityScreen CityScreen;

    public static TScenSetup ScenSetup;

    public static TSpellBook SpellBook;

    public static TFontModule FontModule;

    public static TTCScanner TCScanner;

    public static TFileDlgWindow FileDlgWindow;

    public static TInfoBanner InfoBanner;

    public static TIWindow IWindow;

    public static TItemBanner ItemBanner;

    public static TLeaderSetupWin LeaderSetupWin;

    public static TMessageDlg MessageDlg;

    public static TPInfoW PInfoW;

    public static TProductionScreen ProductionScreen;

    public static TRealmWin RealmWin;

    public static TResultsWin ResultsWin;

    public static TSWindow SWindow;

    public static TSelFactionWin SelFactionWin;

    public static TTCUnit TCUnit;

    public static TUnitBanner UnitBanner;

    public static TUnitWindow UnitWindow;

    public static TVideoDialog VideoDialog;

    public static TGeneral General;

    public static TRelWindow RelWindow;

    public static TPopupWindows PopupWindows;

    public static TIntroScene IntroScene;

    public static THelpWindow HelpWindow;

    public static TIntGfxMod IntGfxMod;

    public static TMapEvents MapEvents;

    public static TSetupEvents SetupEvents;

}
