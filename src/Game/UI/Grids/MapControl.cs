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

public sealed class MapControl : Component
{
    private readonly GameEngine _engine;

    [Published]
    public event MapObjectEvent<Army> ArmySelected;
    [Published]
    public event MapObjectEvent<Army> ArmyDeselected;
    [Published]
    public event MapObjectEvent<Army> ArmyDoubleClick;
    [Published]
    public event MapObjectEvent<Army> ArmyChanged;
    [Published]
    public event MapObjectEvent<Army> ArmyDestroyed;
    [Published]
    public event MapObjectEvent<Structure> StructureSelected;
    [Published]
    public event MapObjectEvent<Structure> StructureDeselected;
    [Published]
    public event MapObjectEvent<Structure> StructureDoubleClick;
    [Published]
    public event MapObjectEvent<Structure> StructureChanged;
    [Published]
    public event MapObjectEvent<Structure> StructureDestroyed;
    [Published]
    public event MapObjectEvent<ItemHexSprite> ItemsSelected;
    [Published]
    public event MapObjectEvent<ItemHexSprite> ItemsDeselected;
    [Published]
    public event MapObjectEvent<Troop> UnitChanged;
    [Published]
    public event MapObjectEvent<Troop> UnitDestroyed;
    [Published]
    public event MapObjectEvent<AbilityActivation> ActivateAbility;
    [Published]
    public event NotifyEvent<object> DiplomaticChange;
    [Published]
    public event NotifyEvent<object> ActivateHsm;
    [Published]
    public event NotifyEvent<object> DeactivateHsm;
    [Published]
    public event MapPlayerEvent IncomeChanged;
    [Published]
    public event MapPlayerEvent UpkeepChanged;
    [Published]
    public event MapPlayerEvent GemsChanged;
    [Published]
    public event MapPlayerEvent PlayerMagicChanged;
    [Published]
    public event NotifyEvent<object> GlobalMagicChanged;
    [Published]
    public event MapRefEvent<ShowMessageInfo> ShowMessage;
    [Published]
    public event MapDayEvent DayChanged;
    [Published]
    public event NotifyEvent<sbyte> NewTurn;
    [Published]
    public event MapPlayerEvent PlayerChanged;
    [Published]
    public event MapPlayerEvent SeatedPlayerChanged;
    [Published]
    public event NotifyEvent<object> Combat;
    [Published]
    public event MapExecuteLogEvent ExecuteEventLog;
    [Published]
    public event MapRefEvent<MapChangedInfo> MapChanged;
    [Published]
    public event MapRefEvent<MapChangedInfo> ScannerIconsChanged;
    [Published]
    public event NotifyEvent<object> MapSettingsChanged;
    [Published]
    public event MapPlayerEvent LogbookChanged;
    [Published]
    public event NotifyEvent<object> GameOver;
    [Published]
    public event MapPlayerEvent GameTerminated;
    [Published]
    public event MapPlayerEvent PlayerTerminated;
    [Published]
    public event NotifyEvent<object> ActivateCombatMap;
    [Published]
    public event NotifyEvent<object> DeactivateCombatMap;
    [Published]
    public event MapTargetEvent TargetSelectionChange;
    [Published]
    public event NotifyEvent<object> ViewPortChange;
    [Published]
    public event MapObjectEvent<SpellCaster> OnCastSpell;
    [Published]
    public event MapEndTurnEvent EndTurn;
    [Published]
    public event NotifyEvent<object> RelationsChanged;
    [Published]
    public event NotifyEvent<object> ExitGame;
    [Published]
    public event MapObjectEvent<HexSprite> HexSpriteChanged;

