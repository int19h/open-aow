using Aow.Game.UI.Dialogs;

namespace Aow.Game.Layout;

// The startup order of the 77 data modules, as AoW.dpr created them.
public static class GameLayouts
{
    public static void Initialize(LayoutSession session, UIContext ui)
    {
        ui.Load(session, module: new UIGraphics());
        ui.Load(session, module: new UIFonts());
        ui.Load(session, module: new General(ui));
        ui.Load(session, module: new TargetSelectionDialog(ui));
        ui.Load(session, module: new SetupEvents(ui));
        ui.Load(session, module: new MapEvents(ui));
        ui.Load(session, module: new CombatMapEvents(ui));
        ui.Load(session, module: new TitleWindow(ui));
        ui.Load(session, module: new PopupWindows(ui));
        ui.Load(session, module: new TaskBar(ui));
        ui.Load(session, module: new ScannerWindow(ui));
        ui.Load(session, module: new MapWindow(ui));
        ui.Load(session, module: new InfoWindow(ui));
        ui.Load(session, module: new ConfirmationDialog(ui));
        ui.Load(session, module: new ControlWindow(ui));
        ui.Load(session, module: new UnitBanner(ui));
        ui.Load(session, module: new UnitWindow(ui));
        ui.Load(session, module: new ChatWindow(ui));
        ui.Load(session, module: new MultiWindow(ui));
        ui.Load(session, module: new FileDialogWindow(ui));
        ui.Load(session, module: new PlayerInfoWindow(ui));
        ui.Load(session, module: new ProgressDialog(ui));
        ui.Load(session, module: new SoundDialog(ui));
        ui.Load(session, module: new VideoDialog(ui));
        ui.Load(session, module: new FastBattleWindow(ui));
        ui.Load(session, module: new PyreScreen(ui));
        ui.Load(session, module: new NewTurnDialog(ui));
        ui.Load(session, module: new ArmyEventDialog(ui));
        ui.Load(session, module: new CityJoinDialog(ui));
        ui.Load(session, module: new ItemBanner(ui));
        ui.Load(session, module: new CityRebelDialog(ui));
        ui.Load(session, module: new FactionSelectionWindow(ui));
        ui.Load(session, module: new MessageReceiveDialog(ui));
        ui.Load(session, module: new HeroInfoDialog(ui));
        ui.Load(session, module: new PartyJoinDialog(ui));
        ui.Load(session, module: new ArmyInfoDialog(ui));
        ui.Load(session, module: new WorldMap(ui));
        ui.Load(session, module: new CombatUnitInfo(ui));
        ui.Load(session, module: new CombatScanner(ui));
        ui.Load(session, module: new ScenarioSetup(ui));
        ui.Load(session, module: new CombatChoice(ui));
        ui.Load(session, module: new RelationWindow(ui));
        ui.Load(session, module: new UnitSelectionDialog(ui));
        ui.Load(session, module: new ArenaDialog(ui));
        ui.Load(session, module: new ConstructDialog(ui));
        ui.Load(session, module: new MagicWindow(ui));
        ui.Load(session, module: new WizardsTowerDialog(ui));
        ui.Load(session, module: new PowerDialog(ui));
        ui.Load(session, module: new SpellBook(ui));
        ui.Load(session, module: new LeaderSetupWindow(ui));
        ui.Load(session, module: new DeformFaceDialog(ui));
        ui.Load(session, module: new HeroUpgradeDialog(ui));
        ui.Load(session, module: new EmailGameSettingsDialog(ui));
        ui.Load(session, module: new CastUnitSpellDialog(ui));
        ui.Load(session, module: new DisjunctionDialog(ui));
        ui.Load(session, module: new HeroJoinDialog(ui));
        ui.Load(session, module: new ResultsWindow(ui));
        ui.Load(session, module: new SynchronizeDialog(ui));
        ui.Load(session, module: new CityScreen(ui));
        ui.Load(session, module: new ProductionScreen(ui));
        ui.Load(session, module: new RealmWindow(ui));
        ui.Load(session, module: new CampaignTransferDialog(ui));
        ui.Load(session, module: new TitleScene(ui));
        // The intro scene is published after its layout ran (E2Globals.IntroScene).
        var intro = new IntroScene(ui);
        session.Load(intro, publish: static _ => { });
        ui.Add(intro);
        ui.Load(session, module: new SendEmailGameDialog(ui));
        ui.Load(session, module: new EmailSettingsDialog(ui));
        ui.Load(session, module: new Pop3Dialog(ui));
        ui.Load(session, module: new SmtpDialog(ui));
        ui.Load(session, module: new HelpWindow(ui));
        ui.Load(session, module: new CampaignJoinDialog(ui));
        ui.Load(session, module: new ItemsFoundDialog(ui));
        ui.Load(session, module: new GamePlayDialog(ui));
        ui.Load(session, module: new PasswordDialog(ui));
        ui.Load(session, module: new InfoBanner(ui));
        ui.Load(session, module: new MessageDialog(ui));
        ui.Load(session, module: new TutorialWindow(ui));
        ui.Load(session, module: new CampaignSettingsDialog(ui));
        session.Complete();
    }
}
