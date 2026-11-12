// Translated from MapCtrl.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.UI.Grids;

public partial class MapControl : Component
{
    private MapObjectEvent<Army> _onSelectArmy;
    private MapObjectEvent<Army> _onDeselectArmy;
    private MapObjectEvent<Army> _onDoubleClickArmy;
    private MapObjectEvent<Army> _onArmyChanged;
    private MapObjectEvent<Army> _onArmyDestroyed;
    private MapObjectEvent<Structure> _onSelectStructure;
    private MapObjectEvent<Structure> _onDeselectStructure;
    private MapObjectEvent<Structure> _onDoubleClickStructure;
    private MapObjectEvent<Structure> _onStructureChanged;
    private MapObjectEvent<Structure> _onStructureDestroyed;
    private MapObjectEvent<ItemHexSprite> _onSelectItem;
    private MapObjectEvent<ItemHexSprite> _onDeselectItem;
    private MapObjectEvent<Troop> _onUnitChanged;
    private MapObjectEvent<Troop> _onUnitDestroyed;
    private MapObjectEvent<AbilityActivation> _onActivateAbility;
    private NotifyEvent<object> _onDiplomaticChange;
    private NotifyEvent<object> _onActivateHsm;
    private NotifyEvent<object> _onDeactivateHsm;
    private MapPlayerEvent _onIncomeChanged;
    private MapPlayerEvent _onUpkeepChanged;
    private MapPlayerEvent _onGemsChanged;
    private MapPlayerEvent _onPlayerMagicChanged;
    private NotifyEvent<object> _onGlobalMagicChanged;
    private MapRefEvent<ShowMessageInfo> _onShowMessage;
    private MapDayEvent _onChangeDay;
    private NotifyEvent<sbyte> _onNewTurn;
    private MapPlayerEvent _onPlayerChanged;
    private MapPlayerEvent _onSeatedPlayerChanged;
    private NotifyEvent<object> _onCombatEvent;
    private MapExecuteLogEvent _onExecuteEventLog;
    private MapRefEvent<MapChangedInfo> _onMapChanged;
    private MapRefEvent<MapChangedInfo> _onScannerIconsChanged;
    private NotifyEvent<object> _onMapSettingsChanged;
    private MapPlayerEvent _onLogbookChanged;
    private NotifyEvent<object> _onGameOver;
    private MapPlayerEvent _onGameTerminated;
    private MapPlayerEvent _onPlayerTerminated;
    private NotifyEvent<object> _onActivateCombatMap;
    private NotifyEvent<object> _onDeactivateCombatMap;
    private MapTargetEvent _onTargetSelectionChange;
    private NotifyEvent<object> _onViewPortChange;
    private MapObjectEvent<SpellCaster> _onCastSpell;
    private MapEndTurnEvent _onEndTurn;
    private NotifyEvent<object> _onRelationsChanged;
    private NotifyEvent<object> _onExitGameEvent;
    private MapObjectEvent<HexSprite> _onHexSpriteChanged;

    // MapCtrl.pas:186
    public override void Dispose()
    {
        if (GameTables.GameEngine != null)
        {
            GameTables.GameEngine.FOnHsmLoaded -= ActivateHsm;
            GameTables.GameEngine.FOnHsmClose -= DeactivateHsm;
            if (StrategicMap.Current != null)
            {
                ReleaseHsm();
            }
        }

        base.Dispose();
    }

    // MapCtrl.pas:174
    public MapControl(Component owner) : base(owner)
    {
        if (GameTables.ValidateGameEngine())
        {
            GameTables.GameEngine.FOnHsmLoaded += ActivateHsm;
            GameTables.GameEngine.FOnHsmClose += DeactivateHsm;
            if (StrategicMap.Current != null)
            {
                LinkToHsm();
            }
        }
    }