    // MapCtrl.pas:186
    public override void Dispose()
    {
        if (_engine is not null)
        {
            _engine.HexSpriteMapLoaded -= engine_HexSpriteMapLoaded;
            _engine.HexSpriteMapClosing -= engine_HexSpriteMapClosing;
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
            _engine.HexSpriteMapLoaded += engine_HexSpriteMapLoaded;
            _engine.HexSpriteMapClosing += engine_HexSpriteMapClosing;
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
        StrategicMap.Current.CurrentPlayerChanged += StrategicMap_CurrentPlayerChanged;
        StrategicMap.Current.TurnStarted += StrategicMap_TurnStarted;
        StrategicMap.Current.TurnEnded += StrategicMap_TurnEnded;
        StrategicMap.Current.SeatedPlayerChanged += StrategicMap_SeatedPlayerChanged;
        StrategicMap.Current.Selection.Selected += SelectHexSprite;
        StrategicMap.Current.Selection.Deselected += DeselectHexSprite;
        StrategicMap.Current.ShowingMessage += StrategicMap_ShowingMessage;
        StrategicMap.Current.MapChanged += StrategicMap_MapChanged;
        StrategicMap.Current.ScannerIconsChanged += StrategicMap_ScannerIconsChanged;
        StrategicMap.Current.VisibilityUpdated += StrategicMap_VisibilityUpdated;
        StrategicMap.Current.CombatTriggered += CombatEvent;
        StrategicMap.Current.StructureChanged += StrategicMap_StructureChanged;
        StrategicMap.Current.StructureDestroyed += StrategicMap_StructureDestroyed;
        StrategicMap.Current.ArmyChanged += StrategicMap_ArmyChanged;
        StrategicMap.Current.ArmyDestroyed += StrategicMap_ArmyDestroyed;
        StrategicMap.Current.UnitChanged += StrategicMap_UnitChanged;
        StrategicMap.Current.UnitDestroyed += StrategicMap_UnitDestroyed;
        StrategicMap.Current.GameTerminated += StrategicMap_GameTerminated;
        StrategicMap.Current.GameEnded += StrategicMap_GameEnded;
        StrategicMap.Current.HexSpriteChanged += StrategicMap_HexSpriteChanged;
        StrategicMap.Current.PlayerTerminated += StrategicMap_PlayerTerminated;
        StrategicMap.Current.ExecutingEventLog += StrategicMap_ExecutingEventLog;
        StrategicMap.Current.TargetSelection.Changed += TargetSelection_Changed;
        StrategicMap.Current.ViewPortChanged += StrategicMap_ViewPortChanged;
        StrategicMap.Current.CosmeticSurgeryCast += CastSpell;
        StrategicMap.Current.AbilityActivated += StrategicMap_AbilityActivated;
        StrategicMap.Current.GlobalMagicControl.Changed += GlobalMagicControl_Changed;
        StrategicMap.Current.PlayerChanged += StrategicMap_PlayerChanged;
        StrategicMap.Current.DiplomaticRelationsChanged += StrategicMap_DiplomaticRelationsChanged;
        StrategicMap.Current.ExitingGame += ExitGameEvent;
        StrategicMap.Current.CombatMapCreated += StrategicMap_CombatMapCreated;
        StrategicMap.Current.CombatMapDestroyed += StrategicMap_CombatMapDestroyed;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            StrategicMap.Current.Players.GetPlayers(i).GemsChanged += Player_GemsChanged;
            StrategicMap.Current.Players.GetPlayers(i).IncomeChanged += Player_IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(i).UpkeepChanged += Player_UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(i).LogbookChanged += Player_LogbookChanged;
            StrategicMap.Current.Players.GetPlayers(i).MagicControl.Changed += MagicControl_Changed;
        }
    }

