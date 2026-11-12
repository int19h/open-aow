using Aow.Game.Layout;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// The game's interface: the interface graphics, the fonts and the dialog modules of the screens,
// one instance of each module type, in place of the Delphi form globals. The UI host creates the
// context after the game root is known, GameLayouts loads the modules into it in the startup
// order, and the host disposes it on shutdown. A module reads the others through the context it
// was created with (UIModule.UI).
//
// The context fills while the modules load: a module is published before its layout runs, as
// Application.CreateForm published the global, so the layout of a later module reads the earlier
// ones through the context. A context that loaded only some modules, as a test builds one, reads
// null for a module it does not hold.
//
// The context carries the game engine the host owns, so the modules reach it as UI.Engine.
public sealed class UIContext(GameEngine engine) : IDisposable
{
    private readonly Dictionary<Type, DataModule> _modules = [];
    private bool _disposed;

    public GameEngine Engine { get; } = engine;

    public UIGraphics Graphics => Module<UIGraphics>();
    public UIFonts Fonts => Module<UIFonts>();
    public General General => Module<General>();
    public TargetSelectionDialog TargetSelectionDialog => Module<TargetSelectionDialog>();
    public SetupEvents SetupEvents => Module<SetupEvents>();
    public MapEvents MapEvents => Module<MapEvents>();
    public CombatMapEvents CombatMapEvents => Module<CombatMapEvents>();
    public TitleWindow TitleWindow => Module<TitleWindow>();
    public PopupWindows PopupWindows => Module<PopupWindows>();
    public TaskBar TaskBar => Module<TaskBar>();
    public ScannerWindow ScannerWindow => Module<ScannerWindow>();
    public MapWindow MapWindow => Module<MapWindow>();
    public InfoWindow InfoWindow => Module<InfoWindow>();
    public ConfirmationDialog ConfirmationDialog => Module<ConfirmationDialog>();
    public ControlWindow ControlWindow => Module<ControlWindow>();
    public UnitBanner UnitBanner => Module<UnitBanner>();
    public UnitWindow UnitWindow => Module<UnitWindow>();
    public ChatWindow ChatWindow => Module<ChatWindow>();
    public MultiWindow MultiWindow => Module<MultiWindow>();
    public FileDialogWindow FileDialogWindow => Module<FileDialogWindow>();
    public PlayerInfoWindow PlayerInfoWindow => Module<PlayerInfoWindow>();
    public ProgressDialog ProgressDialog => Module<ProgressDialog>();
    public SoundDialog SoundDialog => Module<SoundDialog>();
    public VideoDialog VideoDialog => Module<VideoDialog>();
    public FastBattleWindow FastBattleWindow => Module<FastBattleWindow>();
    public PyreScreen PyreScreen => Module<PyreScreen>();
    public NewTurnDialog NewTurnDialog => Module<NewTurnDialog>();
    public ArmyEventDialog ArmyEventDialog => Module<ArmyEventDialog>();
    public CityJoinDialog CityJoinDialog => Module<CityJoinDialog>();
    public ItemBanner ItemBanner => Module<ItemBanner>();
    public CityRebelDialog CityRebelDialog => Module<CityRebelDialog>();
    public FactionSelectionWindow FactionSelectionWindow => Module<FactionSelectionWindow>();
    public MessageReceiveDialog MessageReceiveDialog => Module<MessageReceiveDialog>();
    public HeroInfoDialog HeroInfoDialog => Module<HeroInfoDialog>();
    public PartyJoinDialog PartyJoinDialog => Module<PartyJoinDialog>();
    public ArmyInfoDialog ArmyInfoDialog => Module<ArmyInfoDialog>();
    public WorldMap WorldMap => Module<WorldMap>();
    public CombatUnitInfo CombatUnitInfo => Module<CombatUnitInfo>();
    public CombatScanner CombatScanner => Module<CombatScanner>();
    public ScenarioSetup ScenarioSetup => Module<ScenarioSetup>();
    public CombatChoice CombatChoice => Module<CombatChoice>();
    public RelationWindow RelationWindow => Module<RelationWindow>();
    public UnitSelectionDialog UnitSelectionDialog => Module<UnitSelectionDialog>();
    public ArenaDialog ArenaDialog => Module<ArenaDialog>();
    public ConstructDialog ConstructDialog => Module<ConstructDialog>();
    public MagicWindow MagicWindow => Module<MagicWindow>();
    public WizardsTowerDialog WizardsTowerDialog => Module<WizardsTowerDialog>();
    public PowerDialog PowerDialog => Module<PowerDialog>();
    public SpellBook SpellBook => Module<SpellBook>();
    public LeaderSetupWindow LeaderSetupWindow => Module<LeaderSetupWindow>();
    public DeformFaceDialog DeformFaceDialog => Module<DeformFaceDialog>();
    public HeroUpgradeDialog HeroUpgradeDialog => Module<HeroUpgradeDialog>();
    public EmailGameSettingsDialog EmailGameSettingsDialog => Module<EmailGameSettingsDialog>();
    public CastUnitSpellDialog CastUnitSpellDialog => Module<CastUnitSpellDialog>();
    public DisjunctionDialog DisjunctionDialog => Module<DisjunctionDialog>();
    public HeroJoinDialog HeroJoinDialog => Module<HeroJoinDialog>();
    public ResultsWindow ResultsWindow => Module<ResultsWindow>();
    public SynchronizeDialog SynchronizeDialog => Module<SynchronizeDialog>();
    public CityScreen CityScreen => Module<CityScreen>();
    public ProductionScreen ProductionScreen => Module<ProductionScreen>();
    public RealmWindow RealmWindow => Module<RealmWindow>();
    public CampaignTransferDialog CampaignTransferDialog => Module<CampaignTransferDialog>();
    public TitleScene TitleScene => Module<TitleScene>();
    public IntroScene IntroScene => Module<IntroScene>();
    public SendEmailGameDialog SendEmailGameDialog => Module<SendEmailGameDialog>();
    public EmailSettingsDialog EmailSettingsDialog => Module<EmailSettingsDialog>();
    public Pop3Dialog Pop3Dialog => Module<Pop3Dialog>();
    public SmtpDialog SmtpDialog => Module<SmtpDialog>();
    public HelpWindow HelpWindow => Module<HelpWindow>();
    public CampaignJoinDialog CampaignJoinDialog => Module<CampaignJoinDialog>();
    public ItemsFoundDialog ItemsFoundDialog => Module<ItemsFoundDialog>();
    public GamePlayDialog GamePlayDialog => Module<GamePlayDialog>();
    public PasswordDialog PasswordDialog => Module<PasswordDialog>();
    public InfoBanner InfoBanner => Module<InfoBanner>();
    public MessageDialog MessageDialog => Module<MessageDialog>();
    public TutorialWindow TutorialWindow => Module<TutorialWindow>();
    public CampaignSettingsDialog CampaignSettingsDialog => Module<CampaignSettingsDialog>();

    // The debug message window lives in the confirmation dialog's form.
    public DebugWindow DebugWindow => ConfirmationDialog.Debug;

    // Loads a module through the session: the module is published, its layout runs, and a layout
    // that raises removes the module again and destroys it (LayoutSession.Load).
    public T Load<T>(LayoutSession session, T module) where T : DataModule, ILayoutModule
    {
        session.Load(module, publish: published =>
        {
            if (published is null) _modules.Remove(typeof(T));
            else _modules[typeof(T)] = published;
        });
        return module;
    }

    // Publishes a module that is already built, such as a module the startup publishes only after
    // its layout ran, or a test module without a layout.
    public T Add<T>(T module) where T : DataModule
    {
        _modules[typeof(T)] = module;
        return module;
    }

    // The interface graphics and the fonts own the image libraries and are disposed here, the
    // fonts first, in the reverse order of loading. The dialog modules are not disposed: their
    // destroy handlers have never run at shutdown in the port, and running them is a change of its own.
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        try { Fonts?.Dispose(); }
        finally { Graphics?.Dispose(); }
    }

    private T Module<T>() where T : DataModule =>
        _modules.TryGetValue(typeof(T), out var module) ? (T)module : null;
}
