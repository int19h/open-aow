using Aow.Game.UI.Dialogs;

namespace Aow.Game.Layout;

// The startup order of the 77 data modules, as AoW.dpr created them.
public static class GameLayouts
{
    public static void Initialize(LayoutSession session, UIContext ui)
    {
        _ = ui.Load(session, module: new UIGraphics());
        _ = ui.Load(session, module: new UIFonts());
        _ = ui.Load(session, module: new General(ui));
        _ = ui.Load(session, module: new TargetSelectionDialog(ui));
        _ = ui.Load(session, module: new SetupEvents(ui));
        _ = ui.Load(session, module: new MapEvents(ui));
        _ = ui.Load(session, module: new CombatMapEvents(ui));
        _ = ui.Load(session, module: new TitleWindow(ui));
        _ = ui.Load(session, module: new PopupWindows(ui));
        _ = ui.Load(session, module: new TaskBar(ui));
        _ = ui.Load(session, module: new ScannerWindow(ui));
        _ = ui.Load(session, module: new MapWindow(ui));
        _ = ui.Load(session, module: new InfoWindow(ui));
        _ = ui.Load(session, module: new ConfirmationDialog(ui));
        _ = ui.Load(session, module: new ControlWindow(ui));
        _ = ui.Load(session, module: new UnitBanner(ui));
        _ = ui.Load(session, module: new UnitWindow(ui));
        _ = ui.Load(session, module: new ChatWindow(ui));
        _ = ui.Load(session, module: new MultiWindow(ui));
        _ = ui.Load(session, module: new FileDialogWindow(ui));
        _ = ui.Load(session, module: new PlayerInfoWindow(ui));
        _ = ui.Load(session, module: new ProgressDialog(ui));
        _ = ui.Load(session, module: new SoundDialog(ui));
        _ = ui.Load(session, module: new VideoDialog(ui));
        _ = ui.Load(session, module: new FastBattleWindow(ui));
        _ = ui.Load(session, module: new PyreScreen(ui));
        _ = ui.Load(session, module: new NewTurnDialog(ui));
        _ = ui.Load(session, module: new ArmyEventDialog(ui));
        _ = ui.Load(session, module: new CityJoinDialog(ui));
        _ = ui.Load(session, module: new ItemBanner(ui));
        _ = ui.Load(session, module: new CityRebelDialog(ui));
        _ = ui.Load(session, module: new FactionSelectionWindow(ui));
        _ = ui.Load(session, module: new MessageReceiveDialog(ui));
        _ = ui.Load(session, module: new HeroInfoDialog(ui));
        _ = ui.Load(session, module: new PartyJoinDialog(ui));
        _ = ui.Load(session, module: new ArmyInfoDialog(ui));
        _ = ui.Load(session, module: new WorldMap(ui));
        _ = ui.Load(session, module: new CombatUnitInfo(ui));
        _ = ui.Load(session, module: new CombatScanner(ui));
        _ = ui.Load(session, module: new ScenarioSetup(ui));
        _ = ui.Load(session, module: new CombatChoice(ui));
        _ = ui.Load(session, module: new RelationWindow(ui));
        _ = ui.Load(session, module: new UnitSelectionDialog(ui));
        _ = ui.Load(session, module: new ArenaDialog(ui));
        _ = ui.Load(session, module: new ConstructDialog(ui));
        _ = ui.Load(session, module: new MagicWindow(ui));
        _ = ui.Load(session, module: new WizardsTowerDialog(ui));
        _ = ui.Load(session, module: new PowerDialog(ui));
        _ = ui.Load(session, module: new SpellBook(ui));
        _ = ui.Load(session, module: new LeaderSetupWindow(ui));
        _ = ui.Load(session, module: new CosmagicSurgeryDialog(ui));
        _ = ui.Load(session, module: new HeroUpgradeDialog(ui));
        _ = ui.Load(session, module: new EmailGameSettingsDialog(ui));
        _ = ui.Load(session, module: new CastUnitSpellDialog(ui));
        _ = ui.Load(session, module: new DisjunctionDialog(ui));
        _ = ui.Load(session, module: new HeroJoinDialog(ui));
        _ = ui.Load(session, module: new ResultsWindow(ui));
        _ = ui.Load(session, module: new SynchronizeDialog(ui));
        _ = ui.Load(session, module: new CityScreen(ui));
        _ = ui.Load(session, module: new ProductionScreen(ui));
        _ = ui.Load(session, module: new RealmWindow(ui));
        _ = ui.Load(session, module: new CampaignTransferDialog(ui));
        _ = ui.Load(session, module: new TitleScene(ui));
        // The intro scene is published after its layout ran (E2Globals.IntroScene).
        IntroScene intro = new(ui);
        session.Load(intro, publish: static _ =>
        {
        });
        _ = ui.Add(intro);
        _ = ui.Load(session, module: new SendEmailGameDialog(ui));
        _ = ui.Load(session, module: new EmailSettingsDialog(ui));
        _ = ui.Load(session, module: new Pop3Dialog(ui));
        _ = ui.Load(session, module: new SmtpDialog(ui));
        _ = ui.Load(session, module: new HelpWindow(ui));
        _ = ui.Load(session, module: new CampaignJoinDialog(ui));
        _ = ui.Load(session, module: new ItemsFoundDialog(ui));
        _ = ui.Load(session, module: new GamePlayDialog(ui));
        _ = ui.Load(session, module: new PasswordDialog(ui));
        _ = ui.Load(session, module: new InfoBanner(ui));
        _ = ui.Load(session, module: new MessageDialog(ui));
        _ = ui.Load(session, module: new TutorialWindow(ui));
        _ = ui.Load(session, module: new CampaignSettingsDialog(ui));
        session.Complete();
    }
}
