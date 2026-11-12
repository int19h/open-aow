
using Aow.Game.UI.Dialogs;

namespace Aow.Game.Layout;

public static class GameLayouts
{
    public static CombatMapEvents TacticalBattleMapEvents { get; set; }

    public static void Initialize(LayoutSession session)
    {
        var module0 = new UIGraphics(null);
        session.Load(module0, module => DialogHosts.UIGraphics = module);
        var module1 = new UIFonts(null);
        session.Load(module1, module => DialogHosts.UIFonts = module);
        var module2 = new General(null);
        session.Load(module2, module => DialogHosts.General = module);
        var module3 = new TargetSelectionDialog(null);
        session.Load(module3, module => DialogHosts.TargetSelectionDialog = module);
        var module4 = new SetupEvents(null);
        session.Load(module4, module => DialogHosts.SetupEvents = module);
        var module5 = new MapEvents(null);
        session.Load(module5, module => DialogHosts.MapEvents = module);
        CombatMapEvents module6 = new CombatMapEvents(null);
        session.Load(module6, module => TacticalBattleMapEvents = module);
        var module7 = new TitleWindow(null);
        session.Load(module7, module => DialogHosts.TitleWindow = module);
        var module8 = new PopupWindows(null);
        session.Load(module8, module => DialogHosts.PopupWindows = module);
        TaskBar module9 = new TaskBar(null);
        session.Load(module9, module => DialogHosts.TaskBar = module);
        var module10 = new ScannerWindow(null);
        session.Load(module10, module => DialogHosts.ScannerWindow = module);
        var module11 = new MapWindow(null);
        session.Load(module11, module => DialogHosts.MapWindow = module);
        var module12 = new InfoWindow(null);
        session.Load(module12, module => DialogHosts.InfoWindow = module);
        var module13 = new ConfirmationDialog(null);
        session.Load(module13, module => DialogHosts.ConfirmationDialog = module);
        var module14 = new ControlWindow(null);
        session.Load(module14, module => DialogHosts.ControlWindow = module);
        var module15 = new UnitBanner(null);
        session.Load(module15, module => DialogHosts.UnitBanner = module);
        var module16 = new UnitWindow(null);
        session.Load(module16, module => DialogHosts.UnitWindow = module);
        var module17 = new ChatWindow(null);
        session.Load(module17, module => DialogHosts.ChatWindow = module);
        var module18 = new MultiWindow(null);
        session.Load(module18, module => DialogHosts.MultiWindow = module);
        var module19 = new FileDialogWindow(null);
        session.Load(module19, module => DialogHosts.FileDialogWindow = module);
        var module20 = new PlayerInfoWindow(null);
        session.Load(module20, module => DialogHosts.PlayerInfoWindow = module);
        var module21 = new ProgressDialog(null);
        session.Load(module21, module => DialogHosts.ProgressDialog = module);
        var module22 = new SoundDialog(null);
        session.Load(module22, module => DialogHosts.SoundDialog = module);
        var module23 = new VideoDialog(null);
        session.Load(module23, module => DialogHosts.VideoDialog = module);
        var module24 = new FastBattleWindow(null);
        session.Load(module24, module => FastBattleWindow = module);
        var module25 = new PyreScreen(null);
        session.Load(module25, module => DialogHosts.PyreScreen = module);
        var module26 = new NewTurnDialog(null);
        session.Load(module26, module => NewTurnDialog = module);
        var module27 = new ArmyEventDialog(null);
        session.Load(module27, module => ArmyEventDialog = module);
        var module28 = new CityJoinDialog(null);
        session.Load(module28, module => CityJoinDialog = module);
        var module29 = new ItemBanner(null);
        session.Load(module29, module => DialogHosts.ItemBanner = module);
        var module30 = new CityRebelDialog(null);
        session.Load(module30, module => CityRebelDialog = module);
        var module31 = new FactionSelectionWindow(null);
        session.Load(module31, module => DialogHosts.FactionSelectionWindow = module);
        var module32 = new MessageReceiveDialog(null);
        session.Load(module32, module => MessageReceiveDialog = module);
        var module33 = new HeroInfoDialog(null);
        session.Load(module33, module => HeroInfoDialog = module);
        var module34 = new PartyJoinDialog(null);
        session.Load(module34, module => PartyJoinDialog = module);
        var module35 = new ArmyInfoDialog(null);
        session.Load(module35, module => ArmyInfoDialog = module);
        var module36 = new WorldMap(null);
        session.Load(module36, module => DialogHosts.WorldMap = module);
        CombatUnitInfo module37 = new CombatUnitInfo(null);
        session.Load(module37, module => DialogHosts.CombatUnitInfo = module);
        CombatScanner module38 = new CombatScanner(null);
        session.Load(module38, module => DialogHosts.CombatScanner = module);
        var module39 = new ScenarioSetup(null);
        session.Load(module39, module => DialogHosts.ScenarioSetup = module);
        CombatChoice module40 = new CombatChoice(null);
        session.Load(module40, module => TacticalBattleFastBattleDialog = module);
        var module41 = new RelationWindow(null);
        session.Load(module41, module => DialogHosts.RelationWindow = module);
        var module42 = new UnitSelectionDialog(null);
        session.Load(module42, module => DialogHosts.UnitSelectionDialog = module);
        var module43 = new ArenaDialog(null);
        session.Load(module43, module => ArenaDialog = module);
        var module44 = new ConstructDialog(null);
        session.Load(module44, module => DialogHosts.ConstructDialog = module);
        var module45 = new MagicWindow(null);
        session.Load(module45, module => DialogHosts.MagicWindow = module);
        var module46 = new WizardsTowerDialog(null);
        session.Load(module46, module => WizardsTowerDialog = module);
        var module47 = new PowerDialog(null);
        session.Load(module47, module => DialogHosts.PowerDialog = module);
        var module48 = new SpellBook(null);
        session.Load(module48, module => DialogHosts.SpellBook = module);
        var module49 = new LeaderSetupWindow(null);
        session.Load(module49, module => DialogHosts.LeaderSetupWindow = module);
        var module50 = new DeformFaceDialog(null);
        session.Load(module50, module => DialogHosts.DeformFaceDialog = module);
        var module51 = new HeroUpgradeDialog(null);
        session.Load(module51, module => HeroUpgradeDialog = module);
        var module52 = new EmailGameSettingsDialog(null);
        session.Load(module52, module => DialogHosts.EmailGameSettingsDialog = module);
        var module53 = new CastUnitSpellDialog(null);
        session.Load(module53, module => DialogHosts.CastUnitSpellDialog = module);
        var module54 = new DisjunctionDialog(null);
        session.Load(module54, module => DialogHosts.DisjunctionDialog = module);
        var module55 = new HeroJoinDialog(null);
        session.Load(module55, module => HeroJoinDialog = module);
        var module56 = new ResultsWindow(null);
        session.Load(module56, module => DialogHosts.ResultsWindow = module);
        var module57 = new SynchronizeDialog(null);
        session.Load(module57, module => DialogHosts.SynchronizeDialog = module);
        var module58 = new CityScreen(null);
        session.Load(module58, module => DialogHosts.CityScreen = module);
        var module59 = new ProductionScreen(null);
        session.Load(module59, module => DialogHosts.ProductionScreen = module);
        var module60 = new RealmWindow(null);
        session.Load(module60, module => DialogHosts.RealmWindow = module);
        var module61 = new CampaignTransferDialog(null);
        session.Load(module61, module => DialogHosts.CampaignTransferDialog = module);
        var module62 = new TitleScene(null);
        session.Load(module62, module => DialogHosts.TitleScene = module);
        var module63 = new IntroScene(null);
        session.Load(module63, static _ => { });
        DialogHosts.IntroScene = module63;
        var module64 = new SendEmailGameDialog(null);
        session.Load(module64, module => SendEmailGameDialog = module);
        var module65 = new EmailSettingsDialog(null);
        session.Load(module65, module => DialogHosts.EmailSettingsDialog = module);
        var module66 = new Pop3Dialog(null);
        session.Load(module66, module => DialogHosts.Pop3Dialog = module);
        var module67 = new SmtpDialog(null);
        session.Load(module67, module => DialogHosts.SmtpDialog = module);
        var module68 = new HelpWindow(null);
        session.Load(module68, module => DialogHosts.HelpWindow = module);
        var module69 = new CampaignJoinDialog(null);
        session.Load(module69, module => DialogHosts.CampaignJoinDialog = module);
        var module70 = new ItemsFoundDialog(null);
        session.Load(module70, module => ItemsFoundDialog = module);
        var module71 = new GamePlayDialog(null);
        session.Load(module71, module => DialogHosts.GamePlayDialog = module);
        var module72 = new PasswordDialog(null);
        session.Load(module72, module => PasswordDialog = module);
        var module73 = new InfoBanner(null);
        session.Load(module73, module => DialogHosts.InfoBanner = module);
        var module74 = new MessageDialog(null);
        session.Load(module74, module => DialogHosts.MessageDialog = module);
        var module75 = new TutorialWindow(null);
        session.Load(module75, module => TutorialWindow = module);
        var module76 = new CampaignSettingsDialog(null);
        session.Load(module76, module => DialogHosts.CampaignSettingsDialog = module);
        session.Complete();
    }

