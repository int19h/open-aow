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
    public MapObjectEvent<Army> FOnSelectArmy;
    public MapObjectEvent<Army> FOnDeselectArmy;
    public MapObjectEvent<Army> FOnDblClickArmy;
    public MapObjectEvent<Army> FOnArmyChanged;
    public MapObjectEvent<Army> FOnArmyDestroyed;
    public MapObjectEvent<Structure> FOnSelectStructure;
    public MapObjectEvent<Structure> FOnDeselectStructure;
    public MapObjectEvent<Structure> FOnDblClickStructure;
    public MapObjectEvent<Structure> FOnStructureChanged;
    public MapObjectEvent<Structure> FOnStructureDestroyed;
    public MapObjectEvent<ItemHexSprite> FOnSelectItem;
    public MapObjectEvent<ItemHexSprite> FOnDeselectItem;
    public MapObjectEvent<Troop> FOnUnitChanged;
    public MapObjectEvent<Troop> FOnUnitDestroyed;
    public MapObjectEvent<AbilityActivation> FOnActivateAbility;
    public NotifyEvent<object> FOnDiplomaticChange;
    public NotifyEvent<object> FOnActivateHSM;
    public NotifyEvent<object> FOnDeactivateHSM;
    public MapPlayerEvent FOnIncomeChanged;
    public MapPlayerEvent FOnUpkeepChanged;
    public MapPlayerEvent FOnGemsChanged;
    public MapPlayerEvent FOnPlayerMagicChanged;
    public NotifyEvent<object> FOnGlobalMagicChanged;
    public MapRefEvent<ShowMessageInfo> FOnShowMessage;
    public MapDayEvent FOnChangeDay;
    public NotifyEvent<sbyte> FOnNewTurn;
    public MapPlayerEvent FOnPlayerChanged;
    public MapPlayerEvent FOnSeatedPlayerChanged;
    public NotifyEvent<object> FOnCombatEvent;
    public MapExecuteLogEvent FOnExecuteEventLog;
    public MapRefEvent<MapChangedInfo> FOnMapChanged;
    public MapRefEvent<MapChangedInfo> FOnScannerIconsChanged;
    public NotifyEvent<object> FOnMapSettingsChanged;
    public MapPlayerEvent FOnLogbookChanged;
    public NotifyEvent<object> FOnGameOver;
    public MapPlayerEvent FOnGameTerminated;
    public MapPlayerEvent FOnPlayerTerminated;
    public NotifyEvent<object> FOnActivateCombatMap;
    public NotifyEvent<object> FOnDeactivateCombatMap;
    public MapTargetEvent FOnTargetSelectionChange;
    public NotifyEvent<object> FOnViewPortChange;
    public MapObjectEvent<SpellCaster> FOnCastSpell;
    public MapEndTurnEvent FOnEndTurn;
    public NotifyEvent<object> FOnRelationsChanged;
    public NotifyEvent<object> FOnExitGameEvent;
    public MapObjectEvent<HexSprite> FOnHSChanged;

    // MapCtrl.pas:186
    public override void Dispose()
    {
        if (GameTables.AoWEngine != null)
        {
            GameTables.AoWEngine.FOnHSMLoaded -= ActivateHSM;
            GameTables.AoWEngine.FOnHSMClose -= DeactivateHSM;
            if (AoWHSMap.Current != null)
            {
                ReleaseHSM();
            }
        }

        base.Dispose();
    }

    // MapCtrl.pas:174
    public MapControl(Component owner) : base(owner)
    {
        if (GameTables.ValidateAoWEngine())
        {
            GameTables.AoWEngine.FOnHSMLoaded += ActivateHSM;
            GameTables.AoWEngine.FOnHSMClose += DeactivateHSM;
            if (AoWHSMap.Current != null)
            {
                LinkToHSM();
            }
        }
    }

    // MapCtrl.pas:197
    public void LinkToHSM()
    {
        AoWHSMap.Current.FMouse.FOnDblClick += DblClick;
        AoWHSMap.Current.FNewDayEvents += ChangeDay;
        AoWHSMap.Current.FCurrentPlayerChangedEvents += PlayerChanged;
        AoWHSMap.Current.FNewTurnEvents += NewTurn;
        AoWHSMap.Current.FEndTurnEvents += EndTurn;
        AoWHSMap.Current.FSeatedPlayerChangedEvents += SeatedPlayerChanged;
        AoWHSMap.Current.FSelection.FOnSelect += SelectHS;
        AoWHSMap.Current.FSelection.FOnDeselect += DeselectHS;
        AoWHSMap.Current.FShowMessageEvents += ShowMessage;
        AoWHSMap.Current.FMapChangedEvents += MapChanged;
        AoWHSMap.Current.FScannerIconsChangedEvents += ScannerIconsChanged;
        AoWHSMap.Current.FUpdateVisibilityEvents += MapSettingsChanged;
        AoWHSMap.Current.FCombatEvents += CombatEvent;
        AoWHSMap.Current.FStructureChangedEvents += StructureChanged;
        AoWHSMap.Current.FStructureDestroyedEvents += StructureDestroyed;
        AoWHSMap.Current.FArmyChangedEvents += ArmyChanged;
        AoWHSMap.Current.FArmyDestroyedEvents += ArmyDestroyed;
        AoWHSMap.Current.FUnitChangedEvents += UnitChanged;
        AoWHSMap.Current.FUnitDestroyedEvents += UnitDestroyed;
        AoWHSMap.Current.FGameTerminatedEvents += GameOver;
        AoWHSMap.Current.FGameOverEvents += GameTerminated;
        AoWHSMap.Current.FHSChangedEvents += HSChanged;
        AoWHSMap.Current.FPlayerTerminatedEvents += PlayerTerminated;
        AoWHSMap.Current.FExecuteEventLogEvents += ExecuteEventLog;
        AoWHSMap.Current.FTargetSelection.FOnChange += TargetSelectionChange;
        AoWHSMap.Current.FViewPortChangedEvents += ViewPortChange;
        AoWHSMap.Current.FCosmeticSurgeryEvents += CastSpell;
        AoWHSMap.Current.FOnActivateAbility += ActivateAbility;
        AoWHSMap.Current.FGlobalMagicControl.FChangedEvents += GlobalMagicChanged;
        AoWHSMap.Current.FPlayerChangedEvents += RelationsChanged;
        AoWHSMap.Current.FDiplomaticRelationsChangedEvents += DiplomaticChange;
        AoWHSMap.Current.FExitEvents += ExitGameEvent;
        AoWHSMap.Current.FCombatMapCreatedEvents += ActivateCombatMap;
        AoWHSMap.Current.FCombatMapDestroyedEvents += DeactivateCombatMap;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FGemsChangedEvents += GemsChanged;
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FIncomeChangedEvents += IncomeChanged;
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FUpkeepChangedEvents += UpkeepChanged;
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FLogbookChangedEvents += LogbookChanged;
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FMagicControl.FChangedEvents += PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:244
    public void ReleaseHSM()
    {
        AoWHSMap.Current.FMouse.FOnDblClick -= DblClick;
        AoWHSMap.Current.FNewDayEvents -= ChangeDay;
        AoWHSMap.Current.FCurrentPlayerChangedEvents -= PlayerChanged;
        AoWHSMap.Current.FNewTurnEvents -= NewTurn;
        AoWHSMap.Current.FEndTurnEvents -= EndTurn;
        AoWHSMap.Current.FSeatedPlayerChangedEvents -= SeatedPlayerChanged;
        AoWHSMap.Current.FSelection.FOnSelect -= SelectHS;
        AoWHSMap.Current.FSelection.FOnDeselect -= DeselectHS;
        AoWHSMap.Current.FShowMessageEvents -= ShowMessage;
        AoWHSMap.Current.FMapChangedEvents -= MapChanged;
        AoWHSMap.Current.FScannerIconsChangedEvents -= ScannerIconsChanged;
        AoWHSMap.Current.FUpdateVisibilityEvents -= MapSettingsChanged;
        AoWHSMap.Current.FCombatEvents -= CombatEvent;
        AoWHSMap.Current.FArmyChangedEvents -= ArmyChanged;
        AoWHSMap.Current.FArmyDestroyedEvents -= ArmyDestroyed;
        AoWHSMap.Current.FUnitChangedEvents -= UnitChanged;
        AoWHSMap.Current.FUnitDestroyedEvents -= UnitDestroyed;
        AoWHSMap.Current.FStructureChangedEvents -= StructureChanged;
        AoWHSMap.Current.FStructureDestroyedEvents -= StructureDestroyed;
        AoWHSMap.Current.FHSChangedEvents -= HSChanged;
        AoWHSMap.Current.FGameTerminatedEvents -= GameOver;
        AoWHSMap.Current.FGameOverEvents -= GameTerminated;
        AoWHSMap.Current.FPlayerTerminatedEvents -= PlayerTerminated;
        AoWHSMap.Current.FExecuteEventLogEvents -= ExecuteEventLog;
        AoWHSMap.Current.FTargetSelection.FOnChange -= TargetSelectionChange;
        AoWHSMap.Current.FViewPortChangedEvents -= ViewPortChange;
        AoWHSMap.Current.FCosmeticSurgeryEvents -= CastSpell;
        AoWHSMap.Current.FOnActivateAbility -= ActivateAbility;
        AoWHSMap.Current.FGlobalMagicControl.FChangedEvents -= GlobalMagicChanged;
        AoWHSMap.Current.FPlayerChangedEvents -= RelationsChanged;
        AoWHSMap.Current.FDiplomaticRelationsChangedEvents -= DiplomaticChange;
        AoWHSMap.Current.FExitEvents -= ExitGameEvent;
        AoWHSMap.Current.FCombatMapCreatedEvents -= ActivateCombatMap;
        AoWHSMap.Current.FCombatMapDestroyedEvents -= DeactivateCombatMap;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FGemsChangedEvents -= GemsChanged;
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FIncomeChangedEvents -= IncomeChanged;
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FUpkeepChangedEvents -= UpkeepChanged;
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FLogbookChangedEvents -= LogbookChanged;
            ((Player)(object)(object)AoWHSMap.Current.FPlayers.GetPlayers(I)).FMagicControl.FChangedEvents -= PlayerMagicChanged;
        }
    }

    // MapCtrl.pas:291
    public void ActivateHSM(HSMap Map)
    {
        if (Map.EClass.ClassID == 131089)
        {
            LinkToHSM();
            if (FOnActivateHSM != null)
            {
                FOnActivateHSM(this);
            }
        }
    }

    // MapCtrl.pas:300
    public void DeactivateHSM(HSMap Map)
    {
        if (Map.EClass.ClassID == 131089)
        {
            if (FOnDeactivateHSM != null)
            {
                FOnDeactivateHSM(this);
            }

            ReleaseHSM();
        }
    }

    // MapCtrl.pas:309
    public void DblClick(object Sender)
    {
        HexSprite HS;
        if (FOnDblClickArmy != null)
        {
            HS = AoWHSMap.Current.Selection.FindClass(typeof(ArmyHexSprite));
            if (HS is ArmyHexSprite armyHS)
            {
                FOnDblClickArmy(armyHS.FArmy);
            }
        }

        if (FOnDblClickStructure != null)
        {
            HS = AoWHSMap.Current.Selection.FindClass(typeof(Structure));
            if (HS is Structure structure)
            {
                FOnDblClickStructure(structure);
            }
        }
    }

    // MapCtrl.pas:350
    public void MapChanged(MapChangedNotification Sender)
    {
        try
        {
            if (FOnMapChanged != null)
            {
                FOnMapChanged(ref Sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:359
    public void ScannerIconsChanged(MapChangedNotification Sender)
    {
        try
        {
            if (FOnScannerIconsChanged != null)
            {
                FOnScannerIconsChanged(ref Sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ScannerIconsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:368
    public void MapSettingsChanged(object Sender)
    {
        try
        {
            if (FOnMapSettingsChanged != null)
            {
                FOnMapSettingsChanged(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during MapSettingsChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:324
    public void SelectHS(object Sender)
    {
        HexSpriteSelection Selection;
        try
        {
            Selection = AoWHSMap.Current.Selection;
            if (FOnSelectArmy != null && Selection.EventHS is ArmyHexSprite armyHS && AoWHSMap.Current.FSelectedArmy.GetArmyHS() != null)
            {
                FOnSelectArmy(armyHS.FArmy);
            }

            if (FOnSelectStructure != null && Selection.EventHS is Structure structure)
            {
                FOnSelectStructure(structure);
            }

            if (FOnSelectItem != null && Selection.EventHS is ItemHexSprite item)
            {
                FOnSelectItem(item);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during SelectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:337
    public void DeselectHS(object Sender)
    {
        HexSpriteSelection Selection;
        try
        {
            Selection = AoWHSMap.Current.Selection;
            if (FOnDeselectArmy != null && Selection.EventHS is ArmyHexSprite armyHS)
            {
                FOnDeselectArmy(armyHS.FArmy);
            }

            if (FOnDeselectStructure != null && Selection.EventHS is Structure structure)
            {
                FOnDeselectStructure(structure);
            }

            if (FOnDeselectItem != null && Selection.EventHS is ItemHexSprite item)
            {
                FOnDeselectItem(item);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during DeselectHS event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:377
    public void PlayerChanged(object Sender)
    {
        try
        {
            if (FOnPlayerChanged != null)
            {
                FOnPlayerChanged(AoWHSMap.Current.FCurrentPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:386
    public void ExitGameEvent(object Sender)
    {
        try
        {
            if (FOnExitGameEvent != null)
            {
                FOnExitGameEvent(this);
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
            if (FOnNewTurn != null)
            {
                FOnNewTurn(player);
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
            if (FOnEndTurn != null)
            {
                var listed = (object)AoWHSMap.Current.FPlayers.GetPlayers(player);
                FOnEndTurn(player, !((Player)(object)listed).FSingleProcessCycle);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during EndTurn event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:413
    public void SeatedPlayerChanged(object Sender)
    {
        try
        {
            if (FOnSeatedPlayerChanged != null)
            {
                FOnSeatedPlayerChanged(AoWHSMap.Current.FSeatedPlayer);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during PlayerChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:422
    public void GemsChanged(Player Player)
    {
        try
        {
            if (FOnGemsChanged != null)
            {
                FOnGemsChanged(Player.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Gems Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:431
    public void IncomeChanged(Player Player)
    {
        try
        {
            if (FOnIncomeChanged != null)
            {
                FOnIncomeChanged(Player.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Income Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:440
    public void UpkeepChanged(Player Player)
    {
        try
        {
            if (FOnUpkeepChanged != null)
            {
                FOnUpkeepChanged(Player.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Upkeep Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:449
    public void PlayerMagicChanged(PlayerMagicControl Control)
    {
        try
        {
            if (FOnPlayerMagicChanged != null)
            {
                FOnPlayerMagicChanged(Control.FPlayer.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Player Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:458
    public void GlobalMagicChanged(object Sender)
    {
        try
        {
            if (FOnGlobalMagicChanged != null)
            {
                FOnGlobalMagicChanged(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during Magic Changed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:467
    public void ShowMessage(ShowMessageNotification Sender)
    {
        try
        {
            if (FOnShowMessage != null)
            {
                FOnShowMessage(ref Sender.Info);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ShowMessage event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:476
    public void ChangeDay(object Sender)
    {
        try
        {
            if (FOnChangeDay != null)
            {
                FOnChangeDay(AoWHSMap.Current.FDay);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during ChangeDay event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:485
    public void CombatEvent(object Sender)
    {
        try
        {
            if (FOnCombatEvent != null)
            {
                FOnCombatEvent(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during on combat event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:494
    public void LogbookChanged(Player Player)
    {
        try
        {
            if (FOnLogbookChanged != null)
            {
                FOnLogbookChanged(Player.FPlayerIndex);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during LogbookChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:503
    public void StructureChanged(Structure Obj)
    {
        try
        {
            if (FOnStructureChanged != null)
            {
                FOnStructureChanged(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnStructureChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:512
    public void StructureDestroyed(Structure Obj)
    {
        try
        {
            if (FOnStructureDestroyed != null)
            {
                FOnStructureDestroyed(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnStructureDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:521
    public void ArmyChanged(Army Obj)
    {
        try
        {
            if (FOnArmyChanged != null)
            {
                FOnArmyChanged(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnArmyChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:530
    public void ArmyDestroyed(Army Obj)
    {
        try
        {
            if (FOnArmyDestroyed != null)
            {
                FOnArmyDestroyed(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnArmyDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:539
    public void HSChanged(HexSprite Sender)
    {
        try
        {
            if (FOnHSChanged != null)
            {
                FOnHSChanged(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnHSChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:548
    public void UnitChanged(Troop Obj)
    {
        try
        {
            if (FOnUnitChanged != null)
            {
                FOnUnitChanged(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnUnitChanged event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:557
    public void UnitDestroyed(Troop Obj)
    {
        try
        {
            if (FOnUnitDestroyed != null)
            {
                FOnUnitDestroyed(Obj);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnUnitDestroyed event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:566
    public void GameOver(object Sender)
    {
        try
        {
            if (FOnGameOver != null)
            {
                FOnGameOver(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnGameOver event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:575
    public void GameTerminated(object Sender)
    {
        try
        {
            if (FOnGameTerminated != null)
            {
                FOnGameTerminated((sbyte)AoWHSMap.Current.FGameResult);
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
            if (FOnPlayerTerminated != null)
            {
                FOnPlayerTerminated(player);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnPlayerTerminated event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:593
    public void ExecuteEventLog(ExecuteEventLogNotification Info)
    {
        try
        {
            if (FOnExecuteEventLog != null)
            {
                FOnExecuteEventLog(Info.Info.Log, Info.Info.Immediate, Info.Info.Done);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:603
    public void ActivateCombatMap(object Sender)
    {
        try
        {
            if (FOnActivateCombatMap != null)
            {
                FOnActivateCombatMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:612
    public void DeactivateCombatMap(object Sender)
    {
        try
        {
            if (FOnDeactivateCombatMap != null)
            {
                FOnDeactivateCombatMap(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDeactivateCombatMap event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:621
    public void TargetSelectionChange(TargetSelectionControl Control)
    {
        try
        {
            if (FOnTargetSelectionChange != null && Control != null && (((int)AoWHSMap.Current.FTargetSelection.FTargetMode.Image) & 8) != 0)
            {
                FOnTargetSelectionChange(Control, Control.FChange);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnTargetSelectionChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:630
    public void CastSpell(SpellCaster Sender)
    {
        try
        {
            if (FOnCastSpell != null)
            {
                FOnCastSpell(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnCastSpell event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:639
    public void ViewPortChange(object Sender)
    {
        try
        {
            if (FOnViewPortChange != null)
            {
                FOnViewPortChange(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnViewPortChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:648
    public void ActivateAbility(AbilityActivation Sender)
    {
        try
        {
            if (FOnActivateAbility != null)
            {
                FOnActivateAbility(Sender);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnActivateAbility event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:657
    public void DiplomaticChange(object Sender)
    {
        try
        {
            if (FOnDiplomaticChange != null)
            {
                FOnDiplomaticChange(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnDiplomaticChange event (TAoWMapControl)");
        }
    }

    // MapCtrl.pas:666
    public void RelationsChanged(object Sender)
    {
        try
        {
            if (FOnRelationsChanged != null)
            {
                FOnRelationsChanged(this);
            }
        }
        catch
        {
            RtlUnit.ShowMessage("Exception occurred during OnRelationsChanged event (TAoWMapControl)");
        }
    }

    [Published]
    public MapObjectEvent<Army> OnArmySelected { get => FOnSelectArmy; set => FOnSelectArmy = value; }

    [Published]
    public MapObjectEvent<Army> OnArmyDeselected { get => FOnDeselectArmy; set => FOnDeselectArmy = value; }

    [Published]
    public MapObjectEvent<Army> OnArmyDblClick { get => FOnDblClickArmy; set => FOnDblClickArmy = value; }

    [Published]
    public MapObjectEvent<Army> OnArmyChanged { get => FOnArmyChanged; set => FOnArmyChanged = value; }

    [Published]
    public MapObjectEvent<Army> OnArmyDestroyed { get => FOnArmyDestroyed; set => FOnArmyDestroyed = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureSelected { get => FOnSelectStructure; set => FOnSelectStructure = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureDeselected { get => FOnDeselectStructure; set => FOnDeselectStructure = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureDblClick { get => FOnDblClickStructure; set => FOnDblClickStructure = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureChanged { get => FOnStructureChanged; set => FOnStructureChanged = value; }

    [Published]
    public MapObjectEvent<Structure> OnStructureDestroyed { get => FOnStructureDestroyed; set => FOnStructureDestroyed = value; }

    [Published]
    public MapObjectEvent<Troop> OnUnitChanged { get => FOnUnitChanged; set => FOnUnitChanged = value; }

    [Published]
    public MapObjectEvent<Troop> OnUnitDestroyed { get => FOnUnitDestroyed; set => FOnUnitDestroyed = value; }

    [Published]
    public NotifyEvent<object> OnActivateHSM { get => FOnActivateHSM; set => FOnActivateHSM = value; }

    [Published]
    public NotifyEvent<object> OnDeactivateHSM { get => FOnDeactivateHSM; set => FOnDeactivateHSM = value; }

    [Published]
    public MapPlayerEvent OnGemsChanged { get => FOnGemsChanged; set => FOnGemsChanged = value; }

    [Published]
    public MapPlayerEvent OnIncomeChanged { get => FOnIncomeChanged; set => FOnIncomeChanged = value; }

    [Published]
    public MapPlayerEvent OnUpkeepChanged { get => FOnUpkeepChanged; set => FOnUpkeepChanged = value; }

    [Published]
    public MapRefEvent<ShowMessageInfo> OnShowMessage { get => FOnShowMessage; set => FOnShowMessage = value; }

    [Published]
    public MapDayEvent OnDayChanged { get => FOnChangeDay; set => FOnChangeDay = value; }

    [Published]
    public MapPlayerEvent OnPlayerChanged { get => FOnPlayerChanged; set => FOnPlayerChanged = value; }

    [Published]
    public NotifyEvent<sbyte> OnNewTurn { get => FOnNewTurn; set => FOnNewTurn = value; }

    [Published]
    public MapEndTurnEvent OnEndTurn { get => FOnEndTurn; set => FOnEndTurn = value; }

    [Published]
    public MapObjectEvent<HexSprite> OnHSChanged { get => FOnHSChanged; set => FOnHSChanged = value; }

    [Published]
    public MapPlayerEvent OnSeatedPlayerChanged { get => FOnSeatedPlayerChanged; set => FOnSeatedPlayerChanged = value; }

    [Published]
    public MapRefEvent<MapChangedInfo> OnMapChanged { get => FOnMapChanged; set => FOnMapChanged = value; }

    [Published]
    public MapRefEvent<MapChangedInfo> OnScannerIconsChanged { get => FOnScannerIconsChanged; set => FOnScannerIconsChanged = value; }

    [Published]
    public NotifyEvent<object> OnMapSettingsChanged { get => FOnMapSettingsChanged; set => FOnMapSettingsChanged = value; }

    [Published]
    public NotifyEvent<object> OnCombat { get => FOnCombatEvent; set => FOnCombatEvent = value; }

    [Published]
    public MapPlayerEvent OnLogbookChanged { get => FOnLogbookChanged; set => FOnLogbookChanged = value; }

    [Published]
    public MapTargetEvent OnTargetSelectionChange { get => FOnTargetSelectionChange; set => FOnTargetSelectionChange = value; }

    [Published]
    public MapObjectEvent<SpellCaster> OnCastSpell { get => FOnCastSpell; set => FOnCastSpell = value; }

    [Published]
    public MapObjectEvent<AbilityActivation> OnActivateAbility { get => FOnActivateAbility; set => FOnActivateAbility = value; }

    [Published]
    public MapPlayerEvent OnPlayerMagicChanged { get => FOnPlayerMagicChanged; set => FOnPlayerMagicChanged = value; }

    [Published]
    public NotifyEvent<object> OnGlobalMagicChanged { get => FOnGlobalMagicChanged; set => FOnGlobalMagicChanged = value; }

    [Published]
    public NotifyEvent<object> OnRelationsChanged { get => FOnRelationsChanged; set => FOnRelationsChanged = value; }

    [Published]
    public NotifyEvent<object> OnActivateCombatMap { get => FOnActivateCombatMap; set => FOnActivateCombatMap = value; }

    [Published]
    public NotifyEvent<object> OnDeactivateCombatMap { get => FOnDeactivateCombatMap; set => FOnDeactivateCombatMap = value; }

    [Published]
    public MapObjectEvent<ItemHexSprite> OnItemsSelected { get => FOnSelectItem; set => FOnSelectItem = value; }

    [Published]
    public MapObjectEvent<ItemHexSprite> OnItemsDeselected { get => FOnDeselectItem; set => FOnDeselectItem = value; }

    [Published]
    public NotifyEvent<object> OnGameOver { get => FOnGameOver; set => FOnGameOver = value; }

    [Published]
    public MapPlayerEvent OnGameTerminated { get => FOnGameTerminated; set => FOnGameTerminated = value; }

    [Published]
    public MapPlayerEvent OnPlayerTerminated { get => FOnPlayerTerminated; set => FOnPlayerTerminated = value; }

    [Published]
    public MapExecuteLogEvent OnExecuteEventLog { get => FOnExecuteEventLog; set => FOnExecuteEventLog = value; }

    [Published]
    public NotifyEvent<object> OnViewPortChange { get => FOnViewPortChange; set => FOnViewPortChange = value; }

    [Published]
    public NotifyEvent<object> OnDiplomaticChange { get => FOnDiplomaticChange; set => FOnDiplomaticChange = value; }

    [Published]
    public NotifyEvent<object> OnExitGame { get => FOnExitGameEvent; set => FOnExitGameEvent = value; }

    public delegate void MapPlayerEvent(sbyte Player);
    public delegate void MapDayEvent(int Day);
    public delegate void MapObjectEvent<in TSender>(TSender Obj);
    public delegate void MapRefEvent<TInfo>(ref TInfo Info);
    public delegate void MapEndTurnEvent(sbyte Player, bool Automatic);
    public delegate void MapExecuteLogEvent(EventLog Log, bool Immediate, ExecuteDoneEvent Done);
    public delegate void MapTargetEvent(TargetSelectionControl Control, TargetSelectionChange Change);
}
