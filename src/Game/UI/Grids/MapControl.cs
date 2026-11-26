#nullable disable warnings
// Translated from MapCtrl.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.UI.Grids;

public sealed class MapControl : Component
{
    private readonly GameEngine? _engine;

    // The map the control's relays are linked to, and its players at link time (issue 326).
    private StrategicMap? _linkedMap;
    private List<Player> _linkedPlayers = [];

    private static void ShowMessageLogged(string text)
    {
        HostLog.CatchDialogAtCallSite(RtlHost.Logger, text: text);
        LegacyApplication.ShowMessage(text);
    }

    [Published]
    public event MapObjectEvent<Army>? ArmySelected;
    [Published]
    public event MapObjectEvent<Army>? ArmyDeselected;
    [Published]
    public event EventHandler<MapObjectEventArgs>? ArmyDoubleClicked;
    [Published]
    public event MapObjectEvent<Army>? ArmyChanged;
    [Published]
    public event MapObjectEvent<Army>? ArmyDestroyed;
    [Published]
    public event MapObjectEvent<Structure>? StructureSelected;
    [Published]
    public event MapObjectEvent<Structure>? StructureDeselected;
    [Published]
    public event MapObjectEvent<Structure>? StructureDoubleClicked;
    [Published]
    public event MapObjectEvent<Structure>? StructureChanged;
    [Published]
    public event MapObjectEvent<Structure>? StructureDestroyed;
    [Published]
    public event MapObjectEvent<ItemHexSprite>? ItemsSelected;
    [Published]
    public event MapObjectEvent<ItemHexSprite>? ItemsDeselected;
    [Published]
    public event MapObjectEvent<Troop>? UnitChanged;
    [Published]
    public event EventHandler<UnitDestroyedEventArgs>? UnitDestroyed;
    [Published]
    public event MapObjectEvent<AbilityActivation>? AbilityActivated;
    [Published]
    public event NotifyEvent<object>? DiplomaticRelationsChanged;
    [Published]
    public event EventHandler? HexSpriteMapLoaded;
    [Published]
    public event NotifyEvent<object>? HexSpriteMapClosing;
    [Published]
    public event MapPlayerEvent? IncomeChanged;
    [Published]
    public event MapPlayerEvent? UpkeepChanged;
    [Published]
    public event MapPlayerEvent? GemsChanged;
    [Published]
    public event MapPlayerEvent? PlayerMagicChanged;
    [Published]
    public event NotifyEvent<object>? GlobalMagicChanged;
    [Published]
    public event MapRefEvent<ShowMessageInfo>? ShowingMessage;
    [Published]
    public event MapDayEvent? DayChanged;
    [Published]
    public event EventHandler<PlayerEventArgs>? TurnStarted;
    [Published]
    public event EventHandler<PlayerEventArgs>? CurrentPlayerChanged;
    [Published]
    public event MapPlayerEvent? SeatedPlayerChanged;
    [Published]
    public event EventHandler<CombatEventArgs>? CombatTriggered;
    [Published]
    public event MapExecuteLogEvent? ExecutingEventLog;
    [Published]
    public event MapRefEvent<MapChangedInfo>? MapChanged;
    [Published]
    public event MapRefEvent<MapChangedInfo>? ScannerIconsChanged;
    [Published]
    public event EventHandler? VisibilityUpdated;
    [Published]
    public event MapPlayerEvent? LogbookChanged;
    [Published]
    public event NotifyEvent<object>? GameTerminated;
    [Published]
    public event EventHandler<PlayerEventArgs>? GameEnded;
    [Published]
    public event MapPlayerEvent? PlayerTerminated;
    [Published]
    public event EventHandler? CombatMapCreated;
    [Published]
    public event EventHandler? CombatMapDestroyed;
    [Published]
    public event MapTargetEvent? TargetSelectionChanged;
    [Published]
    public event EventHandler? ViewPortChanged;
    [Published]
    public event MapEndTurnEvent? TurnEnded;
    [Published]
    public event NotifyEvent<object>? PlayerChanged;
    [Published]
    public event EventHandler? ExitingGame;
    [Published]
    public event MapObjectEvent<HexSprite>? HexSpriteChanged;