    // MapCtrl.pas:197
    public void LinkToHsm()
    {
        StrategicMap.Current.Mouse.FOnDoubleClick += DoubleClick;
        StrategicMap.Current.FNewDayEvents += ChangeDay;
        StrategicMap.Current.FCurrentPlayerChangedEvents += PlayerChanged;
        StrategicMap.Current.FNewTurnEvents += NewTurn;
        StrategicMap.Current.FEndTurnEvents += EndTurn;
        StrategicMap.Current.FSeatedPlayerChangedEvents += SeatedPlayerChanged;
        StrategicMap.Current.Selection.FOnSelect += SelectHexSprite;
        StrategicMap.Current.Selection.FOnDeselect += DeselectHexSprite;
        StrategicMap.Current.FShowMessageEvents += ShowMessage;
        StrategicMap.Current.FMapChangedEvents += MapChanged;
        StrategicMap.Current.FScannerIconsChangedEvents += ScannerIconsChanged;
        StrategicMap.Current.FUpdateVisibilityEvents += MapSettingsChanged;
        StrategicMap.Current.FCombatEvents += CombatEvent;
        StrategicMap.Current.FStructureChangedEvents += StructureChanged;
        StrategicMap.Current.FStructureDestroyedEvents += StructureDestroyed;
        StrategicMap.Current.FArmyChangedEvents += ArmyChanged;
        StrategicMap.Current.FArmyDestroyedEvents += ArmyDestroyed;
        StrategicMap.Current.FUnitChangedEvents += UnitChanged;
        StrategicMap.Current.FUnitDestroyedEvents += UnitDestroyed;
        StrategicMap.Current.FGameTerminatedEvents += GameOver;
        StrategicMap.Current.FGameOverEvents += GameTerminated;
        StrategicMap.Current._hexSpriteChangedEvents += HexSpriteChanged;
        StrategicMap.Current.FPlayerTerminatedEvents += PlayerTerminated;
        StrategicMap.Current.FExecuteEventLogEvents += ExecuteEventLog;
        StrategicMap.Current.TargetSelection.FOnChange += TargetSelectionChange;
        StrategicMap.Current.FViewPortChangedEvents += ViewPortChange;
        StrategicMap.Current.FCosmeticSurgeryEvents += CastSpell;
        StrategicMap.Current.FOnActivateAbility += ActivateAbility;
        StrategicMap.Current.GlobalMagicControl.FChangedEvents += GlobalMagicChanged;
        StrategicMap.Current.FPlayerChangedEvents += RelationsChanged;
        StrategicMap.Current.FDiplomaticRelationsChangedEvents += DiplomaticChange;
        StrategicMap.Current.FExitEvents += ExitGameEvent;
        StrategicMap.Current.FCombatMapCreatedEvents += ActivateCombatMap;
        StrategicMap.Current.FCombatMapDestroyedEvents += DeactivateCombatMap;
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).FGemsChangedEvents += GemsChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).FIncomeChangedEvents += IncomeChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).FUpkeepChangedEvents += UpkeepChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).FLogbookChangedEvents += LogbookChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).MagicControl.FChangedEvents += PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:244
    public void ReleaseHsm()
    {
        StrategicMap.Current.Mouse.FOnDoubleClick -= DoubleClick;
        StrategicMap.Current.FNewDayEvents -= ChangeDay;
        StrategicMap.Current.FCurrentPlayerChangedEvents -= PlayerChanged;
        StrategicMap.Current.FNewTurnEvents -= NewTurn;
        StrategicMap.Current.FEndTurnEvents -= EndTurn;
        StrategicMap.Current.FSeatedPlayerChangedEvents -= SeatedPlayerChanged;
        StrategicMap.Current.Selection.FOnSelect -= SelectHexSprite;
        StrategicMap.Current.Selection.FOnDeselect -= DeselectHexSprite;
        StrategicMap.Current.FShowMessageEvents -= ShowMessage;
        StrategicMap.Current.FMapChangedEvents -= MapChanged;
        StrategicMap.Current.FScannerIconsChangedEvents -= ScannerIconsChanged;
        StrategicMap.Current.FUpdateVisibilityEvents -= MapSettingsChanged;
        StrategicMap.Current.FCombatEvents -= CombatEvent;
        StrategicMap.Current.FArmyChangedEvents -= ArmyChanged;
        StrategicMap.Current.FArmyDestroyedEvents -= ArmyDestroyed;
        StrategicMap.Current.FUnitChangedEvents -= UnitChanged;
        StrategicMap.Current.FUnitDestroyedEvents -= UnitDestroyed;
        StrategicMap.Current.FStructureChangedEvents -= StructureChanged;
        StrategicMap.Current.FStructureDestroyedEvents -= StructureDestroyed;
        StrategicMap.Current._hexSpriteChangedEvents -= HexSpriteChanged;
        StrategicMap.Current.FGameTerminatedEvents -= GameOver;
        StrategicMap.Current.FGameOverEvents -= GameTerminated;
        StrategicMap.Current.FPlayerTerminatedEvents -= PlayerTerminated;
        StrategicMap.Current.FExecuteEventLogEvents -= ExecuteEventLog;
        StrategicMap.Current.TargetSelection.FOnChange -= TargetSelectionChange;
        StrategicMap.Current.FViewPortChangedEvents -= ViewPortChange;
        StrategicMap.Current.FCosmeticSurgeryEvents -= CastSpell;
        StrategicMap.Current.FOnActivateAbility -= ActivateAbility;
        StrategicMap.Current.GlobalMagicControl.FChangedEvents -= GlobalMagicChanged;
        StrategicMap.Current.FPlayerChangedEvents -= RelationsChanged;
        StrategicMap.Current.FDiplomaticRelationsChangedEvents -= DiplomaticChange;
        StrategicMap.Current.FExitEvents -= ExitGameEvent;
        StrategicMap.Current.FCombatMapCreatedEvents -= ActivateCombatMap;
        StrategicMap.Current.FCombatMapDestroyedEvents -= DeactivateCombatMap;
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).FGemsChangedEvents -= GemsChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).FIncomeChangedEvents -= IncomeChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).FUpkeepChangedEvents -= UpkeepChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).FLogbookChangedEvents -= LogbookChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).MagicControl.FChangedEvents -= PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:291
    public void ActivateHsm(GameMap map)
    {
        if (map.EngineClass.ClassId == 131089)
        {
            LinkToHsm();
            if (_onActivateHsm != null)
            {
                _onActivateHsm(this);
            }
        }
    }

    // MapCtrl.pas:300
    public void DeactivateHsm(GameMap map)
    {
        if (map.EngineClass.ClassId == 131089)
        {
            if (_onDeactivateHsm != null)
            {
                _onDeactivateHsm(this);
            }

            ReleaseHsm();
        }
    }

    // MapCtrl.pas:309
    public void DoubleClick(object sender)
    {
        HexSprite hexSprite;
        if (_onDoubleClickArmy != null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(ArmyHexSprite));
            if (hexSprite is ArmyHexSprite armyHexSprite)
            {
                _onDoubleClickArmy(armyHexSprite.Army);
            }
        }

        if (_onDoubleClickStructure != null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(Structure));
            if (hexSprite is Structure structure)
            {
                _onDoubleClickStructure(structure);
            }
        }
    }

    // MapCtrl.pas:350
    public void MapChanged(MapChangedNotification sender)
    {
        try
        {
            if (_onMapChanged != null)
            {
                _onMapChanged(ref sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:359
    public void ScannerIconsChanged(MapChangedNotification sender)
    {
        try
        {
            if (_onScannerIconsChanged != null)
            {
                _onScannerIconsChanged(ref sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ScannerIconsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:368
    public void MapSettingsChanged(object sender)
    {
        try
        {
            if (_onMapSettingsChanged != null)
            {
                _onMapSettingsChanged(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapSettingsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:324
    public void SelectHexSprite(object sender)
    {
        HexSpriteSelection selection;
        try
        {
            selection = StrategicMap.Current.Selection;
            if (_onSelectArmy != null && selection.EventHexSprite is ArmyHexSprite armyHexSprite && StrategicMap.Current.SelectedArmy.GetArmyHexSprite() != null)
            {
                _onSelectArmy(armyHexSprite.Army);
            }

            if (_onSelectStructure != null && selection.EventHexSprite is Structure structure)
            {
                _onSelectStructure(structure);
            }

            if (_onSelectItem != null && selection.EventHexSprite is ItemHexSprite item)
            {
                _onSelectItem(item);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during SelectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:337
    public void DeselectHexSprite(object sender)
    {
        HexSpriteSelection selection;
        try
        {
            selection = StrategicMap.Current.Selection;
            if (_onDeselectArmy != null && selection.EventHexSprite is ArmyHexSprite armyHexSprite)
            {
                _onDeselectArmy(armyHexSprite.Army);
            }

            if (_onDeselectStructure != null && selection.EventHexSprite is Structure structure)
            {
                _onDeselectStructure(structure);
            }

            if (_onDeselectItem != null && selection.EventHexSprite is ItemHexSprite item)
            {
                _onDeselectItem(item);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during DeselectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:377
    public void PlayerChanged(object sender)
    {
        try
        {
            if (_onPlayerChanged != null)
            {
                _onPlayerChanged(StrategicMap.Current.CurrentPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:386
    public void ExitGameEvent(object sender)
    {
        try
        {
            if (_onExitGameEvent != null)
            {
                _onExitGameEvent(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ExitGame event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:395
    public void NewTurn(sbyte player)
    {
        try
        {
            if (_onNewTurn != null)
            {
                _onNewTurn(player);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during NewTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:404
    public void EndTurn(sbyte player)
    {
        try
        {
            if (_onEndTurn != null)
            {
                var listed = (object)StrategicMap.Current.Players.GetPlayers(player);
                _onEndTurn(player, !((Player)(object)listed).SingleProcessCycle);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during EndTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:413
    public void SeatedPlayerChanged(object sender)
    {
        try
        {
            if (_onSeatedPlayerChanged != null)
            {
                _onSeatedPlayerChanged(StrategicMap.Current.SeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:422
    public void GemsChanged(Player player)
    {
        try
        {
            if (_onGemsChanged != null)
            {
                _onGemsChanged(player.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Gems Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:431
    public void IncomeChanged(Player player)
    {
        try
        {
            if (_onIncomeChanged != null)
            {
                _onIncomeChanged(player.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Income Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:440
    public void UpkeepChanged(Player player)
    {
        try
        {
            if (_onUpkeepChanged != null)
            {
                _onUpkeepChanged(player.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Upkeep Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:449
    public void PlayerMagicChanged(PlayerMagicControl control)
    {
        try
        {
            if (_onPlayerMagicChanged != null)
            {
                _onPlayerMagicChanged(control.Player.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Player Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:458
    public void GlobalMagicChanged(object sender)
    {
        try
        {
            if (_onGlobalMagicChanged != null)
            {
                _onGlobalMagicChanged(sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:467
    public void ShowMessage(ShowMessageNotification sender)
    {
        try
        {
            if (_onShowMessage != null)
            {
                _onShowMessage(ref sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ShowMessage event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:476
    public void ChangeDay(object sender)
    {
        try
        {
            if (_onChangeDay != null)
            {
                _onChangeDay(StrategicMap.Current.Day);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ChangeDay event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:485
    public void CombatEvent(object sender)
    {
        try
        {
            if (_onCombatEvent != null)
            {
                _onCombatEvent(sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during on combat event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:494
    public void LogbookChanged(Player player)
    {
        try
        {
            if (_onLogbookChanged != null)
            {
                _onLogbookChanged(player.PlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during LogbookChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:503
    public void StructureChanged(Structure objectValue)
    {
        try
        {
            if (_onStructureChanged != null)
            {
                _onStructureChanged(objectValue);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnStructureChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:512
    public void StructureDestroyed(Structure objectValue)
    {
        try
        {
            if (_onStructureDestroyed != null)
            {
                _onStructureDestroyed(objectValue);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnStructureDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:521
    public void ArmyChanged(Army objectValue)
    {
        try
        {
            if (_onArmyChanged != null)
            {
                _onArmyChanged(objectValue);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnArmyChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:530
    public void ArmyDestroyed(Army objectValue)
    {
        try
        {
            if (_onArmyDestroyed != null)
            {
                _onArmyDestroyed(objectValue);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnArmyDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:539
    public void HexSpriteChanged(HexSprite sender)
    {
        try
        {
            if (_onHexSpriteChanged != null)
            {
                _onHexSpriteChanged(sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnHSChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:548
    public void UnitChanged(Troop objectValue)
    {
        try
        {
            if (_onUnitChanged != null)
            {
                _onUnitChanged(objectValue);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnUnitChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:557
    public void UnitDestroyed(Troop objectValue)
    {
        try
        {
            if (_onUnitDestroyed != null)
            {
                _onUnitDestroyed(objectValue);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnUnitDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:566
    public void GameOver(object sender)
    {
        try
        {
            if (_onGameOver != null)
            {
                _onGameOver(sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnGameOver event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:575
    public void GameTerminated(object sender)
    {
        try
        {
            if (_onGameTerminated != null)
            {
                _onGameTerminated((sbyte)StrategicMap.Current.GameResult);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnGameTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:584
    public void PlayerTerminated(sbyte player)
    {
        try
        {
            if (_onPlayerTerminated != null)
            {
                _onPlayerTerminated(player);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnPlayerTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:593
    public void ExecuteEventLog(ExecuteEventLogNotification info)
    {
        try
        {
            if (_onExecuteEventLog != null)
            {
                _onExecuteEventLog(info.Info.Log, info.Info.Immediate, info.Info.Done);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:603
    public void ActivateCombatMap(object sender)
    {
        try
        {
            if (_onActivateCombatMap != null)
            {
                _onActivateCombatMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:612
    public void DeactivateCombatMap(object sender)
    {
        try
        {
            if (_onDeactivateCombatMap != null)
            {
                _onDeactivateCombatMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDeactivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:621
    public void TargetSelectionChange(TargetSelectionControl control)
    {
        try
        {
            if (_onTargetSelectionChange != null && control != null && (((int)StrategicMap.Current.TargetSelection.TargetMode.Image) & 8) != 0)
            {
                _onTargetSelectionChange(control, control.Change);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnTargetSelectionChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:630
    public void CastSpell(SpellCaster sender)
    {
        try
        {
            if (_onCastSpell != null)
            {
                _onCastSpell(sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnCastSpell event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:639
    public void ViewPortChange(object sender)
    {
        try
        {
            if (_onViewPortChange != null)
            {
                _onViewPortChange(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnViewPortChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:648
    public void ActivateAbility(AbilityActivation sender)
    {
        try
        {
            if (_onActivateAbility != null)
            {
                _onActivateAbility(sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateAbility event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:657
    public void DiplomaticChange(object sender)
    {
        try
        {
            if (_onDiplomaticChange != null)
            {
                _onDiplomaticChange(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDiplomaticChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:666
    public void RelationsChanged(object sender)
    {
        try
        {
            if (_onRelationsChanged != null)
            {
                _onRelationsChanged(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnRelationsChanged event (TAoWMapControl)");
        }
    }

    [Published]
    public MapObjectEvent<Army> OnArmySelected { get => _onSelectArmy; set => _onSelectArmy = value; }

    [Published]
    public MapObjectEvent<Army> OnArmyDeselected { get => _onDeselectArmy; set => _onDeselectArmy = value; }

    [Published]
    public MapObjectEvent<Army> OnArmyDoubleClick { get => _onDoubleClickArmy; set => _onDoubleClickArmy = value; }

    [Published]
    public MapObjectEvent<Army> OnArmyChanged { get => _onArmyChanged; set => _onArmyChanged = value; }

    [Published]
    public MapObjectEvent<Army> OnArmyDestroyed { get => _onArmyDestroyed; set => _onArmyDestroyed = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureSelected { get => _onSelectStructure; set => _onSelectStructure = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureDeselected { get => _onDeselectStructure; set => _onDeselectStructure = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureDoubleClick { get => _onDoubleClickStructure; set => _onDoubleClickStructure = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureChanged { get => _onStructureChanged; set => _onStructureChanged = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureDestroyed { get => _onStructureDestroyed; set => _onStructureDestroyed = value; }

    [Published]
    public MapObjectEvent<Troop> OnUnitChanged { get => _onUnitChanged; set => _onUnitChanged = value; }

    [Published]
    public MapObjectEvent<Troop> OnUnitDestroyed { get => _onUnitDestroyed; set => _onUnitDestroyed = value; }

    [Published]
    public NotifyEvent<object> OnActivateHsm { get => _onActivateHsm; set => _onActivateHsm = value; }

    [Published]
    public NotifyEvent<object> OnDeactivateHsm { get => _onDeactivateHsm; set => _onDeactivateHsm = value; }

    [Published]
    public MapPlayerEvent OnGemsChanged { get => _onGemsChanged; set => _onGemsChanged = value; }

    [Published]
    public MapPlayerEvent OnIncomeChanged { get => _onIncomeChanged; set => _onIncomeChanged = value; }

    [Published]
    public MapPlayerEvent OnUpkeepChanged { get => _onUpkeepChanged; set => _onUpkeepChanged = value; }

    [Published]
    public MapRefEvent<ShowMessageInfo> OnShowMessage { get => _onShowMessage; set => _onShowMessage = value; }

    [Published]
    public MapDayEvent OnDayChanged { get => _onChangeDay; set => _onChangeDay = value; }

    [Published]
    public MapPlayerEvent OnPlayerChanged { get => _onPlayerChanged; set => _onPlayerChanged = value; }

    [Published]
    public NotifyEvent<sbyte> OnNewTurn { get => _onNewTurn; set => _onNewTurn = value; }

    [Published]
    public MapEndTurnEvent OnEndTurn { get => _onEndTurn; set => _onEndTurn = value; }

    [Published]
    public MapObjectEvent<HexSprite> OnHexSpriteChanged { get => _onHexSpriteChanged; set => _onHexSpriteChanged = value; }

    [Published]
    public MapPlayerEvent OnSeatedPlayerChanged { get => _onSeatedPlayerChanged; set => _onSeatedPlayerChanged = value; }

    [Published]
    public MapRefEvent<MapChangedInfo> OnMapChanged { get => _onMapChanged; set => _onMapChanged = value; }

    [Published]
    public MapRefEvent<MapChangedInfo> OnScannerIconsChanged { get => _onScannerIconsChanged; set => _onScannerIconsChanged = value; }

    [Published]
    public NotifyEvent<object> OnMapSettingsChanged { get => _onMapSettingsChanged; set => _onMapSettingsChanged = value; }

    [Published]
    public NotifyEvent<object> OnCombat { get => _onCombatEvent; set => _onCombatEvent = value; }

    [Published]
    public MapPlayerEvent OnLogbookChanged { get => _onLogbookChanged; set => _onLogbookChanged = value; }

    [Published]
    public MapTargetEvent OnTargetSelectionChange { get => _onTargetSelectionChange; set => _onTargetSelectionChange = value; }

    [Published]
    public MapObjectEvent<SpellCaster> OnCastSpell { get => _onCastSpell; set => _onCastSpell = value; }

    [Published]
    public MapObjectEvent<AbilityActivation> OnActivateAbility { get => _onActivateAbility; set => _onActivateAbility = value; }

    [Published]
    public MapPlayerEvent OnPlayerMagicChanged { get => _onPlayerMagicChanged; set => _onPlayerMagicChanged = value; }

    [Published]
    public NotifyEvent<object> OnGlobalMagicChanged { get => _onGlobalMagicChanged; set => _onGlobalMagicChanged = value; }

    [Published]
    public NotifyEvent<object> OnRelationsChanged { get => _onRelationsChanged; set => _onRelationsChanged = value; }

    [Published]
    public NotifyEvent<object> OnActivateCombatMap { get => _onActivateCombatMap; set => _onActivateCombatMap = value; }

    [Published]
    public NotifyEvent<object> OnDeactivateCombatMap { get => _onDeactivateCombatMap; set => _onDeactivateCombatMap = value; }

    [Published]
    public MapObjectEvent<ItemHexSprite> OnItemsSelected { get => _onSelectItem; set => _onSelectItem = value; }

    [Published]
    public MapObjectEvent<ItemHexSprite> OnItemsDeselected { get => _onDeselectItem; set => _onDeselectItem = value; }

    [Published]
    public NotifyEvent<object> OnGameOver { get => _onGameOver; set => _onGameOver = value; }

    [Published]
    public MapPlayerEvent OnGameTerminated { get => _onGameTerminated; set => _onGameTerminated = value; }

    [Published]
    public MapPlayerEvent OnPlayerTerminated { get => _onPlayerTerminated; set => _onPlayerTerminated = value; }

    [Published]
    public MapExecuteLogEvent OnExecuteEventLog { get => _onExecuteEventLog; set => _onExecuteEventLog = value; }

    [Published]
    public NotifyEvent<object> OnViewPortChange { get => _onViewPortChange; set => _onViewPortChange = value; }

    [Published]
    public NotifyEvent<object> OnDiplomaticChange { get => _onDiplomaticChange; set => _onDiplomaticChange = value; }

    [Published]
    public NotifyEvent<object> OnExitGame { get => _onExitGameEvent; set => _onExitGameEvent = value; }

    public delegate void MapPlayerEvent(sbyte Player);
    public delegate void MapDayEvent(int Day);
    public delegate void MapObjectEvent<in TSender>(TSender Obj);
    public delegate void MapRefEvent<TInfo>(ref TInfo Info);
    public delegate void MapEndTurnEvent(sbyte Player, bool Automatic);
    public delegate void MapExecuteLogEvent(EventLog Log, bool Immediate, ExecuteDoneEvent Done);
    public delegate void MapTargetEvent(TargetSelectionControl Control, TargetSelectionChange Change);
}
