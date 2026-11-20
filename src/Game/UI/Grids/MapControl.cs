#nullable disable
// Translated from MapCtrl.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.UI.Grids;

public class MapControl : Component
{
    private readonly GameEngine _engine;

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
        if (_engine is not null)
        {
            _engine.HexSpriteMapLoaded -= ActivateHsm;
            _engine.HexSpriteMapClosing -= DeactivateHsm;
            if (StrategicMap.Current is not null)
            {
                ReleaseHsm();
            }
        }

        base.Dispose();
    }

    // MapCtrl.pas:174
    public MapControl(Component owner) : base(owner)
    {
        // The engine of the dialog module that owns the control, as the original read the
        // engine global.
        _engine = (owner as UIModule)?.UI.Engine;
        if (_engine is not null)
        {
            _engine.HexSpriteMapLoaded += ActivateHsm;
            _engine.HexSpriteMapClosing += DeactivateHsm;
            if (StrategicMap.Current is not null)
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
        StrategicMap.Current.HexSpriteChanged += HexSpriteChanged;
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
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            StrategicMap.Current.Players.GetPlayers(i).GemsChanged += GemsChanged;
            StrategicMap.Current.Players.GetPlayers(i).IncomeChanged += IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(i).UpkeepChanged += UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(i).LogbookChanged += LogbookChanged;
            StrategicMap.Current.Players.GetPlayers(i).MagicControl.Changed += PlayerMagicChanged;
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
        StrategicMap.Current.HexSpriteChanged -= HexSpriteChanged;
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
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            StrategicMap.Current.Players.GetPlayers(i).GemsChanged -= GemsChanged;
            StrategicMap.Current.Players.GetPlayers(i).IncomeChanged -= IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(i).UpkeepChanged -= UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(i).LogbookChanged -= LogbookChanged;
            StrategicMap.Current.Players.GetPlayers(i).MagicControl.Changed -= PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:291
    public void ActivateHsm(GameMap map)
    {
        if (map.EngineClass.ClassId == 131089)
        {
            LinkToHsm();
            OnActivateHsm?.Invoke(this);
        }
    }

    // MapCtrl.pas:300
    public void DeactivateHsm(GameMap map)
    {
        if (map.EngineClass.ClassId == 131089)
        {
            OnDeactivateHsm?.Invoke(this);

            ReleaseHsm();
        }
    }

    // MapCtrl.pas:309
    public void DoubleClick(object sender)
    {
        HexSprite hexSprite;
        if (OnArmyDoubleClick is not null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(ArmyHexSprite));
            if (hexSprite is ArmyHexSprite armyHexSprite)
            {
                OnArmyDoubleClick(armyHexSprite.Army);
            }
        }

        if (OnStructureDoubleClick is not null)
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
            if (OnMapChanged is not null)
            {
                MapChangedInfo info = sender.Info;
                OnMapChanged(in info);
                sender.Info = info;
            }
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during MapChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:359
    public void ScannerIconsChanged(MapChangedNotification sender)
    {
        try
        {
            if (OnScannerIconsChanged is not null)
            {
                MapChangedInfo info = sender.Info;
                OnScannerIconsChanged(in info);
                sender.Info = info;
            }
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during ScannerIconsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:368
    public void MapSettingsChanged(object sender)
    {
        try
        {
            OnMapSettingsChanged?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during MapSettingsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:324
    public void SelectHexSprite(object sender)
    {
        try
        {
            HexSpriteSelection selection = StrategicMap.Current.Selection;
            if (OnArmySelected is not null && selection.EventHexSprite is ArmyHexSprite armyHexSprite && StrategicMap.Current.SelectedArmy.ArmyHexSprite is not null)
            {
                OnArmySelected(armyHexSprite.Army);
            }

            if (OnStructureSelected is not null && selection.EventHexSprite is Structure structure)
            {
                OnStructureSelected(structure);
            }

            if (OnItemsSelected is not null && selection.EventHexSprite is ItemHexSprite item)
            {
                OnItemsSelected(item);
            }
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during SelectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:337
    public void DeselectHexSprite(object sender)
    {
        try
        {
            HexSpriteSelection selection = StrategicMap.Current.Selection;
            if (OnArmyDeselected is not null && selection.EventHexSprite is ArmyHexSprite armyHexSprite)
            {
                OnArmyDeselected(armyHexSprite.Army);
            }

            if (OnStructureDeselected is not null && selection.EventHexSprite is Structure structure)
            {
                OnStructureDeselected(structure);
            }

            if (OnItemsDeselected is not null && selection.EventHexSprite is ItemHexSprite item)
            {
                OnItemsDeselected(item);
            }
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during DeselectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:377
    public void PlayerChanged(object sender)
    {
        try
        {
            OnPlayerChanged?.Invoke(StrategicMap.Current.CurrentPlayer);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:386
    public void ExitGameEvent(object sender)
    {
        try
        {
            OnExitGame?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during ExitGame event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:395
    public void NewTurn(sbyte player)
    {
        try
        {
            OnNewTurn?.Invoke(player);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during NewTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:404
    public void EndTurn(sbyte player)
    {
        try
        {
            if (OnEndTurn is not null)
            {
                Player listed = StrategicMap.Current.Players.GetPlayers(player);
                OnEndTurn(player, !listed.SingleProcessCycle);
            }
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during EndTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:413
    public void SeatedPlayerChanged(object sender)
    {
        try
        {
            OnSeatedPlayerChanged?.Invoke(StrategicMap.Current.SeatedPlayer);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:422
    public void GemsChanged(Player player)
    {
        try
        {
            OnGemsChanged?.Invoke(player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Gems Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:431
    public void IncomeChanged(Player player)
    {
        try
        {
            OnIncomeChanged?.Invoke(player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Income Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:440
    public void UpkeepChanged(Player player)
    {
        try
        {
            OnUpkeepChanged?.Invoke(player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Upkeep Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:449
    public void PlayerMagicChanged(PlayerMagicControl control)
    {
        try
        {
            OnPlayerMagicChanged?.Invoke(control.Player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Player Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:458
    public void GlobalMagicChanged(object sender)
    {
        try
        {
            OnGlobalMagicChanged?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:467
    public void ShowMessage(ShowMessageNotification sender)
    {
        try
        {
            if (OnShowMessage is not null)
            {
                ShowMessageInfo info = sender.Info;
                OnShowMessage(in info);
                sender.Info = info;
            }
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during ShowMessage event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:476
    public void ChangeDay(object sender)
    {
        try
        {
            OnDayChanged?.Invoke(StrategicMap.Current.Day);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during ChangeDay event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:485
    public void CombatEvent(object sender)
    {
        try
        {
            OnCombat?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during on combat event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:494
    public void LogbookChanged(Player player)
    {
        try
        {
            OnLogbookChanged?.Invoke(player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during LogbookChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:503
    public void StructureChanged(Structure objectValue)
    {
        try
        {
            OnStructureChanged?.Invoke(objectValue);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnStructureChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:512
    public void StructureDestroyed(Structure objectValue)
    {
        try
        {
            OnStructureDestroyed?.Invoke(objectValue);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnStructureDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:521
    public void ArmyChanged(Army objectValue)
    {
        try
        {
            OnArmyChanged?.Invoke(objectValue);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnArmyChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:530
    public void ArmyDestroyed(Army objectValue)
    {
        try
        {
            OnArmyDestroyed?.Invoke(objectValue);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnArmyDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:539
    public void HexSpriteChanged(HexSprite sender)
    {
        try
        {
            OnHexSpriteChanged?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnHSChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:548
    public void UnitChanged(Troop objectValue)
    {
        try
        {
            OnUnitChanged?.Invoke(objectValue);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnUnitChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:557
    public void UnitDestroyed(Troop objectValue)
    {
        try
        {
            OnUnitDestroyed?.Invoke(objectValue);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnUnitDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:566
    public void GameOver(object sender)
    {
        try
        {
            OnGameOver?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnGameOver event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:575
    public void GameTerminated(object sender)
    {
        try
        {
            OnGameTerminated?.Invoke((sbyte)StrategicMap.Current.GameResult);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnGameTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:584
    public void PlayerTerminated(sbyte player)
    {
        try
        {
            OnPlayerTerminated?.Invoke(player);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnPlayerTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:593
    public void ExecuteEventLog(ExecuteEventLogNotification info)
    {
        try
        {
            OnExecuteEventLog?.Invoke(info.Info.Log, info.Info.Immediate, info.Info.Done);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:603
    public void ActivateCombatMap(object sender)
    {
        try
        {
            OnActivateCombatMap?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnActivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:612
    public void DeactivateCombatMap(object sender)
    {
        try
        {
            OnDeactivateCombatMap?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnDeactivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:621
    public void TargetSelectionChange(TargetSelectionControl control)
    {
        try
        {
            if (OnTargetSelectionChange is not null && control is not null && (((int)StrategicMap.Current.TargetSelection.TargetMode.Image) & 8) != 0)
            {
                OnTargetSelectionChange(control, control.Change);
            }
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnTargetSelectionChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:630
    public void CastSpell(SpellCaster sender)
    {
        try
        {
            OnCastSpell?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnCastSpell event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:639
    public void ViewPortChange(object sender)
    {
        try
        {
            OnViewPortChange?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnViewPortChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:648
    public void ActivateAbility(AbilityActivation sender)
    {
        try
        {
            OnActivateAbility?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnActivateAbility event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:657
    public void DiplomaticChange(object sender)
    {
        try
        {
            OnDiplomaticChange?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnDiplomaticChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:666
    public void RelationsChanged(object sender)
    {
        try
        {
            OnRelationsChanged?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnRelationsChanged event (TAoWMapControl)");
        }
    }

    public delegate void MapPlayerEvent(sbyte Player);
    public delegate void MapDayEvent(int Day);
    public delegate void MapObjectEvent<in TSender>(TSender Obj);
    public delegate void MapRefEvent<TInfo>(ref readonly TInfo Info);
    public delegate void MapEndTurnEvent(sbyte Player, bool Automatic);
    public delegate void MapExecuteLogEvent(EventLog Log, bool Immediate, ExecuteDoneEvent Done);
    public delegate void MapTargetEvent(TargetSelectionControl Control, TargetSelectionChange Change);
}