    // MapCtrl.pas:186
    public override void Dispose()
    {
        if (_engine is not null)
        {
            _engine.HexSpriteMapLoaded -= engine_HexSpriteMapLoaded;
            _engine.HexSpriteMapClosing -= engine_HexSpriteMapClosing;
            ReleaseHsm();
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
            _engine.HexSpriteMapLoaded += engine_HexSpriteMapLoaded;
            _engine.HexSpriteMapClosing += engine_HexSpriteMapClosing;
            if (StrategicMap.Current is not null)
            {
                LinkToHsm(StrategicMap.Current);
            }
        }
    }

    // MapCtrl.pas:197. The original links the global map on each loaded raise and holds no linked flag, so a
    // second raise for the live map adds a second copy of every relay (MapCtrl.pas:197, 291).
    // The port remembers the map it linked: linking it again changes nothing, and linking
    // another map releases the first (issue 326, the at-most-one-handler check).
    public void LinkToHsm(StrategicMap map)
    {
        if (_linkedMap == map)
        {
            return;
        }

        ReleaseHsm();
        _linkedMap = map;
        _linkedPlayers = [.. Enumerable.Range(0, map.Players.Count).Select(map.Players.GetPlayers)];
        map.Mouse.DoubleClicked += DoubleClick;
        map.DayStarted += ChangeDay;
        map.CurrentPlayerChanged += StrategicMap_CurrentPlayerChanged;
        map.TurnStarted += StrategicMap_TurnStarted;
        map.TurnEnded += StrategicMap_TurnEnded;
        map.SeatedPlayerChanged += StrategicMap_SeatedPlayerChanged;
        map.Selection.Selected += SelectHexSprite;
        map.Selection.Deselected += DeselectHexSprite;
        map.ShowingMessage += StrategicMap_ShowingMessage;
        map.MapChanged += StrategicMap_MapChanged;
        map.ScannerIconsChanged += StrategicMap_ScannerIconsChanged;
        map.VisibilityUpdated += StrategicMap_VisibilityUpdated;
        map.CombatTriggered += CombatEvent;
        map.StructureChanged += StrategicMap_StructureChanged;
        map.StructureDestroyed += StrategicMap_StructureDestroyed;
        map.ArmyChanged += StrategicMap_ArmyChanged;
        map.ArmyDestroyed += StrategicMap_ArmyDestroyed;
        map.UnitChanged += StrategicMap_UnitChanged;
        map.UnitDestroyed += StrategicMap_UnitDestroyed;
        map.GameTerminated += StrategicMap_GameTerminated;
        map.GameEnded += StrategicMap_GameEnded;
        map.HexSpriteChanged += StrategicMap_HexSpriteChanged;
        map.PlayerTerminated += StrategicMap_PlayerTerminated;
        map.ExecutingEventLog += StrategicMap_ExecutingEventLog;
        map.TargetSelection.Changed += TargetSelection_Changed;
        map.ViewPortChanged += StrategicMap_ViewPortChanged;
        map.AbilityActivated += StrategicMap_AbilityActivated;
        map.GlobalMagicControl.Changed += GlobalMagicControl_Changed;
        map.PlayerChanged += StrategicMap_PlayerChanged;
        map.DiplomaticRelationsChanged += StrategicMap_DiplomaticRelationsChanged;
        map.ExitingGame += ExitGameEvent;
        map.CombatMapCreated += StrategicMap_CombatMapCreated;
        map.CombatMapDestroyed += StrategicMap_CombatMapDestroyed;
        for (int i = 0, last = _linkedPlayers.Count - 1; i <= last; i++)
        {
            _linkedPlayers[i].GemsChanged += Player_GemsChanged;
            _linkedPlayers[i].IncomeChanged += Player_IncomeChanged;
            _linkedPlayers[i].UpkeepChanged += Player_UpkeepChanged;
            _linkedPlayers[i].LogbookChanged += Player_LogbookChanged;
            _linkedPlayers[i].MagicControl.Changed += MagicControl_Changed;
        }
    }