    public static TutorialWindow TutorialWindow { get; private set; }
    public static SendEmailGameDialog SendEmailGameDialog { get; private set; }
    public static PasswordDialog PasswordDialog { get; private set; }
    public static ArmyEventDialog ArmyEventDialog { get; private set; }
    public static CityRebelDialog CityRebelDialog { get; private set; }
    public static CityJoinDialog CityJoinDialog { get; private set; }
    public static PartyJoinDialog PartyJoinDialog { get; private set; }
    public static HeroJoinDialog HeroJoinDialog { get; private set; }
    public static MessageReceiveDialog MessageReceiveDialog { get; private set; }
    public static HeroUpgradeDialog HeroUpgradeDialog { get; private set; }
    public static HeroInfoDialog HeroInfoDialog { get; private set; }
    public static WizardsTowerDialog WizardsTowerDialog { get; private set; }
    public static ArenaDialog ArenaDialog { get; private set; }
    public static ItemsFoundDialog ItemsFoundDialog { get; private set; }
    public static FastBattleWindow FastBattleWindow { get; private set; }
    public static NewTurnDialog NewTurnDialog { get; private set; }
    public static CombatChoice TacticalBattleFastBattleDialog { get; private set; }
    public static ArmyInfoDialog ArmyInfoDialog { get; private set; }
}
