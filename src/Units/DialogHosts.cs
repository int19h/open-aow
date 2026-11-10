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
    public static global::Aow.Game.Ui.Dialogs.SoundDlg SoundDlg;

    public static global::Aow.Game.Ui.Dialogs.MultiWindow MultiWindow;

    public static global::Aow.Game.Ui.Dialogs.MagicWin MagicWin;

    public static global::Aow.Game.Ui.Dialogs.TBWindow TBWindow;

    public static global::Aow.Game.Ui.Dialogs.ChatWindow ChatWindow;

    public static global::Aow.Game.Ui.Dialogs.ConstructDlg ConstructDlg;

    public static global::Aow.Game.Ui.Dialogs.DisjunctionDlg DisjunctionDlg;

    public static GamePlayDlg GamePlayDialog;

    public static global::Aow.Game.Ui.Dialogs.TitleScene TitleScene;

    public static global::Aow.Game.Ui.Dialogs.CampaignJoinDlg CampaignJoinDlg;

    public static global::Aow.Game.Ui.Dialogs.TitleWindow TitleWindow;

    public static global::Aow.Game.Ui.Dialogs.CampaignTransferDlg CampaignTransferDlg;

    public static global::Aow.Game.Ui.Dialogs.PowerDlg PowerDlg;

    public static global::Aow.Game.Ui.Dialogs.SynchronizeDlg SynchronizeDlg;

    public static DWin DialogWin;

    public static global::Aow.Game.Ui.Dialogs.CampaignSettingsDlg CampaignSettingsDlg;

    public static global::Aow.Game.Ui.Dialogs.ProgressDialog ProgressDialog;

    public static global::Aow.Game.Ui.Dialogs.UnitSelectionDlg UnitSelectionDlg;

    public static global::Aow.Game.Ui.Dialogs.EmailSettingsDlg EmailSettingsDlg;

    public static global::Aow.Game.Ui.Dialogs.EmailGameSettingsDlg EmailGameSettingsDlg;

    public static global::Aow.Game.Ui.Dialogs.POP3Dlg POP3Dlg;

    public static global::Aow.Game.Ui.Dialogs.CastUnitSpellDlg CastUnitSpellDlg;

    public static global::Aow.Game.Ui.Dialogs.DeformFaceDlg DeformFaceDlg;

    public static global::Aow.Game.Ui.Dialogs.SMTPDlg SMTPDlg;

    public static global::Aow.Game.Ui.Dialogs.TargetSelectionDlg TargetSelectionDlg;

    public static global::Aow.Game.Ui.Dialogs.WorldMap WorldMap;

    public static global::Aow.Game.Ui.Dialogs.ControlWin ControlWin;

    public static global::Aow.Game.Ui.Dialogs.MWindow MWindow;

    public static global::Aow.Game.Ui.Dialogs.PyreScreen PyreScreen;

    public static global::Aow.Game.Ui.Dialogs.CityScreen CityScreen;

    public static global::Aow.Game.Ui.Dialogs.ScenSetup ScenSetup;

    public static global::Aow.Game.Ui.Dialogs.SpellBook SpellBook;

    public static global::Aow.Game.Ui.Dialogs.FontModule FontModule;

    public static global::Aow.Game.Ui.Dialogs.TCScanner TCScanner;

    public static global::Aow.Game.Ui.Dialogs.FileDlgWindow FileDlgWindow;

    public static global::Aow.Game.Ui.Dialogs.InfoBanner InfoBanner;

    public static global::Aow.Game.Ui.Dialogs.IWindow IWindow;

    public static global::Aow.Game.Ui.Dialogs.ItemBanner ItemBanner;

    public static global::Aow.Game.Ui.Dialogs.LeaderSetupWin LeaderSetupWin;

    public static global::Aow.Game.Ui.Dialogs.MessageDlg MessageDlg;

    public static global::Aow.Game.Ui.Dialogs.PInfoW PInfoW;

    public static global::Aow.Game.Ui.Dialogs.ProductionScreen ProductionScreen;

    public static global::Aow.Game.Ui.Dialogs.RealmWin RealmWin;

    public static global::Aow.Game.Ui.Dialogs.ResultsWin ResultsWin;

    public static global::Aow.Game.Ui.Dialogs.SWindow SWindow;

    public static global::Aow.Game.Ui.Dialogs.SelFactionWin SelFactionWin;

    public static global::Aow.Game.Ui.Dialogs.TCUnit TCUnit;

    public static global::Aow.Game.Ui.Dialogs.UnitBanner UnitBanner;

    public static global::Aow.Game.Ui.Dialogs.UnitWindow UnitWindow;

    public static global::Aow.Game.Ui.Dialogs.VideoDialog VideoDialog;

    public static global::Aow.Game.Ui.Dialogs.General General;

    public static global::Aow.Game.Ui.Dialogs.RelWindow RelWindow;

    public static global::Aow.Game.Ui.Dialogs.PopupWindows PopupWindows;

    public static global::Aow.Game.Ui.Dialogs.IntroScene IntroScene;

    public static global::Aow.Game.Ui.Dialogs.HelpWindow HelpWindow;

    public static global::Aow.Game.Ui.Dialogs.IntGfxMod IntGfxMod;

    public static global::Aow.Game.Ui.Dialogs.MapEvents MapEvents;

    public static global::Aow.Game.Ui.Dialogs.SetupEvents SetupEvents;

}