    // MapCtrl.pas:244
    public void ReleaseHsm()
    {
        if (_linkedMap is not StrategicMap map)
        {
            return;
        }

        _linkedMap = null;
        map.Mouse.DoubleClicked -= DoubleClick;
        map.DayStarted -= ChangeDay;
        map.CurrentPlayerChanged -= StrategicMap_CurrentPlayerChanged;
        map.TurnStarted -= StrategicMap_TurnStarted;
        map.TurnEnded -= StrategicMap_TurnEnded;
        map.SeatedPlayerChanged -= StrategicMap_SeatedPlayerChanged;
        map.Selection.Selected -= SelectHexSprite;
        map.Selection.Deselected -= DeselectHexSprite;
        map.ShowingMessage -= StrategicMap_ShowingMessage;
        map.MapChanged -= StrategicMap_MapChanged;
        map.ScannerIconsChanged -= StrategicMap_ScannerIconsChanged;
        map.VisibilityUpdated -= StrategicMap_VisibilityUpdated;
        map.CombatTriggered -= CombatEvent;
        map.ArmyChanged -= StrategicMap_ArmyChanged;
        map.ArmyDestroyed -= StrategicMap_ArmyDestroyed;
        map.UnitChanged -= StrategicMap_UnitChanged;
        map.UnitDestroyed -= StrategicMap_UnitDestroyed;
        map.StructureChanged -= StrategicMap_StructureChanged;
        map.StructureDestroyed -= StrategicMap_StructureDestroyed;
        map.HexSpriteChanged -= StrategicMap_HexSpriteChanged;
        map.GameTerminated -= StrategicMap_GameTerminated;
        map.GameEnded -= StrategicMap_GameEnded;
        map.PlayerTerminated -= StrategicMap_PlayerTerminated;
        map.ExecutingEventLog -= StrategicMap_ExecutingEventLog;
        map.TargetSelection.Changed -= TargetSelection_Changed;
        map.ViewPortChanged -= StrategicMap_ViewPortChanged;
        map.AbilityActivated -= StrategicMap_AbilityActivated;
        map.GlobalMagicControl.Changed -= GlobalMagicControl_Changed;
        map.PlayerChanged -= StrategicMap_PlayerChanged;
        map.DiplomaticRelationsChanged -= StrategicMap_DiplomaticRelationsChanged;
        map.ExitingGame -= ExitGameEvent;
        map.CombatMapCreated -= StrategicMap_CombatMapCreated;
        map.CombatMapDestroyed -= StrategicMap_CombatMapDestroyed;
        for (int i = 0, last = _linkedPlayers.Count - 1; i <= last; i++)
        {
            _linkedPlayers[i].GemsChanged -= Player_GemsChanged;
            _linkedPlayers[i].IncomeChanged -= Player_IncomeChanged;
            _linkedPlayers[i].UpkeepChanged -= Player_UpkeepChanged;
            _linkedPlayers[i].LogbookChanged -= Player_LogbookChanged;
            _linkedPlayers[i].MagicControl.Changed -= MagicControl_Changed;
        }

        _linkedPlayers = [];
    }

    // MapCtrl.pas:291
    public void engine_HexSpriteMapLoaded(GameMap map)
    {
        if (map.EngineClass.ClassId == StrategicMap.ClassId)
        {
            LinkToHsm((StrategicMap)map);
            HexSpriteMapLoaded?.Invoke(this, e: EventArgs.Empty);
        }
    }

    // MapCtrl.pas:300
    public void engine_HexSpriteMapClosing(GameMap map)
    {
        if (map.EngineClass.ClassId == StrategicMap.ClassId)
        {
            HexSpriteMapClosing?.Invoke(this);

            ReleaseHsm();
        }
    }

    // MapCtrl.pas:309
    public void DoubleClick(object sender)
    {
        HexSprite? hexSprite;
        if (ArmyDoubleClicked is not null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(ArmyHexSprite));
            if (hexSprite is ArmyHexSprite armyHexSprite)
            {
                ArmyDoubleClicked(this, e: new MapObjectEventArgs(armyHexSprite.Army));
            }
        }