    // MapCtrl.pas:244
    public void ReleaseHsm()
    {
        StrategicMap.Current.Mouse.DoubleClicked -= DoubleClick;
        StrategicMap.Current.DayStarted -= ChangeDay;
        StrategicMap.Current.CurrentPlayerChanged -= StrategicMap_CurrentPlayerChanged;
        StrategicMap.Current.TurnStarted -= StrategicMap_TurnStarted;
        StrategicMap.Current.TurnEnded -= StrategicMap_TurnEnded;
        StrategicMap.Current.SeatedPlayerChanged -= StrategicMap_SeatedPlayerChanged;
        StrategicMap.Current.Selection.Selected -= SelectHexSprite;
        StrategicMap.Current.Selection.Deselected -= DeselectHexSprite;
        StrategicMap.Current.ShowingMessage -= StrategicMap_ShowingMessage;
        StrategicMap.Current.MapChanged -= StrategicMap_MapChanged;
        StrategicMap.Current.ScannerIconsChanged -= StrategicMap_ScannerIconsChanged;
        StrategicMap.Current.VisibilityUpdated -= StrategicMap_VisibilityUpdated;
        StrategicMap.Current.CombatTriggered -= CombatEvent;
        StrategicMap.Current.ArmyChanged -= StrategicMap_ArmyChanged;
        StrategicMap.Current.ArmyDestroyed -= StrategicMap_ArmyDestroyed;
        StrategicMap.Current.UnitChanged -= StrategicMap_UnitChanged;
        StrategicMap.Current.UnitDestroyed -= StrategicMap_UnitDestroyed;
        StrategicMap.Current.StructureChanged -= StrategicMap_StructureChanged;
        StrategicMap.Current.StructureDestroyed -= StrategicMap_StructureDestroyed;
        StrategicMap.Current.HexSpriteChanged -= StrategicMap_HexSpriteChanged;
        StrategicMap.Current.GameTerminated -= StrategicMap_GameTerminated;
        StrategicMap.Current.GameEnded -= StrategicMap_GameEnded;
        StrategicMap.Current.PlayerTerminated -= StrategicMap_PlayerTerminated;
        StrategicMap.Current.ExecutingEventLog -= StrategicMap_ExecutingEventLog;
        StrategicMap.Current.TargetSelection.Changed -= TargetSelection_Changed;
        StrategicMap.Current.ViewPortChanged -= StrategicMap_ViewPortChanged;
        StrategicMap.Current.CosmeticSurgeryCast -= CastSpell;
        StrategicMap.Current.AbilityActivated -= StrategicMap_AbilityActivated;
        StrategicMap.Current.GlobalMagicControl.Changed -= GlobalMagicControl_Changed;
        StrategicMap.Current.PlayerChanged -= StrategicMap_PlayerChanged;
        StrategicMap.Current.DiplomaticRelationsChanged -= StrategicMap_DiplomaticRelationsChanged;
        StrategicMap.Current.ExitingGame -= ExitGameEvent;
        StrategicMap.Current.CombatMapCreated -= StrategicMap_CombatMapCreated;
        StrategicMap.Current.CombatMapDestroyed -= StrategicMap_CombatMapDestroyed;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            StrategicMap.Current.Players.GetPlayers(i).GemsChanged -= Player_GemsChanged;
            StrategicMap.Current.Players.GetPlayers(i).IncomeChanged -= Player_IncomeChanged;
            StrategicMap.Current.Players.GetPlayers(i).UpkeepChanged -= Player_UpkeepChanged;
            StrategicMap.Current.Players.GetPlayers(i).LogbookChanged -= Player_LogbookChanged;
            StrategicMap.Current.Players.GetPlayers(i).MagicControl.Changed -= MagicControl_Changed;
        }
    }

    // MapCtrl.pas:291
    public void engine_HexSpriteMapLoaded(GameMap map)
    {
        if (map.EngineClass.ClassId == StrategicMap.ClassId)
        {
            LinkToHsm();
            ActivateHsm?.Invoke(this);
        }
    }

    // MapCtrl.pas:300
    public void engine_HexSpriteMapClosing(GameMap map)
    {
        if (map.EngineClass.ClassId == StrategicMap.ClassId)
        {
            DeactivateHsm?.Invoke(this);

            ReleaseHsm();
        }
    }

    // MapCtrl.pas:309
    public void DoubleClick(object sender)
    {
        HexSprite hexSprite;
        if (ArmyDoubleClick is not null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(ArmyHexSprite));
            if (hexSprite is ArmyHexSprite armyHexSprite)
            {
                ArmyDoubleClick(armyHexSprite.Army);
            }
        }

        if (StructureDoubleClick is not null)
        {
            hexSprite = StrategicMap.Current.Selection.FindClass(typeof(Structure));
            if (hexSprite is Structure structure)
            {
                StructureDoubleClick(structure);
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
            LegacyApplication.ShowMessage("Exception occurred during MapChanged event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during ScannerIconsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:368
    public void StrategicMap_VisibilityUpdated(object sender)
    {
        try
        {
            MapSettingsChanged?.Invoke(this);
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
            LegacyApplication.ShowMessage("Exception occurred during SelectHS event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during DeselectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:377
    public void StrategicMap_CurrentPlayerChanged(object sender)
    {
        try
        {
            PlayerChanged?.Invoke(StrategicMap.Current.CurrentPlayer);
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
            ExitGame?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during ExitGame event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:395
    public void StrategicMap_TurnStarted(sbyte player)
    {
        try
        {
            NewTurn?.Invoke(player);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during NewTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:404
    public void StrategicMap_TurnEnded(sbyte player)
    {
        try
        {
            if (EndTurn is not null)
            {
                Player listed = StrategicMap.Current.Players.GetPlayers(player);
                EndTurn(player, Automatic: !listed.SingleProcessCycle);
            }
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during EndTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:413
    public void StrategicMap_SeatedPlayerChanged(object sender)
    {
        try
        {
            SeatedPlayerChanged?.Invoke(StrategicMap.Current.SeatedPlayer);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:422
    public void Player_GemsChanged(Player player)
    {
        try
        {
            GemsChanged?.Invoke(player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Gems Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:431
    public void Player_IncomeChanged(Player player)
    {
        try
        {
            IncomeChanged?.Invoke(player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Income Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:440
    public void Player_UpkeepChanged(Player player)
    {
        try
        {
            UpkeepChanged?.Invoke(player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Upkeep Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:449
    public void MagicControl_Changed(PlayerMagicControl control)
    {
        try
        {
            PlayerMagicChanged?.Invoke(control.Player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during Player Magic Changed event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:467
    public void StrategicMap_ShowingMessage(ShowMessageNotification sender)
    {
        try
        {
            if (ShowMessage is not null)
            {
                ShowMessageInfo info = sender.Info;
                ShowMessage(in info);
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
            DayChanged?.Invoke(StrategicMap.Current.Day);
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
            Combat?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during on combat event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:494
    public void Player_LogbookChanged(Player player)
    {
        try
        {
            LogbookChanged?.Invoke(player.PlayerIndex);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during LogbookChanged event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnStructureChanged event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnStructureDestroyed event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnArmyChanged event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnArmyDestroyed event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnHSChanged event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnUnitChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:557
    public void StrategicMap_UnitDestroyed(Troop objectValue)
    {
        try
        {
            UnitDestroyed?.Invoke(objectValue);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnUnitDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:566
    public void StrategicMap_GameTerminated(object sender)
    {
        try
        {
            GameOver?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnGameOver event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:575
    public void StrategicMap_GameEnded(object sender)
    {
        try
        {
            GameTerminated?.Invoke((sbyte)StrategicMap.Current.GameResult);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnGameTerminated event (TAoWMapControl)");
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
            LegacyApplication.ShowMessage("Exception occurred during OnPlayerTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:593
    public void StrategicMap_ExecutingEventLog(ExecuteEventLogNotification info)
    {
        try
        {
            ExecuteEventLog?.Invoke(info.Info.Log, info.Info.Immediate, info.Info.Done);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:603
    public void StrategicMap_CombatMapCreated(object sender)
    {
        try
        {
            ActivateCombatMap?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnActivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:612
    public void StrategicMap_CombatMapDestroyed(object sender)
    {
        try
        {
            DeactivateCombatMap?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnDeactivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:621
    public void TargetSelection_Changed(TargetSelectionControl control)
    {
        try
        {
            if (TargetSelectionChange is not null && control is not null && (((int)StrategicMap.Current.TargetSelection.TargetMode) & 8) != 0)
            {
                TargetSelectionChange(control, control.Change);
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
    public void StrategicMap_ViewPortChanged(object sender)
    {
        try
        {
            ViewPortChange?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnViewPortChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:648
    public void StrategicMap_AbilityActivated(AbilityActivation sender)
    {
        try
        {
            ActivateAbility?.Invoke(sender);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnActivateAbility event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:657
    public void StrategicMap_DiplomaticRelationsChanged(object sender)
    {
        try
        {
            DiplomaticChange?.Invoke(this);
        }
        catch
        {
            LegacyApplication.ShowMessage("Exception occurred during OnDiplomaticChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:666
    public void StrategicMap_PlayerChanged(object sender)
    {
        try
        {
            RelationsChanged?.Invoke(this);
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
