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
    [Published]
    public event MapObjectEvent<Army> OnArmySelected;
    [Published]
    public event MapObjectEvent<Army> OnArmyDeselected;
    [Published]
    public event MapObjectEvent<Army> OnArmyDoubleClick;
    [Published]
    public event MapObjectEvent<Army> OnArmyChanged;
    [Published]
    public event MapObjectEvent<Army> OnArmyDestroyed;
    [Published]
    public event MapObjectEvent<Structure> OnStructureSelected;
    [Published]
    public event MapObjectEvent<Structure> OnStructureDeselected;
    [Published]
    public event MapObjectEvent<Structure> OnStructureDoubleClick;
    [Published]
    public event MapObjectEvent<Structure> OnStructureChanged;
    [Published]
    public event MapObjectEvent<Structure> OnStructureDestroyed;
    [Published]
    public event MapObjectEvent<ItemHexSprite> OnItemsSelected;
    [Published]
    public event MapObjectEvent<ItemHexSprite> OnItemsDeselected;
    [Published]
    public event MapObjectEvent<Troop> OnUnitChanged;
    [Published]
    public event MapObjectEvent<Troop> OnUnitDestroyed;
    [Published]
    public event MapObjectEvent<AbilityActivation> OnActivateAbility;
    [Published]
    public event NotifyEvent<object> OnDiplomaticChange;
    [Published]
    public event NotifyEvent<object> OnActivateHsm;
    [Published]
    public event NotifyEvent<object> OnDeactivateHsm;
    [Published]
    public event MapPlayerEvent OnIncomeChanged;
    [Published]
    public event MapPlayerEvent OnUpkeepChanged;
    [Published]
    public event MapPlayerEvent OnGemsChanged;
    [Published]
    public event MapPlayerEvent OnPlayerMagicChanged;
    [Published]
    public event NotifyEvent<object> OnGlobalMagicChanged;
    [Published]
    public event MapRefEvent<ShowMessageInfo> OnShowMessage;
    [Published]
    public event MapDayEvent OnDayChanged;
    [Published]
    public event NotifyEvent<sbyte> OnNewTurn;
    [Published]
    public event MapPlayerEvent OnPlayerChanged;
    [Published]
    public event MapPlayerEvent OnSeatedPlayerChanged;
    [Published]
    public event NotifyEvent<object> OnCombat;
    [Published]
    public event MapExecuteLogEvent OnExecuteEventLog;
    [Published]
    public event MapRefEvent<MapChangedInfo> OnMapChanged;
    [Published]
    public event MapRefEvent<MapChangedInfo> OnScannerIconsChanged;
    [Published]
    public event NotifyEvent<object> OnMapSettingsChanged;
    [Published]
    public event MapPlayerEvent OnLogbookChanged;
    [Published]
    public event NotifyEvent<object> OnGameOver;
    [Published]
    public event MapPlayerEvent OnGameTerminated;
    [Published]
    public event MapPlayerEvent OnPlayerTerminated;
    [Published]
    public event NotifyEvent<object> OnActivateCombatMap;
    [Published]
    public event NotifyEvent<object> OnDeactivateCombatMap;
    [Published]
    public event MapTargetEvent OnTargetSelectionChange;
    [Published]
    public event NotifyEvent<object> OnViewPortChange;
    [Published]
    public event MapObjectEvent<SpellCaster> OnCastSpell;
    [Published]
    public event MapEndTurnEvent OnEndTurn;
    [Published]
    public event NotifyEvent<object> OnRelationsChanged;
    [Published]
    public event NotifyEvent<object> OnExitGame;
    [Published]
    public event MapObjectEvent<HexSprite> OnHexSpriteChanged;

    // MapCtrl.pas:186
    public override void Dispose()
    {
        if (GameTables.GameEngine != null)
        {
            GameTables.GameEngine.HexSpriteMapLoaded -= ActivateHsm;
            GameTables.GameEngine.HexSpriteMapClosing -= DeactivateHsm;
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
            GameTables.GameEngine.HexSpriteMapLoaded += ActivateHsm;
            GameTables.GameEngine.HexSpriteMapClosing += DeactivateHsm;
            if (StrategicMap.Current != null)
            {
                LinkToHsm();
            }
        }
    }

    // MapCtrl.pas:197
    public void LinkToHsm()
    {
        StrategicMap.Current.Mouse.DoubleClicked += DoubleClick;
        StrategicMap.Current.DayStarted += ChangeDay;
        StrategicMap.Current.CurrentPlayerChanged += PlayerChanged;
        StrategicMap.Current.TurnStarted += NewTurn;
        StrategicMap.Current.TurnEnded += EndTurn;
        StrategicMap.Current.SeatedPlayerChanged += SeatedPlayerChanged;
        StrategicMap.Current.Selection.Selected += SelectHexSprite;
        StrategicMap.Current.Selection.Deselected += DeselectHexSprite;
        StrategicMap.Current.ShowingMessage += ShowMessage;
        StrategicMap.Current.MapChanged += MapChanged;
        StrategicMap.Current.ScannerIconsChanged += ScannerIconsChanged;
        StrategicMap.Current.VisibilityUpdated += MapSettingsChanged;
        StrategicMap.Current.CombatTriggered += CombatEvent;
        StrategicMap.Current.StructureChanged += StructureChanged;
        StrategicMap.Current.StructureDestroyed += StructureDestroyed;
        StrategicMap.Current.ArmyChanged += ArmyChanged;
        StrategicMap.Current.ArmyDestroyed += ArmyDestroyed;
        StrategicMap.Current.UnitChanged += UnitChanged;
        StrategicMap.Current.UnitDestroyed += UnitDestroyed;
        StrategicMap.Current.GameTerminated += GameOver;
        StrategicMap.Current.GameEnded += GameTerminated;
        StrategicMap.Current._hexSpriteChangedEvents += HexSpriteChanged;
        StrategicMap.Current.PlayerTerminated += PlayerTerminated;
        StrategicMap.Current.ExecutingEventLog += ExecuteEventLog;
        StrategicMap.Current.TargetSelection.Changed += TargetSelectionChange;
        StrategicMap.Current.ViewPortChanged += ViewPortChange;
        StrategicMap.Current.CosmeticSurgeryCast += CastSpell;
        StrategicMap.Current.AbilityActivated += ActivateAbility;
        StrategicMap.Current.GlobalMagicControl.Changed += GlobalMagicChanged;
        StrategicMap.Current.PlayerChanged += RelationsChanged;
        StrategicMap.Current.DiplomaticRelationsChanged += DiplomaticChange;
        StrategicMap.Current.ExitingGame += ExitGameEvent;
        StrategicMap.Current.CombatMapCreated += ActivateCombatMap;
        StrategicMap.Current.CombatMapDestroyed += DeactivateCombatMap;
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).GemsChanged += GemsChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).IncomeChanged += IncomeChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).UpkeepChanged += UpkeepChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).LogbookChanged += LogbookChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).MagicControl.Changed += PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:244
    public void ReleaseHsm()
    {
        StrategicMap.Current.Mouse.DoubleClicked -= DoubleClick;
        StrategicMap.Current.DayStarted -= ChangeDay;
        StrategicMap.Current.CurrentPlayerChanged -= PlayerChanged;
        StrategicMap.Current.TurnStarted -= NewTurn;
        StrategicMap.Current.TurnEnded -= EndTurn;
        StrategicMap.Current.SeatedPlayerChanged -= SeatedPlayerChanged;
        StrategicMap.Current.Selection.Selected -= SelectHexSprite;
        StrategicMap.Current.Selection.Deselected -= DeselectHexSprite;
        StrategicMap.Current.ShowingMessage -= ShowMessage;
        StrategicMap.Current.MapChanged -= MapChanged;
        StrategicMap.Current.ScannerIconsChanged -= ScannerIconsChanged;
        StrategicMap.Current.VisibilityUpdated -= MapSettingsChanged;
        StrategicMap.Current.CombatTriggered -= CombatEvent;
        StrategicMap.Current.ArmyChanged -= ArmyChanged;
        StrategicMap.Current.ArmyDestroyed -= ArmyDestroyed;
        StrategicMap.Current.UnitChanged -= UnitChanged;
        StrategicMap.Current.UnitDestroyed -= UnitDestroyed;
        StrategicMap.Current.StructureChanged -= StructureChanged;
        StrategicMap.Current.StructureDestroyed -= StructureDestroyed;
        StrategicMap.Current._hexSpriteChangedEvents -= HexSpriteChanged;
        StrategicMap.Current.GameTerminated -= GameOver;
        StrategicMap.Current.GameEnded -= GameTerminated;
        StrategicMap.Current.PlayerTerminated -= PlayerTerminated;
        StrategicMap.Current.ExecutingEventLog -= ExecuteEventLog;
        StrategicMap.Current.TargetSelection.Changed -= TargetSelectionChange;
        StrategicMap.Current.ViewPortChanged -= ViewPortChange;
        StrategicMap.Current.CosmeticSurgeryCast -= CastSpell;
        StrategicMap.Current.AbilityActivated -= ActivateAbility;
        StrategicMap.Current.GlobalMagicControl.Changed -= GlobalMagicChanged;
        StrategicMap.Current.PlayerChanged -= RelationsChanged;
        StrategicMap.Current.DiplomaticRelationsChanged -= DiplomaticChange;
        StrategicMap.Current.ExitingGame -= ExitGameEvent;
        StrategicMap.Current.CombatMapCreated -= ActivateCombatMap;
        StrategicMap.Current.CombatMapDestroyed -= DeactivateCombatMap;
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).GemsChanged -= GemsChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).IncomeChanged -= IncomeChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).UpkeepChanged -= UpkeepChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).LogbookChanged -= LogbookChanged;
            ((Player)(object)(object)StrategicMap.Current.Players.GetPlayers(I)).MagicControl.Changed -= PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:291
    public void ActivateHsm(GameMap map)
    {
        if (map.EngineClass.ClassId == 131089)
        {
            LinkToHsm();
            if (OnActivateHsm != null)
            {
                OnActivateHsm(this);
            }
        }
    }

    // MapCtrl.pas:300
    public void DeactivateHsm(GameMap map)
    {
        if (map.EngineClass.ClassId == 131089)
        {
            if (OnDeactivateHsm != null)
            {
                OnDeactivateHsm(this);
            }

            ReleaseHsm();
        }
    }

    // MapCtrl.pas:309
    public void DoubleClick(object sender)
    {
        HexSprite hexSprite;
        if (OnArmyDoubleClick != null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(ArmyHexSprite));
            if (hexSprite is ArmyHexSprite armyHexSprite)
            {
                OnArmyDoubleClick(armyHexSprite.Army);
            }
        }

        if (OnStructureDoubleClick != null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(Structure));
            if (hexSprite is Structure structure)
            {
                OnStructureDoubleClick(structure);
            }
        }
    }

    // MapCtrl.pas:350
    public void MapChanged(MapChangedNotification sender)
    {
        try
        {
            if (OnMapChanged != null)
            {
                OnMapChanged(ref sender.Info);
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
            if (OnScannerIconsChanged != null)
            {
                OnScannerIconsChanged(ref sender.Info);
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
            if (OnMapSettingsChanged != null)
            {
                OnMapSettingsChanged(this);
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
            if (OnArmySelected != null && selection.EventHexSprite is ArmyHexSprite armyHexSprite && StrategicMap.Current.SelectedArmy.GetArmyHexSprite() != null)
            {
                OnArmySelected(armyHexSprite.Army);
            }

            if (OnStructureSelected != null && selection.EventHexSprite is Structure structure)
            {
                OnStructureSelected(structure);
            }

            if (OnItemsSelected != null && selection.EventHexSprite is ItemHexSprite item)
            {
                OnItemsSelected(item);
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
            if (OnArmyDeselected != null && selection.EventHexSprite is ArmyHexSprite armyHexSprite)
            {
                OnArmyDeselected(armyHexSprite.Army);
            }

            if (OnStructureDeselected != null && selection.EventHexSprite is Structure structure)
            {
                OnStructureDeselected(structure);
            }

            if (OnItemsDeselected != null && selection.EventHexSprite is ItemHexSprite item)
            {
                OnItemsDeselected(item);
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
            if (OnPlayerChanged != null)
            {
                OnPlayerChanged(StrategicMap.Current.CurrentPlayer);
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
            if (OnExitGame != null)
            {
                OnExitGame(this);
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
            if (OnNewTurn != null)
            {
                OnNewTurn(player);
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
            if (OnEndTurn != null)
            {
                var listed = (object)StrategicMap.Current.Players.GetPlayers(player);
                OnEndTurn(player, !((Player)(object)listed).SingleProcessCycle);
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
            if (OnSeatedPlayerChanged != null)
            {
                OnSeatedPlayerChanged(StrategicMap.Current.SeatedPlayer);
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
            if (OnGemsChanged != null)
            {
                OnGemsChanged(player.PlayerIndex);
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
            if (OnIncomeChanged != null)
            {
                OnIncomeChanged(player.PlayerIndex);
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
            if (OnUpkeepChanged != null)
            {
                OnUpkeepChanged(player.PlayerIndex);
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
            if (OnPlayerMagicChanged != null)
            {
                OnPlayerMagicChanged(control.Player.PlayerIndex);
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
            if (OnGlobalMagicChanged != null)
            {
                OnGlobalMagicChanged(sender);
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
            if (OnShowMessage != null)
            {
                OnShowMessage(ref sender.Info);
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
            if (OnDayChanged != null)
            {
                OnDayChanged(StrategicMap.Current.Day);
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
            if (OnCombat != null)
            {
                OnCombat(sender);
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
            if (OnLogbookChanged != null)
            {
                OnLogbookChanged(player.PlayerIndex);
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
            if (OnStructureChanged != null)
            {
                OnStructureChanged(objectValue);
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
            if (OnStructureDestroyed != null)
            {
                OnStructureDestroyed(objectValue);
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
            if (OnArmyChanged != null)
            {
                OnArmyChanged(objectValue);
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
            if (OnArmyDestroyed != null)
            {
                OnArmyDestroyed(objectValue);
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
            if (OnHexSpriteChanged != null)
            {
                OnHexSpriteChanged(sender);
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
            if (OnUnitChanged != null)
            {
                OnUnitChanged(objectValue);
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
            if (OnUnitDestroyed != null)
            {
                OnUnitDestroyed(objectValue);
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
            if (OnGameOver != null)
            {
                OnGameOver(sender);
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
            if (OnGameTerminated != null)
            {
                OnGameTerminated((sbyte)StrategicMap.Current.GameResult);
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
            if (OnPlayerTerminated != null)
            {
                OnPlayerTerminated(player);
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
            if (OnExecuteEventLog != null)
            {
                OnExecuteEventLog(info.Info.Log, info.Info.Immediate, info.Info.Done);
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
            if (OnActivateCombatMap != null)
            {
                OnActivateCombatMap(this);
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
            if (OnDeactivateCombatMap != null)
            {
                OnDeactivateCombatMap(this);
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
            if (OnTargetSelectionChange != null && control != null && (((int)StrategicMap.Current.TargetSelection.TargetMode.Image) & 8) != 0)
            {
                OnTargetSelectionChange(control, control.Change);
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
            if (OnCastSpell != null)
            {
                OnCastSpell(sender);
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
            if (OnViewPortChange != null)
            {
                OnViewPortChange(this);
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
            if (OnActivateAbility != null)
            {
                OnActivateAbility(sender);
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
            if (OnDiplomaticChange != null)
            {
                OnDiplomaticChange(this);
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
            if (OnRelationsChanged != null)
            {
                OnRelationsChanged(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnRelationsChanged event (TAoWMapControl)");
        }
    }

    public delegate void MapPlayerEvent(sbyte Player);
    public delegate void MapDayEvent(int Day);
    public delegate void MapObjectEvent<in TSender>(TSender Obj);
    public delegate void MapRefEvent<TInfo>(ref TInfo Info);
    public delegate void MapEndTurnEvent(sbyte Player, bool Automatic);
    public delegate void MapExecuteLogEvent(EventLog Log, bool Immediate, ExecuteDoneEvent Done);
    public delegate void MapTargetEvent(TargetSelectionControl Control, TargetSelectionChange Change);
}