        if (StructureDoubleClicked is not null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(Structure));
            if (hexSprite is Structure structure)
            {
                StructureDoubleClicked(structure);
            }
        }
    }

    // MapCtrl.pas:350
    public void StrategicMap_MapChanged(MapChangedNotification sender)
    {
        try
        {
            if (MapChanged is not null)
            {
                MapChangedInfo info = sender.Info;
                MapChanged(in info);
                sender.Info = info;
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during MapChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:359
    public void StrategicMap_ScannerIconsChanged(MapChangedNotification sender)
    {
        try
        {
            if (ScannerIconsChanged is not null)
            {
                MapChangedInfo info = sender.Info;
                ScannerIconsChanged(in info);
                sender.Info = info;
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during ScannerIconsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:368
    public void StrategicMap_VisibilityUpdated(object sender)
    {
        try
        {
            VisibilityUpdated?.Invoke(this, e: EventArgs.Empty);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during MapSettingsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:324
    public void SelectHexSprite(object sender)
    {
        try
        {
            HexSpriteSelection selection = StrategicMap.Current.Selection;
            if (ArmySelected is not null && selection.EventHexSprite is ArmyHexSprite armyHexSprite && StrategicMap.Current.SelectedArmy.ArmyHexSprite is not null)
            {
                ArmySelected(armyHexSprite.Army);
            }

            if (StructureSelected is not null && selection.EventHexSprite is Structure structure)
            {
                StructureSelected(structure);
            }

            if (ItemsSelected is not null && selection.EventHexSprite is ItemHexSprite item)
            {
                ItemsSelected(item);
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during SelectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:337
    public void DeselectHexSprite(object sender)
    {
        try
        {
            HexSpriteSelection selection = StrategicMap.Current.Selection;
            if (ArmyDeselected is not null && selection.EventHexSprite is ArmyHexSprite armyHexSprite)
            {
                ArmyDeselected(armyHexSprite.Army);
            }

            if (StructureDeselected is not null && selection.EventHexSprite is Structure structure)
            {
                StructureDeselected(structure);
            }

            if (ItemsDeselected is not null && selection.EventHexSprite is ItemHexSprite item)
            {
                ItemsDeselected(item);
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during DeselectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:377
    public void StrategicMap_CurrentPlayerChanged(object sender)
    {
        try
        {
            CurrentPlayerChanged?.Invoke(this, e: new PlayerEventArgs((sbyte)PlayerIdExtensions.ToOriginal(StrategicMap.Current.CurrentPlayer)));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:386
    public void ExitGameEvent(object sender)
    {
        try
        {
            ExitingGame?.Invoke(this, e: EventArgs.Empty);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during ExitGame event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:395
    public void StrategicMap_TurnStarted(sbyte player)
    {
        try
        {
            TurnStarted?.Invoke(this, e: new PlayerEventArgs(player));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during NewTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:404
    public void StrategicMap_TurnEnded(sbyte player)
    {
        try
        {
            if (TurnEnded is not null)
            {
                Player? listed = StrategicMap.Current.Players.GetPlayers(player);
                TurnEnded(player, Automatic: !listed.SingleProcessCycle);
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during EndTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:413
    public void StrategicMap_SeatedPlayerChanged(object sender)
    {
        try
        {
            SeatedPlayerChanged?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(StrategicMap.Current.SeatedPlayer));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:422
    public void Player_GemsChanged(Player player)
    {
        try
        {
            GemsChanged?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(player.PlayerIndex));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during Gems Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:431
    public void Player_IncomeChanged(Player player)
    {
        try
        {
            IncomeChanged?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(player.PlayerIndex));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during Income Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:440
    public void Player_UpkeepChanged(Player player)
    {
        try
        {
            UpkeepChanged?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(player.PlayerIndex));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during Upkeep Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:449
    public void MagicControl_Changed(PlayerMagicControl control)
    {
        try
        {
            PlayerMagicChanged?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(control.Player.PlayerIndex));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during Player Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:458
    public void GlobalMagicControl_Changed(object sender)
    {
        try
        {
            GlobalMagicChanged?.Invoke(sender);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:467
    public void StrategicMap_ShowingMessage(ShowMessageNotification sender)
    {
        try
        {
            if (ShowingMessage is not null)
            {
                ShowMessageInfo info = sender.Info;
                ShowingMessage(in info);
                sender.Info = info;
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during ShowMessage event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:476
    public void ChangeDay(object sender)
    {
        try
        {
            DayChanged?.Invoke(StrategicMap.Current.Day);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during ChangeDay event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:485
    public void CombatEvent(object sender)
    {
        try
        {
            CombatTriggered?.Invoke(this, e: new CombatEventArgs(sender));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during on combat event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:494
    public void Player_LogbookChanged(Player player)
    {
        try
        {
            LogbookChanged?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(player.PlayerIndex));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during LogbookChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:503
    public void StrategicMap_StructureChanged(Structure objectValue)
    {
        try
        {
            StructureChanged?.Invoke(objectValue);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnStructureChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:512
    public void StrategicMap_StructureDestroyed(Structure objectValue)
    {
        try
        {
            StructureDestroyed?.Invoke(objectValue);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnStructureDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:521
    public void StrategicMap_ArmyChanged(Army objectValue)
    {
        try
        {
            ArmyChanged?.Invoke(objectValue);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnArmyChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:530
    public void StrategicMap_ArmyDestroyed(Army objectValue)
    {
        try
        {
            ArmyDestroyed?.Invoke(objectValue);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnArmyDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:539
    public void StrategicMap_HexSpriteChanged(HexSprite sender)
    {
        try
        {
            HexSpriteChanged?.Invoke(sender);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnHSChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:548
    public void StrategicMap_UnitChanged(Troop objectValue)
    {
        try
        {
            UnitChanged?.Invoke(objectValue);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnUnitChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:557
    public void StrategicMap_UnitDestroyed(Troop objectValue)
    {
        try
        {
            UnitDestroyed?.Invoke(this, e: new UnitDestroyedEventArgs(objectValue));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnUnitDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:566
    public void StrategicMap_GameTerminated(object sender)
    {
        try
        {
            GameTerminated?.Invoke(sender);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnGameOver event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:575
    public void StrategicMap_GameEnded(object sender)
    {
        try
        {
            GameEnded?.Invoke(this, e: new PlayerEventArgs((sbyte)StrategicMap.Current.GameResult));
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnGameTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:584
    public void StrategicMap_PlayerTerminated(sbyte player)
    {
        try
        {
            PlayerTerminated?.Invoke(player);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnPlayerTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:593
    public void StrategicMap_ExecutingEventLog(ExecuteEventLogNotification info)
    {
        try
        {
            ExecutingEventLog?.Invoke(info.Info.Log, info.Info.Immediate, info.Info.Done);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnExecuteEventLog event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:603
    public void StrategicMap_CombatMapCreated(object sender)
    {
        try
        {
            CombatMapCreated?.Invoke(this, e: EventArgs.Empty);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnActivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:612
    public void StrategicMap_CombatMapDestroyed(object sender)
    {
        try
        {
            CombatMapDestroyed?.Invoke(this, e: EventArgs.Empty);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnDeactivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:621
    public void TargetSelection_Changed(object? sender, TargetSelectionChangedEventArgs e)
    {
        TargetSelectionControl control = (TargetSelectionControl)sender!;
        try
        {
            if (TargetSelectionChanged is not null && control is not null && (((int)StrategicMap.Current.TargetSelection.TargetMode) & 8) != 0)
            {
                TargetSelectionChanged(control, e.Change);
            }
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnTargetSelectionChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:639
    public void StrategicMap_ViewPortChanged(object sender)
    {
        try
        {
            ViewPortChanged?.Invoke(this, e: EventArgs.Empty);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnViewPortChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:648
    public void StrategicMap_AbilityActivated(AbilityActivation sender)
    {
        try
        {
            AbilityActivated?.Invoke(sender);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnActivateAbility event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:657
    public void StrategicMap_DiplomaticRelationsChanged(object sender)
    {
        try
        {
            DiplomaticRelationsChanged?.Invoke(this);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnDiplomaticChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:666
    public void StrategicMap_PlayerChanged(object sender)
    {
        try
        {
            PlayerChanged?.Invoke(this);
        }
        catch
        {
            ShowMessageLogged("Exception occurred during OnRelationsChanged event (TAoWMapControl)");
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
